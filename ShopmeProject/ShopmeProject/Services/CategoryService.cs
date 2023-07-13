using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class CategoryService
    {
        public const int CATEGORY_PER_PAGE = 4;

        private MyDbContext _context;

        public CategoryService()
        {
            _context = new MyDbContext();
        }
        public IEnumerable<Category> ListAll()
        {
            return _context.Categories
         .Include("Children");
        }
        public int GetMaxCategoryId()
        {
            int maxId = _context.Categories.Max(c => c.Id);
            return maxId;
        }


        public List<Category> GetAllCategories()
        {
            var categories = _context.Categories.ToList();
            var result = new List<Category>();
            foreach (var category in categories.Where(c => c.Parent == null))
            {
                AddCategoryWithChildren(result, category, 0);
            }
            return result;
        }

        private void AddCategoryWithChildren(List<Category> result, Category category, int level)
        {
            // Add the category to the result list
            category.Name = new string('-', level * 2) + category.Name; // prefix name with "--"
            result.Add(category);

            // Recursively add the children categories
            foreach (var child in category.Children)
            {
                AddCategoryWithChildren(result, child, level + 1);
            }
        }

        public void save(Category category, int ParentId)
        {
            Category parent = _context.Categories.Find(ParentId);
            if(parent != null)
            {
                string allParentIds = parent.allParentIDs == null ? "-" : parent.allParentIDs;
               
                allParentIds += parent.Id.ToString() + "-";
                category.allParentIDs = allParentIds;
            }


            var existingCategory = _context.Categories.Find(category.Id);

            if (existingCategory != null)
            {
                // Update existing user
                existingCategory.Name = category.Name;
                existingCategory.Alias = category.Alias;
                existingCategory.allParentIDs = category.allParentIDs;
                existingCategory.Enabled = category.Enabled;
               
                if (category.Image != "")
                {
                    existingCategory.Image = category.Image;
                }
               

                if(ParentId != 0)
                {
                    existingCategory.ParentId = ParentId;
                }
                else
                {
                    existingCategory.ParentId = null;
                }
            }
            else
            {
                // Create new user

                if (ParentId != 0)
                {
                   category.ParentId = ParentId;
                }
                _context.Categories.Add(category);
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

        public bool isNameUnique(int id, string name)
        {
            Category userByName = _context.Set<Category>().SingleOrDefault(u => u.Name == name);
            if (userByName == null) return true;
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
                if (userByName != null) return false;
            }
            else
            {
                if (userByName.Id != id) return false;
            }

            return true;
        }
        public bool isAliasUnique(int id, string Alias)
        {
            Category userByAlias = _context.Set<Category>().SingleOrDefault(u => u.Alias == Alias);
            if (userByAlias == null) return true;
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
                if (userByAlias != null) return false;
            }
            else
            {
                if (userByAlias.Id != id) return false;
            }

            return true;
        }

        public Category Get(int id)
        {
           return  _context.Categories.Find(id);
        }

        public int CountCategoryById(int id)
        {
            return _context.Categories.Count(u => u.Id == id);
        }

        public void delete(int id)
        {
            Category categoryToDelete = _context.Categories.Find(id);
            if (categoryToDelete != null)
            {
                _context.Categories.Remove(categoryToDelete);
                _context.SaveChanges();
            }

        }


        public void updateCategoryEnabledStatus(int id, bool enabled)
        {
            var existingCategory = _context.Categories.Find(id);
            if (existingCategory != null)
            {
                existingCategory.Enabled = enabled;
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

        public IEnumerable<Category> listByPage(int pageNum, string keyword)
        {
            var query = GetAllCategories()
           .Skip((pageNum - 1) * CATEGORY_PER_PAGE)
           .Take(CATEGORY_PER_PAGE);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = GetAllCategories()
                   .Where(x =>
                    x.Name.Contains(keyword) ||
                    x.Alias.Contains(keyword) )
                   .Skip((pageNum - 1) * CATEGORY_PER_PAGE)
                   .Take(CATEGORY_PER_PAGE)
                   ;
            }

            var records = query.ToList();
            return records;
        }
        public int categoryPerPage()
        {
            return CATEGORY_PER_PAGE;
        }

        public int totalRecordCategory(string keyword)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(keyword))
            {
                totalRecords = GetAllCategories()
                    .Where(x =>
                     x.Name.Contains(keyword) ||
                     x.Alias.Contains(keyword)).Count();
            }
            else
            {
                totalRecords = GetAllCategories().Count();
            }
            return totalRecords;
        }

        public int totalPageCategory(string keyword)
        {
            int totalRecords = totalRecordCategory(keyword); // get the total number of records in the table
            int totalPages = (int)Math.Ceiling((double)totalRecords / CATEGORY_PER_PAGE); // calculate the total number of pages
            return totalPages;
        }
    }
}