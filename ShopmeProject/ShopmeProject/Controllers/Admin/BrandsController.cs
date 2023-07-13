using ShopmeProject.Controllers.Admin;
using ShopmeProject.DTO;
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
    public class BrandsController : BaseController
    {
        private CategoryService categoryService;
        private BrandsService brandsService;

        public BrandsController()
        {
            brandsService = new BrandsService();
            categoryService = new CategoryService();
        }
        // GET: Brands
        public ActionResult Index()
        {
            return ListByPage(1, "");
        }

        public ActionResult NewBrands()
        {
            var brand = new Brand();
            var ListCategories = categoryService.GetAllCategories();
            var brandViewModel = new BrandViewModel()
            {
                Brand = brand,
                ListCategories = ListCategories
            };
            ViewBag.PageTitle = "Create New Brand";
            return View(brandViewModel);
        }
        public ActionResult saveBrand(Brand Brand, int[] categoriesId, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                int IdImg;
                if (Brand.Id == 0)
                {
                    IdImg = brandsService.GetMaxBrandId() + 1;
                }
                else
                {
                    IdImg = Brand.Id;
                }

                var fileName = Path.GetFileName(image.FileName);
                string uploadDir = Server.MapPath("/brand-logos/" + IdImg);

                Brand.Logo = fileName;
                FileUploadUtil.CleanDir(uploadDir);
                FileUploadUtil.SaveFile(uploadDir, fileName, image);

            }
            brandsService.save(Brand, categoriesId);
           
            TempData["Message"] = "The brand has been saved successfully.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult CheckDuplicateName(int id, string name)
        {
            if (brandsService.isNameUnique(id, name) == true)
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
           
            Brand brand = brandsService.Get(id);
            if (brand == null)
            {
                TempData["Message"] = "Could not find any brand with ID " + id;
                return RedirectToAction("Index");
            }

            ViewBag.PageTitle = "Edit Brand (ID: " + id + ")";

            var ListCategories = categoryService.GetAllCategories();
            var brandViewModel = new BrandViewModel()
            {
                Brand = brand,
                ListCategories = ListCategories
            };
            int[] categoryIds = brand.Categories.Select(c => c.Id).ToArray();

            ViewBag.CategoriesId = categoryIds; // replace with your actual array of selected category IDs


            return View("NewBrands", brandViewModel);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (brandsService.CountBrandById(id) > 0)
            {
                brandsService.delete(id);
                TempData["message"] = "The brand ID " + id + " has been deleted successfully";
            }
            else
            {
                TempData["message"] = "Could not find any category with ID " + id;
            }
            return RedirectToAction("Index");
        }

        public ActionResult ListByPage(int pageNum, string keyword)
        {
            // IEnumerable<User> listUsers = userService.ListAll();
            IEnumerable<Brand> brands = brandsService.listByPage(pageNum, keyword);

            long startCount = (pageNum - 1) * brandsService.brandPerPage() + 1;
            long endCount = startCount + brandsService.brandPerPage() - 1;
            if (endCount > brandsService.totalRecordBrand(keyword))
            {
                endCount = brandsService.totalRecordBrand(keyword);
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            ViewBag.CurrentPage = pageNum;
            ViewBag.TotalPage = brandsService.totalPageBrand(keyword);
            ViewBag.StartCount = startCount;
            ViewBag.EndCount = endCount;
            ViewBag.TotalItems = brandsService.totalRecordBrand(keyword);
            ViewBag.KeyWord = keyword;
            return View("Index", brands);
        }

        public ActionResult ListCategoriesByBrand(int id)
        {
           
                var brand = brandsService.Get(id);

                var categories = brand.Categories.Select(category => new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                }).ToList();

                return Json(categories, JsonRequestBehavior.AllowGet);
           
        }
    }
}