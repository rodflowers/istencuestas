using istEncuestasMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace istEncuestasMVC.Controllers
{
    public class EncuestaController : Controller
    {
        // GET: Encuesta
        public ActionResult Index()
        {

            XMLReader readXML = new XMLReader();
            var data = readXML.RetrunListOfEncuesta();

            return View(data.ToList());
            
        }

        public ActionResult SubFamilia()
        {
            return View();
        }
    }
}