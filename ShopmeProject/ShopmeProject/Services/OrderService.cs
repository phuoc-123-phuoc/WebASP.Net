using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class OrderService
    {
        public const int ORDERS_PER_PAGE = 4;
        private MyDbContext _context;

        public OrderService()
        {
            _context = new MyDbContext();
        }

        public List<Order> listAllOrder()
        {
            return _context.Order.OrderBy(c => c.Id).ToList();
        }

        public Order Get(int id)
        {
            return _context.Order.FirstOrDefault(x => x.Id == id);
        }
       
        public List<Order> listByPage(int pageNum, string keyword)
        {
           
            var query = _context.Order
           .OrderByDescending(x => x.Id)
           .Skip((pageNum - 1) * ORDERS_PER_PAGE)
           .Take(ORDERS_PER_PAGE);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = _context.Order
                    .OrderByDescending(x => x.Id)
                   .Where(x =>
                    x.Id.ToString().Contains(keyword) ||
                    x.Customer.FirstName.Contains(keyword)||
                     x.Customer.LastName.Contains(keyword) ||
                    x.total.ToString().Contains(keyword) ||
                    x.orderTime.ToString().Contains(keyword) ||
                    x.country.Contains(keyword) ||
                     x.city.Contains(keyword) ||
                      x.state.Contains(keyword) ||
                    x.paymentMethod.Contains(keyword) ||
                    x.state.Contains(keyword))
                   .Skip((pageNum - 1) * ORDERS_PER_PAGE)
                   .Take(ORDERS_PER_PAGE)
                   ;
            }

            var records = query.ToList();
            return records;
        }

        public int orderPerPage()
        {
            return ORDERS_PER_PAGE;
        }

        public int totalRecordOrder(string keyword)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(keyword))
            {
                totalRecords = _context.Order
                    .Where(x =>
                    x.Id.ToString().Contains(keyword) ||
                    x.Customer.FirstName.Contains(keyword) ||
                     x.Customer.LastName.Contains(keyword) ||
                    x.total.ToString().Contains(keyword) ||
                    x.orderTime.ToString().Contains(keyword) ||
                    x.country.Contains(keyword) ||
                     x.city.Contains(keyword) ||
                      x.state.Contains(keyword) ||
                    x.paymentMethod.Contains(keyword) ||
                    x.state.Contains(keyword)).Count();
            }
            else
            {
                totalRecords = _context.Order.Count();
            }
            return totalRecords;
        }

        public int totalPageOrder(string keyword)
        {
            int totalRecords = totalRecordOrder(keyword); // get the total number of records in the table
            int totalPages = (int)Math.Ceiling((double)totalRecords / ORDERS_PER_PAGE); // calculate the total number of pages
            return totalPages;
        }

        public int CountOrdersById(int id)
        {
            return _context.Order.Count(u => u.Id == id);
        }

        public void delete(int id)
        {
            Order orderToDelete = _context.Order.Find(id);
            var orderdetails = _context.OrderDetail.Where(x => x.OrderId == orderToDelete.Id);
            if (orderToDelete != null)
            {
                _context.OrderDetail.RemoveRange(orderdetails);
                _context.Order.Remove(orderToDelete);
                _context.SaveChanges();
            }

        }

        public List<Country> listAllCountries()
        {
            return _context.Country.ToList();
        }

        public void MapDetailsValue(int orderId, List<OrderTrack> orderTracks)
        {
            if (orderId == 0) return;
            Order order = _context.Order.Find(orderId);
            //Them and update
            foreach (var item in orderTracks)
            {
                if (item.Id == 0)
                {
                    order.OrderTrack.Add(item);
                    
                }
                else if (order.OrderTrack.Any(x => x.Id == item.Id))
                {
                    var existingOrderTrack = order.OrderTrack.FirstOrDefault(i => i.Id == item.Id);
                   
                    if (existingOrderTrack != null)
                    {
                        existingOrderTrack.Notes = item.Notes;
                        existingOrderTrack.UpdatedTime = item.UpdatedTime;
                        existingOrderTrack.Status = item.Status;
                    }
                }

            }
            //Delete
            // Select all details IDs from the collection
            var orderTrackIds = orderTracks.Select(i => i.Id).ToArray();
            // Select images to delete that are not in the given array of IDs
            var orderTrackToDelete = order.OrderTrack.Where(i => !orderTrackIds.Contains(i.Id));
            _context.OrderTrack.RemoveRange(orderTrackToDelete);
            _context.SaveChanges();
        }

        public void updateStatus(int orderId, string status)
        {
            Order orderInDB = _context.Order.Find(orderId);
            OrderTrack orderTrack = new OrderTrack();
            orderTrack.Status = status;

           

            switch (status)
            {
                case "PICKED":
                    orderTrack.Notes = OrderStatusExtensions.DefaultDescription(ShopmeProject.Models.OrderStatus.PICKED);
                    break;
                case "SHIPPING":
                    orderTrack.Notes = OrderStatusExtensions.DefaultDescription(ShopmeProject.Models.OrderStatus.SHIPPING);
                    break;
                case "DELIVERED":
                    orderTrack.Notes = OrderStatusExtensions.DefaultDescription(ShopmeProject.Models.OrderStatus.DELIVERED);
                    break;
                case "RETURNED":
                    orderTrack.Notes = OrderStatusExtensions.DefaultDescription(ShopmeProject.Models.OrderStatus.RETURNED);
                    break;
                default:
                    Console.WriteLine("Status is unknown");
                    break;
            }


           
            orderTrack.UpdatedTime = DateTime.Now;
            if (!orderInDB.OrderTrack.Contains(orderInDB.OrderTrack.FirstOrDefault(x => x.Status == status)))
            {
                orderInDB.OrderTrack.Add(orderTrack);
            }
            _context.SaveChanges();

        }
    }
}