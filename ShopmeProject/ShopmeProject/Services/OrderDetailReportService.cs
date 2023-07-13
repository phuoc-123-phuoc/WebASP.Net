using ShopmeProject.DTO;
using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class OrderDetailReportService : AbstractReportService
    {
        private MyDbContext _context;

        public OrderDetailReportService()
        {
            _context = new MyDbContext();
        }
        protected override List<ReportItemDTO> GetReportDataByDateRangeInternal(DateTime startDate, DateTime endDate, string reportType)
        {
            List<OrderDetailDTO> listOrderDetails = null;
            if (reportType.Equals(ReportType.CATEGORY))
            {
                listOrderDetails = FindWithCategoryAndTimeBetween(startDate, endDate);
            }
            else if (reportType.Equals(ReportType.PRODUCT))
            {
                listOrderDetails = FindWithProductAndTimeBetween(startDate, endDate);
            }

            List<ReportItemDTO> listReportItems = new List<ReportItemDTO>();

            foreach(OrderDetailDTO detail in listOrderDetails)
            {
                string identifier = "";

               
                    identifier = detail.Name;
               
                   

                ReportItemDTO reportItem = new ReportItemDTO(identifier);

                float grossSales = detail.subtotal + detail.shippingCost;
                float netSales = detail.subtotal - detail.productCost;

               // int itemIndex = listReportItems.IndexOf(reportItem);
                int itemIndex = listReportItems.FindIndex(item => item.identifier == reportItem.identifier);

                if (itemIndex >= 0)
                {
                    reportItem = listReportItems[itemIndex];
                    reportItem.AddGrossSales(grossSales);
                    reportItem.AddNetSales(netSales);
                    reportItem.IncreaseProductsCount(detail.quantity);

                   

                }
                else
                {

                    listReportItems.Add(new ReportItemDTO(identifier, grossSales, netSales, detail.quantity));

                   
                }
            }
            return listReportItems;

        }


        public List<OrderDetailDTO> FindWithCategoryAndTimeBetween(DateTime startTime, DateTime endTime)
        {
            return (from d in _context.OrderDetail
                    where d.Order.orderTime >= startTime && d.Order.orderTime <= endTime
                    select new OrderDetailDTO
                    {
                        Name = d.Product.Category.Name ,
                        quantity = d.quantity,
                        productCost = d.productCost,
                        shippingCost = d.shippingCost,
                        subtotal = d.subtotal
                    }).ToList();
        }

        public List<OrderDetailDTO> FindWithProductAndTimeBetween(DateTime startTime, DateTime endTime)
        {
            return (from d in _context.OrderDetail
                    where d.Order.orderTime >= startTime && d.Order.orderTime <= endTime
                    select new OrderDetailDTO
                    {
                        quantity = d.quantity,
                        Name = d.Product.Name ,
                        productCost = d.productCost,
                        shippingCost = d.shippingCost,
                        subtotal = d.subtotal
                    }).ToList();
        }
    }
}