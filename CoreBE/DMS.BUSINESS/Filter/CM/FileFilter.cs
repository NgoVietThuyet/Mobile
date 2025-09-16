using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DMS.BUSINESS.Filter.CM
{
    public class FileFilter : BaseFilter
    {
        public string? Id { get; set; }
        public string? RefrenceFileId { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
    }
}
