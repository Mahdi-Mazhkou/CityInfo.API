﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private FileExtensionContentTypeProvider fileExtensionContentType;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentType)
        {
            this.fileExtensionContentType = fileExtensionContentType;
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var pathToFile = "webapiBanner.rar";
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            var bytes= System.IO.File.ReadAllBytes(pathToFile);

            if (!fileExtensionContentType.TryGetContentType(pathToFile, out var  contentType))
            {
                contentType = "application/octet-stream";
            }
            return File(bytes,contentType,Path.GetFileName(pathToFile));
        }
    }
}
