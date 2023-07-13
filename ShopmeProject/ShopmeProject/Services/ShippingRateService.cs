using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class ShippingRateService
    {
        public const int SHIPPINGRATE_PER_PAGE = 4;
        public const int DIM_DIVISOR = 139;
        private MyDbContext _context;

        public ShippingRateService()
        {
            _context = new MyDbContext();
        }

        public List<ShippingRate> listByPage()
        {
            return _context.ShippingRate.Include("Country").ToList();
        }

        public string save(ShippingRate rateInForm)
        {
            ShippingRate rateInDB = _context.ShippingRate
                .FirstOrDefault(x => x.CountryId == rateInForm.CountryId
                && x.state == rateInForm.state);
            bool foundExistingRateInNewMode = rateInForm.Id == 0 && rateInDB != null;
            bool foundDifferentExistingRateInEditMode = rateInForm.Id != 0 && rateInDB != null && rateInForm.Id != rateInDB.Id;
            if(foundExistingRateInNewMode || foundDifferentExistingRateInEditMode)
            {
                return "There's already a rate for the destination ";
                       
            }
            if(rateInDB != null)
            {
                rateInDB.rate = rateInForm.rate;
                rateInDB.days = rateInForm.days;
                rateInDB.CountryId = rateInForm.CountryId;
                rateInDB.codSupported = rateInForm.codSupported;
                rateInDB.state = rateInForm.state;
            }
            else
            {
                _context.ShippingRate.Add(rateInForm);
            }
            _context.SaveChanges();
            return null;
        }

        public ShippingRate get(int id)
        {
            return _context.ShippingRate.Find(id);
        }

        public string updateCODSupport(int id, bool codSupported)
        {
            int count = _context.ShippingRate.Where(x => x.Id == id).Count();
            if(count == 0)
            {
                return "Could not find shipping rate with ID " + id;
            }

            ShippingRate shippingRate = _context.ShippingRate.Find(id);
            shippingRate.codSupported = codSupported;
            _context.SaveChanges();
            return null;
        }

        public string delete(int id)
        {
            int count = _context.ShippingRate.Where(x => x.Id == id).Count();
            if (count == 0)
            {
                return "Could not find shipping rate with ID " + id;
            }
            ShippingRate shippingRate = _context.ShippingRate.Find(id);
            _context.ShippingRate.Remove(shippingRate);
            _context.SaveChanges();
            return null;
        }

        public IEnumerable<ShippingRate> listByPage(int pageNum, string keyword)
        {
            var query = _context.ShippingRate
           .OrderBy(x => x.Id)
           .Skip((pageNum - 1) * SHIPPINGRATE_PER_PAGE)
           .Take(SHIPPINGRATE_PER_PAGE);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = _context.ShippingRate
                    .OrderBy(x => x.Id)
                   .Where(x =>
                    x.Country.Name.Contains(keyword)
                    || x.state.Contains(keyword))
                   .Skip((pageNum - 1) * SHIPPINGRATE_PER_PAGE)
                   .Take(SHIPPINGRATE_PER_PAGE)
                   ;
            }

            var records = query.ToList();
            return records;
        }

        public int shippingRatePerPage()
        {
            return SHIPPINGRATE_PER_PAGE;
        }

        public int totalRecordShippingRate(string keyword)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(keyword))
            {
                totalRecords = _context.ShippingRate
                    .Where(x =>
                     x.Country.Name.Contains(keyword)
                     || x.state.Contains(keyword)).Count();
            }
            else
            {
                totalRecords = _context.ShippingRate.Count();
            }
            return totalRecords;
        }

        public int totalPageShippingRate(string keyword)
        {
            int totalRecords = totalRecordShippingRate(keyword); // get the total number of records in the table
            int totalPages = (int)Math.Ceiling((double)totalRecords / SHIPPINGRATE_PER_PAGE); // calculate the total number of pages
            return totalPages;
        }

        public List<Country> listAllCountries()
        {
            return _context.Country.ToList();
        }

        public float calculateShippingCost(int productId, int countryId, string state)
        {
            ShippingRate shippingRate = _context.ShippingRate
                .FirstOrDefault(x => x.CountryId == countryId
                && x.state == state);
            if (shippingRate == null)
            {
                return 0;
            }

            Product product = _context.Products.Find(productId);
            float dimWeight = (product.Length * product.Width * product.Height) / DIM_DIVISOR;
            float finalWeight = product.Height > dimWeight ? product.Weight : dimWeight;

            return finalWeight * shippingRate.rate;
        }
    }
}