using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NzWalks.Service.Blob;

namespace NzWalks.Controllers
{
    [ApiController]
    [Route("api/v1/blob")]
    public class BlobHelper : ControllerBase
    {
        private readonly IBlobService _blobService;

        public BlobHelper(IBlobService blobService)
        {
            _blobService = blobService;
        }

        // [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobUrl = await _blobService.UploadFileAsync(file, fileName);

            return Ok(new { url = blobUrl });
        }

        // optional: delete blob
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFile([FromQuery] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("File name is required.");

            var deleted = await _blobService.DeleteFileAsync(fileName);
            if (!deleted)
                return NotFound("File not found.");

            return Ok(new { Message = "File deleted successfully" });
        }

    }
}
