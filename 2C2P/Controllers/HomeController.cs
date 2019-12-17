using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2C2P.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new DataTable());
        }

        public ActionResult Clear()
        {
            return View(new DataTable());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload, string btn)
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {

                    if (upload.FileName.EndsWith(".csv"))
                    {
                        Stream stream = upload.InputStream;
                        DataTable csvTable = new DataTable();
                        using (CsvReader csvReader =
                            new CsvReader(new StreamReader(stream), true))
                        {
                            csvTable.Load(csvReader);
                        }
                        return View("Index", csvTable);
                    }
                    else if (upload.FileName.EndsWith(".xml"))
                    {
                        ModelState.AddModelError("File", "Unknown format");
                        ModelState.AddModelError("File", "Unknown format2");
                        ModelState.AddModelError("File", "Unknown format3");
                        ModelState.AddModelError("File", "Unknown format4");
                        ModelState.AddModelError("File", "Unknown format5");
                        return View("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Unknown format");
                        return View("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return View("index");
        }
    }
}
