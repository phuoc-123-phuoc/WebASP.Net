using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("MyConnectionString")
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImage { get; set; }

        public DbSet<ProductDetail> ProductDetail { get; set; }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Currency> Currencys { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<ShippingRate> ShippingRate { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderTrack> OrderTrack { get; set; }


    }
}