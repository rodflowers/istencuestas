using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace istEncuestasMVC.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Index()
        {
            UszipProxy.USZipSoapClient obj = new UszipProxy.USZipSoapClient();

            return View(obj.GetInfoByState("NY"));
        }

      
    }
}