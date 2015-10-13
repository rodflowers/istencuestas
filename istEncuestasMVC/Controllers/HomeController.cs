using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using istEncuestasMVC.Models;

namespace istEncuestasMVC.Controllers
{
    public class HomeController : Controller
    {

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ShowEmpresa()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowEmpresa(Empresa e)
        {
            if (ModelState.IsValid)
            {

                
                TempData.Add("Correo", e.RepEmail);

                servMEDAtencionProxy.servMEDAtencion obj = new servMEDAtencionProxy.servMEDAtencion();
                var sRespuesta = obj.wsValidaEmpSiso(e.RutEmpresa);

                System.Xml.XmlDocument xDocumento = new System.Xml.XmlDocument();



                if (sRespuesta != null)
                {

                    xDocumento.LoadXml(sRespuesta);
                    

                    XMLReader readXML = new XMLReader();
                    var data = readXML.ReturnListOfEmpresa(xDocumento);

                    if (data[0].Codigo == 1)
                    {
                        return RedirectToAction("Index", "Encuesta");
                    }

                    
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult RazonSocial(string rutempresa)
        {         
            System.Xml.XmlDocument xDocumento = new System.Xml.XmlDocument();
            servMEDAtencionProxy.servMEDAtencion obj = new servMEDAtencionProxy.servMEDAtencion();
            var sRespuesta = obj.wsValidaEmpSiso(rutempresa);

            if (sRespuesta != null)
            {
                xDocumento.LoadXml(sRespuesta);

                XMLReader readXML = new XMLReader();
                var data = readXML.ReturnListOfEmpresa(xDocumento);

                if (data[0].GlsError != null)
                {
                    string razonsocial = data[0].GlsError;
                    string[] razonsocial2 = razonsocial.Split(';');

                    if (razonsocial2.Count() > 1)
                    {
                        return Json(new { razonsocial = razonsocial2[1] });
                    }
                    else
                    {
                        return Json(new { razonsocial = razonsocial2[0] });
                    }
                }
            }

            return Json(new { razonsocial = "No existe" });
          
        }

        public ActionResult RazonSocialView(string razonsocial)
        {
            return View(razonsocial);
        }

        public ActionResult About()
        {
            ViewBag.Message = "I like games.";

            return View();
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Hola que tal!!!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}