using ShopmeProject.DTO;
using ShopmeProject.Models;
using ShopmeProject.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Admin
{
    [JwtAuthentication("Salesperson", "Shipper", "Admin")]
    public class OrderController : BaseController
    {
        private OrderService orderService;
        private SettingService settingService;

        public OrderController()
        {
            orderService = new OrderService();
            settingService = new SettingService();
        }

        public ActionResult Index()
        {
            return ListByPage(1, "");
            //IEnumerable<Product> products = productService.ListAll();
            //return View(products);
        }
        // GET: Order
        public ActionResult ListByPage(int pageNum, string keyword)
        {
            List<Setting> Settings = settingService.getCurrencySettings();

            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }
            List<Order> listOrder = orderService.listByPage(pageNum,keyword);

            
            long startCount = (pageNum - 1) * orderService.orderPerPage() + 1;
            long endCount = startCount + orderService.orderPerPage() - 1;
            if (endCount > orderService.totalRecordOrder(keyword))
            {
                endCount = orderService.totalRecordOrder(keyword);
            }
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            ViewBag.CurrentPage = pageNum;
            ViewBag.TotalPage = orderService.totalPageOrder(keyword);
            ViewBag.StartCount = startCount;
            ViewBag.EndCount = endCount;
            ViewBag.TotalItems = orderService.totalRecordOrder(keyword);
            ViewBag.KeyWord = keyword;
            IEnumerable<string> roles = GetRoles();
            if(roles.Contains("Shipper") && roles != null)
            return View("orders_shipper",listOrder);
            return View("Index",listOrder);
        }

        public ActionResult Detail(int id)
        {
            List<Setting> Settings = settingService.getCurrencySettings();

            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }

            Order order = orderService.Get(id);
            return PartialView("order_details_modal", order);
        }

        public ActionResult Delete(int id)
        {
            if (orderService.CountOrdersById(id) > 0)
            {
                orderService.delete(id);
                TempData["message"] = "The order ID " + id + " has been deleted successfully";
            }
            else
            {
                TempData["message"] = "Could not find any order with ID " + id;
            }
            return RedirectToAction("Index");
        }

        public ActionResult editOrder(int id)
        {
            Order order = orderService.Get(id);
            List<Country> listCountries = orderService.listAllCountries();
            ViewBag.Title = "Manage Orders";
            ViewBag.pageTitle = "Edit Track Order (ID: " + id + " )";
           
            ViewBag.OrderStatus = ShopmeProject.Services.OrderStatus.orderStatus;
            return View("order_form", order);
        }

        [HttpPost]
        public ActionResult saveOrder(int orderId, string[] trackStatus, string[] trackNotes,
           DateTime[] trackDate,  string[] trackId)
        {
            List<OrderTrack> orderTracks = new List<OrderTrack>();
            setOrderTrack(trackStatus, trackNotes, trackDate, orderTracks, trackId);
            orderService.MapDetailsValue(orderId, orderTracks);
            
            return RedirectToAction("Index");
        }

        public  void setOrderTrack(string[] trackStatus, string[] trackNotes,
           DateTime[] trackDate, List<OrderTrack> orderTracks, string[] trackId)
        {
            if (trackNotes == null || trackNotes.Length == 0) return;
            for (int count = 0; count < trackNotes.Length; count++)
            {
                if (trackNotes[count] != "" || trackNotes[count] != "")
                {
                    int id = Convert.ToInt32(trackId[count]);
                    string note = trackNotes[count];
                    string status = trackStatus[count];
                    DateTime updatedTime = trackDate[count];
                    orderTracks.Add(new OrderTrack() {Id = id, Notes = note, UpdatedTime = updatedTime, Status = status });
                   
                }

            }

        }

        [HttpPost]
        public ActionResult updateOrderStatus(int id , string status)
        {
            orderService.updateStatus(id, status);
            OrderResponseDTO order = new OrderResponseDTO() { orderId = id, status = status };
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<string> GetRoles()
        {
            var user = User as ClaimsPrincipal;
            string email = "";

            if (user != null)
            {
                email = user.FindFirst("email")?.Value;
                var roles = user.Claims
                .Where(c => c.Type == "role")
                .Select(c => c.Value);
                return roles;

            }

           
            return null;
        }
    }
}