using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Helpers
{
    public static class CustomHtmlHelper
    {
        public static MvcHtmlString FormatPrice(this HtmlHelper helper, string currencySymbol, string currencySymbolPosition, double productPrice, string thousandsPointType, string decimalDigits, string decimalPointType)
        {
            if(thousandsPointType == "POINT")
            {
                thousandsPointType = ".";
            }
            else
            {
                thousandsPointType = ",";
            }
            if(decimalPointType == "POINT")
            {
                decimalPointType = ".";
            }
            else
            {
                decimalPointType = ",";
            }
            var formattedPrice = "";
            if (currencySymbolPosition == "Before price")
            {
                formattedPrice += currencySymbol;
            }
            formattedPrice += string.Format("{0:N" + decimalDigits + "}", productPrice);

            formattedPrice = formattedPrice.Replace(",", thousandsPointType).Replace(".", decimalPointType);
            if (currencySymbolPosition == "After price")
            {
                formattedPrice += currencySymbol;
            }
            return new MvcHtmlString(formattedPrice);
        }
    }
}