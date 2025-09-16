using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Dtos.AD;
using DMS.BUSINESS.Filter.AD;
using DMS.BUSINESS.Models;
using DMS.BUSINESS.Services.AD;
using DMS.CORE.Entities.MT;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.AD
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementMeetingController(IManagementMeetingService service) : ControllerBase
    {
        public readonly IManagementMeetingService _service = service;

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
                transferObject.GetMessage("2000", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetAllMeeting")]
        public async Task<IActionResult> GetAllMeeting()
        {
            var transferObject = new TransferObject();
            var result = await _service.GetAllMeeting();
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

        [HttpPost("InsertMeeting")]
        public async Task<IActionResult> InsertMeeting([FromBody] MeetingModels data)
        {
            var transferObject = new TransferObject();
            await _service.InsertMeeting(data);
            if (_service.Status)
            {
                transferObject.Status = true;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }




        [HttpGet("GetDataMeeting")]
        public async Task<IActionResult> GetDataMeeting([FromQuery] string meetingId)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetDataMeeting(meetingId);
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


        [HttpPut("UpdateMeeting")]
        public async Task<IActionResult> UpdateMeeting([FromBody] MeetingModels data)
        {
            var transferObject = new TransferObject();
            await _service.UpdateMeeting(data);
            if (_service.Status)
            {
                transferObject.Status = true;
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