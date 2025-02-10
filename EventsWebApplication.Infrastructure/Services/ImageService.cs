using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace EventsWebApplication.Infrastructure.Services
{
    public class ImageService : IImageService
    {

        private string _rootPath = Directory.GetCurrentDirectory();

        public async Task<string> SaveImageAsync(IFormFile imageFile, string uploadFolder = "uploads")
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null; 
            }
            var uploadsFolder = Path.Combine(_rootPath, uploadFolder);
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Path.Combine(uploadFolder, uniqueFileName); //
        }
    }
}
