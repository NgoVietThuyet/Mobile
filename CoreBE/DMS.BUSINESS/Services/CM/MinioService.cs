
using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.CM;
using DMS.BUSINESS.Dtos.MD;
using DMS.BUSINESS.Dtos.MT;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace DMS.BUSINESS.Services.CM
{
    public interface IMinioService : IGenericService<TblMtMeeting, MeetingDto>
    {
        Task<List<TblMtMeetingFile>> UploadFile(List<IFormFile> files);
        Task<(byte[], string, string)> GetDocument(string filePath);
        Task<string> GetPresignedUrl(string filePath, int expiryInSeconds = 3600);
        Task<TblMtMeetingFile> UploadFileFromBytes(byte[] fileBytes, string originalFileName, string contentType);
        Task<(byte[], string, string)> DownloadDirectFromMinio(string fileId);
    }

    public class MinioService : GenericService<TblMtMeeting, MeetingDto>, IMinioService
    {

        private readonly IConfiguration _configuration;
        private readonly IMinioClient _minioClient;

        public MinioService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, IMinioClient minioClient)
            : base(dbContext, mapper)
        {
            _configuration = configuration;
            _minioClient = minioClient;
        }
        public async Task<List<TblMtMeetingFile>> UploadFile(List<IFormFile> files)
        {
            try
            {
                var data = new List<TblMtMeetingFile>();

                var bucket = _configuration["Minio:BucketName"];
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
                }

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                    {
                        continue;
                    }

                    var fileId = Guid.NewGuid().ToString();
                    var now = DateTime.Now;
                    var folderPath = $"{now:yyyy_MM_dd}";
                    var ext = Path.GetExtension(file.FileName);
                    var objectName = $"{folderPath}_{fileId}{ext}";
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

                    data.Add(new TblMtMeetingFile
                    {
                        Id = fileId,
                        MeetingId = "",
                        UserName = "",
                        Name = file.FileName,
                        FileName = file.FileName,
                        FileType = ext.Replace(".", ""),
                        FilePath = objectName,
                        IsSharedAll = false,
                    });
                }

                return data;
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return null;
            }
        }


        public async Task<(byte[], string, string)> GetDocument(string filePath)
        {
            try
            {
                var bucketName = _configuration["Minio:BucketName"];
                var objectName = filePath;

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

                var fileDb = _dbContext.TblMtMeetingFile.FirstOrDefault(x => x.FilePath == filePath);

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



        public async Task<string> GetPresignedUrl(string filePath, int expiryInSeconds = 3600)
        {
            try
            {
                var bucketName = _configuration["Minio:BucketName"];
                var presignedUrlArgs = new PresignedGetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filePath)
                    .WithExpiry(expiryInSeconds); // thời gian sống của link

                var url = await _minioClient.PresignedGetObjectAsync(presignedUrlArgs);
                var uri = new Uri(url);
                var relativePath = uri.AbsolutePath.TrimStart('/') + uri.Query;

                return relativePath;
            }
            catch (Exception ex)
            {
                // xử lý lỗi
                return null;
            }
        }

        
        public async Task<TblMtMeetingFile> UploadFileFromBytes(byte[] fileBytes, string originalFileName, string contentType)
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
                var folderPath = $"{now:yyyy_MM_dd}";
                var ext = Path.GetExtension(originalFileName);
                var objectName = $"{folderPath}_{fileId}{ext}";

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

                var fileRecord = new TblMtMeetingFile
                {
                    Id = fileId,
                    MeetingId = "",
                    UserName = "",
                    Name = originalFileName,
                    FileName = originalFileName,
                    FileType = ext.Replace(".", ""),
                    FilePath = objectName,
                    IsSharedAll = false,
                };

                return fileRecord;
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return null;
            }
        }
        
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

                var fileDb = _dbContext.TblMtMeetingFile.FirstOrDefault(x => x.FilePath == fileId);

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
