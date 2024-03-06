using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Database;
using TaskManager.Database.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Controllers
{
    [SwaggerTag("files")]
    [Route("api/files/")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileController(IConfiguration configuration, TaskManagerContext context, IWebHostEnvironment hostingEnvironment)
        {
            _config = configuration;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("upload", Name  = "upload-file")]
        public async Task<ActionResult<FileModel>> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не был загружен.");

            var uploadsFolder = Path.Combine(_config["Files:WebPathRoot"], "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var fileModel = new FileModel()
            {
                FileName = file.FileName,
                FilePath = fileName // Сохраняем имя файла (GUID) в базе данных
            };

            _context.FileModels.Add(fileModel);
            await _context.SaveChangesAsync();

            return Ok(fileModel);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var fileModel = await _context.FileModels.FirstOrDefaultAsync(x => x.Id == id);
            if (fileModel == null)
                return NotFound();

            var uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "Data", "uploads");
            var filePath = Path.Combine(uploadsFolder, fileModel.FilePath);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/octet-stream", fileModel.FileName);
        }
    }
}
