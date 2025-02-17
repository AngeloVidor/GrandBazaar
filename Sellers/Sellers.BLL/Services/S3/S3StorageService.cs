using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sellers.BLL.Interfaces.S3;
using Sellers.BLL.Services.S3.Settings;

namespace Sellers.BLL.Services.S3
{
    public class S3StorageService : IS3StorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AWSSettings _awsSettings;

        public S3StorageService(IAmazonS3 s3Client, IOptions<AWSSettings> awsSettings)
        {
            _s3Client = s3Client;
            _awsSettings = awsSettings.Value;
        }

        public async Task<string> UploadImagAsync(IFormFile file)
        {
            if (_awsSettings.SecretKey == null || string.IsNullOrEmpty(_awsSettings.SecretKey))
            {
                Console.WriteLine("SecretKey's null or empty");
            }

            Console.WriteLine(_awsSettings.SecretKey);

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Image file is empty");
            }

            var fileTransferUtility = new TransferUtility(_s3Client);

            using (var stream = file.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = $"{Guid.NewGuid()}_{file.FileName}",
                    BucketName = _awsSettings.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };
                await fileTransferUtility.UploadAsync(uploadRequest);
                return $"https://{_awsSettings.BucketName}.s3.us-east-1.amazonaws.com/{uploadRequest.Key}";
            }
        }
    }
}