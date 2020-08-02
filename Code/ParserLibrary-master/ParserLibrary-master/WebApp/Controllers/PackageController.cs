using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ParserLibrary.Core;
using ParserLibrary.Core.GooglePlay;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PackageController : Controller
    {
        private ApplicationContext db;
        public PackageController(ApplicationContext context)
        {
            db = context;
        }

        // GET: PackageController
        public async Task<IActionResult> Index()
        {
            return View(await db.Packages.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Package package)
        {
            if (db.Packages.Where(p=>p.Name.Equals(package.Name)).Count()>0) {
                return Redirect("/");
            }

            {
                HttpClient client = new HttpClient();
                var responseMessage = await client.GetAsync("https://play.google.com/store/apps/details?id=" + package.Name);

                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Response.StatusCode = 404;
                    return Content("Package Name не найден.");
                }
            }

            db.Packages.Add(package);
            await db.SaveChangesAsync();

            await Task.Run(()=> {
                GooglePlayReviewsSettings settings = new GooglePlayReviewsSettings(package.Name);
                ParserWorker<string[][]> parserWorker = new ParserWorker<string[][]>(new GooglePlayReviewsParser(), settings);

                parserWorker.OnNewData += ParserWorker_OnNewData;

                parserWorker.Start();
            });

            return RedirectToAction("Index");
        }

        private void ParserWorker_OnNewData(object arg1, string[][] comments)
        {
            var worker = arg1 as ParserWorker<string[][]>;

            var package = db.Packages.Where(p => p.Name.Equals(worker.Settings.PackageName)).First();

            if (package == null) throw new Exception();

            foreach (var item in comments)
            {
                db.Reviews.Add(new PackageReview()
                {
                    Package = package,
                    Comment = Regex.Unescape(item[0]).Replace('"', '\''),
                    Timestamp = long.Parse(item[1]),
                    GpUrl = item[2],
                    Category = db.Categories.Where(c => c.Identificator < 0).First()
                });
            }

            db.SaveChanges();
        }
    }
}
