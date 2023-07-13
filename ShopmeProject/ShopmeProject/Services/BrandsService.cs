using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class BrandsService
    {
        public const int BRAND_PER_PAGE = 4;

        private MyDbContext _context;

        public BrandsService()
        {
            _context = new MyDbContext();
        }
        //public IEnumerable<Brand> ListAll()
        //{
        //    return _context.Brands
        //        .Select(b => new Brand { Id = b.Id, Name = b.Name });
        //}

        public IEnumerable<Brand> ListAll()
        {
            return _context.Brands
         .Include("Categories");
        }

        public bool isNameUnique(int id, string name)
        {
            Brand brandByName = _context.Set<Brand>().SingleOrDefault(u => u.Name == name);
            if (brandByName == null) return true;
            bool isCreatingNew;
            if (id == 0)
            {
                isCreatingNew = true;
            }
            else
            {
                isCreatingNew = false;
            }


            if (isCreatingNew)
            {
                if (brandByName != null) return false;
            }
            else
            {
                if (brandByName.Id != id) return false;
            }

            return true;
        }

        public void save(Brand brand, int[] categoriesId)
        {
          
            var existingBrand = _context.Brands.Find(brand.Id);

            if (existingBrand != null)
            {
                // Update existing brand
                existingBrand.Name = brand.Name;
                existingBrand.Categories.Clear();
              
                if (brand.Logo != null)
                {
                    existingBrand.Logo = brand.Logo;
                   
                }
                foreach (var id in categoriesId)
                {
                    var categoryInDatabase = _context.Categories.Find(id);
                    existingBrand.Categories.Add(categoryInDatabase);
                    
                }
            }
            else
            {
                // Create new brand
                foreach (var id in categoriesId)
                {
                    var categoryInDatabase = _context.Categories.Find(id);
                    brand.Categories.Add(categoryInDatabase);

                }

              
                _context.Brands.Add(brand);
            }

            _context.SaveChanges();
        }

        public int GetMaxBrandId()
        {
            int maxId = _context.Brands.Max(c => c.Id);
            return maxId;
        }

        public Brand Get(int id)
        {
            return _context.Brands.Find(id);
        }

        public int CountBrandById(int id)
        {
            return _context.Brands.Count(u => u.Id == id);
        }

        public void delete(int id)
        {
            Brand brandToDelete = _context.Brands.Find(id);
            if (brandToDelete != null)
            {
                _context.Brands.Remove(brandToDelete);
                _context.SaveChanges();
            }

        }

        public IEnumerable<Brand> listByPage(int pageNum, string keyword)
        {
            var query =_context.Brands
           .OrderBy(x => x.Id)
           .Skip((pageNum - 1) * BRAND_PER_PAGE)
           .Take(BRAND_PER_PAGE);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = _context.Brands
                    .OrderBy(x => x.Id)
                   .Where(x =>
                    x.Name.Contains(keyword))
                   .Skip((pageNum - 1) * BRAND_PER_PAGE)
                   .Take(BRAND_PER_PAGE)
                   ;
            }

            var records = query.ToList();
            return records;
        }

        public int brandPerPage()
        {
            return BRAND_PER_PAGE;
        }

        public int totalRecordBrand(string keyword)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(keyword))
            {
                totalRecords = _context.Brands
                    .Where(x =>
                     x.Name.Contains(keyword)).Count();
            }
            else
            {
                totalRecords = _context.Brands.Count();
            }
            return totalRecords;
        }

        public int totalPageBrand(string keyword)
        {
            int totalRecords = totalRecordBrand(keyword); // get the total number of records in the table
            int totalPages = (int)Math.Ceiling((double)totalRecords / BRAND_PER_PAGE); // calculate the total number of pages
            return totalPages;
        }


    }
}