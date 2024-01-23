using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NLog.Web.LayoutRenderers;
using System.Reflection.Metadata.Ecma335;

namespace RestaurantAPI2.Controllers
{
    [Route("file")]
    //[Authorize]
    public class FileController : ControllerBase
    {
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "FileName" })]
        public ActionResult GetFile([FromQuery]string FileName)
        {
            string rootPath = Directory.GetCurrentDirectory();

            var filePath = $"{rootPath}/PrivateFiles/{FileName}";

            //czy plik istnieje
            var fileExists = System.IO.File.Exists(filePath);
            if (!fileExists)
            {
                return NotFound();
            }

            var contentType = new FileExtensionContentTypeProvider();
            contentType.TryGetContentType(filePath, out var mimeType);

            var fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, mimeType,FileName);
        }

        [HttpPost]
        public ActionResult Upload([FromForm]IFormFile file)
        {
            if (file != null && file.Length > 0) 
            {
                var rootPath = Directory.GetCurrentDirectory();
                var filePath = $"{rootPath}/PrivateFiles/{file.FileName}";
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok();
            }
            else 
            {
                return BadRequest();
            }
        }
    }
}
