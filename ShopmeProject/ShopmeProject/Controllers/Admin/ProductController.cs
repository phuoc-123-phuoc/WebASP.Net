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
    [JwtAuthentication("Salesperson", "Editor", "Admin")]
    public class ProductController : BaseController
    {
        private ProductService productService;
        private BrandsService brandsService;
        private CategoryService categoryService;
        public ProductController()
        {
            productService = new ProductService();
            brandsService = new BrandsService();
            categoryService = new CategoryService();
        }
        // GET: Product
        public ActionResult Index()
        {
            return ListByPage(1, "",0);
            //IEnumerable<Product> products = productService.ListAll();
            //return View(products);
        }

        public ActionResult NewProduct()
        {
            IEnumerable<Brand> brands = brandsService.ListAll();
            Product product = new Product();
            ProductViewModel productViewModel = new ProductViewModel()
            {
                brands = brands,
                product =product
            };
            return View(productViewModel);
        }

        public ActionResult Save(Product product,
            HttpPostedFileBase fileImage,
            HttpPostedFileBase[] extraImage,
            string[] detailIDs,
            string[] detailNames,
            string[] detailValues,
            string[] imageIDs,
            string[] imageNames)
        {


            int IdImg;
            if (product.Id == 0)
            {
                IdImg = productService.GetMaxProductId() + 1;

            }
            else
            {
                IdImg = product.Id;
            }


            string uploadDir = Server.MapPath("/product-images/" + IdImg);

            ProductSaveHelper.setMainImageName(fileImage, product);
            ProductSaveHelper.setExistingExtraImageNames(imageIDs, imageNames, product);
            ProductSaveHelper.setNewExtraImageNames(extraImage, product);
            ProductSaveHelper.saveUploadedImages(fileImage, extraImage, uploadDir);
            ProductSaveHelper.setProductDetails(detailNames, detailValues, product,detailIDs);
            ProductSaveHelper.deleteExtraImagesWeredRemovedOnForm(product, uploadDir);
            productService.Save(product);
          
            TempData["Message"] = "The product has been saved successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CheckDuplicateName(int id, string name)
        {
            
            return Content(productService.checkUnique(id, name));
        }

        [HttpGet]
        public ActionResult SetEnabled(int id, bool enabled)
        {

            productService.updateProductEnabledStatus(id, enabled);
            string status = enabled ? "enabled" : "disabled";
            string message = $"The product ID {id} has been {status}";

            TempData["message"] = message;

            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (productService.CountProductsById(id) > 0)
            {
                productService.delete(id);
                TempData["message"] = "The product ID " + id + " has been deleted successfully";
            }
            else
            {
                TempData["message"] = "Could not find any product with ID " + id;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            IEnumerable<Brand> brands = brandsService.ListAll();
            Product product = productService.Get(id);
            if (product == null)
            {
                TempData["Message"] = "Could not find any product with ID " + id;
                return RedirectToAction("Index");
            }

            ViewBag.PageTitle = "Edit Product (ID: " + id + ")";

            ProductViewModel productViewModel = new ProductViewModel()
            {
                brands = brands,
                product = product
            };

            return View("NewProduct", productViewModel);
        }

        public ActionResult Detail(int id)
        {
            Product product = productService.Get(id);
            return PartialView("product_detail_modal",product);
        }

        public ActionResult ListByPage(int pageNum, string keyword, int categoryId = 0)
        {
            // IEnumerable<User> listUsers = userService.ListAll();
            IEnumerable<Product> products = productService.listByPage(pageNum, keyword, categoryId);
            var ListCategories = categoryService.GetAllCategories();
            long startCount = (pageNum - 1) * productService.productPerPage() + 1;
            long endCount = startCount + productService.productPerPage() - 1;
            if (endCount > productService.totalRecordProduct(keyword, categoryId))
            {
                endCount = productService.totalRecordProduct(keyword, categoryId);
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            if(categoryId != 0)
            {
                ViewBag.categoryId = categoryId;
            }
            ViewBag.CurrentPage = pageNum;
            ViewBag.TotalPage = productService.totalPageProduct(keyword, categoryId);
            ViewBag.StartCount = startCount;
            ViewBag.EndCount = endCount;
            ViewBag.TotalItems = productService.totalRecordProduct(keyword, categoryId);
            ViewBag.KeyWord = keyword;
            ViewBag.ListCategories = ListCategories;
            return View("Index", products);
        }


    }
}