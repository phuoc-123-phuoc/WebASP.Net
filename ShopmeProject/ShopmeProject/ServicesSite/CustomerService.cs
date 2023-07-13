using Microsoft.AspNet.Identity;
using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace ShopmeProject.ServicesSite
{
    public class CustomerService
    {
        private MyDbContext _context;

        public CustomerService()
        {
            _context = new MyDbContext();
        }

       
        public List<Country> listAllCountries()
        {
            return _context.Country.OrderBy(c => c.Name).ToList();
        }

        public bool isNameUnique(string email)
        {
            Customer customerByEmail = _context.Set<Customer>().SingleOrDefault(u => u.Email == email);
            if (customerByEmail == null) return true;


            return false;
        }

        public void save(Customer customer)
        {
            var passwordHasher = new PasswordHasher();



            // Create new customer
            string randomString = GenerateRandomString(64);
            customer.verificationCode = randomString;
            customer.Enabled = false;
                 customer.CreatedTime = DateTime.Now;
                var hashedPassword = passwordHasher.HashPassword(customer.Password);
                customer.Password = hashedPassword;
            customer.authenticationType = "DATABASE";
               
                _context.Customer.Add(customer);


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

        public string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

        public bool verify(string verificationCode)
        {
            Customer customer = _context.Customer.FirstOrDefault(x => x.verificationCode == verificationCode);

            if (customer == null || customer.Enabled)
            {
                return false;
            }
            else
            {
                customer.Enabled = true;
                customer.verificationCode = null;
                _context.SaveChanges();
                return true;
            }
        }

        public void updateAuthenticationType(Customer customer, string type)
        {
            if(customer.authenticationType == null || customer.authenticationType != type)
            {
                Customer existingCustomer = _context.Customer.Find(customer.Id);
                existingCustomer.authenticationType = type;
                _context.SaveChanges();
            }
        }

        public bool validInfor(string email, string password)
        {
            var customer = _context.Customer.SingleOrDefault(u => u.Email == email && u.Enabled == true);
            if (customer == null)
            {
                return false;
            }

            var passwordVerificationResult = Crypto.VerifyHashedPassword(customer.Password, password);
            return passwordVerificationResult;
        }

        public Customer getCustomerByEmail(string email)
        {
            var customer = _context.Customer.FirstOrDefault(u => u.Email == email);
            return customer;
        }

        public Customer Get(string email)
        {
            return _context.Customer.FirstOrDefault(x => x.Email == email);
        }

        public void Save(Customer customer)
        {
            if (customer.Id > 0)
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
                //customerInDb.Email = customer.Email;


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

        public string updateResetPasswordToken(string email)
        {
            
            Customer customer = _context.Customer.FirstOrDefault(x => x.Email == email);
            if (customer != null)
            {
                string token = GenerateRandomString(30);
                customer.resetPasswordToken = token;
                _context.SaveChanges();

                return token;
            }
            else
            {
                return "not found";
            }
        }

        public Customer getByResetPasswordToken(string token)
        {
            // TODO Auto-generated method stub
            return _context.Customer.FirstOrDefault(x => x.resetPasswordToken == token);
        }

        public string updatePassword(string token, string newPassword) 
        {
            // TODO Auto-generated method stub
            Customer customer = _context.Customer.FirstOrDefault(x => x.resetPasswordToken == token);
		    if (customer == null) {
			return "No customer found: invalid token";
             }
            var passwordHasher = new PasswordHasher();
            var hashedPassword = passwordHasher.HashPassword(newPassword);
            customer.Password = hashedPassword;
            customer.resetPasswordToken = null;
            _context.SaveChanges();
            return null;
	    }	


    }
}