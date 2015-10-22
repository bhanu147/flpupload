using flpupload.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace flpupload.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Save(string fileString)
        {
            var fileDto = new FileDto();
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            //this filedto will contain all the details of the file like name,original file name, size, type etc
            //use this value to insert into database
            fileDto = serializer.Deserialize<FileDto>(fileString);
            string filebase64string = fileDto.File.Substring(fileDto.File.IndexOf(',') + 1);
            byte[] content = Convert.FromBase64String(filebase64string);
            // this will create the file and save into the directory
            //Replace the C:\ with the file location they had mentioned
            using (FileStream stream = new FileStream(@"C:\" + fileDto.OriginalName, FileMode.Create, FileAccess.ReadWrite))
            {
                stream.Write(content, 0, content.Length);
                stream.Close();
            }
            var result = new JsonResult();
            result.MaxJsonLength = Int32.MaxValue;
            result.Data = fileDto;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}
