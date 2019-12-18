using _2C2P.Database;
using LumenWorks.Framework.IO.Csv;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace _2C2P.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();

        public ActionResult Index(List<Transaction> lisdata)
        {
            return View(lisdata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase upload, string btn)
        {
            try
            {
                if (btn == "Upload")
                {
                    if (upload == null)
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }

                    else if(upload.ContentLength > 1000000)
                    {
                        ModelState.AddModelError("File", "File size is max 1 MB");
                    }

                    else if (upload.FileName.EndsWith(".csv"))
                    {
                        Stream stream = upload.InputStream;
                        DataTable csvTable = new DataTable();

                        using (CsvReader csvReader = new CsvReader(new StreamReader(stream), false))
                        {
                            csvTable.Load(csvReader);
                        }

                        return View("Index", GenerateDataCSV(csvTable));
                    }
                    else if (upload.FileName.EndsWith(".xml"))
                    {
                        Stream stream = upload.InputStream;
                        XElement xmlRaw = XElement.Load(stream);
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlRaw.ToString());

                        string data = JsonConvert.SerializeXmlNode(xmlDoc);
                        JObject jsonDat = JObject.Parse(data);

                        return View("Index", GenerateDataXML(jsonDat));
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Unknown format");
                    }

                    return View("Index");
                }
                else
                {
                    return View("index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("File", e.Message);
                return View("Index");
            }                   
        }

        public List<Transaction> GenerateDataCSV(DataTable csvTable)
        {
            List<Transaction> lisdata = new List<Transaction>();

            for (int i = 0; i < csvTable.Rows.Count; i++)
            {
                DataRow dRow = csvTable.Rows[i];

                Transaction data = new Transaction();

                data.TransactionID = dRow["Column1"].ToString();
                data.Amount = Convert.ToDecimal(dRow["Column2"].ToString());
                data.CurrencyCode = dRow["Column3"].ToString();
                data.TransactionDate = DateTime.ParseExact(dRow["Column4"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                data.StatusDisplay = dRow["Column5"].ToString();
                data.UploadDate = DateTime.Now;

                if (data.StatusDisplay == "Approved")
                {
                    data.Status = "A";
                }
                else if (data.StatusDisplay == "Failed")
                {
                    data.Status = "R";
                }
                else
                {
                    data.Status = "D";
                }

                lisdata.Add(data);
            }

            db.Transactions.AddRange(lisdata);
            db.SaveChanges();

            return(lisdata);
        }

        public List<Transaction> GenerateDataXML(JObject jsonDat)
        {
            List<Transaction> lisdata = new List<Transaction>();




            return lisdata;
        }
    }
}
