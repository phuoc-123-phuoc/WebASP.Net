using ShopmeProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ShopmeProject.ServicesSite
{
    public class AddressService
    {
        private MyDbContext _context;

        public AddressService()
        {
            _context = new MyDbContext();
        }

        public List<Address> listAddressBook(Customer customer)
        {
           
            // TODO Auto-generated method stub
            return _context.Address.Where(x => x.CustomerId == customer.Id).ToList();
        }

        public void save(Address address)
        {
            if(address.Id == 0)
            {
                _context.Address.Add(address);
            }
            else
            {
                Address addressInDb = _context.Address.FirstOrDefault(x => x.Id == address.Id);
                addressInDb.FirstName = address.FirstName;
                addressInDb.LastName = address.LastName;
                addressInDb.phoneNumber = address.phoneNumber;
                addressInDb.city = address.city;
                addressInDb.CountryId = address.CountryId;
                addressInDb.state = address.state;
                addressInDb.postalCode = address.postalCode;
            }
           
            _context.SaveChanges();
        }

        public Address get(int addressId, int customerId)
        {
            return _context.Address.FirstOrDefault(x => x.CustomerId == customerId && x.Id == addressId);
        }

        public void delete(int addressId, int customerId)
        {
            Address addressToDelete = _context.Address.FirstOrDefault(x => x.CustomerId == customerId && x.Id == addressId);

            _context.Address.Remove(addressToDelete);
            _context.SaveChanges();
        }

        public void setDefaultAddress(int defaultAddressId, int customerId)
        {
            if (defaultAddressId > 0)
            {
                Address address = _context.Address.FirstOrDefault(x => x.Id == defaultAddressId);
                address.defaultForShipping = true;
            }

            var addressesToUpdate = _context.Address.Where(a => a.CustomerId == customerId && a.Id != defaultAddressId).ToList();

            foreach (var address in addressesToUpdate)
            {
                address.defaultForShipping = false;
            }

            _context.SaveChanges();
        }

        public Address getDefaultAddress(Customer customer)
        {
           
            var Address= _context.Address.FirstOrDefault(x => x.defaultForShipping == true && x.CustomerId == customer.Id);
           
            return Address;
        }

    }
}