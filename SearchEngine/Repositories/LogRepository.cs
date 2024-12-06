using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SearchEngine.Data;
using SearchEngine.Models;
using System;
using System.IO;
using System.Web;

namespace SearchEngine.Repositories
{
    public class LogRepository : ILogRepository
    {

        private readonly IWebHostEnvironment _hostingEnvironment;

        public LogRepository(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task WriteToLog(string txt)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, @"logs\");
            if (!System.IO.Directory.Exists(path)) {
                System.IO.Directory.CreateDirectory(path);
            }

            string fullFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "logs", DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt");
            string hour = DateTime.Now.ToString("HH:mm");
            if (!File.Exists(fullFilePath))
            {
                File.Create(fullFilePath).Dispose();
            }

            
            using (TextWriter tw = new StreamWriter(fullFilePath, true))
            {
                await tw.WriteLineAsync(hour + ": " + txt);
            }
            
        }
    }
}
