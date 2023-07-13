using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ShopmeProject.ServiceSite
{
    public class CategoryService
    {
        private MyDbContext _context;

        public CategoryService()
        {
            _context = new MyDbContext();
        }

        public IEnumerable<Category> listNoChildrenCategories()
        {
           
            IEnumerable<Category> listNoChildrenCategories = _context.Categories.Where(x =>
                    x.Enabled == true ||
                    x.ParentId == null);


            return listNoChildrenCategories;
        }

        public Category getCategory(string alias)
        {
            return _context.Categories.FirstOrDefault(x => x.Alias == alias);
        }

        public IEnumerable<Category> getCategoryParents(Category child)
        {
            List<Category> listParents = new List<Category>();
            Category parent = child.Parent;
            listParents.Add(child);
            while (parent != null)
            {
                listParents.Add(parent);
                parent = parent.Parent;
               
            }


            listParents.Reverse();

            return listParents;
        }
    }
}