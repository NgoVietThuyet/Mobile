using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Dtos.CM
{
    public class OnlyOfficeCallbackModel
    {
        public int? status { get; set; }
        public string? url { get; set; }
        public string? key { get; set; }
        public string? changesurl { get; set; }
        public string? filetype { get; set; }
        public string? lastsave { get; set; }
        public HistoryInfo? history { get; set; }
        public List<string>? users { get; set; }
        public List<ActionInfo>? actions { get; set; }
        public string[]? actionslog { get; set; }
        public string? error { get; set; }
        public string? token { get; set; } 
    }

    public class ActionInfo
    {
        public int? type { get; set; }
        public string? userid { get; set; }
    }

    public class HistoryInfo
    {
        public object? changes { get; set; }
        public string? serverVersion { get; set; }
    }
}
