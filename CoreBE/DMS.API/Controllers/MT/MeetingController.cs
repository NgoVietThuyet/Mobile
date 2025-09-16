using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Dtos.CM;
using DMS.BUSINESS.Dtos.MT;
using DMS.BUSINESS.Models;
using DMS.BUSINESS.Services.HUB;
using DMS.BUSINESS.Services.MT;
using DMS.CORE.Entities.MT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Minio;
using Minio.DataModel.Args;
using NPOI.SS.Formula.Functions;
using System.Security.Claims;

namespace DMS.API.Controllers.MT
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController(IHttpClientFactory httpClientFactory, IMeetingService service, IConfiguration configuration, IMinioClient minioClient, IHubContext<NotificationHub> hubContext) : ControllerBase
    {
        public readonly IMeetingService _service = service;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMinioClient _minioClient = minioClient;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
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

        [HttpGet("GetMeetingsByUser")]
        public async Task<IActionResult> GetMeetingsByUser([FromQuery] string userId)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetMeetingsByUser(userId);
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

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] MeetingDto data)
        {
            var transferObject = new TransferObject();
            var result = await _service.Add(data);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0100", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service);
            }
            return Ok(transferObject);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] MeetingDto data)
        {
            var transferObject = new TransferObject();
            await _service.Update(data);
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetPersonalDocuments")]
        public async Task<IActionResult> GetPersonalDocuments([FromQuery] string username, [FromQuery] string meetingId)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetPersonalDocuments(username, meetingId);
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

        [HttpPost("UploadFilePersonalDocuments")]
        public async Task<IActionResult> UploadFilePersonalDocuments(List<IFormFile> files, [FromQuery] string username, [FromQuery] string meetingId)
        {
            var transferObject = new TransferObject();
            if (files == null || files.Count == 0)
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.MessageObject.Message = "Không có file được chọn";
                return Ok(transferObject);
            }
            var result = await _service.UploadFilePersonalDocuments(files, username, meetingId);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.MessageObject.Message = "Upload file thành công";
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetDocumentjson/{pathFile}")]
        public async Task<IActionResult> GetDocumentJson(string pathFile)
        {
            try
            {
                var (fileData, fileName, contentType) = await _service.GetDocument(pathFile);
                if (_service.Status && fileData != null)
                {
                    // Kiểm tra nếu là file Excalidraw
                    if (fileName?.EndsWith(".excalidraw", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        // Trả về JSON content cho Excalidraw files
                        var jsonContent = System.Text.Encoding.UTF8.GetString(fileData);
                        return Content(jsonContent, "application/json");
                    }
                    else
                    {
                        // Trả về file binary cho các loại file khác
                        return File(fileData, contentType ?? "application/octet-stream", fileName);
                    }
                }
                else
                {
                    return NotFound(new TransferObject
                    {
                        Status = false,
                        MessageObject = new MessageObject
                        {
                            MessageType = MessageType.Error,
                            Message = "File không tồn tại trên MinIO"
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new TransferObject
                {
                    Status = false,
                    MessageObject = new MessageObject
                    {
                        MessageType = MessageType.Error,
                        Message = $"Lỗi khi lấy file: {ex.Message}"
                    }
                });
            }
        }

        [HttpGet("GetDocument/{pathFile}")]
        public async Task<IActionResult> GetDocument(string pathFile)
        {
            try
            {
                var (fileData, fileName, contentType) = await _service.GetDocument(pathFile);

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
                            Message = "File không tồn tại trên MinIO"
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
                        Message = $"Lỗi khi tải file: {ex.Message}"
                    }
                };
                return StatusCode(500, transferObject);
            }
        }
        [HttpHead("GetDocumentPDF/{path}")]

        public async Task<IActionResult> GetDocumentPDF(string filePath)
        {
            try
            {
                var bucket = _configuration["Minio:BucketName"];
                var objectName = filePath;

                var stat = await _minioClient.StatObjectAsync(
                    new StatObjectArgs().WithBucket(bucket).WithObject(objectName));

                var size = stat.Size;
                var contentType = stat.ContentType ?? "application/octet-stream";

                if (HttpMethods.IsHead(Request.Method))
                {
                    Response.Headers["Content-Length"] = size.ToString();
                    Response.Headers["Content-Type"] = contentType;
                    Response.Headers["Accept-Ranges"] = "bytes";
                    return Ok();
                }

                const long SMALL_FILE_LIMIT = 5 * 1024 * 1024;

                if (size < SMALL_FILE_LIMIT)
                {
                    var ms = new MemoryStream();
                    await minioClient.GetObjectAsync(new GetObjectArgs()
                        .WithBucket(bucket)
                        .WithObject(objectName)
                        .WithCallbackStream(stream => stream.CopyTo(ms)));
                    ms.Position = 0;

                    return new FileStreamResult(ms, contentType)
                    {
                        FileDownloadName = Path.GetFileName(objectName)
                    };
                }
                else
                {
                    var presignedUrl = await minioClient.PresignedGetObjectAsync(
                        new PresignedGetObjectArgs()
                            .WithBucket(bucket)
                            .WithObject(objectName)
                            .WithExpiry(10 * 60));

                    var httpClient = new HttpClient();

                    var rangeHeader = Request.Headers["Range"].FirstOrDefault();
                    var request = new HttpRequestMessage(HttpMethod.Get, presignedUrl);
                    request.Headers.TryAddWithoutValidation("Range", rangeHeader);

                    var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                    foreach (var header in response.Headers)
                        Response.Headers[header.Key] = header.Value.ToArray();
                    foreach (var header in response.Content.Headers)
                        Response.Headers[header.Key] = header.Value.ToArray();

                    Response.StatusCode = (int)response.StatusCode;

                    var stream = await response.Content.ReadAsStreamAsync();
                    return new FileStreamResult(stream, contentType);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new TransferObject
                {
                    Status = false,
                    MessageObject = new MessageObject
                    {
                        MessageType = MessageType.Error,
                        Message = $"Lỗi khi tải file: {ex.Message}"
                    }
                });
            }
        }

        [HttpPost("CallbackOnlyOffice")]
        public async Task<IActionResult> CallbackOnlyOffice([FromQuery] string filePath, [FromBody] OnlyOfficeCallbackModel model)
        {
            try
            {
                if (model.status == 2 || model.status == 6)
                {
                    var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.GetAsync(model.url);

                    if (!response.IsSuccessStatusCode)
                    {
                        return Ok(new { error = 1 });
                    }

                    var stream = await response.Content.ReadAsStreamAsync();
                    var contentLength = response.Content.Headers.ContentLength ?? 0;

                    await _service.PutFileObject(filePath, stream, contentLength);

                    if (!_service.Status)
                    {
                        Console.WriteLine("Lưu file thất bại");
                        return Ok(new { error = 1 });
                    }

                    return Ok(new { error = 0 });
                }
                else
                {
                    return Ok(new { error = 0 });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during save: {ex.Message}");
                return Ok(new { error = 0 });
            }

        }

        [HttpGet("GetDetaiMeeting")]
        public async Task<IActionResult> GetDetaiMeeting([FromQuery] string meetId, string userId = "")
        {
            var transferObject = new TransferObject();
            var result = await _service.GetDetaiMeeting(meetId, userId);
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

        [HttpPost("SaveFileDraw")]
        public async Task<IActionResult> SaveFileDraw([FromForm] BlobFile Filedraw)
        {
            try
            {

                var stream = Filedraw.file.OpenReadStream();
                var contentLength = Filedraw.file.Length;
                var result = await _service.PutFileObjectDraw(Filedraw.filePath, stream, contentLength, Filedraw.MeetingId, Filedraw.UserName, Filedraw.fileName);
                var transferObject = new TransferObject();

                if (!_service.Status)
                {
                    Console.WriteLine("Lưu file thất bại");
                    return Ok(new { error = 1 });
                }
                transferObject.Data = result;

                return Ok(transferObject);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during save: {ex.Message}");
                return Ok(new { error = 0 });
            }
        }


        [HttpPost("CreateFileFromTemplate")]
        public async Task<IActionResult> CreateFileFromTemplate([FromBody] CreateFileFromTemplateRequest request)
        {
            try
            {
                var transferObject = new TransferObject();

                var result = await _service.CreateFileFromTemplate(request);

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
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during save: {ex.Message}");
                return Ok(new { error = 0 });
            }
        }


        [HttpPost("ShareFileAllMeet")]
        public async Task<IActionResult> ShareFileAllMeet([FromBody] CreateFileFromTemplateRequest request)
        {
            try
            {
                var transferObject = new TransferObject();

                var result = await _service.ShareFileAllMeet(request);

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
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during save: {ex.Message}");
                return Ok(new { error = 0 });
            }
        }

      

  
        [HttpGet("GetMuteParticipant")]
        public async Task<IActionResult> GetMuteParticipant([FromQuery] string meetingId, string userId, string typeMute)
        {
            var transferObject = new TransferObject();

            await _hubContext.Clients.Group(meetingId).SendAsync("ReceiveNotification",
                new
                {
                    type = "mute-participant",
                    userId = userId,
                    message = "Có thông báo mới từ hệ thống",
                    payload = new
                    {
                        typeMute = typeMute,
                    }
                });
            if (_service.Status)
            {
                transferObject.Data = null;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }
        [HttpGet("GetMuteAll")]

        public async Task<IActionResult> GetMuteAll([FromQuery] string meetingId, string typeMute, bool status)
        {
            var transferObject = new TransferObject();

            await _hubContext.Clients.Group(meetingId).SendAsync("ReceiveNotification",
                new
                {
                    type = "mute-All",                   
                    message = "Có thông báo mới từ hệ thống",
                   
                    payload = new
                    {
                        typeMute = typeMute,
                        status= status
                    }
                });
            if (_service.Status)
            {
                transferObject.Data = null;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetRoomSeatInfo")]

       

        [HttpGet("GetScreenVote")]
        public async Task<IActionResult> GetScreenVote([FromQuery] string meetingId, string voteId)
        {
            var transferObject = new TransferObject();

            var data = await _service.GetVoteDetail(voteId);
            await _hubContext.Clients.Group(meetingId).SendAsync("ReceiveNotification", new 
            {
                type = "Start-vote",
                payload = data
            });
            if (_service.Status)
            {
                //transferObject.Data = data;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("KickMember")]
        public async Task<IActionResult> KickMember([FromQuery] string meetingId, string userIdJsi)
        {
            var transferObject = new TransferObject();

            //var data = await _service.GetVoteDetail(voteId);
            await _hubContext.Clients.Group(meetingId).SendAsync("ReceiveNotification", new
            {
                type = "KickMember",
                payload = new
                {
                    meetingId = meetingId,
                    userIdJsi = userIdJsi
                }
            });
            if (_service.Status)
            {
                //transferObject.Data = data;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        public class SendVoteParam
        {
           public  string ? meetingId  { get; set; }
            public string ? voteId { get; set; }
            public string ? userId { get; set; }
            public string ? Suggestions { get; set; }
            public string ? selectedOption { get; set; }
        }
        [HttpPost("SendVote")]

        public async Task<IActionResult> SendVote([FromBody] SendVoteParam data)
        {
            var transferObject = new TransferObject();

           
            await _hubContext.Clients.Group(data.meetingId).SendAsync("ReceiveNotification", new
            {
                type = "Send-vote",
                payload = new
                {
                    meeting= data.meetingId,
                    voteId= data.voteId,
                    Suggestions= data.Suggestions,
                    selectedOption= data.selectedOption

                }
            });
            if (_service.Status)
            {
                //transferObject.Data = data;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }



        [HttpPost("InsertResultVote")]
        public async Task<IActionResult> InsertResultVote([FromBody] TblMtVoteResult request)
        {
            try
            {
                var transferObject = new TransferObject();

                var data = await _service.InsertResultVote(request);

                await _hubContext.Clients.Group(request.MeetingId).SendAsync("ReceiveNotification", new
                {
                    type = "Send-vote",
                    payload = data
                });

                if (_service.Status)
                {
                    transferObject.Data = data;
                }
                else
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.GetMessage("0001", _service);
                }
                return Ok(transferObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during save: {ex.Message}");
                return Ok(new { error = 0 });
            }
        }


        [HttpPost("EndVote")]
        public async Task<IActionResult> EndVote([FromBody] TblMtVotes vote)
        {
            try
            {
                var transferObject = new TransferObject();

                var data = await _service.EndVote(vote.Id);

                await _hubContext.Clients.Group(vote.MeetingId).SendAsync("ReceiveNotification", new
                {
                    type = "endVote",
                    payload = data
                });

                if (_service.Status)
                {
                    transferObject.Data = data;
                }
                else
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.GetMessage("0001", _service);
                }
                return Ok(transferObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during save: {ex.Message}");
                return Ok(new { error = 0 });
            }
        }

        [HttpGet("StartMeeting")]
        public async Task<IActionResult> StartMeeting([FromQuery] string meetingId)
        {
            var transferObject = new TransferObject();

            var data = await _service.StartMeeting(meetingId);
            await _hubContext.Clients.Group(meetingId).SendAsync("ReceiveNotification", new
            {
                type = "startMeeting",
                payload = data
            });

            if (_service.Status)
            {
                //transferObject.Data = data;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("EndMeeting")]
        public async Task<IActionResult> EndMeeting([FromQuery] string meetingId)
        {
            var transferObject = new TransferObject();

            var data = await _service.EndMeeting(meetingId);
            await _hubContext.Clients.Group(meetingId).SendAsync("ReceiveNotification", new
            {
                type = "EndMeeting",
                payload = data
            });

            if (_service.Status)
            {
                //transferObject.Data = data;
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

}

