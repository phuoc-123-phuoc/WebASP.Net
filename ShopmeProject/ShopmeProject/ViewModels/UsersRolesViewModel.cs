using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.ViewModels
{
    public class UsersRolesViewModel
    {
        public User user { get; set; }
        public List<Role> Roles { get; set; }
    }
}