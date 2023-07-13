using ShopmeProject.DTO;
using ShopmeProject.Models;
using ShopmeProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class OrderService
    {
		public const int ORDERS_PER_PAGE = 4;
		private MyDbContext _context;

        public OrderService()
        {
            _context = new MyDbContext();
        }
		public Order createOrder(Customer customer, Address address, List<CartItem> cartItems,
			string paymentMethod, CheckoutInfo checkoutInfo)
		{

			
			Order newOrder = new Order();
			newOrder.orderTime = DateTime.Now;

			if (paymentMethod == "PAYPAL")
			{
				newOrder.status= "PAID";
			}
			else
			{
				newOrder.status = "NEW";
			}

			newOrder.CustomerId = customer.Id;
			newOrder.productCost = checkoutInfo.productCost;
			newOrder.subtotal = checkoutInfo.productTotal;
			newOrder.shippingCost =checkoutInfo.shippingCostTotal;
			newOrder.tax = 0.0f;
			newOrder.total =checkoutInfo.paymentTotal;
			newOrder.paymentMethod = paymentMethod ;
			newOrder.deliverDays = checkoutInfo.deliverDays;
			newOrder.deliverDate = checkoutInfo.getDeliverDate;

			if (address == null)
			{
				copyAddressFromCustomer(newOrder, customer);
			}
			else
			{
				copyShippingAddress(newOrder, address);
			}

			ICollection<OrderDetail> orderDetails = newOrder.OrderDetail;

			foreach (CartItem cartItem in cartItems)
			{
				Product product = cartItem.Product;

				OrderDetail orderDetail = new OrderDetail();
				orderDetail.OrderId = newOrder.Id;
				orderDetail.ProductId = product.Id;
				orderDetail.quantity = cartItem.Quantity;
				orderDetail.unitPrice = product.getDiscountPrice;
				orderDetail.productCost = product.Cost * cartItem.Quantity;
				orderDetail.subtotal = cartItem.getsubtotal;
				orderDetail.shippingCost = cartItem.shippingCost;
				orderDetails.Add(orderDetail);
			}

			 OrderTrack track = new OrderTrack();
			
			track.Status = "NEW";
            track.Notes = OrderStatusExtensions.DefaultDescription(OrderStatus.NEW);
			track.UpdatedTime = DateTime.Now;
			newOrder.OrderTrack.Add(track);

			_context.Order.Add(newOrder);
			_context.SaveChanges();
            return newOrder;
		}

		public void copyAddressFromCustomer(Order newOrder, Customer customer)
		{
			newOrder.FirstName = customer.FirstName;
			newOrder.LastName = customer.LastName;
			newOrder.phoneNumber = customer.phoneNumber;
			newOrder.addressLine1 = customer.addressLine1;
			newOrder.addressLine2 = customer.addressLine2;
			newOrder.city = customer.city;
			newOrder.country = customer.Country.Name;
			newOrder.postalCode = customer.postalCode;
			newOrder.state = customer.state;
			
		}

		public void copyShippingAddress(Order newOrder,Address address)
		{
			newOrder.FirstName = address.FirstName;
			newOrder.LastName = address.LastName;
			newOrder.phoneNumber = address.phoneNumber;
			newOrder.addressLine1 = address.addressLine1;
			newOrder.addressLine2 = address.addressLine2;
			newOrder.city = address.city;
			newOrder.country = address.Country.Name;
			newOrder.postalCode = address.postalCode;
			newOrder.state = address.state;
		}

		public List<Order> listForCustomerByPage(Customer customer,int pageNum, string keyword)
		{
			if (customer.Id == 0 || customer == null) return null;
			
			var query = _context.Order
			.Where(x =>	x.CustomerId == customer.Id)
		   .OrderByDescending(x => x.Id)
		   .Skip((pageNum - 1) * ORDERS_PER_PAGE)
		   .Take(ORDERS_PER_PAGE);

			if (!string.IsNullOrEmpty(keyword))
			{
				query = _context.Order
					.OrderByDescending(x => x.Id)
				   .Where(x =>
					x.CustomerId == customer.Id &&(
					x.Id.ToString().Contains(keyword) ||
					x.Customer.FirstName.Contains(keyword) ||
					 x.Customer.LastName.Contains(keyword) ||
					x.total.ToString().Contains(keyword) ||
					x.orderTime.ToString().Contains(keyword) ||
					x.country.Contains(keyword) ||
					 x.city.Contains(keyword) ||
					  x.state.Contains(keyword) ||
					x.paymentMethod.Contains(keyword) ||
					x.state.Contains(keyword)))
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

		public int totalRecordOrder(Customer customer,string keyword)
		{
			int totalRecords;
			if (!string.IsNullOrEmpty(keyword))
			{
				totalRecords = _context.Order
					.Where(x =>
					x.CustomerId == customer.Id && (
					x.Id.ToString().Contains(keyword) ||
					x.Customer.FirstName.Contains(keyword) ||
					 x.Customer.LastName.Contains(keyword) ||
					x.total.ToString().Contains(keyword) ||
					x.orderTime.ToString().Contains(keyword) ||
					x.country.Contains(keyword) ||
					 x.city.Contains(keyword) ||
					  x.state.Contains(keyword) ||
					x.paymentMethod.Contains(keyword) ||
					x.state.Contains(keyword))).Count();
			}
			else
			{
				totalRecords = _context.Order.Where(x => x.CustomerId == customer.Id).Count();
			}
			return totalRecords;
		}

		public int totalPageOrder(Customer customer,string keyword)
		{
			int totalRecords = totalRecordOrder(customer,keyword); // get the total number of records in the table
			int totalPages = (int)Math.Ceiling((double)totalRecords / ORDERS_PER_PAGE); // calculate the total number of pages
			return totalPages;
		}

		public Order Get(int id)
		{
			return _context.Order.FirstOrDefault(x => x.Id == id);
		}

		public void setOrderReturnRequested(OrderReturnRequest request, Customer customer)
        {
			Order order = _context.Order.FirstOrDefault(x => x.CustomerId == customer.Id && x.Id == request.orderId);
			if (order == null)
			{
				return;
			}
			
			if (order.OrderTrack.Any(t => t.Status == "RETURN_REQUESTED")) return;
			
			OrderTrack track = new OrderTrack();

			track.Status = "RETURN_REQUESTED";

			string notes = "Reason: " + request.reason;
			if (!string.IsNullOrEmpty(request.note))
			{
				notes += ". " + request.note;
			}


			track.Notes = notes;
			track.UpdatedTime = DateTime.Now;
			order.OrderTrack.Add(track);
				

			_context.SaveChanges();
		}
	}
}