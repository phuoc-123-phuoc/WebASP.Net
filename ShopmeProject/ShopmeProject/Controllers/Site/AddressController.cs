using ShopmeProject.Models;
using ShopmeProject.ServicesSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Site
{
    public class AddressController : BaseSiteController
    {
        private AddressService addressService;

        private CustomerService customerService;

        public AddressController()
        {
            addressService = new AddressService();
            customerService = new CustomerService();
        }
        // GET: Address
        public ActionResult showAddressBook( string redirect="")
        {
            var user = User as ClaimsPrincipal;
            string email = "";
            var name = "";
            if (user != null)
            {
                email = user.FindFirst("email")?.Value;
                name = user.FindFirst("unique_name")?.Value;
            }

            AuthenticationControllerHelperUtil authenticationControllerHelperUtil = new AuthenticationControllerHelperUtil();
            Customer customer = authenticationControllerHelperUtil.getAuthenticatedCustomer(email);
            List<Address> listAddresses = addressService.listAddressBook(customer);
            bool usePrimaryAddressAsDefault = true;
            foreach( Address address in listAddresses){
                if (address.defaultForShipping)
                {
                    usePrimaryAddressAsDefault = false;
                    break;
                }
            }
            if(redirect != "")
            {
                ViewBag.redirect = "cart";
            }
            ViewBag.listAddresses = listAddresses;
            ViewBag.customer = customer;
            ViewBag.usePrimaryAddressAsDefault = usePrimaryAddressAsDefault;
            return View("addresses");
        }

        public ActionResult newAddress()
        {
            List<Country> listCountries = customerService.listAllCountries();
            ViewBag.listCountries = listCountries;
             var address = new Address();
            ViewBag.Title = "Add New Address";
            return View("address_form", address);
        }

        [HttpPost]
        public ActionResult saveAddress(Address address)
        {
            var user = User as ClaimsPrincipal;
            string email = "";
            var name = "";
            if (user != null)
            {
                email = user.FindFirst("email")?.Value;
                name = user.FindFirst("unique_name")?.Value;
            }

            AuthenticationControllerHelperUtil authenticationControllerHelperUtil = new AuthenticationControllerHelperUtil();
            Customer customer = authenticationControllerHelperUtil.getAuthenticatedCustomer(email);

            address.CustomerId = customer.Id;
            addressService.save(address);
            return RedirectToAction("showAddressBook");
        }

        public ActionResult editAddress(int addressId)
        {
            Customer customer = GetCustomer();
            List<Country> listCountries = customerService.listAllCountries();
            Address address = addressService.get(addressId, customer.Id);
           
            ViewBag.listCountries = listCountries;
            ViewBag.Title = "Edit Address (ID: " + addressId + ")";
            return View("address_form", address);
        }
        public ActionResult deleteAddress(int addressId)
        {
            Customer customer = GetCustomer();
            addressService.delete(addressId, customer.Id);
            ViewBag.message = "The address ID " + addressId + " has been deleted.";
            return RedirectToAction("showAddressBook");
        }

        public ActionResult setDefaultAddress(int addressId, string redirect="")
        {
            Customer customer = GetCustomer();
            addressService.setDefaultAddress(addressId, customer.Id);
            if(redirect == "cart")
            {
                return RedirectToAction("viewCart", "ShoppingCart");
            }
            return RedirectToAction("showAddressBook");
        }
        public Customer GetCustomer()
        {
            var user = User as ClaimsPrincipal;
            string email = "";
            var name = "";
            if (user != null)
            {
                email = user.FindFirst("email")?.Value;
                name = user.FindFirst("unique_name")?.Value;
            }

            AuthenticationControllerHelperUtil authenticationControllerHelperUtil = new AuthenticationControllerHelperUtil();
            Customer customer = authenticationControllerHelperUtil.getAuthenticatedCustomer(email);
            return customer;
        }
    }
}