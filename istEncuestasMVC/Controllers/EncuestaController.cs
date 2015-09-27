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

        [HttpPost]
        public ActionResult SubFamilia(string encuestaid)
        {

            return Json(new { result = "Redirect", url = Url.Action("ListSubFamilia", "Encuesta", new  { iddet = encuestaid }) });
        }

        // GET: Encuesta
        public ActionResult ListSubFamilia(string iddet)
        {
            XMLReader readXML = new XMLReader();
            var data = readXML.RetrunListOfSubFamilia();

            return View(data.ToList());
        }

        [HttpPost]
        public ActionResult DetalleSubFamilia(string subfamid, int totpreg, int numpreg)
        {
            return Json(new { result = "Redirect", url = Url.Action("ListDetalleSubFamilia", "Encuesta", new { iddet = subfamid, totnum = totpreg, preg = numpreg }) });
        }

        // GET: Encuesta
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ListDetalleSubFamilia(string iddet, int totnum, int preg)
        {
            XMLReader readXML = new XMLReader();
            var data = readXML.RetrunListOfDetSubFamilia();
            var query = data.Where(p => p.Val_Par_Alf_Num == iddet);
            query = query.Where(p => p.Num_Orden == preg);
            return View(query.ToList());
        }

       



    }
}