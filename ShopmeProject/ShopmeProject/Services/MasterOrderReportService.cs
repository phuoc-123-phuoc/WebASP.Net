using ShopmeProject.DTO;
using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class MasterOrderReportService : AbstractReportService
    {
        private MyDbContext _context;

        public MasterOrderReportService()
        {
            _context = new MyDbContext();
        }
        protected override List<ReportItemDTO> GetReportDataByDateRangeInternal(DateTime startDate, DateTime endDate, string reportType)
        {
            List<Order> listOrders = FindByOrderTimeBetween(startDate, endDate);
           
            List<ReportItemDTO> listReportItems = MasterOrderReportServiceUtil.CreateReportData(startDate, endDate, dateFormatter, reportType);

            MasterOrderReportServiceUtil.calculateSalesForReportData(listOrders, listReportItems, dateFormatter);

            return listReportItems;
        }

        public List<Order> FindByOrderTimeBetween(DateTime startTime, DateTime endTime)
        {
            return _context.Order
                .Where(o => o.orderTime >= startTime && o.orderTime <= endTime)
                .OrderBy(o => o.orderTime)
                .ToList();
        }

    }
}