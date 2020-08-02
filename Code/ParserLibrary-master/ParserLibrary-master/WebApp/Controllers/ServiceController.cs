using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ServiceController : Controller
    {
        private ApplicationContext db;
        private static List<string> _postData = new List<string>();
        public ServiceController(ApplicationContext context)
        {
            db = context;
        }

        public string Test()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(",review,score");
            int i = 0;
            foreach (var item in db.Reviews.Where(r => r.Category.Equals(null)))
            {
                result.AppendLine(i + ",\"" + item.Comment + "\",1");
                i++;
            }
            return result.ToString();
        }

        public IActionResult Index()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("review");
            foreach (var item in db.Reviews.Where(r => r.Positive<0))
            {
                result.AppendLine(item.Comment);
            }
            return Content(result.ToString());
        }

        public IActionResult Debug()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("Принятые данные " + result.Length + " :");
            int i = 0;
            foreach (var item in _postData)
            {
                result.AppendLine(i + "\t" + item);
                result.AppendLine();
                i++;
            }

            return Content(result.ToString());
        }

        //[HttpPost]
        //public IActionResult Index(string data)
        //{
        //    _postData.Add(data);
        //    return Content(data.Length.ToString());
        //}

        [HttpPost]
        public IActionResult File(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                _postData.Add("Принят файл " + uploadedFile.FileName);
                using (StreamReader reader = new StreamReader(uploadedFile.OpenReadStream()))
                {
                    using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        // указываем используемый разделитель
                        csvReader.Configuration.Delimiter = ",";
                        csvReader.Configuration.HeaderValidated = null;
                        // получаем строки
                        try
                        {
                            IEnumerable<CsvFile> result =
                                csvReader.GetRecords<CsvFile>();


                            List<CsvFile> comments = new List<CsvFile>(result);

                            foreach (var item in comments)
                            {
                                var review = db.Reviews.Where(r => r.Comment.Equals(item.review)).FirstOrDefault();
                                if (review != null && (review.Positive != item.score || review.Category ==null || review.Category.Identificator != item.clas))
                                {
                                    review.Positive = item.score;

                                    if (db.Categories.Any(c => c.Identificator.Equals(item.clas))) {
                                        review.Category = db.Categories.Where(c=>c.Identificator.Equals(item.clas)).First();
                                    }
                                }
                                
                            }

                            db.SaveChanges();

                            return Content("ok");
                        }
                        catch
                        {
                            return Content("Incorrect file");
                        }
                    }
                }
            }

            return Content("not ok");
        }
    }
}
