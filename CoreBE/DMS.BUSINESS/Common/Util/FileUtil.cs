namespace DMS.BUSINESS.Common.Util
{
    public class FileUtil
    {
        private static IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
                { ".3g2", "VIDEO" },
                { ".3gp", "VIDEO" },
                { ".3gp2", "VIDEO" },
                { ".3gpp", "VIDEO" },
                { ".asf", "VIDEO" },
                { ".m2t", "VIDEO" },
                { ".m2ts", "VIDEO" },
                { ".mkv", "VIDEO" },
                { ".mod", "VIDEO" },
                { ".movie", "VIDEO" },
                { ".mp2", "VIDEO" },
                { ".mp2v", "VIDEO" },
                { ".mp4v", "VIDEO" },
                { ".mpa", "VIDEO" },
                { ".mpe", "VIDEO" },
                { ".mpeg", "VIDEO" },
                { ".mpg", "VIDEO" },
                { ".mts", "VIDEO" },
                { ".mxf", "VIDEO" },
                { ".ogv", "VIDEO" },
                { ".rm", "VIDEO" },
                { ".swf", "VIDEO" },
                { ".ts", "VIDEO" },
                { ".vob", "VIDEO" },
                { ".webm", "VIDEO" },
                { ".wm", "VIDEO" },
                { ".wmv", "VIDEO" },
                { ".wmx", "VIDEO" },
                { ".xvid", "VIDEO" },
                { ".doc", "DOCUMENT" },
                { ".docx", "DOCUMENT" },
                { ".ppt", "PRESENTATION" },
                { ".pptx", "PRESENTATION" },
                { ".xls", "DOCUMENT" },
                { ".xlsx", "DOCUMENT" },
                { ".jpg", "IMAGE" },
                { ".jpeg", "IMAGE" },
                { ".png", "IMAGE" },
                { ".gif", "IMAGE" },
                { ".bmp", "IMAGE" },
                { ".pdf", "DOCUMENT" },
                { ".txt", "TEXT" },
                { ".dot", "DOCUMENT" },
                { ".dotx", "DOCUMENT" },
                { ".xlsm", "DOCUMENT" },
                { ".xlsb", "DOCUMENT" },
                { ".pptm", "PRESENTATION" },
                { ".pot", "PRESENTATION" },
                { ".potx", "PRESENTATION" },
                { ".potm", "PRESENTATION" },
                { ".ppa", "PRESENTATION" },
                { ".ppam", "PRESENTATION" },
                { ".pps", "PRESENTATION" },
                { ".ppsx", "PRESENTATION" },
                { ".ppsm", "PRESENTATION" },
                { ".sldx", "PRESENTATION" },
                { ".sldm", "PRESENTATION" },
                { ".docm", "DOCUMENT" },
                { ".dotm", "DOCUMENT" },
                { ".accdb", "DATABASE" },
                { ".accde", "DATABASE" },
                { ".accdt", "DATABASE" },
                { ".accdr", "DATABASE" },
                { ".odb", "DATABASE" },
                { ".avi", "VIDEO" },
                { ".flv", "VIDEO" },
                { ".h264", "VIDEO" },
                { ".m4v", "VIDEO" },
                { ".mov", "VIDEO" },
                { ".mp4", "VIDEO" },
                { ".aac", "AUDIO" },
                { ".aiff", "AUDIO" },
                { ".flac", "AUDIO" },
                { ".m4a", "AUDIO" },
                { ".mp3", "AUDIO" },
                { ".ogg", "AUDIO" },
                { ".wav", "AUDIO" },
                { ".wma", "AUDIO" },
            };

        public static string GetFileType(string extension)
        {
            try
            {
                //if (_mappings.TryGetValue(extension, out string fileType))
                //{
                //    return fileType;
                //}
                //return "UNKNOWN";

                return extension;
            }catch(Exception)
            {
                return "UNKNOWN";
            }
        }
    };

    
}

