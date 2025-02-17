using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sellers.BLL.Services.S3.Settings
{
    public class AWSSettings
    {
        public string BucketName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
    }
}