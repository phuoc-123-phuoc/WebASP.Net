using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class ProductService
    {
        public const int PRODUCT_PER_PAGE = 10;
        public const int SEARCH_RESULTS_PER_PAGE = 10;
       

        private MyDbContext _context;

        public ProductService()
        {
            _context = new MyDbContext();
        }

        public IEnumerable<Product> listByPage(int pageNum, string keyword, int categoryId)
        {

            IEnumerable<Product> query;

            if (categoryId != 0 && !string.IsNullOrEmpty(keyword))
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

        public Product getProduct(string alias)
        {
            return _context.Products.FirstOrDefault(x => x.Alias == alias);
        }

        public IEnumerable<Product> search(int pageNum,string searchString)
        {
            
            searchString = searchString.Replace(" ", " OR ");
            IEnumerable<Product> results = _context.Products
                .SqlQuery("SELECT * FROM Products WHERE CONTAINS((Name, ShortDescription, FullDescription), {0})", searchString).ToList()
                .Skip((pageNum - 1) * SEARCH_RESULTS_PER_PAGE)
                   .Take(SEARCH_RESULTS_PER_PAGE)
                   ;

            return results;
        }

        public int producSearchtPerPage()
        {
            return SEARCH_RESULTS_PER_PAGE;
        }

        public int totalSearchRecordProduct(string searchString)
        {
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(searchString))
            {

                searchString = searchString.Replace(" ", " OR ");
                totalRecords = _context.Products
                .SqlQuery("SELECT * FROM Products WHERE CONTAINS((Name, ShortDescription, FullDescription), {0})", searchString).ToList()
                   .Count()
                   ;
            }
            

            return totalRecords;
        }

        public int totalSearchPageProduct(string searchString)
        {
            int totalRecords = totalSearchRecordProduct(searchString); // get the total number of records in the table
            int totalPages = (int)Math.Ceiling((double)totalRecords / PRODUCT_PER_PAGE); // calculate the total number of pages
            return totalPages;
        }
    }
}