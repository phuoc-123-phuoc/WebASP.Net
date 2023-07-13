using ShopmeProject.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public abstract class AbstractReportService
    {
        protected DateTimeFormatInfo dateFormatter;

        public List<ReportItemDTO> GetReportDataLast7Days(string reportType)
        {
           

            return GetReportDataLastXDays(7, reportType);
        }

        public List<ReportItemDTO> GetReportDataLast28Days(string reportType)
        {
           

            return GetReportDataLastXDays(28, reportType);
        }

        protected List<ReportItemDTO> GetReportDataLastXDays(int days, string reportType)
        {
           

            DateTime endTime = DateTime.Now;
            DateTime startTime = DateTime.Now.AddDays(-(days - 1));

           

            dateFormatter = new DateTimeFormatInfo { ShortDatePattern = "yyyy-MM-dd" }; 

            return GetReportDataByDateRangeInternal(startTime, endTime, reportType);
        }

        public List<ReportItemDTO> GetReportDataLast6Months(string reportType)
        {
            

            return GetReportDataLastXMonths(6, reportType);
        }

        public List<ReportItemDTO> GetReportDataLastYear(string reportType)
        {
           

            return GetReportDataLastXMonths(12, reportType);
        }

        protected List<ReportItemDTO> GetReportDataLastXMonths(int months, string reportType)
        {
           
            DateTime endTime = DateTime.Now;
            DateTime startTime = DateTime.Now.AddMonths(-(months - 1));

           

            dateFormatter =  new DateTimeFormatInfo { ShortDatePattern = "yyyy-MM" };

            return GetReportDataByDateRangeInternal(startTime, endTime, reportType);
        }

        public List<ReportItemDTO> GetReportDataByDateRange(DateTime startTime, DateTime endTime, string reportType)
        {
          
            dateFormatter = new DateTimeFormatInfo { ShortDatePattern = "yyyy-MM-dd" };
            return GetReportDataByDateRangeInternal(startTime, endTime, reportType);
        }

        protected abstract List<ReportItemDTO> GetReportDataByDateRangeInternal(DateTime startDate, DateTime endDate, string reportType);
    }
}