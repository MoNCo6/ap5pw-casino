using Casino.Application.Abstraction;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Application.Implementation
{
    // Service class to handle file uploads
    public class FileUploadService : IFileUploadService
    {
        // Property to store the root path where files will be saved
        public string RootPath { get; set; }

        // Constructor to set the root path for file storage
        public FileUploadService(string rootPath)
        {
            this.RootPath = rootPath;
        }

        // Method to upload a file asynchronously
        public async Task<string> FileUploadAsync(IFormFile fileToUpload, string folderNameOnServer)
        {
            // Initialize the output file path
            string filePathOutput = String.Empty;

            // Get the file name without the extension
            var fileName = Path.GetFileNameWithoutExtension(fileToUpload.FileName);
            // Get the file extension
            var fileExtension = Path.GetExtension(fileToUpload.FileName);

            // Combine the folder name and the file name to create a relative file path
            var fileRelative = Path.Combine(folderNameOnServer, fileName + fileExtension);
            // Combine the root path and the relative path to create the full file path
            var filePath = Path.Combine(this.RootPath, fileRelative);

            // Create the directory if it doesn't exist
            Directory.CreateDirectory(Path.Combine(this.RootPath, folderNameOnServer));
            // Create a new file stream and copy the uploaded file's content to it
            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                await fileToUpload.CopyToAsync(stream);
            }

            // Set the output file path to the relative path
            filePathOutput = Path.DirectorySeparatorChar + fileRelative;

            // Return the relative file path as the result
            return filePathOutput;
        }
    }
}