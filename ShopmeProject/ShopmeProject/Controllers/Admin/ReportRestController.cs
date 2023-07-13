using ShopmeProject.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Admin
{
    public class ReportRestController : BaseController
    {
        private MasterOrderReportService masterOrderReportService;

        private OrderDetailReportService orderDetailReportService;
        public ReportRestController()
        {
            masterOrderReportService = new MasterOrderReportService();
            orderDetailReportService = new OrderDetailReportService();
        }
        // GET: ReportRest
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult GetReportDataByDatePeriod(string period)
        {
           

            switch (period)
            {
                case "last_7_days":
                   
                    return Json( masterOrderReportService.GetReportDataLast7Days(ReportType.DAY), JsonRequestBehavior.AllowGet); 

                case "last_28_days":

                    return Json(masterOrderReportService.GetReportDataLast28Days(ReportType.DAY), JsonRequestBehavior.AllowGet);
                  

                case "last_6_months":
                    return Json(masterOrderReportService.GetReportDataLast6Months(ReportType.MONTH), JsonRequestBehavior.AllowGet);
                    

                case "last_year":
                    return Json(masterOrderReportService.GetReportDataLastYear(ReportType.MONTH), JsonRequestBehavior.AllowGet);
                    

                default:
                    return Json(masterOrderReportService.GetReportDataLast7Days(ReportType.DAY), JsonRequestBehavior.AllowGet);
                   
            }
        }

        public ActionResult GgetReportDataByDatePeriod( string startDate, string endDate)
        {
            DateTime startTime = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return Json(masterOrderReportService.GetReportDataByDateRange(startTime, endTime, ReportType.DAY), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getReportDataByCategoryOrProduct(string groupBy, string period)
        {
            switch (period)
            {
                case "last_7_days":
                    return Json(orderDetailReportService.GetReportDataLast7Days(groupBy.ToUpper()), JsonRequestBehavior.AllowGet);
                   
                case "last_28_days":
                    return Json(orderDetailReportService.GetReportDataLast28Days(groupBy.ToUpper()), JsonRequestBehavior.AllowGet);
                    

                case "last_6_months":
                    return Json(orderDetailReportService.GetReportDataLast6Months(groupBy.ToUpper()), JsonRequestBehavior.AllowGet);
                   
                case "last_year":
                    return Json(orderDetailReportService.GetReportDataLastYear(groupBy.ToUpper()), JsonRequestBehavior.AllowGet);
                   
                default:
                    return Json(orderDetailReportService.GetReportDataLast7Days(groupBy.ToUpper()), JsonRequestBehavior.AllowGet);
                   
            }
        }
        
        public ActionResult getReportDataByCategoryOrProductDateRange(string groupBy, string startDate, string endDate)
        {
            DateTime startTime = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            
            return Json(orderDetailReportService.GetReportDataByDateRange(startTime, endTime, groupBy.ToUpper()), JsonRequestBehavior.AllowGet);
        }
    }
}