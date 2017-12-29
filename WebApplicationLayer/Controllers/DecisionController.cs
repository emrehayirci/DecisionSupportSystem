using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using DecisionLayer;

namespace WebApplicationLayer.Controllers
{
    public class DecisionController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Calculate(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = path + Path.GetFileName(postedFile.FileName);
                postedFile.SaveAs(filePath);

                var results = DecisionMaking.Decide(filePath);

                ViewBag.Message = "Upload Success!";
                ViewBag.Results = results;
                return View();
            }
            else
            {
                throw new Exception("No File");
            }
        }
    }
}