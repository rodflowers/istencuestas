using istEncuestasMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using istEncuestasMVC.Helpers;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Web.Hosting;
using System.IO;

namespace istEncuestasMVC.Controllers
{
    public class EncuestaController : Controller
    {



        // GET: Encuesta
        public ActionResult Index()
        {

            //XMLReader readXML = new XMLReader();
            //var data = readXML.RetrunListOfEncuesta();

            //Session["Resp"] = null;
            Session["dictionary"] = null;
            var dictionary = new Dictionary<string, string>();
            Session["dictionary"] = dictionary;
            Session["ENCUESTA_ID"] = null;

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

            //Cargo variable encuesta ID & Respuestas ini
            if (Session["ENCUESTA_ID"] == null)
            {
                Session["ENCUESTA_ID"] = iddet;
            }
            

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
                    //ViewBag.contadorSE = data.Count;
                    Session["contadorSE"] = data.Count;
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
        public ActionResult DetalleSubFamilia(string encid, string subfamid, int totpreg, int numpreg, int countse)
        {
           
            if (Session["Click"] == null)
            {
                Session["Click"] = 0;
            }
            int click = (int)Session["Click"];
            Session["Click"] = click + 1;           

            return Json(new { result = "Redirect", url = Url.Action("ListDetalleSubFamilia", "Encuesta", new { idenc = encid, idsubenc = subfamid, totnum = totpreg, preg = numpreg, contadorse = countse }) });
        }

        // GET: Encuesta
        public ActionResult ListDetalleSubFamilia(string idenc, string idsubenc, int totnum, int preg, int contadorse)
        {
           
            ViewBag.encuestaid = idenc;
            ViewBag.grupoen = idenc;
            ViewBag.subencid = idsubenc;
            ViewBag.totpreg = totnum;
            ViewBag.pregunta = preg;
            //contador sub encuestas

            //ViewBag.contadorse = contadorse;
            //Session["ContadorSE"] = contadorse;
           

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


            //Guarda respuesta
            string _resp = "";
            if (Session["Resp"] != null)
            {
                _resp = string.Concat(Session["Resp"].ToString(), ";");
            }

            _resp = string.Concat(_resp, codnum, ",", resp);

            Session["Resp"] = _resp;

            //Guarda respuesta 2
            Dictionary<string, string> dictionary = (Dictionary<string, string>)Session["dictionary"];

            if (dictionary.ContainsKey(codnum.ToString()))
            {
                dictionary[codnum.ToString()] = resp;
            }
            else
            {
                dictionary.Add(codnum.ToString(), resp.ToString());
            }

            Session["dictionary"] = dictionary;

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
        public async Task<ActionResult> Enviar(string observacion)
        //public async Task<ActionResult> Enviar(string encuestaid, string finalizada)
        
        {
            Session["observacion"] = observacion;
            Session["Click"] = null;
            Session["ContadorSE"] = null;

           

            Dictionary<string, string> dictionary = (Dictionary<string, string>)Session["dictionary"];
            string respuestas = "";
           

            foreach (var item in dictionary)
            {
                if (respuestas == "")
                {
                    respuestas = string.Concat(item.Key, ",", item.Value);
                }
                else
                {
                    respuestas = string.Concat(respuestas, ";", item.Key, ",", item.Value);
                }

                
            }

            Session["Resp"] = respuestas; 
            GuardaEncuesta();

            //new StreamWriter(Server.MapPath("~/Report/prueba.txt"), true);

            Correo correo = new Correo();
            ClsCreaPdf.ReportPdf clsrep = new ClsCreaPdf.ReportPdf();
            string Str_Error = String.Empty;

            string strruta_server = (Server.MapPath(@"~/Report/"));

            //string _respuestas = Session["Resp"].ToString();
            string url = clsrep.GeneraPdf(Session["ENCUESTA_ID"].ToString(),
                                           Session["NombreEmpresa"].ToString(),
                                          Session["RutEmpresa"].ToString(),
                                          "",
                                          Session["RepEmail"].ToString(),
                                          strruta_server,
                                          respuestas,
                                          Session["RepNombre"].ToString(),
                                          observacion,
                                          ref Str_Error);

            var mail2 = Session["RepEmail"]; //TempData["Correo"];
            TempData.Keep("Correo");
            //string mail = correo.SendEmail((string)mail2, url);
            var mail = await correo.EnviaCorreo((string)mail2, url, Session["RepNombre"].ToString(), Session["RepCargo"].ToString(), Session["NombreEmpresa"].ToString());
            //mail2 = "rodrigo.flores01@gmail.com";

           if (mail == "S")
            {
                return Json(new { result = "Redirect", url = Url.Action("Index", "Encuesta") });
            }
           else
            {
                return Json(new { result = "Redirect", url = Url.Action("Error", "Encuesta") });
            }

        }


        // GET: Encuesta
        public ActionResult Error(string desc_err)
        {
            return View(desc_err);
        }

        public void GuardaEncuesta()
        {
            ServiceITLProxy.ServiceITL obj = new ServiceITLProxy.ServiceITL();
           
            try
            {
                string[] rutemp = Session["RutEmpresa"].ToString().Split('-');
                string eid = Session["ENCUESTA_ID"].ToString();
                string res = Session["Resp"].ToString();
                string obs = Session["observacion"].ToString();
                string rute = rutemp[0].PadLeft(15, '0').Trim();
                string reprut = Session["RepRut"].ToString().PadLeft(15, '0').Trim();
                string rnom = Session["RepNombre"].ToString();
                string rcar = Session["RepCargo"].ToString();
                string remail = Session["RepEmail"].ToString();
                string tele = Session["RepTelefono"].ToString();
                
                string resp = obj.IngresarRespEncuesta(0, eid, res, obs, rute, rute, rute, rnom, rcar, remail, tele, eid, eid);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            finally
            {
                obj = null;
            }
        }

    }

    

}
