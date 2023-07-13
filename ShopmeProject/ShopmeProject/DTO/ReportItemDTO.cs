using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.DTO
{
    public class ReportItemDTO
    {
        public string identifier { get; set; }
        public float grossSales { get; set; }
        public float netSales { get; set; }
        public int ordersCount { get; set; }
        public int productsCount { get; set; }

        public ReportItemDTO()
        {
        }

        public ReportItemDTO(string Identifier)
        {
            identifier = Identifier;
        }

        public ReportItemDTO(string Identifier, float GrossSales, float NetSales)
        {
            identifier = Identifier;
            grossSales = GrossSales;
            netSales = NetSales;
        }

        public ReportItemDTO(string Identifier, float GrossSales, float NetSales, int ProductsCount)
        {
            identifier = Identifier;
            grossSales = GrossSales;
            netSales = NetSales;
            productsCount = ProductsCount;
        }

        public void IncreaseProductsCount(int count)
        {
            productsCount += count;
        }

        public override int GetHashCode()
        {
            return identifier.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;
            ReportItemDTO other = (ReportItemDTO)obj;
            return identifier == other.identifier;
        }

        public void AddGrossSales(float amount)
        {
            grossSales += amount;
        }

        public void AddNetSales(float amount)
        {
            netSales += amount;
        }

        public void IncreaseOrdersCount()
        {
            ordersCount++;
        }

        public override string ToString()
        {
            return $"ReportItemDTO [Identifier={identifier}, GrossSales={grossSales}, NetSales={netSales}, OrdersCount={ordersCount}]";
        }
    }
}