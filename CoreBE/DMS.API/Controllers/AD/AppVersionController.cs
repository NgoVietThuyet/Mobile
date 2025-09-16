using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Dtos.AD;
using DMS.BUSINESS.Services.AD;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.AD
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppVersionController(IAppVersionService service) : ControllerBase
    {
        private readonly IAppVersionService _service = service;

        [HttpGet("GetCurrentVersion")]
        public async Task<IActionResult> GetCurrentVersion()
        {
            var transferObject = new TransferObject();
            var result = await _service.GetCurrentVersion();
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
        public async Task<IActionResult> Insert([FromBody] AppVersionDto message)
        {
            var transferObject = new TransferObject();
            var result = await _service.Add(message);
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
    }
}

