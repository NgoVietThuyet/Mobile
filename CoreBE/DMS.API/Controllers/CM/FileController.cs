using Common;
using DMS.BUSINESS.Dtos.CM;
using DMS.BUSINESS.Services.CM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.CM
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ICmFileService _fileService;

        public FileController(ICmFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Upload multiple files
        /// </summary>
        /// <param name="files">List of files to upload</param>
        /// <param name="customPath">Custom folder path (optional)</param>
        /// <returns>List of uploaded file information</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files, [FromQuery] string? customPath = null)
        {
            try
            {
                if (files == null || !files.Any())
                {
                    return BadRequest(new { message = "No files provided" });
                }

                var result = await _fileService.UploadFiles(files, customPath);

                if (!_fileService.Status)
                {
                    return BadRequest(new { message = "Upload failed", error = _fileService.Exception?.Message });
                }

                return Ok(new
                {
                    message = "Files uploaded successfully",
                    data = result,
                    count = result.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Upload file from byte array
        /// </summary>
        /// <param name="request">Upload request containing file data</param>
        /// <returns>Uploaded file information</returns>
        [HttpPost("upload-bytes")]
        public async Task<IActionResult> UploadFileFromBytes([FromBody] UploadFromBytesRequest request)
        {
            try
            {
                if (request == null || request.FileBytes == null || request.FileBytes.Length == 0)
                {
                    return BadRequest(new { message = "No file data provided" });
                }

                if (string.IsNullOrEmpty(request.FileName))
                {
                    return BadRequest(new { message = "File name is required" });
                }

                var result = await _fileService.UploadFileFromBytes(
                    request.FileBytes,
                    request.FileName,
                    request.ContentType ?? "application/octet-stream",
                    request.CustomPath
                );

                if (!_fileService.Status)
                {
                    return BadRequest(new { message = "Upload failed", error = _fileService.Exception?.Message });
                }

                return Ok(new { message = "File uploaded successfully", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Download file by path (fileId is actually the file path in MinIO)
        /// </summary>
        /// <param name="fileId">File path in MinIO</param>
        /// <returns>File content</returns>
        [HttpGet("download/{*fileId}")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            try
            {
                var (fileBytes, fileName, contentType) = await _fileService.GetDocument(fileId);

                if (!_fileService.Status)
                {
                    return NotFound(new { message = "File not found", error = _fileService.Exception?.Message });
                }

                if (fileBytes == null)
                {
                    return NotFound(new { message = "File not found" });
                }

                return File(fileBytes, contentType ?? "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get presigned URL for file access
        /// </summary>
        /// <param name="fileId">File path in MinIO</param>
        /// <param name="expiryInSeconds">URL expiry time in seconds (default: 3600)</param>
        /// <returns>Presigned URL</returns>
        [HttpGet("presigned-url/{*fileId}")]
        public async Task<IActionResult> GetPresignedUrl(string fileId, [FromQuery] int expiryInSeconds = 3600)
        {
            try
            {
                var url = await _fileService.GetPresignedUrl(fileId, expiryInSeconds);

                if (!_fileService.Status)
                {
                    return BadRequest(new { message = "Failed to generate presigned URL", error = _fileService.Exception?.Message });
                }

                if (string.IsNullOrEmpty(url))
                {
                    return NotFound(new { message = "File not found or URL generation failed" });
                }

                return Ok(new { url, expiryInSeconds });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete file by path
        /// </summary>
        /// <param name="fileId">File path in MinIO</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{*fileId}")]
        public async Task<IActionResult> DeleteFile(string fileId)
        {
            try
            {
                var result = await _fileService.DeleteFile(fileId);

                if (!_fileService.Status)
                {
                    return BadRequest(new { message = "Delete failed", error = _fileService.Exception?.Message });
                }

                if (!result)
                {
                    return NotFound(new { message = "File not found or delete failed" });
                }

                return Ok(new { message = "File deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Copy file
        /// </summary>
        /// <param name="sourceFileId">Source file path in MinIO</param>
        /// <param name="customPath">Custom destination path (optional)</param>
        /// <returns>Copied file information</returns>
        [HttpPost("copy/{*sourceFileId}")]
        public async Task<IActionResult> CopyFile(string sourceFileId, [FromQuery] string? customPath = null)
        {
            try
            {
                var result = await _fileService.CopyFile(sourceFileId, customPath);

                if (!_fileService.Status)
                {
                    return BadRequest(new { message = "Copy failed", error = _fileService.Exception?.Message });
                }

                if (result == null)
                {
                    return NotFound(new { message = "Source file not found" });
                }

                return Ok(new { message = "File copied successfully", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }



        [HttpGet("DownloadDirect/{path}")]
        public async Task<IActionResult> DownloadDirect(string path)
        {
            try
            {
                var (fileData, fileName, contentType) = await _fileService.DownloadDirectFromMinio(path);

                if (_fileService.Status && fileData != null)
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

        #region Image Methods

        /// <summary>
        /// Upload multiple images (validates image types)
        /// </summary>
        /// <param name="images">List of image files</param>
        /// <returns>List of uploaded image information</returns>
        [HttpPost("upload-images")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> images)
        {
            try
            {
                if (images == null || !images.Any())
                {
                    return BadRequest(new { message = "No images provided" });
                }

                var result = await _fileService.UploadImages(images);

                if (!_fileService.Status)
                {
                    return BadRequest(new { message = "Upload failed", error = _fileService.Exception?.Message });
                }

                return Ok(new
                {
                    message = "Images uploaded successfully",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("UploadVoiceFile")]
        public async Task<IActionResult> UploadVoiceFile(IFormFile file, string meetingId)
        {
            try
            {
                if (file == null)
                {
                    return BadRequest(new { message = "No images provided" });
                }

                var result = await _fileService.UploadVoiceFile(file, meetingId);

                if (!_fileService.Status)
                {
                    return BadRequest(new { message = "Upload failed", error = _fileService.Exception?.Message });
                }

                return Ok(new
                {
                    message = "File Voice uploaded successfully",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("UploadFileSummary")]
        public async Task<IActionResult> UploadFileSummary(IFormFile file, string meetingId)
        {
            try
            {
                if (file == null)
                {
                    return BadRequest(new { message = "No images provided" });
                }

                var result = await _fileService.UploadFileSummary(file, meetingId);

                if (!_fileService.Status)
                {
                    return BadRequest(new { message = "Upload failed", error = _fileService.Exception?.Message });
                }

                return Ok(new
                {
                    message = "File summary uploaded successfully",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Download image by ID or path
        /// </summary>
        /// <param name="fileId">Image file ID or path</param>
        /// <returns>Image content</returns>
        [HttpGet("download-image/{*fileId}")]
        public async Task<IActionResult> DownloadImage(string fileId)
        {
            try
            {
                var (imageBytes, fileName) = await _fileService.DownloadImage(fileId);

                if (!_fileService.Status)
                {
                    return BadRequest(new { message = "Download failed", error = _fileService.Exception?.Message });
                }

                if (imageBytes == null)
                {
                    return NotFound(new { message = "Image not found" });
                }

                // Determine content type based on file extension
                var contentType = GetImageContentType(fileName);
                return File(imageBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get direct image URL for display
        /// </summary>
        /// <param name="fileId">Image file ID or path</param>
        /// <returns>Direct image URL</returns>
        [HttpGet("image-url/{*fileId}")]
        public async Task<IActionResult> GetImageUrl(string fileId)
        {
            try
            {
                var url = await _fileService.GetImageUrl(fileId);

                if (!_fileService.Status)
                {
                    return BadRequest(new { message = "Failed to get image URL", error = _fileService.Exception?.Message });
                }

                if (string.IsNullOrEmpty(url))
                {
                    return NotFound(new { message = "Image not found" });
                }

                return Ok(new { url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        #endregion

       
        #region Helper Methods

        private string GetImageContentType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return "image/jpeg";

            var extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                ".tiff" or ".tif" => "image/tiff",
                _ => "image/jpeg"
            };
        }

        #endregion
    }

    #region Request Models

    public class UploadFromBytesRequest
    {
        public byte[] FileBytes { get; set; }
        public string FileName { get; set; }
        public string? ContentType { get; set; }
        public string? CustomPath { get; set; }
    }

    #endregion
}