using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.ViewModels
{
    public class CategoryViewModel
    {
        public Category category { get; set; }
        public List<Category> ListCategories { get; set; }

    }
}