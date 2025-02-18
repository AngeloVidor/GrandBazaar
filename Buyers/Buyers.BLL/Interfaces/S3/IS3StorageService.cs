using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Buyers.BLL.Interfaces.S3
{
    public interface IS3StorageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}