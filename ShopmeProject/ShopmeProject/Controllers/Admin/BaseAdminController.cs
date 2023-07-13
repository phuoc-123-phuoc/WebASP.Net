using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Controllers.Admin
{
    [JwtAuthentication]
    public class BaseAdminController : BaseController
    {
    }
}