using istEncuestasMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using istEncuestasMVC.Helpers;
using System.Threading.Tasks;

namespace istEncuestasMVC.Controllers
{
    public class EncuestaController : Controller
    {



        // GET: Encuesta
        public ActionResult Index()
        {

            //XMLReader readXML = new XMLReader();
            //var data = readXML.RetrunListOfEncuesta();

            System.Xml.XmlDocument xDocumento = new System.Xml.XmlDocument();
            List<Encuesta> data = new List<Encuesta>();
            ServiceITLProxy.ServiceITL obj = new ServiceITLProxy.ServiceITL();
            var sRespuesta = obj.Encuesta("");

            if (sRespuesta != null)
            {
                xDocumento.LoadXml(sRespuesta);

                XMLReader readXML = new XMLReader();
                data = readXML.ReturnListOfEncuesta(xDocumento);

            }

            return View(data.ToList());

        }

        [HttpPost]
        public ActionResult SubFamilia(string encuestaid, string finalizada)
        {

            return Json(new { result = "Redirect", url = Url.Action("ListSubFamilia", "Encuesta", new { iddet = encuestaid, finalizada = finalizada }) });
        }

        // GET: Encuesta
        public ActionResult ListSubFamilia(string iddet, string finalizada)
        {

            var data = new List<Encuesta>();

            if (TempData["ListSubFamilia"] == null)
            {
                //XMLReader readXML = new XMLReader();
                //data = readXML.RetrunListOfSubFamilia(iddet);

                System.Xml.XmlDocument xDocumento = new System.Xml.XmlDocument();
                ServiceITLProxy.ServiceITL obj = new ServiceITLProxy.ServiceITL();
                var sRespuesta = obj.SubFamilia(iddet);

                if (sRespuesta != null)
                {
                    xDocumento.LoadXml(sRespuesta);

                    XMLReader readXML = new XMLReader();
                    data = readXML.ReturnListOfSubFamilia(xDocumento);
                    TempData["ListSubFamilia"] = data;
                }


            }
            else
            {
                data = TempData["ListSubFamilia"] as List<Encuesta>;
                TempData.Keep("ListSubFamilia");
            }

            //inicializa datos de SubFamilia 
            var myEnc = data.ToList();

            var found = myEnc.FirstOrDefault(c => c.ID_ENCUESTA_SUB == iddet);
            if (found != null)
            {
                found.Finalizada = "S";
            }


            return View(data.ToList());
        }

        [HttpPost]
        public ActionResult DetalleSubFamilia(string encid, string subfamid, int totpreg, int numpreg)
        {

            return Json(new { result = "Redirect", url = Url.Action("ListDetalleSubFamilia", "Encuesta", new { idenc = encid, idsubenc = subfamid, totnum = totpreg, preg = numpreg }) });
        }

        // GET: Encuesta
        public ActionResult ListDetalleSubFamilia(string idenc, string idsubenc, int totnum, int preg)
        {

            ViewBag.encuestaid = idenc;
            ViewBag.grupoen = idenc;
            ViewBag.subencid = idsubenc;
            ViewBag.totpreg = totnum;
            ViewBag.pregunta = preg;


            List<Encuesta> data = new List<Encuesta>();
            System.Xml.XmlDocument xDocumento = new System.Xml.XmlDocument();
            ServiceITLProxy.ServiceITL obj = new ServiceITLProxy.ServiceITL();
            var sRespuesta = obj.Detalle(idenc, idsubenc);

            if (sRespuesta != null)
            {
                xDocumento.LoadXml(sRespuesta);

                XMLReader readXML = new XMLReader();
                data = readXML.ReturnListOfDetalle(xDocumento);

                var query = data.Where(p => p.ID_ENCUESTA == idenc);
                query = query.Where(p => p.ID_ENCUESTA_SUB == idsubenc);
                query = query.Where(p => p.Num_Orden == preg);

                var myList = query.ToList();

                //inicializa datos de encuesta 
                if (TempData["Encuesta"] == null)
                {
                    Encuesta enc = new Encuesta();
                    List<Encuesta> lisobj = new List<Encuesta>();
                    TempData["Encuesta"] = lisobj;
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
                    enc.ID_ENCUESTA = myList[0].ID_ENCUESTA;
                    enc.ID_ENCUESTA_SUB = myList[0].ID_ENCUESTA_SUB;
                    enc.NOM_ENCUESTA_SUB = myList[0].NOM_ENCUESTA_SUB;
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

            //lee datos XML
            //XMLReader readXML = new XMLReader();
            //var data = readXML.RetrunListOfDetSubFamilia();
            //var query = data.Where(p => p.Val_Par_Alf_Num == ViewBag.grupoen);
            //query = query.Where(p => p.Num_Orden == ViewBag.pregunta);

            //var myList = query.ToList();

            return View();

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

        [HttpPost]
        public ActionResult FinIndex(string encuestaid, string finalizada)
        {

            return Json(new { result = "Redirect", url = Url.Action("Observacion", "Encuesta") });
            //return Json(new { result = "Redirect", url = Url.Action("Index", "Encuesta") });
        }

        [HttpPost]
        public ActionResult FinBack(string encuestaid, string finalizada)
        {

            return Json(new { result = "Redirect", url = Url.Action("Index", "Encuesta") });

        }


        // GET: Encuesta
        public ActionResult Observacion()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Enviar(string encuestaid, string finalizada)
        {
            Correo correo = new Correo();
            ClsCreaPdf.ReportPdf clsrep = new ClsCreaPdf.ReportPdf();
            string Str_Error = String.Empty;
            string strruta_server = (Server.MapPath("~/Report/"));
            string url = clsrep.GeneraPdf("ITL1",
                                          "ISAPRE CONSALUD",
                                          "1.111.111-1",
                                          "PEDRO FONTOVA 6650",
                                          "contacto@consalud.cl",
                                          strruta_server,
                                          "1,1;2,2;3,5;4,3;7,2;8,3",
                                          ref Str_Error);

            var mail2 = TempData["Correo"];
            TempData.Keep("Correo");
            //string mail = correo.SendEmail((string)mail2, url);
            var mail = await correo.EnviaCorreo((string)mail2, url);
            //string mail = Correo. SendEmail("jochin33@gmail.com", url);

            

            return Json(new { result = "Redirect", url = Url.Action("Index", "Encuesta") });

        }


    }
}