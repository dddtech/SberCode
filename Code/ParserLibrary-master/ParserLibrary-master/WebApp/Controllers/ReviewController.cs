using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ReviewController : Controller
    {
        private ApplicationContext db;
        public ReviewController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Comments(string packName,int categoryId)
        {
            if (String.IsNullOrWhiteSpace(packName))
            {
                return Redirect("/package");
            }

            Package package = db.Packages.Where(p => p.Name.Equals(packName)).FirstOrDefault();

            if (package == null)
            {
                return Redirect("/package");
            }

            //Category category = (categoryId == Category.Base.Identificator) ? Category.Base : db.Categories.Where(c=>c.Identificator.Equals(categoryId)).FirstOrDefault();
            Category category = db.Categories.Where(c => c.Identificator.Equals(categoryId)).FirstOrDefault();

            if (category == null) {
                return Redirect("/package");
            }

            PackageReview[] reviews;
            //if (!categoryId.Equals(Category.Base.Identificator)) {
            reviews = db.Reviews.Where(r => r.Package.Id.Equals(package.Id) && r.Category.Identificator.Equals(categoryId)).ToArray();
            //}
            //else {
            //    reviews = db.Reviews.Where(r => r.Package.Id.Equals(package.Id) && r.Category == null).ToArray();
            //}

            return View(reviews);
        }

        public IActionResult Index(string packName)
        {
            if (String.IsNullOrWhiteSpace(packName)) {
                return Redirect("/package");
            }

            Package package = db.Packages.Where(p => p.Name.Equals(packName)).FirstOrDefault();

            if (package == null) {
                return Redirect("/package");
            }

            Dictionary<int,CategoryCount> result = new Dictionary<int, CategoryCount>();
            //result.Add(Category.Base.Identificator, new CategoryCount() { Category=Category.Base,Count=0,Package=package});
            foreach (var item in db.Categories)
            {
                result.Add(item.Identificator,new CategoryCount() { Category=item,Count=0,Package=package});
            }

            foreach (var item in db.Reviews.Where(r => r.Package.Id.Equals(package.Id)))
            {
                var category =  item.Category;
                result[category.Identificator].Count+=1;
            }
            return View(result.Values.ToArray());
        }
    }
}
