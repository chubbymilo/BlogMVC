using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Data.FileManager
{
    public class FileManager:IFileManager
    {
        private string _imagePath;
        public FileManager(IConfiguration config)
        {
            _imagePath = config["Path:Images"];
        }
        public async Task<string> SaveImage(IFormFile image)

        {
            if (image == null)
            {
                return null;
            }
            var path_save = Path.Combine(_imagePath);
            if (!Directory.Exists(path_save))
            {
                Directory.CreateDirectory(path_save);
            }
            var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
            var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mime}";

            using (var fileStream = new FileStream(Path.Combine(path_save, fileName), FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return fileName;
        }

        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
        }

        public void RemoveImage(string fileName)
        {
            try
            {
                if (fileName != null)
                {
                    File.Delete(Path.Combine(_imagePath,fileName));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
