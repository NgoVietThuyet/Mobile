using AutoMapper;
using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Dtos.CM;
using DMS.BUSINESS.Dtos.MT;
using DMS.BUSINESS.Models;
using DMS.BUSINESS.Services.HUB;
using DMS.BUSINESS.Services.MT;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Minio;
using Minio.DataModel.Args;
using System.Security.Claims;

namespace DMS.API.Controllers.MT
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController(IChatService service, IConfiguration configuration, IMinioClient minioClient, IHubContext<NotificationHub> hubContext) : ControllerBase
    {
        public readonly IChatService _service = service;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMinioClient _minioClient = minioClient;
        public readonly IHubContext<NotificationHub> _hubContext = hubContext;

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] BaseFilter filter)
        {
            var transferObject = new TransferObject();
            var result = await _service.Search(filter);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] ChatCreateDto chatCreateDto)
        {
            var transferObject = new TransferObject();
            var senderId = chatCreateDto.SenderId; // Using PascalCase property
           
            if (string.IsNullOrEmpty(senderId))
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.MessageObject.Message = "Không thể xác định user hiện tại";
                return Ok(transferObject);
            }

            var result = await _service.SendMessage(chatCreateDto, senderId);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.MessageObject.Message = "Gửi tin nhắn thành công";

                // Send real-time notification via SignalR
                if (chatCreateDto.MessageType == "P")
                {
                    await _hubContext.Clients.Group(chatCreateDto.MeetingId).SendAsync("Chat",
                         new
                         {
                             type = "send-public",
                             payload = new
                             {
                             }


                         });
                }
                if (chatCreateDto.MessageType != "P")
                {
                    await _hubContext.Clients.Group(chatCreateDto.MeetingId).SendAsync("Chat",
                         new
                         {
                             type = "send-private",
                             payload = new
                             {
                                 reciveId = chatCreateDto.ReceiverId
                             }


                         });


                }
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service);
            }
            return Ok(transferObject);
        }

        [HttpPost("SendFileMessage")]
        public async Task<IActionResult> SendFileMessage([FromForm] ChatFileDto chatFileDto)
        {
            var transferObject = new TransferObject();
            var senderId = chatFileDto.SenderId; // Using PascalCase property

            if (string.IsNullOrEmpty(senderId))
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.MessageObject.Message = "Không thể xác định user hiện tại";
                return Ok(transferObject);
            }

            if (chatFileDto.File == null || chatFileDto.File.Length == 0)
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.MessageObject.Message = "Không có file được chọn";
                return Ok(transferObject);
            }

            var chatCreateDto = new ChatCreateDto
            {
                MeetingId = chatFileDto.MeetingId,
                MessageType = chatFileDto.MessageType,
                ReceiverId = chatFileDto.ReceiverId,
                Content = $"[File: {chatFileDto.File.FileName}]",
                ContentType = chatFileDto.File.ContentType.StartsWith("image/") ? "IMAGE" : "FILE"
            };

            var result = await _service.SendFileMessage(chatCreateDto, chatFileDto.File, senderId);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.MessageObject.Message = "Gửi file thành công";

                // Send real-time notification via SignalR
                await _hubContext.Clients.Group(chatFileDto.MeetingId).SendAsync("ReceiveMessage",
                    new
                    {
                        type = "new-file-message",
                        messageType = chatFileDto.MessageType,
                        data = result,
                        senderId = senderId
                    });
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("EditMessage")]
        public async Task<IActionResult> EditMessage([FromBody] EditMessageDto editDto)
        {
            var transferObject = new TransferObject();
            var senderId = editDto.SenderId; // Using PascalCase property

            var result = await _service.EditMessage(editDto.MessageId, editDto.NewContent, senderId);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.MessageObject.Message = "Chỉnh sửa tin nhắn thành công";

                // Send real-time notification via SignalR
                await _hubContext.Clients.Group(result.MeetingId).SendAsync("ReceiveMessage",
                    new
                    {
                        type = "message-edited",
                        data = result,
                        senderId = senderId
                    });
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpDelete("DeleteMessage/{messageId}")]
        public async Task<IActionResult> DeleteMessage(string messageId, [FromQuery] string senderId)
        {
            var transferObject = new TransferObject();

            // Get message info before deleting for SignalR notification
            var messageInfo = await _service.GetMessageById(messageId, senderId);
            var meetingId = messageInfo?.MeetingId;

            var result = await _service.DeleteMessage(messageId, senderId);
            if (_service.Status && result)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.MessageObject.Message = "Xóa tin nhắn thành công";

                // Send real-time notification via SignalR
                if (!string.IsNullOrEmpty(meetingId))
                {
                    await _hubContext.Clients.Group(meetingId).SendAsync("ReceiveMessage",
                        new
                        {
                            type = "message-deleted",
                            messageId = messageId,
                            senderId = senderId
                        });
                }
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0105", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetPublicMessages")]
        public async Task<IActionResult> GetPublicMessages([FromQuery] string meetingId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetPublicMessages(meetingId, page, pageSize);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetPrivateMessages")]
        public async Task<IActionResult> GetPrivateMessages([FromQuery] string meetingId, [FromQuery] string senderId, [FromQuery] string targetUserId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetPrivateMessages(meetingId, senderId, targetUserId, page, pageSize);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetAllMessages")]
        public async Task<IActionResult> GetAllMessages([FromQuery] string meetingId, [FromQuery] string senderId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetAllMessagesForUser(meetingId, senderId, page, pageSize);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("SearchMessages")]
        public async Task<IActionResult> SearchMessages([FromQuery] string meetingId, [FromQuery] string senderId, [FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var transferObject = new TransferObject();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.MessageObject.Message = "Từ khóa tìm kiếm không được để trống";
                return Ok(transferObject);
            }

            var result = await _service.SearchMessages(meetingId, keyword, senderId, page, pageSize);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpPost("ReplyToMessage")]
        public async Task<IActionResult> ReplyToMessage([FromBody] ReplyMessageDto replyDto)
        {
            var transferObject = new TransferObject();
            var senderId = replyDto.SenderId; // Using PascalCase property

            var result = await _service.ReplyToMessage(replyDto.ChatCreateDto, replyDto.ReplyToMessageId, senderId);

            
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.MessageObject.Message = "Reply tin nhắn thành công";

                // Send real-time notification via SignalR
                if (replyDto.ChatCreateDto.MessageType == "P")
                {
                    await _hubContext.Clients.Group(replyDto.ChatCreateDto.MeetingId).SendAsync("Chat",
                         new
                         {
                             type = "reply-public",
                             payload = new { }


                         });
                }

                if (replyDto.ChatCreateDto.MessageType != "P")
                {
                    await _hubContext.Clients.Group(replyDto.ChatCreateDto.MeetingId).SendAsync("Chat",
                         new
                         {
                             type = "reply-private",
                             payload = new
                             {
                                 reciveId = replyDto.ChatCreateDto.ReceiverId
                             }
                         });
                }
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetMessageById/{messageId}")]
        public async Task<IActionResult> GetMessageById(string messageId, [FromQuery] string senderId)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetMessageById(messageId, senderId);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetAttachmentFile/{filePath}")]
        public async Task<IActionResult> GetAttachmentFile(string filePath)
        {
            try
            {
                var (fileData, fileName, contentType) = await _service.GetAttachmentFile(filePath);
                if (_service.Status && fileData != null)
                {
                    return File(fileData, contentType ?? "application/octet-stream", fileName);
                }
                else
                {
                    return NotFound(new TransferObject
                    {
                        Status = false,
                        MessageObject = new MessageObject
                        {
                            MessageType = MessageType.Error,
                            Message = "File không tồn tại"
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                var transferObject = new TransferObject
                {
                    Status = false,
                    MessageObject = new MessageObject
                    {
                        MessageType = MessageType.Error,
                        Message = $"Lỗi khi tải file: {ex.Message}"
                    }
                };
                return StatusCode(500, transferObject);
            }
        }

        [HttpGet("ValidateUserInMeeting")]
        public async Task<IActionResult> ValidateUserInMeeting([FromQuery] string meetingId, [FromQuery] string senderId)
        {
            var transferObject = new TransferObject();
            var result = await _service.ValidateUserInMeeting(meetingId, senderId);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.MessageObject.Message = result ? "User có quyền truy cập cuộc họp" : "User không có quyền truy cập cuộc họp";
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }
    }

    // Supporting DTOs for the controller
    public class ChatFileDto
    {
        public string MeetingId { get; set; }
        public string MessageType { get; set; }
        public string? ReceiverId { get; set; }
        public string? SenderId { get; set; }
        public IFormFile File { get; set; }
    }

    public class EditMessageDto
    {
        public string MessageId { get; set; }
        public string SenderId { get; set; }
        public string NewContent { get; set; }
    }

    public class ReplyMessageDto
    {
        public ChatCreateDto ChatCreateDto { get; set; }
        public string ReplyToMessageId { get; set; }
        public string SenderId { get; set; }
    }
}