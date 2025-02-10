using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.Interfaces.Image
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string uploadFolder = "uploads");
    }
}
