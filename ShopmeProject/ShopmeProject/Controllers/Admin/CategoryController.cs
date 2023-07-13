using ShopmeProject.Controllers.Admin;
using ShopmeProject.Models;
using ShopmeProject.Services;
using ShopmeProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers
{
    

       [JwtAuthentication("Editor", "Admin")]
    public class CategoryController : BaseController
    {
        private CategoryService categoryService;

        public CategoryController()
        {
            categoryService = new CategoryService();
        }
        // GET: Category
        public ActionResult Index()
        {
            return ListByPage(1, "");
        }
        public ActionResult NewCategory()
        {
            var category = new Category();

            var ListCategories = categoryService.GetAllCategories();
            var categoryViewModel = new CategoryViewModel()
            {
                category = category,
                ListCategories = ListCategories
            };
            ViewBag.PageTitle = "Create New Category";
            return View(categoryViewModel);
        }
        [HttpPost]
        public ActionResult saveCategory(Category category, int ParentId, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                int IdImg;
                if (category.Id == 0)
                {
                    IdImg = categoryService.GetMaxCategoryId() + 1;
                }
                else
                {
                    IdImg = category.Id;
                }
               
                var fileName = Path.GetFileName(image.FileName);
                string uploadDir = Server.MapPath("/category-images/" + IdImg);
                
                category.Image = fileName;
                FileUploadUtil.CleanDir(uploadDir);
                FileUploadUtil.SaveFile(uploadDir, fileName, image);

            }
            categoryService.save(category, ParentId);
            TempData["Message"] = "The user has been saved successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CheckDuplicateName(int id, string name)
        {
            if (categoryService.isNameUnique(id, name) == true)
            {
                return Content("OK");
            }
            else
            {
                return Content("Duplicated");
            }
        }

        [HttpPost]
        public ActionResult CheckDuplicateAlias(int id, string Alias)
        {
            if (categoryService.isAliasUnique(id, Alias) == true)
            {
                return Content("OK");
            }
            else
            {
                return Content("Duplicated");
            }
        }

        public ActionResult Edit(int id)
        {
            Category category = categoryService.Get(id);

            if (category == null)
            {
                TempData["Message"] = "Could not find any category with ID " + id;
                return RedirectToAction("Index");
            }

            ViewBag.PageTitle = "Edit Category (ID: " + id + ")";

            var ListCategories = categoryService.GetAllCategories();
            category.Name = category.Name.Replace("-", "");
            var categoryViewModel = new CategoryViewModel()
            {
                category = category,
                ListCategories = ListCategories
            };


            return View("NewCategory",categoryViewModel);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (categoryService.CountCategoryById(id) > 0)
            {
                categoryService.delete(id);
                TempData["message"] = "The category ID " + id + " has been deleted successfully";
            }
            else
            {
                TempData["message"] = "Could not find any category with ID " + id;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SetEnabled(int id, bool enabled)
        {

            categoryService.updateCategoryEnabledStatus(id, enabled);
            string status = enabled ? "enabled" : "disabled";
            string message = $"The category ID {id} has been {status}";

            TempData["message"] = message;

            return RedirectToAction("Index", "Category");
        }

        public ActionResult ListByPage(int pageNum, string keyword)
        {
            // IEnumerable<User> listUsers = userService.ListAll();
            IEnumerable<Category> categories = categoryService.listByPage(pageNum, keyword);

            long startCount = (pageNum - 1) * categoryService.categoryPerPage() + 1;
            long endCount = startCount + categoryService.categoryPerPage() - 1;
            if (endCount > categoryService.totalRecordCategory(keyword))
            {
                endCount = categoryService.totalRecordCategory(keyword);
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            ViewBag.CurrentPage = pageNum;
            ViewBag.TotalPage = categoryService.totalPageCategory(keyword);
            ViewBag.StartCount = startCount;
            ViewBag.EndCount = endCount;
            ViewBag.TotalItems = categoryService.totalRecordCategory(keyword);
            ViewBag.KeyWord = keyword;
            return View("Index", categories);
        }
    }
}