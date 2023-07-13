using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class CountryService
    {
        private MyDbContext _context;

        public CountryService()
        {
            _context = new MyDbContext();
        }

        public List<Country> listAll()
        {
            return _context.Country.OrderBy(c => c.Name).ToList();
        }

        public Country Save(Country country)
        {
            
            if (country.Id == 0)
            {
                _context.Country.Add(country);
            }
            else
            {
                Country countryInDb = _context.Country.Find(country.Id);
                countryInDb.Name = country.Name;
                countryInDb.Code = country.Code;
            }
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            return country;
        }

        public string delete(int id)
        {
            Country countryToDelete = _context.Country.Find(id);
            if (countryToDelete != null)
            {
                _context.Country.Remove(countryToDelete);
                _context.SaveChanges();
                return "Delete successfull";
            }
            return "Detete false";
        }

        public bool isNameUnique(string name)
        {
            Country countryByName = _context.Set<Country>().SingleOrDefault(u => u.Name == name);
            if (countryByName == null) return true;
          

            return false;
        }

    }
    
}