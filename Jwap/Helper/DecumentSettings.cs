using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;

namespace Jwap.API.Helper
{
    public static class DecumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFiles");
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            // return filePath.Replace('\\' , '/');
            return Path.Combine("http://127.0.0.1:8887/", fileName);
        }

        public static void RemoveFile(string fileName, string FolderName)
        {
            var folderPath = Path.Combine(@Directory.GetCurrentDirectory(), "wwwroot/Files", FolderName);
            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        public static IFormFile ConvertFromJsonStringToAudio(string audio)
        {
            var bytes = Convert.FromBase64String(audio);
            var stream = new MemoryStream(bytes);
            //var contentType = "audio/wav"; // or whatever audio format you're using


            //var memoryStream = new MemoryStream(Encoding.Default.GetBytes(JsonConvert.SerializeObject(audio)));
            var formFile = new FormFile(stream, 0, stream.Length, "audio", "audio.webm");
            return formFile;
        }
    }
}
