using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Casino.Application.Abstraction
{
    public interface IFileUploadService
    {
        Task<string> FileUploadAsync(IFormFile fileToUpload, string folderNameOnServer);
    }
}