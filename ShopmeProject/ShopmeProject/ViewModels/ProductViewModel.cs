using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.ViewModels
{
    public class ProductViewModel
    {
        public Product product { get; set; }
        public IEnumerable<Category> category { get; set; }
        public IEnumerable<Brand> brands { get; set; }
    }
}