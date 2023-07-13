using ShopmeProject.DTO;
using ShopmeProject.Models;
using ShopmeProject.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Admin
{
    [JwtAuthentication("Admin")]
    public class SettingController : BaseController
    {
        private SettingService settingService;
        private CountryService countryService;
        private StateService stateService;

        public SettingController()
        {
            countryService = new CountryService();
            settingService = new SettingService();
            stateService = new StateService();
        }

        // GET: Setting
        public ActionResult Index()
        {
            List<Currency> Currencies = settingService.GetCurrencies();
            List<Setting> Settings = settingService.GetSettings();
            foreach (Setting setting in Settings)
            {
                ViewData[setting.Key] = setting.Value;
            }
            ViewBag.Currencies = Currencies;
            return View();
        }

        public ActionResult saveGeneralSettings(
            string SITE_NAME,
            string COPYRIGHT,
            string CURRENCY_ID,
            string CURRENCY_SYMBOL_POSITION,
            string DECIMAL_POINT_TYPE,
            string DECIMAL_DIGITS,
            string THOUSANDS_POINT_TYPE,
             HttpPostedFileBase fileImage

            )
        {
            string SITE_LOGO = "";
            if (fileImage != null && fileImage.ContentLength > 0)
            {
                

                var fileName = Path.GetFileName(fileImage.FileName);
                string uploadDir = Server.MapPath("/site-logo/");
                SITE_LOGO = fileName;
                FileUploadUtil.CleanDir(uploadDir);
                FileUploadUtil.SaveFile(uploadDir, fileName, fileImage);

            }
            settingService.SaveGeneral(SITE_NAME, COPYRIGHT, CURRENCY_ID,
                CURRENCY_SYMBOL_POSITION, DECIMAL_POINT_TYPE, DECIMAL_DIGITS, THOUSANDS_POINT_TYPE, SITE_LOGO);
            return Redirect("Index");
        }

        public ActionResult listAll()
        {
            var countries = countryService.listAll();

            var countriesDto = countries.Select(x => new CountryDTO
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code
            }).ToList();


            return Json(countriesDto, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Save(Country country)
        {
            var countrySaved = countryService.Save(country);

            return Json(countrySaved, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            string msg = countryService.delete(id);
            return Content(msg);
        }

        [HttpPost]
        public ActionResult CheckDuplicateName(string name)
        {
            if (countryService.isNameUnique(name) == true)
            {
                return Content("OK");
            }
            else
            {
                return Content("Duplicated");
            }
        }

        public ActionResult listAllStateByCountry(int Id)
        {
            var states = stateService.listAll(Id);

            var statesDto = states.Select(x => new StateDTO
            {
                id = x.Id,
                name = x.Name
            }).ToList();

            return Json(statesDto, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SaveState(State state)
        {
            var stateSaved = stateService.Save(state);

            return Json(stateSaved, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteState(int id)
        {
            string msg = stateService.delete(id);
            return Content(msg);
        }

        [HttpPost]
        public ActionResult checkDuplicateNameState(string name)
        {
            if (stateService.isNameUnique(name) == true)
            {
                return Content("OK");
            }
            else
            {
                return Content("Duplicated");
            }
        }
        public ActionResult saveMailServerSetttings(
            string MAIL_HOST,
            string MAIL_PORT,
            string MAIL_USERNAME,
            string MAIL_PASSWORD,
            string SMTP_AUTH,
            string SMTP_SECURED,
            string MAIL_FROM,
            string MAIL_SENDER_NAME
            )
        {
            settingService.SaveMailServer(MAIL_HOST,
                                        MAIL_PORT,
                                        MAIL_USERNAME,
                                        MAIL_PASSWORD,
                                        SMTP_AUTH,
                                        SMTP_SECURED,
                                        MAIL_FROM,
                                        MAIL_SENDER_NAME);
            return Redirect("Index");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult saveMailTemplateSetttings(
            string CUSTOMER_VERIFY_SUBJECT,
            string CUSTOMER_VERIFY_CONTENT,
            string ORDER_CONFIRMATION_SUBJECT,
            string ORDER_CONFIRMATION_CONTENT)
        {
            settingService.SaveMailTemplates(CUSTOMER_VERIFY_SUBJECT, CUSTOMER_VERIFY_CONTENT,ORDER_CONFIRMATION_SUBJECT,ORDER_CONFIRMATION_CONTENT);
            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult savePaymentSetttings(
          string PAYPAL_API_BASE_URL,
          string PAYPAL_API_CLIENT_ID,
          string PAYPAL_API_CLIENT_SECRET)
        {
            settingService.SavePayment(PAYPAL_API_BASE_URL, PAYPAL_API_CLIENT_ID, PAYPAL_API_CLIENT_SECRET);
            return Redirect("Index");
        }

    }
}