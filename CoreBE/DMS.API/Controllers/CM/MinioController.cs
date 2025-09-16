using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Common;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Services.CM;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using Newtonsoft.Json;

namespace DMS.API.Controllers.CM
{
    [ApiController]
    [Route("api/[controller]")]
    public class MinioController(IHttpClientFactory httpClientFactory, IMinioService service, IConfiguration configuration, IMinioClient minioClient) : ControllerBase
    {

        public readonly IMinioService _service = service;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMinioClient _minioClient = minioClient;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;


        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            var transferObject = new TransferObject();
            if (files == null || files.Count == 0)
            {
                transferObject.Status = false;
                transferObject.MessageObject.Message = "Không có file được chọn";
                return Ok(transferObject);
            }
            var result = await _service.UploadFile(files);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.Message = "Upload file thành công";
            }
            else
            {
                transferObject.Status = false;
                transferObject.GetMessage("0101", _service);
            }
            return Ok(transferObject);
        }


        [HttpGet("GetDocument/{filePath}")]
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

        [HttpGet("DownloadDirect/{path}")]
        public async Task<IActionResult> DownloadDirect(string path)
        {
            try
            {
                var (fileData, fileName, contentType) = await _service.DownloadDirectFromMinio(path);

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
                            //MessageType = MessageType.Error,
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
                        //MessageType = MessageType.Error,
                        Message = $"Lỗi khi tải file: {ex.Message}"
                    }
                };
                return StatusCode(500, transferObject);
            }
        }
    }

}
