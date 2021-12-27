using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.StaticFiles;

namespace FileUploadApi.Controllers
{
    
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        [HttpPost]
        [Route("Upload File")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            await WriteFile(file);
            return Ok();
        }
        private async Task WriteFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            string fileName = Path.GetRandomFileName();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files", fileName + extension);
            var stream = new FileStream(path, FileMode.Create);
            {
                await file.CopyToAsync(stream);
            }
        }   


        [HttpGet("Download File")]
            public async Task <ActionResult> DownloadFile(string FileName)
            {
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files", FileName);
                var provider = new FileExtensionContentTypeProvider();
                if(!provider.TryGetContentType(filepath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }
                var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
                return File(bytes, contentType, Path.GetFileName(filepath));
            }
        }

        
    }