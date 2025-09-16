using AutoMapper;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.CM;
using DMS.CORE;
using DMS.CORE.Entities.CM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace DMS.BUSINESS.Services.CM
{
    public interface ICmFileService : IGenericService<TblCmFile, FileDto>
    {
        #region File Operations
        /// <summary>
        /// Upload multiple files to MinIO
        /// </summary>
        /// <param name="files">List of files to upload</param>
        /// <param name="customPath">Custom folder path (optional)</param>
        /// <returns>List of uploaded file information</returns>
        Task<List<FileDto>> UploadFiles(List<IFormFile> files, string? customFilePath = null);

        /// <summary>
        /// Upload file from byte array
        /// </summary>
        /// <param name="fileBytes">File content as byte array</param>
        /// <param name="fileName">File name</param>
        /// <param name="contentType">MIME content type</param>
        /// <param name="customPath">Custom folder path (optional)</param>
        /// <returns>Uploaded file information</returns>
        Task<FileDto> UploadFileFromBytes(byte[] fileBytes, string fileName, string contentType, string? customFilePath = null);

        /// <summary>
        /// Get file content from MinIO
        /// </summary>
        /// <param name="fileId">File path in MinIO</param>
        /// <returns>Tuple of (file bytes, file name, content type)</returns>
        Task<(byte[], string, string)> GetDocument(string fileId);

        /// <summary>
        /// Generate presigned URL for file access
        /// </summary>
        /// <param name="fileId">File path in MinIO</param>
        /// <param name="expiryInSeconds">URL expiry time in seconds (default: 3600)</param>
        /// <returns>Presigned URL</returns>
        Task<string> GetPresignedUrl(string fileId, int expiryInSeconds = 3600);

        /// <summary>
        /// Delete file from MinIO and mark as deleted in database
        /// </summary>
        /// <param name="fileId">File path in MinIO</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteFile(string fileId);

        /// <summary>
        /// Copy file in MinIO to a new location
        /// </summary>
        /// <param name="sourceFileId">Source file path in MinIO</param>
        /// <param name="customPath">Custom destination path (optional)</param>
        /// <returns>Copied file information</returns>
        Task<FileDto> CopyFile(string sourceFileId, string? customFilePath = null);
        #endregion


        //hàm lưu voice
        Task<FileDto> UploadVoiceFile(IFormFile file, string meetingId);
        Task<FileDto> UploadFileSummary(IFormFile file, string meetingId);


        //hàm tải file
        Task<(byte[], string, string)> DownloadDirectFromMinio(string fileId);

        #region Image Operations
        /// <summary>
        /// Upload multiple images with validation
        /// </summary>
        /// <param name="images">List of image files</param>
        /// <returns>List of uploaded image information</returns>
        Task<List<FileDto>> UploadImages(List<IFormFile> images);

        /// <summary>
        /// Download image by ID or path
        /// </summary>
        /// <param name="fileId">Image file ID or path</param>
        /// <returns>Tuple of (image bytes, file name)</returns>
        Task<(byte[], string)> DownloadImage(string fileId);

        /// <summary>
        /// Get direct URL for image display
        /// </summary>
        /// <param name="fileId">Image file ID or path</param>
        /// <returns>Direct image URL</returns>
        Task<string> GetImageUrl(string fileId);
        #endregion
    }

    public class CmFileService : GenericService<TblCmFile, FileDto>, ICmFileService
    {
        private readonly IConfiguration _configuration;
        private readonly IMinioClient _minioClient;

        public CmFileService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, IMinioClient minioClient)
            : base(dbContext, mapper)
        {
            _configuration = configuration;
            _minioClient = minioClient;
        }

        public async Task<List<FileDto>> UploadFiles(List<IFormFile> files, string? customFilePath = null)
        {
            try
            {
                var results = new List<FileDto>();
                var bucket = _configuration["Minio:BucketName"];

                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
                }

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0) continue;

                    var fileId = Guid.NewGuid().ToString();
                    var now = DateTime.Now;
                    var folderFilePath = customFilePath ?? $"{now:yyyy_MM_dd}";
                    var ext = Path.GetExtension(file.FileName);
                    var objectName = $"{customFilePath}/{fileId}{ext}";

                    using (var stream = file.OpenReadStream())
                    {
                        var putObjectArgs = new PutObjectArgs()
                            .WithBucket(bucket)
                            .WithObject(objectName)
                            .WithStreamData(stream)
                            .WithObjectSize(stream.Length)
                            .WithContentType(file.ContentType);

                        await _minioClient.PutObjectAsync(putObjectArgs);
                    }

                    // Save to database
                    var fileRecord = new TblCmFile
                    {
                        Id = fileId,
                        FileName = file.FileName,
                        FileType = file.ContentType,
                        FileSize = file.Length,
                        FilePath = objectName,
                        IsAllowDelete = true,
                        CreateDate = DateTime.Now,
                        IsDeleted = false
                    };

                    await _dbContext.TblCmFile.AddAsync(fileRecord);
                    results.Add(_mapper.Map<FileDto>(fileRecord));
                }

                await _dbContext.SaveChangesAsync();
                return results;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return new List<FileDto>();
            }
        }

        public async Task<FileDto> UploadFileFromBytes(byte[] fileBytes, string fileName, string contentType, string? customFilePath = null)
        {
            try
            {
                var bucket = _configuration["Minio:BucketName"];
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
                }

                var fileId = Guid.NewGuid().ToString();
                var now = DateTime.Now;
                var folderFilePath = customFilePath ?? $"{now:yyyy_MM_dd}";
                var ext = Path.GetExtension(fileName);
                var objectName = $"{folderFilePath}/{fileId}{ext}";

                using (var stream = new MemoryStream(fileBytes))
                {
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucket)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(stream.Length)
                        .WithContentType(contentType);

                    await _minioClient.PutObjectAsync(putObjectArgs);
                }

                var fileRecord = new TblCmFile
                {
                    Id = fileId,
                    FileName = fileName,
                    FileType = contentType,
                    FileSize = fileBytes.Length,
                    FilePath = objectName,
                    IsAllowDelete = true,
                    CreateDate = DateTime.Now,
                    IsDeleted = false
                };

                await _dbContext.TblCmFile.AddAsync(fileRecord);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<FileDto>(fileRecord);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        //public async Task<FileDto> UploadVoiceFile(byte[] fileBytes, string fileName, string contentType, string? customFilePath = null, string meetingId)
        public async Task<FileDto> UploadVoiceFile(IFormFile file, string meetingId)
        {
            try
            {
                var bucket = _configuration["Minio:BucketName"];
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
                }

                var fileId = Guid.NewGuid().ToString();
                var now = DateTime.Now;
                var fileName = $"{meetingId}_{now :yyyyMMdd_HHmmss}";
                var folderFilePath = $"voice";
                var ext = Path.GetExtension(file.FileName);
                var objectName = $"{folderFilePath}/{fileName}{ext}";
                var contentType = file.ContentType;


                using (var stream = file.OpenReadStream())
                {
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucket)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(stream.Length)
                        .WithContentType(contentType);

                    await _minioClient.PutObjectAsync(putObjectArgs);
                }


                var fileRecord = new TblCmFile
                {
                    Id = fileId,
                    FileName = fileName,
                    RefrenceFileId = meetingId,
                    FileType = ext.Replace(".", ""),
                    FileSize = file.Length,
                    FilePath = objectName,
                    IsAllowDelete = true,
                    CreateDate = DateTime.Now,
                    IsDeleted = false
                };

                await _dbContext.TblCmFile.AddAsync(fileRecord);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<FileDto>(fileRecord);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<FileDto> UploadFileSummary(IFormFile file, string meetingId)
        {
            try
            {
                var bucket = _configuration["Minio:BucketName"];
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
                }

                var fileId = Guid.NewGuid().ToString();
                var now = DateTime.Now;
                var ext = Path.GetExtension(file.FileName);
                var fileName = $"{meetingId}_{now:yyyyMMdd_HHmmss}{ext}";
                var objectName = $"{fileName}";
                var contentType = file.ContentType;


                using (var stream = file.OpenReadStream())
                {
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucket)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(stream.Length)
                        .WithContentType(contentType);

                    await _minioClient.PutObjectAsync(putObjectArgs);
                }


                var fileRecord = new TblCmFile
                {
                    Id = fileId,
                    FileName = fileName,
                    RefrenceFileId = meetingId,
                    FileType = ext.Replace(".", ""),
                    FileSize = file.Length,
                    FilePath = objectName,
                    IsAllowDelete = true,
                    CreateDate = DateTime.Now,
                    IsDeleted = false
                };

                await _dbContext.TblCmFile.AddAsync(fileRecord);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<FileDto>(fileRecord);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }


        public async Task<(byte[], string, string)> GetDocument(string fileId)
        {
            try
            {
                var bucketName = _configuration["Minio:BucketName"];

                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileId);
                var stat = await _minioClient.StatObjectAsync(statObjectArgs);

                var memoryStream = new MemoryStream();
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileId)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream));

                await _minioClient.GetObjectAsync(getObjectArgs);
                memoryStream.Position = 0;

                var fileDb = await _dbContext.TblCmFile.FirstOrDefaultAsync(x => x.FilePath == fileId);
                return (memoryStream.ToArray(), fileDb?.FileName ?? stat.ObjectName, stat.ContentType);
            }
            catch (ObjectNotFoundException)
            {
                Status = false;
                Exception = new ArgumentException("File không tồn tại");
                return (null, null, null);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return (null, null, null);
            }
        }

        public async Task<string> GetPresignedUrl(string fileId, int expiryInSeconds = 3600)
        {
            try
            {
                var bucketName = _configuration["Minio:BucketName"];
                var presignedUrlArgs = new PresignedGetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileId)
                    .WithExpiry(expiryInSeconds);

                return await _minioClient.PresignedGetObjectAsync(presignedUrlArgs);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public async Task<bool> DeleteFile(string fileId)
        {
            try
            {
                var bucketName = _configuration["Minio:BucketName"];

                // Delete from MinIO
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileId);
                await _minioClient.RemoveObjectAsync(removeObjectArgs);

                // Mark as deleted in database
                var fileDb = await _dbContext.TblCmFile.FirstOrDefaultAsync(x => x.FilePath == fileId);
                if (fileDb != null)
                {
                    fileDb.IsDeleted = true;
                    await _dbContext.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return false;
            }
        }

        //public async Task<Stream> GetFileStreamAsync(string objectName)
        //{
        //    var memoryStream = new MemoryStream();
        //    var bucketName = _configuration["Minio:BucketName"];

        //    try
        //    {
        //        await _minioClient.GetObjectAsync(new GetObjectArgs()
        //            .WithBucket(bucketName)
        //            .WithObject(objectName)
        //            .WithCallbackStream(stream => stream.CopyTo(memoryStream))
        //        );

        //        memoryStream.Position = 0; // reset stream về đầu
        //        return memoryStream;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log lỗi và ném lại hoặc xử lý tùy ý
        //        throw new Exception($"Lỗi khi tải file từ MinIO: {ex.Message}", ex);
        //    }
        //}

        public async Task<FileDto> CopyFile(string sourceFileId, string? customFilePath = null)
        {
            try
            {
                var bucketName = _configuration["Minio:BucketName"];

                // Get source file info
                var sourceFile = await _dbContext.TblCmFile.FirstOrDefaultAsync(x => x.FilePath == sourceFileId);
                if (sourceFile == null) return null;

                // Generate new file path
                var fileId = Guid.NewGuid().ToString();
                var now = DateTime.Now;
                var folderFilePath = customFilePath ?? $"{now:yyyy_MM_dd}";
                var ext = Path.GetExtension(sourceFile.FileName);
                var objectName = $"{folderFilePath}/{fileId}{ext}";

                // Copy file in MinIO
                var copySourceObjectArgs = new CopySourceObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(sourceFileId);

                var copyObjectArgs = new CopyObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithCopyObjectSource(copySourceObjectArgs);

                await _minioClient.CopyObjectAsync(copyObjectArgs);

                // Save new record to database
                var newFileRecord = new TblCmFile
                {
                    Id = fileId,
                    FileName = sourceFile.FileName,
                    FileType = sourceFile.FileType,
                    FileSize = sourceFile.FileSize,
                    FilePath = objectName,
                    IsAllowDelete = true,
                    CreateDate = DateTime.Now,
                    IsDeleted = false
                };

                await _dbContext.TblCmFile.AddAsync(newFileRecord);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<FileDto>(newFileRecord);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        #region Image Methods

        /// <summary>
        /// Upload danh sách ảnh lên MinIO
        /// </summary>
        public async Task<List<FileDto>> UploadImages(List<IFormFile> images)
        {
            try
            {
                var results = new List<FileDto>();
                var bucket = _configuration["Minio:BucketName"];
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };

                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
                }

                foreach (var image in images)
                {
                    if (image == null || image.Length == 0) continue;

                    // Validate image file
                    if (!allowedTypes.Contains(image.ContentType.ToLower())) continue;

                    var fileId = Guid.NewGuid().ToString();
                    var now = DateTime.Now;
                    var folderFilePath = $"images/{now:yyyy_MM_dd}";
                    var ext = Path.GetExtension(image.FileName);
                    var objectName = $"{folderFilePath}/{fileId}{ext}";

                    using (var stream = image.OpenReadStream())
                    {
                        var putObjectArgs = new PutObjectArgs()
                            .WithBucket(bucket)
                            .WithObject(objectName)
                            .WithStreamData(stream)
                            .WithObjectSize(stream.Length)
                            .WithContentType(image.ContentType);

                        await _minioClient.PutObjectAsync(putObjectArgs);
                    }

                    var fileRecord = new TblCmFile
                    {
                        Id = fileId,
                        FileName = image.FileName,
                        FileType = image.ContentType,
                        FileSize = image.Length,
                        FilePath = objectName,
                        IsAllowDelete = true,
                        CreateDate = DateTime.Now,
                        IsDeleted = false
                    };

                    await _dbContext.TblCmFile.AddAsync(fileRecord);
                    results.Add(_mapper.Map<FileDto>(fileRecord));
                }

                await _dbContext.SaveChangesAsync();
                return results;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return new List<FileDto>();
            }
        }

        /// <summary>
        /// Download ảnh về dạng byte array
        /// </summary>
        public async Task<(byte[], string)> DownloadImage(string fileId)
        {
            try
            {
                var bucketName = _configuration["Minio:BucketName"];

                var fileDb = await _dbContext.TblCmFile.FirstOrDefaultAsync(x => x.Id == fileId || x.FilePath == fileId);
                if (fileDb == null) return (null, null);

                var memoryStream = new MemoryStream();
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileDb.FilePath)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream));

                await _minioClient.GetObjectAsync(getObjectArgs);
                memoryStream.Position = 0;

                return (memoryStream.ToArray(), fileDb.FileName);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return (null, null);
            }
        }

        /// <summary>
        /// Lấy URL trực tiếp để hiển thị ảnh
        /// </summary>
        public async Task<string> GetImageUrl(string fileId)
        {
            try
            {
                var fileDb = await _dbContext.TblCmFile.FirstOrDefaultAsync(x => x.Id == fileId || x.FilePath == fileId);
                if (fileDb == null) return null;

                var bucketName = _configuration["Minio:BucketName"];
                var endpoint = _configuration["Minio:Endpoint"];
                var port = _configuration["Minio:Port"];
                var useSSL = bool.Parse(_configuration["Minio:UseSSL"] ?? "false");

                var protocol = useSSL ? "https" : "http";
                var baseUrl = port != "80" && port != "443"
                    ? $"{protocol}://{endpoint}:{port}"
                    : $"{protocol}://{endpoint}";

                return $"{baseUrl}/{bucketName}/{fileDb.FilePath}";
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Tải file trực tiếp từ MinIO
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<(byte[], string, string)> DownloadDirectFromMinio(string fileId)
        {
            try
            {
                var bucketName = _configuration["Minio:BucketName"];
                var objectName = fileId;

                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);
                var stat = await _minioClient.StatObjectAsync(statObjectArgs);

                var memoryStream = new MemoryStream();
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream));

                await _minioClient.GetObjectAsync(getObjectArgs);

                memoryStream.Position = 0;

                var fileDb = _dbContext.TblCmFile.FirstOrDefault(x => x.FilePath == fileId);

                return (memoryStream.ToArray(), fileDb == null ? stat.ObjectName : fileDb.FileName, stat.ContentType);
            }
            catch (ObjectNotFoundException)
            {
                Status = false;
                Exception = new ArgumentException("File không tồn tại trên MinIO");
                return (null, null, null);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return (null, null, null);
            }
        }
    }
}