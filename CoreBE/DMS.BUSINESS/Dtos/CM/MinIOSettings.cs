using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Dtos.CM
{
    public class MinioSettings
    {
        public string Endpoint { get; set; }
        public int Port { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public bool UseSSL { get; set; }
        public string BucketName { get; set; }
        public bool Public { get; set; }
    }

}
