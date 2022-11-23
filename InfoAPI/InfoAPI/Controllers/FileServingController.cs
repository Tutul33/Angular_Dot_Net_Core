using BusinessLayer.Management.Person;
using Domains.IRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoAPI.Controllers
{
    [Produces("application/json"), EnableCors("AppPolicy")]
    [ApiController]
    public class FileServingController : ControllerBase
    {
        private FileServingController _tktMgt = null;
        IPersonalInfo personMgt = null;
        private IWebHostEnvironment _hostingEnvironment;
        public FileServingController(IWebHostEnvironment hostingEnvironment)
        {            
            _hostingEnvironment = hostingEnvironment;
            personMgt = new PersonMgt();
        }
        [Route("api/FileLoader/{fileName}")]
        public async Task<object> file(string fileName)
        {
            byte[] file = null; string returnType = "";
            try
            {
                string fileSavePath = _hostingEnvironment.WebRootPath;
              
                if (!string.IsNullOrEmpty(fileName))
                {
                    var arrayExtens = fileName.Split(".");
                    string extension = arrayExtens[arrayExtens.Length - 1];
                    extension = extension.ToLower();
                    switch (extension)
                    {
                        case "jpg":
                            returnType = "image/jpg";
                            break;
                        case "jpeg":
                            returnType = "image/jpeg";
                            break;
                        case "png":
                            returnType = "image/png";
                            break;
                        case "gif":
                            returnType = "image/gif";
                            break;
                        case "doc":
                            returnType = "application/msword";
                            break;
                        case "pdf":
                            returnType = "application/pdf";
                            break;
                        case "docx":
                            returnType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            break;
                        case "txt":
                            returnType = "text/plain";
                            break;
                        case "csv":
                            returnType = "application/octet-stream";
                            break;
                        case "xls":
                            returnType = "application/vnd.ms-excel";
                            break;
                        case "xlsx":
                            returnType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            break;
                        case "ppt":
                            returnType = "application/vnd.ms-powerpoint";
                            break;
                        case "pptx":
                            returnType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                            break;
                        default:
                            returnType = "";
                            break;
                    }

                    file = await personMgt.getFile(fileSavePath, fileName);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return File(file, returnType);
        }
    }
}
