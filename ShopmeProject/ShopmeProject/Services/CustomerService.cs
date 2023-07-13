using Microsoft.AspNet.Identity;
using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ShopmeProject.Services
{
    public class CustomerService
    {
        public const int CUSTOMERS_PER_PAGE = 4;
        private MyDbContext _context;

        public CustomerService()
        {
            _context = new MyDbContext();
        }

        public List<Country> listAllCountries()
        {
            return _context.Country.OrderBy(c => c.Name).ToList();
        }


        public List<Customer> listAllCustomer()
        {
            return _context.Customer.OrderBy(c => c.FirstName).ToList();
        }

        public void updateCustomerEnabledStatus(int id, bool enabled)
        {
            var existingCustomer = _context.Customer.Find(id);
            if (existingCustomer != null)
            {
                existingCustomer.Enabled = enabled;
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
            }
        }

        public Customer Get(int id)
        {
            return _context.Customer.Find(id);
        }

        public bool isEmailUnique(int id, string email)
        {
           
            Customer existCustomer = _context.Customer.FirstOrDefault(x => x.Email == email);

            if (existCustomer != null && existCustomer.Id != id)
            {
                // found another customer having the same email
                return false;
            }

            return true;
        }

        public void Save(Customer customer)
        {
            if(customer.Id > 0)
            {
                Customer customerInDb = _context.Customer.Find(customer.Id);
                if (customer.Password != null)
                {
                    var passwordHasher = new PasswordHasher();
                    var hashedPassword = passwordHasher.HashPassword(customer.Password);
                    customer.Password = hashedPassword;
                    customerInDb.Password = customer.Password;
                }
               
                customerInDb.FirstName = customer.FirstName;
                customerInDb.LastName = customer.LastName;
                customerInDb.Email = customer.Email;

               
                customerInDb.phoneNumber = customer.phoneNumber;
                customerInDb.addressLine1 = customer.addressLine1;
                customerInDb.addressLine2 = customer.addressLine2;
                customerInDb.city = customer.city;
                customerInDb.CountryId = customer.CountryId;
                customerInDb.state = customer.state;
                customerInDb.postalCode = customer.postalCode;
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
            }
            
        }

        public void delete(int id)
        {
            Customer customerToDelete = _context.Customer.Find(id);
            if (customerToDelete != null)
            {
                _context.Customer.Remove(customerToDelete);
                _context.SaveChanges();
            }

        }

        public int CountCustomerById(int id)
        {
            return _context.Customer.Count(u => u.Id == id);
        }
        public int customerPerPage()
        {
            return CUSTOMERS_PER_PAGE;
        }

        public int totalRecordCustomer(string keyword)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(keyword))
            {
                totalRecords = _context.Customer
                    .Where(x =>
                     x.FirstName.Contains(keyword) ||
                     x.LastName.Contains(keyword) ||
                     x.Email.Contains(keyword)).Count();
            }
            else
            {
                totalRecords = _context.Customer.Count();
            }
            return totalRecords;
        }

        public int totalPageCustomer(string keyword)
        {
            int totalRecords = totalRecordCustomer(keyword); // get the total number of records in the table
            int totalPages = (int)Math.Ceiling((double)totalRecords / CUSTOMERS_PER_PAGE); // calculate the total number of pages
            return totalPages;
        }

        public IEnumerable<Customer> listByPage(int pageNum, string keyword)
        {
            var query = _context.Customer
           .OrderBy(x => x.Id)
           .Skip((pageNum - 1) * CUSTOMERS_PER_PAGE)
           .Take(CUSTOMERS_PER_PAGE);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = _context.Customer
                    .OrderBy(x => x.Id)
                   .Where(x =>
                    x.FirstName.Contains(keyword) ||
                    x.LastName.Contains(keyword) ||
                    x.Email.Contains(keyword))
                   .Skip((pageNum - 1) * CUSTOMERS_PER_PAGE)
                   .Take(CUSTOMERS_PER_PAGE)
                   ;
            }

            var records = query.ToList();
            return records;
        }

    }
}