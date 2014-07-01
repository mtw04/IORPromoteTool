using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using IORPromoteTool.Models;
using LeankitLibrary.EntitiesCustom;

namespace IORPromoteTool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Helen: To fix default re-route issue

            //Works around an issue where angular routing translates http://server/virtualdirectory as 
            //http://server/virtualdirectory#/virtualdirectory instead of http://server/virtualdirectory/#/Index
            //This can be fixed in angular as well, using the following patch file:
            //Misc\Angular Patches\fix issue when there is no trailing space on the virtual directory - angular.js.patch
         
            if (this.HttpContext != null && this.HttpContext.Request != null)
            {
                string url = this.HttpContext.Request.RawUrl;
                if (url != "/" && string.Equals(url, this.HttpContext.Request.ApplicationPath, StringComparison.OrdinalIgnoreCase))
                {
                    return this.Redirect(this.HttpContext.Request.ApplicationPath + "/");
                }
            }

            return View();
        }
        
    }
}
