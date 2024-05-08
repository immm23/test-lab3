using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers
{
    [ApiController]
    [Route("api/images/")]
    public class ImagesController : ControllerBase
    {
        private readonly string _path;

        public ImagesController()
        {
            _path = Path.Combine(Directory.GetCurrentDirectory(), "images");

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }

        [HttpGet]
        [Route("{path}/get/")]
        public IActionResult GetImage([FromRoute]string path)
        {
            string filePath = Path.Combine(_path, path + ".png");

            if (!System.IO.File.Exists(filePath))
            {
                return StatusCode(404);
            }

            int amountOfTries = 10;
            int deleyOnRetry = 500;

            for (int i = 0; i < amountOfTries; i++)
            {
                try
                {
                    var stream = System.IO.File.Open(filePath, FileMode.Open);
                    {
                        return File(stream, "application/octet-stream");
                    }
                }
                catch when (i <= amountOfTries)
                {
                    Thread.Sleep(deleyOnRetry);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        [Route("{path}/new/")]
        public async Task<IActionResult> CreateImage([FromRoute]string path, [FromForm]IFormFile file)
        {
            if(file.Length <= 0)
            {
                return BadRequest("File is empty");
            }

            string filePath = Path.Combine(_path, path + ".png");
            await using (Stream fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                await file.CopyToAsync(fileStream);
            }
            return CreatedAtAction(nameof(CreateImage), file);
            
        }

        [HttpPut]
        [Authorize]
        [Route("{path}/update/")]
        public async Task<IActionResult> UpdateImage([FromRoute]string path, [FromForm]IFormFile file)
        {
            string filePath = Path.Combine(_path, path + ".png");

            if(file.Length <= 0 || System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            await using (Stream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(fileStream);
            }
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("{path}/delete")]
        public ActionResult RemoveImage([FromRoute] string path)
        {
            string filePath = Path.Combine(_path, path.Replace(" ", "") + ".png");

            if (System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

       
            System.IO.File.Delete(filePath);
            return Ok();
        }
    }
}
