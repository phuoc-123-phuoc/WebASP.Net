using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class ProductService
    {
        public const int PRODUCT_PER_PAGE = 4;
        private MyDbContext _context;


        public ProductService()
        {
            _context = new MyDbContext();
        }

        public IEnumerable<Product> ListAll()
        {
            return _context.Products.Include("Brand").Include("Category");
        }

        public void Save(Product product)
        {
            if (product.Id == 0)
            {
                product.CreatedTime = DateTime.Now;
            }
            if (string.IsNullOrEmpty(product.Alias))
            {
                string defaultAlias = product.Name.Replace(" ", "-");
                product.Alias = defaultAlias;
            }
            else
            {
                product.Alias = product.Alias.Replace(" ", "-");
            }
            
            product.UpdatedTime = DateTime.Now;

            if(product.Id == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                Product productInDb = _context.Products.Find(product.Id);
                MapValue(productInDb,product);
               
              
            }
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void MapValue(Product productInDb, Product product)
        {
            productInDb.Name = product.Name;
            productInDb.Alias = product.Alias;
            productInDb.ShortDescription = product.ShortDescription;
            productInDb.FullDescription = product.FullDescription;
           
            productInDb.UpdatedTime = product.UpdatedTime;
            productInDb.Enabled = product.Enabled;
            productInDb.InStock = product.InStock;
            productInDb.Cost = product.Cost;
            productInDb.Price = product.Price;
            productInDb.DiscountPercent = product.DiscountPercent;
            productInDb.Length = product.Length;
            productInDb.Width = product.Width;
            productInDb.Weight = product.Weight;
            productInDb.Height = product.Height;
            productInDb.ReviewCount = product.ReviewCount;
            productInDb.AverageRating = product.AverageRating;
            productInDb.MainImage = product.MainImage ?? productInDb.MainImage;
            productInDb.CategoryId = product.CategoryId;
            productInDb.BrandId = product.BrandId;
            MapImageValue(productInDb, product);
            MapDetailsValue(productInDb, product);
        }
        public void MapDetailsValue(Product productInDb, Product product)
        {
            //Them and update
            foreach (var item in product.Details)
            {
                if (item.Id == 0)
                {
                    productInDb.Details.Add(item);
                }
                else if (productInDb.Details.Any(x => x.Id == item.Id))
                {
                    var existingDetails = productInDb.Details.FirstOrDefault(i => i.Id == item.Id);
                    if (existingDetails != null)
                    {
                        existingDetails.Name = item.Name;
                        existingDetails.Value = item.Value;
                    }
                }

            }
            //Delete
            // Select all details IDs from the collection
            var detailsIds = product.Details.Select(i => i.Id).ToArray();
            // Select images to delete that are not in the given array of IDs
            var detailsToDelete = productInDb.Details.Where(i => !detailsIds.Contains(i.Id));
            _context.ProductDetail.RemoveRange(detailsToDelete);
        }
        public void MapImageValue(Product productInDb, Product product)
        {
            //Them and update
           foreach(var item in product.Images){
                if(item.Id ==0)
                {
                    productInDb.Images.Add(item);
                }else if(productInDb.Images.Any(x => x.Id == item.Id))
                {
                    var existingImage = productInDb.Images.FirstOrDefault(i => i.Id == item.Id);
                    if (existingImage != null)
                    {
                        existingImage.Name = item.Name;
                    }
                }
                
            }
            //Delete
            // Select all image IDs from the collection
            var imageIds = product.Images.Select(i => i.Id).ToArray();
            // Select images to delete that are not in the given array of IDs
            var imagesToDelete = productInDb.Images.Where(i => !imageIds.Contains(i.Id));
           _context.ProductImage.RemoveRange(imagesToDelete);
        }

        public string checkUnique(int id, string name)
        {
            bool isCreatingNew = id == 0;
            Product productByName = _context.Set<Product>().SingleOrDefault(u => u.Name == name);
            if (isCreatingNew)
            {
                if (productByName != null) return "Duplicated";
            }
            else
            {
                if(productByName != null && productByName.Id != id)
                {
                    return "Duplicated";
                }
            }
            return "OK";
        }

        public void updateProductEnabledStatus(int id, bool enabled)
        {
            var existingProduct = _context.Products.Find(id);
            if (existingProduct != null)
            {
                existingProduct.Enabled = enabled;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);
                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
        }

        public void delete(int id)
        {
            Product productToDelete = _context.Products.Find(id);
            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                _context.SaveChanges();
            }

        }

        public int CountProductsById(int id)
        {
            return _context.Products.Count(u => u.Id == id);
        }

        public int GetMaxProductId()
        {
            int maxId = _context.Products.Max(c => c.Id);
            return maxId;
        }

        public Product Get(int id)
        {
            return _context.Products.Find(id);
        }

        public IEnumerable<Product> listByPage(int pageNum, string keyword, int categoryId)
        {

            IEnumerable<Product> query;

            if(categoryId !=0 && !string.IsNullOrEmpty(keyword))
            {
                query = _context.Products
                    .OrderBy(x => x.Id)
                   .Where(x =>
                    x.Name.Contains(keyword) &&
                    (x.CategoryId == categoryId ||
                    x.Category.allParentIDs.Contains(categoryId.ToString())))
                   .Skip((pageNum - 1) * PRODUCT_PER_PAGE)
                   .Take(PRODUCT_PER_PAGE)
                   ;
            }
            else if (categoryId != 0)
            {
                query = _context.Products
                    .OrderBy(x => x.Id)
                   .Where(x =>
                    x.CategoryId == categoryId ||
                    x.Category.allParentIDs.Contains(categoryId.ToString()))
                   .Skip((pageNum - 1) * PRODUCT_PER_PAGE)
                   .Take(PRODUCT_PER_PAGE)
                   ;
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                query = _context.Products
                    .OrderBy(x => x.Id)
                   .Where(x =>
                    x.Name.Contains(keyword))
                   .Skip((pageNum - 1) * PRODUCT_PER_PAGE)
                   .Take(PRODUCT_PER_PAGE)
                   ;
            }
            else
            {
               query = _context.Products
              .OrderBy(x => x.Id)
              .Skip((pageNum - 1) * PRODUCT_PER_PAGE)
              .Take(PRODUCT_PER_PAGE);
            }

            var records = query.ToList();
            return records;
        }

        public int productPerPage()
        {
            return PRODUCT_PER_PAGE;
        }

        public int totalRecordProduct(string keyword, int categoryId)
        {
            int totalRecords;

            if (categoryId != 0 && !string.IsNullOrEmpty(keyword))
            {
                totalRecords = _context.Products
                  
                   .Where(x =>
                    x.Name.Contains(keyword) &&
                    (x.CategoryId == categoryId ||
                    x.Category.allParentIDs.Contains(categoryId.ToString())))
                   .Count()
                   ;
            }
            else if (categoryId != 0)
            {
                totalRecords = _context.Products
                   
                   .Where(x =>
                    x.CategoryId == categoryId ||
                    x.Category.allParentIDs.Contains(categoryId.ToString()))
                   .Count()
                   ;
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                totalRecords = _context.Products
                   
                   .Where(x =>
                    x.Name.Contains(keyword))
                   .Count()
                   ;
            }
            else
            {
                totalRecords = _context.Products
              .Count();
            }

            return totalRecords;
        }

        public int totalPageProduct(string keyword, int categoryId)
        {
            int totalRecords = totalRecordProduct(keyword, categoryId); // get the total number of records in the table
            int totalPages = (int)Math.Ceiling((double)totalRecords / PRODUCT_PER_PAGE); // calculate the total number of pages
            return totalPages;
        }

    }
}