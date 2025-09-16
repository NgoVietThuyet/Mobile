using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Dtos.AD;
using DMS.BUSINESS.Filter.AD;
using DMS.BUSINESS.Services.AD;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.MD
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService service) : ControllerBase
    {
        public readonly IUserService _service = service;

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] UserFilter filter)
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
                transferObject.GetMessage("2000", _service);
            }
            return Ok(transferObject);
        }


        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] UserCreateDto user)
        {
            var transferObject = new TransferObject();
            var result = await _service.AddUser(user);
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




        [HttpPost("InsertGuest")]
        public async Task<IActionResult> InsertGuest(string meetingId, string guestName)
        {
            var transferObject = new TransferObject();

            try
            {
                // Validation input
                if (string.IsNullOrEmpty(meetingId))
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.MessageObject.Message = "Meeting ID không được để trống";
                    return BadRequest(transferObject);
                }

                if (string.IsNullOrEmpty(guestName))
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.MessageObject.Message = "Tên guest không được để trống";
                    return BadRequest(transferObject);
                }

                // Gọi service method
                var result = await _service.AddUserGuest(meetingId, guestName);

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
            catch (Exception ex)
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.MessageObject.Message = "Có lỗi xảy ra: " + ex.Message;
                return StatusCode(500, transferObject);
            }
        }


        [HttpPost("GuestJoinMeeting")]
        public async Task<IActionResult> GuestJoinMeeting(string? meetingId, string? meetingCode , string? guestName)
        {
            var transferObject = new TransferObject();

            try
            {
                // Validation input
                if (string.IsNullOrEmpty(meetingId))
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.MessageObject.Message = "Meeting ID không được để trống";
                    return BadRequest(transferObject);
                }

                if (string.IsNullOrEmpty(guestName))
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.MessageObject.Message = "Tên guest không được để trống";
                    return BadRequest(transferObject);
                }

                if (string.IsNullOrEmpty(meetingCode))
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.MessageObject.Message = "MeetingCode không được để trống";
                    return BadRequest(transferObject);
                }

                // Gọi service method
                var result = await _service.GuestJoinMeeting(meetingId, meetingCode, guestName);

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
            catch (Exception ex)
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.MessageObject.Message = "Có lỗi xảy ra: " + ex.Message;
                return StatusCode(500, transferObject);
            }
        }


        [HttpPost("LoginUserMember")]
        public async Task<IActionResult> LoginUserMember(string meetingId, string userId)
        {
            var transferObject = new TransferObject();

            try
            {
                // Validation input
                if (string.IsNullOrEmpty(meetingId))
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.MessageObject.Message = "Meeting ID không được để trống";
                    return BadRequest(transferObject);
                }

                if (string.IsNullOrEmpty(userId))
                {
                    transferObject.Status = false;
                    transferObject.MessageObject.MessageType = MessageType.Error;
                    transferObject.MessageObject.Message = "id người dùng không được để trống";
                    return BadRequest(transferObject);
                }


                // Gọi service method
                var result = await _service.LoginUserMember(meetingId, userId);

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
            catch (Exception ex)
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.MessageObject.Message = "Có lỗi xảy ra: " + ex.Message;
                return StatusCode(500, transferObject);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] UserFilterLite filter)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetAll(filter);
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

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(string userId)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetUserById(userId);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("2000", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserCreateDto user)
        {
            var transferObject = new TransferObject();
            await _service.Update(user);
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

        [HttpDelete("Delete/{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            var transferObject = new TransferObject();
            await _service.Delete(userId);
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0105", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0106", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("RegisterFace")]
        public IActionResult RegisterFace([FromQuery] string pkid)
        {
            var transferObject = new TransferObject();
            var result = _service.RegisterFaceAsync(pkid);
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
        [HttpGet("GetUIdFace")]
        public IActionResult GetUIdFace([FromQuery] string username)
        {
            var transferObject = new TransferObject();
            var result = _service.GetUIdFace(username);
            if (_service.Status)
            {
                transferObject.Data = result;
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
    }
}
