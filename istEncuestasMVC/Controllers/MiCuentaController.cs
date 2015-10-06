using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using istEncuestasMVC.Models;
using System.Web.Security;

namespace istEncuestasMVC.Controllers
{
    public class MiCuentaController : Controller
    {
      
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login l, string ReturnUrl = "" )
        {
            if (ModelState.IsValid)
            { 

                Service700vipProxy.Service700vip obj = new Service700vipProxy.Service700vip();
                var value = obj.WsIstSemEmpIngreso(l.Username.PadLeft(15, '0'), l.Password);

                if (value != null)
                {
                    if (value != "N")
                    {
                        FormsAuthentication.SetAuthCookie(l.Username, l.RememberMe);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("ShowEmpresa", "Home");
                        }
                    }
                }
                ModelState.Remove("Password");

            }
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Logout", "MiCuenta");
        }
    }
}