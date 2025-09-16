using Common;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DMS.API.Controllers.AD
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/[controller]/[action]")]
    public class LogController : Controller
    {
        [Route("~/Log")]
        public IActionResult Index()
        {
            List<FileLogDto> fileListModel = [];
            string path = Path.Combine(Directory.GetParent(typeof(Program).Assembly.Location).FullName, "Logs");
            var fileList = Directory.EnumerateFiles(path);
            foreach (var file in fileList)
            {
                FileInfo f = new(file);
                FileLogDto fileModel = new()
                {
                    FileName = Path.GetFileName(file),
                    FileAccessed = f.LastAccessTime,
                    FileSizeText = (f.Length < 1024) ?
                         f.Length.ToString() + " B" : f.Length / 1024 + " KB"
                };

                fileListModel.Add(fileModel);
            }
            ViewBag.ListFile = fileListModel;
            return PartialView();
        }

        public IActionResult ViewLog(string id)
        {
            try{
                string path = Path.Combine(Directory.GetParent(typeof(Program).Assembly.Location).FullName, "Logs",id);
                var lstLine = new List<string>();
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using var sr = new StreamReader(fs, Encoding.Default);
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        lstLine.Add(line);
                    }
                }

                ViewBag.Content = lstLine;
                return PartialView();
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        public IActionResult Download(string id)
        {
            try
            {
                string path = Path.Combine(Directory.GetParent(typeof(Program).Assembly.Location).FullName, "Logs",id);
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                MemoryStream ms = new();
                fs.CopyTo(ms);
                var bytes = ms.ToArray();
                ms.Dispose();
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }
    }
}
