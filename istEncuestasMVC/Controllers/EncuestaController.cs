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
        public ActionResult SubFamilia(string encuestaid, string finalizada)
        {

            return Json(new { result = "Redirect", url = Url.Action("ListSubFamilia", "Encuesta", new  { iddet = encuestaid, finalizada = finalizada }) });
        }

        // GET: Encuesta
        public ActionResult ListSubFamilia(string iddet, string finalizada)
        {
            XMLReader readXML = new XMLReader();
            var data = readXML.RetrunListOfSubFamilia();



            //inicializa datos de SubFamilia 
            var myEnc = data.ToList();

            var found = myEnc.FirstOrDefault(c => c.Val_Par_Alf_Num == iddet);
            if (found != null)
            {
                found.Finalizada = "S";
            }


            return View(data.ToList());
        }

        [HttpPost]
        public ActionResult DetalleSubFamilia(string subfamid, int totpreg, int numpreg)
        {
           
            return Json(new { result = "Redirect", url = Url.Action("ListDetalleSubFamilia", "Encuesta", new { iddet = subfamid, totnum = totpreg, preg = numpreg }) });
        }

        // GET: Encuesta
        public ActionResult ListDetalleSubFamilia(string iddet, int totnum, int preg)
        {

            ViewBag.grupoen = iddet;
            ViewBag.totpreg = totnum;
            ViewBag.pregunta = preg;

            //lee datos XML
            XMLReader readXML = new XMLReader();
            var data = readXML.RetrunListOfDetSubFamilia();
            var query = data.Where(p => p.Val_Par_Alf_Num == ViewBag.grupoen);
            query = query.Where(p => p.Num_Orden == ViewBag.pregunta);

            var myList = query.ToList();

            //inicializa datos de encuesta 
            if (TempData["Encuesta"] == null)
            { 
                Encuesta enc = new Encuesta();
                List<Encuesta> obj = new List<Encuesta>();
                TempData["Encuesta"] = obj;
            }

            //manipulo datos encuesta
            var obj2 = TempData["Encuesta"] as List<Encuesta>;
            TempData.Keep("Encuesta");
        
            var found = obj2.FirstOrDefault(c => c.Cod_Num == myList[0].Cod_Num);
            if (found != null)
            {
                found.Respuesta = "2";
                TempData["Encuesta"] = obj2;
            }
            else
            {
                Encuesta enc = new Encuesta();
                enc.Cod_Par_Acceso = myList[0].Cod_Par_Acceso;
                enc.Val_Par_Alf_Num = myList[0].Val_Par_Alf_Num;
                enc.Cod_Num = myList[0].Cod_Num;
                enc.Num_Orden = myList[0].Num_Orden;
                enc.Gls_Pregunta = myList[0].Gls_Pregunta;
                enc.Cant_Preguntas = myList[0].Cant_Preguntas;
                enc.Respuesta = "1";
               

                obj2.Add(enc);
                TempData["Encuesta"] = obj2;
            }

           

            return View(query.ToList());
            
            
        }

        [HttpPost]
        public ActionResult ActualizaRespuesta(int codnum, string resp)
        {

            //manipulo datos encuesta
            var obj2 = TempData["Encuesta"] as List<Encuesta>;
            TempData.Keep("Encuesta");

            var found = obj2.FirstOrDefault(c => c.Cod_Num == codnum);
            if (found != null)
            {
                found.Respuesta = resp;
                TempData["Encuesta"] = obj2;
            }

            return View();
        }






    }
}