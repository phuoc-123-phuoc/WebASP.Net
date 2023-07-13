using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class ShoppingCartService
    {
        private MyDbContext _context;

        public ShoppingCartService()
        {
            _context = new MyDbContext();
        }
        public string addProduct(int productId, int quantity, Customer customer)
        {
            int  updatedQuantity = quantity;
            CartItem cartItem = _context.CartItem
                .FirstOrDefault(x => x.CustomerId == customer.Id
                && x.ProductId == productId);
            if (cartItem != null)
            {
                updatedQuantity = cartItem.Quantity + quantity;
                if (updatedQuantity > 5)
                {
                    return "Could not add more " + quantity + " item(s)"
                            + " because there's already " + cartItem.Quantity + " item(s) "
                            + "in your shopping cart. Maximum allowed quantity is 5.";
                }
                cartItem.Quantity = updatedQuantity;

            }
            else
            {
                cartItem = new CartItem();
                cartItem.ProductId = productId;
                cartItem.CustomerId = customer.Id;
                cartItem.Quantity = updatedQuantity;
                _context.CartItem.Add(cartItem);
            }
            _context.SaveChanges();
                return updatedQuantity.ToString();
        }
        public List<CartItem> listCartItems(Customer customer)
        {
            return _context.CartItem.Where(x => x.CustomerId == customer.Id).ToList();
        }

        public float updateQuantity(int productId, int quantity, Customer customer)
        {

            CartItem cartItem = _context.CartItem
                .FirstOrDefault(x => x.CustomerId == customer.Id
                && x.ProductId == productId);
            cartItem.Quantity = quantity;
            _context.SaveChanges();
            return cartItem.getsubtotal;

        }

        public void removeProduct(int productId, Customer customer)
        {
            CartItem cartItem = _context.CartItem
                .FirstOrDefault(x => x.CustomerId == customer.Id
                && x.ProductId == productId);
            _context.CartItem.Remove(cartItem);
            _context.SaveChanges();
           
        }

        public void deleteByCustomer(Customer customer)
        {
             var  cartItemToDelete = _context.CartItem.Where(x => x.CustomerId == customer.Id);
            _context.CartItem.RemoveRange(cartItemToDelete);
            _context.SaveChanges();
            
        }
    }
}