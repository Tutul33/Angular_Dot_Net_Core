using BusinessLayer.Management.Person;
using DataModel.ViewModel;
using Domains.IRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace InfoAPI.Controllers
{
    [Route("api/[controller]"), Produces("application/json"), EnableCors("AppPolicy")]
    [ApiController]
    public class PersonalInfoController : ControllerBase
    {
        IPersonalInfo personMgt = null;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public PersonalInfoController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            personMgt = new PersonMgt();
        }
        
        [HttpDelete("[action]")]
        public async Task<object> Delete([FromQuery] string param)
        {
            object result = null; object resdata = null;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                vmPersonalInfo info = JsonConvert.DeserializeObject<vmPersonalInfo>(data[0].ToString());
                string fileSavePath = _hostingEnvironment.WebRootPath;
                resdata = await personMgt.Delete(info.UserId, fileSavePath);


            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result = new
            {
                resdata
            };
        }
        [HttpGet("[action]")]
        public async Task<object> GetList([FromQuery] string param)
        {
            dynamic data = JsonConvert.DeserializeObject(param);
            vmCommon cmnParam = JsonConvert.DeserializeObject<vmCommon>(data[0].ToString());
            return await personMgt.GetList(cmnParam);
        }
        [HttpGet("[action]")]
        public async Task<object> GetCountryAndCity([FromQuery] string param)
        {            
            return await personMgt.GetCountryAndCity();
        }
        [HttpGet("[action]")]
        public async Task<object> GetSkills([FromQuery] string param)
        {
            return await personMgt.GetSkills();
        }
        [HttpGet("{id:int}")]
        public async Task<object> GetById(int id)
        {
            vmCommon cmnParam = new vmCommon();
            cmnParam.id = id;
            return await personMgt.GetList(cmnParam);
        }
        [HttpPost("[action]")]
        public async Task<object> Save()
        {
            object resdata = null;
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var ReqFiles = formCollection.Files;
                var listAttachment = new List<vmAttachment>();
                foreach (var attachedFile in ReqFiles)
                {
                    string serverPathAttachmentFile = string.Empty;
                    if (attachedFile != null)
                    {
                        if (attachedFile.Length > 0)
                        {
                            string fileName = ContentDispositionHeaderValue.Parse(attachedFile.ContentDisposition).FileName.Trim('"');

                            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            var arrayExtens = fileName.Split(".");
                            var exten = arrayExtens[arrayExtens.Length - 1];
                            fileName = fileName.Substring(0, fileName.Length - (exten.Length + 1)) + "_" + newFileName + "." + exten;

                            string base64String = "";
                            using (var ms = new MemoryStream())
                            {
                                attachedFile.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                base64String = Convert.ToBase64String(fileBytes);
                                // act on the Base64 data
                            }
                            var objAttachment = new vmAttachment();
                            objAttachment.FileName = fileName;
                            objAttachment.FileData = base64String;
                            objAttachment.Extension = exten;
                            objAttachment.attachmentFile = "";
                            listAttachment.Add(objAttachment);
                        }
                    }
                }
                var data = Request.Form["personalInfo"].ToString();
                vmPersonalInfo info = JsonConvert.DeserializeObject<vmPersonalInfo>(data.ToString());
                info.WebRootPath = _hostingEnvironment.WebRootPath;
                info.attachments = listAttachment;
                resdata = await personMgt.Save(info);
                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return new { resdata };
        }
        [HttpPut("[action]")]
        public async Task<object> Update()
        {
            object resdata = null;
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var ReqFiles = formCollection.Files;
                var listAttachment = new List<vmAttachment>();
                foreach (var attachedFile in ReqFiles)
                {
                    string serverPathAttachmentFile = string.Empty;
                    if (attachedFile != null)
                    {
                        if (attachedFile.Length > 0)
                        {
                            string fileName = ContentDispositionHeaderValue.Parse(attachedFile.ContentDisposition).FileName.Trim('"');

                            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            var arrayExtens = fileName.Split(".");
                            var exten = arrayExtens[arrayExtens.Length - 1];
                            fileName = fileName.Substring(0, fileName.Length - (exten.Length + 1)) + "_" + newFileName + "." + exten;

                            string base64String = "";
                            using (var ms = new MemoryStream())
                            {
                                attachedFile.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                base64String = Convert.ToBase64String(fileBytes);
                                // act on the Base64 data
                            }
                            var objAttachment = new vmAttachment();
                            objAttachment.FileName = fileName;
                            objAttachment.FileData = base64String;
                            objAttachment.Extension = exten;
                            objAttachment.attachmentFile = "";
                            listAttachment.Add(objAttachment);
                        }
                    }
                }
                var data = Request.Form["personalInfo"].ToString();
                vmPersonalInfo info = JsonConvert.DeserializeObject<vmPersonalInfo>(data.ToString());
                info.WebRootPath = _hostingEnvironment.WebRootPath;
                info.attachments = listAttachment;
                resdata = await personMgt.Update(info);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return new { resdata };
        }
    }
}
