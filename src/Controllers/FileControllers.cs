using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database;
using TaskManager.Database.Models;

namespace TaskManager.Controllers
{

    [Route("api/files/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationContext _context;

        public FileController(IWebHostEnvironment environment, ApplicationContext context)
        {
            _environment = environment;
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не был загружен.");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var fileModel = new FileModel
            {
                FileName = file.FileName,
                FilePath = fileName // Сохраняем имя файла (GUID) в базе данных
            };

            _context.FileModels.Add(fileModel);
            await _context.SaveChangesAsync();

            return Ok(fileModel); 
        }

        [HttpGet("download/{id}")]
        public IActionResult DownloadFile(int id)
        {
            var fileModel = _context.FileModels.Find(id);
            if (fileModel == null)
                return NotFound();

            var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileModel.FilePath);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/octet-stream", fileModel.FileName);
        }
    }
}
