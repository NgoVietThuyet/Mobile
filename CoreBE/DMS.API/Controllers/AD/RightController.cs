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
    public class RightController(IRightService service) : ControllerBase
    {
        public readonly IRightService _service = service;

        [HttpGet("GetRightTree")]
        public async Task<IActionResult> GetRightTree()
        {
            var transferObject = new TransferObject();
            var result = await _service.BuildDataForTree();
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

        [HttpGet("GetRightOfUser")]
        public async Task<IActionResult> GetRightOfUser(string userName)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetRightOfUser(userName);
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

        [HttpPut("Update-Order")]
        public async Task<IActionResult> UpdateRight([FromBody] RightDto moduleDto)
        {
            var transferObject = new TransferObject();
            await _service.UpdateOrderTree(moduleDto);
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

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] RightDto rightDto)
        {
            var transferObject = new TransferObject();
            var result = await _service.Add(rightDto);
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
        public async Task<IActionResult> Update([FromBody] RightDto moduleDto)
        {
            var transferObject = new TransferObject();
            await _service.Update(moduleDto);
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

  

        [HttpDelete("Delete/{code}")]
        public async Task<IActionResult> Delete([FromRoute] string code)
        {
            var transferObject = new TransferObject();

            var result = await _service.Delete(code);
            if (_service.Status && result != null)
            {
                transferObject.Status = true;
                transferObject.Data = result;
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


    }
}
