using E7Store.Models;
using E7Store.Models.Data;
using E7Store.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E7Store.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            // declare the list of CategoryVM
            List<CategoryVM> categoryVMList;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Init the list
                categoryVMList = db.Categories
                                .ToArray()
                                .OrderBy(x => x.Sorting)
                                .Select(x => new CategoryVM(x))
                                .ToList();
            }
            return View(categoryVMList);
        }
        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string categoryName)
        {
            // Declare the id
            string id;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // check if category name is unique
                if (db.Categories.Any(x => x.Name == categoryName))
                    return "titletaken";

                // init data
                var data = new Category();

                // add to data
                data.Name = categoryName;
                data.Slug = categoryName.Replace(" ", "-").ToLower();
                data.Sorting = 100;

                db.Categories.Add(data);
                // save data
                db.SaveChanges();

                //get the id
                id = data.Id.ToString();
            }

            // return the id
            return id;
        }

        // POST: Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // set initial count
                int count = 1;

                // declare Category
                Category data;

                // set sorting for each category
                foreach (var categoryId in id)
                {
                    data = db.Categories.Find(categoryId);
                    data.Sorting = count;

                    db.SaveChanges();
                    count++;
                }
            }
        }

        // DELETE: Admin/Shop/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // get the page
                Category data = db.Categories.Find(id);

                // remove the page
                db.Categories.Remove(data);

                // Save
                db.SaveChanges();
            }

            // Redirect
            return RedirectToAction("Categories");
        }

        // POST: Admin/Shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCategoryName, int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // check if category name is unique
                if (db.Categories.Any(x => x.Name == newCategoryName))
                {
                    return "titletaken";
                }

                // get the data
                Category data = db.Categories.Find(id);

                // edit data
                data.Name = newCategoryName;
                data.Slug = newCategoryName.Replace(" ", "-").ToLower();

                // save changes
                db.SaveChanges();
            }

            // return 
            return "ok";
        }

    }
}

