using ShopmeProject.Models;
using ShopmeProject.ServiceSite;
using ShopmeProject.ServicesSite;
using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Site
{
    public class ProductSiteController : BaseSiteController
    {
        private CategoryService categoryService;
        private ProductService productService;
        private Services.SettingService settingService;
        // GET: HomeSite
        public ProductSiteController()
        {
            settingService = new Services.SettingService();
            categoryService = new CategoryService();
            productService = new ProductService();
        }
        // GET: ProductSite
        public ActionResult viewCategoryFirstPage(string alias)
        {
            Category category = categoryService.getCategory(alias);

           
            IEnumerable<Category> listCategoryParents = categoryService.getCategoryParents(category);
            //foreach(var cat in listCategoryParents)
            //{
            //    Debug.WriteLine(cat.Name);
            //}
            return viewCategoryByPage(1,"",alias);
            
        }

        public ActionResult viewCategoryByPage(int pageNum = 1, string keyword="", string alias = "")
        {
            List<Setting> Settings = settingService.GetSettings();

            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }

            Category category = categoryService.getCategory(alias);

            IEnumerable<Category> listCategoryParents = categoryService.getCategoryParents(category);

            IEnumerable<Product> pageProducts = productService.listByPage(1, keyword, category.Id);
            //foreach(var cat in listCategoryParents)
            //{
            //    Debug.WriteLine(cat.Name);
            //}

            long startCount = (pageNum - 1) * productService.productPerPage() + 1;
            long endCount = startCount + productService.productPerPage() - 1;
            if (endCount > productService.totalRecordProduct(keyword, category.Id))
            {
                endCount = productService.totalRecordProduct(keyword, category.Id);
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
           
            ViewBag.CurrentPage = pageNum;
            ViewBag.TotalPage = productService.totalPageProduct(keyword, category.Id);
            ViewBag.StartCount = startCount;
            ViewBag.EndCount = endCount;
            ViewBag.TotalItems = productService.totalRecordProduct(keyword, category.Id);
            ViewBag.KeyWord = keyword;
            ViewBag.pageProducts = pageProducts;
            ViewBag.category = category;
            return View("products_by_category", listCategoryParents);
        }

        public ActionResult viewProductDetail(string alias = "")
        {
            List<Setting> Settings = settingService.GetSettings();

            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }

            Product product = productService.getProduct(alias);
            IEnumerable<Category> listCategoryParents = categoryService.getCategoryParents(product.Category);
            ViewBag.listCategoryParents = listCategoryParents;
            ViewBag.product = product;
            return View("product_detail");
        }
        public ActionResult searchFirstPage(string searchString)
        {
            return searchByPage(1, searchString);
        }
        public ActionResult searchByPage(int pageNum, string searchString)
        {
            List<Setting> Settings = settingService.GetSettings();

            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }
            IEnumerable<Product> pageProducts = productService.search(pageNum,searchString);

            long startCount = (pageNum - 1) * productService.producSearchtPerPage() + 1;
            long endCount = startCount + productService.producSearchtPerPage() - 1;
            if (endCount > productService.totalSearchRecordProduct(searchString))
            {
                endCount = productService.totalSearchRecordProduct(searchString);
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }

            ViewBag.CurrentPage = pageNum;
            ViewBag.TotalPage = productService.totalSearchPageProduct(searchString);
            ViewBag.StartCount = startCount;
            ViewBag.EndCount = endCount;
            ViewBag.TotalItems = productService.totalSearchRecordProduct(searchString);
            ViewBag.searchString = searchString;
            ViewBag.pageProducts = pageProducts;
          

            return View("search_result");
        }

    }
}