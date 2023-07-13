using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.ViewModels
{
    public class BrandViewModel
    {
        public Brand Brand { get; set; }
        public List<Category> ListCategories { get; set; }
    }
}