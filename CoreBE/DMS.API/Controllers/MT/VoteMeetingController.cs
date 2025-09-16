using System.Security.Claims;
using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Dtos.MT;
using DMS.BUSINESS.Models;
using DMS.BUSINESS.Services.HUB;
using DMS.BUSINESS.Services.MT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DMS.API.Controllers.MT
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteMeetingController(IVoteService service, IHubContext<VoteMeetingHub> hubContext) : ControllerBase
    {
        public readonly IVoteService _service = service;
        public readonly IHubContext<VoteMeetingHub> _hubContext = hubContext;

        [HttpPost("CreateVoteMeeting")]
        public async Task<IActionResult> CreateVoteMeeting([FromBody] VotesModels votesModels)
        {
            var transferObject = new TransferObject();

            await _service.CreateVoteMeeting(votesModels);

            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageDetail = "Thêm biểu quyết thành công";
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageDetail = _service.MessageObject.MessageDetail;
                transferObject.MessageObject.MessageType = MessageType.Error;
            }

            return Ok(transferObject);
        }

        [HttpPost("UpdateVoteMeeting")]
        public async Task<IActionResult> UpdateVoteMeeting([FromBody] VotesModels votesModels)
        {
            var transferObject = new TransferObject();

            await _service.UpdateVoteMeeting(votesModels);

            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageDetail = "Cập nhật biểu quyết thành công";
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageDetail = _service.MessageObject.MessageDetail;
                transferObject.MessageObject.MessageType = MessageType.Error;
            }

            return Ok(transferObject);
        }

        [HttpGet("GetDetailVotes")]
        public async Task<IActionResult> GetDetailVotes([FromQuery] string voteId)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetDetailVotes(voteId);

            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.Data = result;
                transferObject.MessageObject.MessageDetail = "Lấy chi tiết biểu quyết thành công.";
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageDetail = _service.MessageObject.MessageDetail;
                transferObject.MessageObject.MessageType = MessageType.Error;
            }

            return Ok(transferObject);
        }


        [HttpPost("JoinVote")]
        public async Task<IActionResult> JoinVote([FromBody] VoteJoinRequest request)
        {
            try
            {
                var transferObject = new TransferObject();
                var voteId = request.VoteId;

                var userId = User.FindFirst("userId")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var result = await _service.JoinVote(voteId, userId);

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

        [HttpPost("SubmitVoteForm")]
        public async Task<IActionResult> SubmitVoteForm([FromBody] ResultVoteRequest request)
        {
            try
            {
                var transferObject = new TransferObject();
                var voteId = request.VoteId;

                var userId = Request.Headers["UserId"].ToString();

                await _service.SubmitVoteForm(request, userId);

                if (!_service.Status)

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

        [HttpGet("GetVoteByMeetingId")]
        public async Task<IActionResult> GetVoteByMeetingId([FromQuery] string meetingId)
        {
            try
            {
                var transferObject = new TransferObject();
                var userId = Request.Headers["UserId"].FirstOrDefault();
                var result = await _service.GetListVoteByMeetingId(meetingId, userId);

                transferObject.Data = result;

                if (!_service.Status)
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.MessageObject.MessageDetail = _service.MessageObject.MessageDetail;
                    transferObject.GetMessage("0001", _service);
                }

                return Ok(transferObject);
            }
            catch
            {
                return Ok(new { error = 0 });
            }

        }

        [HttpDelete("DeleteVote")]
        public async Task<IActionResult> DeleteVote([FromQuery] string voteId)
        {
            try
            {
                var transferObject = new TransferObject();
                await _service.DeleteVote(voteId);

                if (!_service.Status)
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.MessageObject.MessageDetail = _service.MessageObject.MessageDetail;
                    transferObject.GetMessage("0001", _service);
                }

                return Ok(transferObject);
            }
            catch
            {
                return Ok(new { error = 0 });
            }
        }

        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromQuery] string voteId, [FromBody] VoteStatusRequest voteStatus)
        {
            try
            {
                var transferObject = new TransferObject();
                await _service.UpdateStatusVote(voteId, voteStatus.Status);

                if (!_service.Status)
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.MessageObject.MessageDetail = _service.MessageObject.MessageDetail;
                    transferObject.GetMessage("0001", _service);
                }

                return Ok(transferObject);
            }
            catch
            {
                return Ok(new { error = 0 });
            }
        }

        [HttpPost("StartVote")]
        public async Task<IActionResult> StartVote([FromBody] VoteSendRequest voteObject)
        {
            try
            {
                var transferObject = new TransferObject();
                if (voteObject == null || string.IsNullOrEmpty(voteObject.MeetingId) || string.IsNullOrEmpty(voteObject.VoteId) || string.IsNullOrEmpty(voteObject.UserId))
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;

                    return Ok(transferObject);
                }

                var meetingId = voteObject.MeetingId;
                var voteId = voteObject.VoteId;
                var userId = voteObject.UserId;



                await _service.StartVote(voteId);

                if (!_service.Status)
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;

                    return Ok(transferObject);
                }

                await _hubContext.Clients.Group($"Vote_Meeting_{meetingId}_{voteId}").SendAsync("ReceiveVote", new
                {
                    type = "start_vote",
                    voteId,
                    message = "Tham gia biểu quyết",
                });

                return Ok(transferObject);
            }
            catch
            {
                return Ok(new { error = 0 });
            }
        }

        [HttpPost("EndVote")]
        public async Task<IActionResult> EndVote([FromQuery] string voteId)
        {
            try
            {
                var transferObject = new TransferObject();
                if (string.IsNullOrEmpty(voteId))
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;

                    return Ok(transferObject);
                }

                await _service.UpdateStatusVote(voteId, "COMPLETED");

                return Ok(transferObject);
            }
            catch
            {
                return Ok(new { error = 0 });
            }
        }

        [HttpGet("GetAnswerVote")]
        public async Task<IActionResult> GetAnswerVote([FromQuery] string voteId, [FromQuery] string meetingId)
        {
            try
            {
                var transferObject = new TransferObject();
                if (string.IsNullOrEmpty(voteId))
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;

                    return Ok(transferObject);
                }

                var result = await _service.GetAnswerVote(voteId, meetingId);

                if (_service.Status)
                {
                    transferObject.Data = result;
                }
                else
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                }

                return Ok(transferObject);

            }
            catch
            {
                return Ok(new { error = 0 });
            }
        }
    }
}
