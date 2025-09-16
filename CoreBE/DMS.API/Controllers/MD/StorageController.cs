using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS;
using DMS.BUSINESS.Dtos.MD;
using DMS.BUSINESS.Services.MD;
using DMS.CORE.Entities.MD;
using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;

namespace DMS.API.Controllers.MD
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController(IStorageService service) : ControllerBase
    {
        public readonly IStorageService _service = service;

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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] BaseMdFilter filter)
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
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] StorageDto data)
        {
            var transferObject = new TransferObject();
            data.Id = Guid.NewGuid().ToString();
            await _service.Add(data);
            if (_service.Status)
            {
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
        [HttpPost("ImportExcel")]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            var result = await _service.ImportExcel(file);

            var transferObject = new TransferObject
            {
                Status = _service.Status,
                Data = result
            };

            if (_service.Status)
            {
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0100", _service);
            }
            else
            {
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service);
            }

            return Ok(transferObject);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] StorageDto data)
        {
            var transferObject = new TransferObject();
            await _service.UpdateInfo(data);
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
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var transferObject = new TransferObject();
            await _service.DeleteInfo(id);
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

        
    }
}
