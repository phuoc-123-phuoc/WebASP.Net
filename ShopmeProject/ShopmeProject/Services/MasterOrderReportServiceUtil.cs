using ShopmeProject.DTO;
using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class MasterOrderReportServiceUtil
    {
        public static void calculateSalesForReportData(List<Order> listOrders, List<ReportItemDTO> listReportItems,
            DateTimeFormatInfo dateFormatter)
        {
            foreach (Order order in listOrders)
            {
                string orderDateString = order.orderTime.ToString("yyyy-MM-dd");



                ReportItemDTO reportItem = new ReportItemDTO(orderDateString);


                int itemIndex = listReportItems.FindIndex(item => item.Equals(reportItem));



                if (itemIndex >= 0)
                {
                    reportItem = listReportItems[itemIndex];

                    reportItem.AddGrossSales(order.total);
                    reportItem.AddNetSales(order.subtotal - order.productCost);
                    reportItem.IncreaseOrdersCount();


                }
            }



        }

        public static List<ReportItemDTO> CreateReportData(DateTime startTime, DateTime endTime,
        DateTimeFormatInfo dateFormatter, string reportType)
        {
          

            List<ReportItemDTO> listReportItems = new List<ReportItemDTO>();

            DateTime currentDate = startTime;
         
            string dateString = currentDate.ToString("yyyy-MM-dd");
        

            listReportItems.Add(new ReportItemDTO(dateString));
         

            do
            {
                if (reportType == ReportType.DAY)
                {
                  
                    currentDate = currentDate.AddDays(1);
                   
                }
                else if (reportType == ReportType.MONTH)
                {
                   
                    currentDate = currentDate.AddMonths(1);
                  
                }

                dateString = currentDate.ToString("yyyy-MM-dd");
                

                listReportItems.Add(new ReportItemDTO(dateString));
               
            }
            while (currentDate < endTime);

          

            return listReportItems;
        }

    }
}