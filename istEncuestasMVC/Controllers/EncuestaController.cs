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

            Session["Resp"] = null;

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


            //new StreamWriter(Server.MapPath(@"~/Report/prueba.txt"), true);

            return View();
            //new StreamWriter(Server.MapPath("~/Report/release_notification_emails.txt"), true);

            //Correo correo = new Correo();
            //ClsCreaPdf.ReportPdf clsrep = new ClsCreaPdf.ReportPdf();
            //string Str_Error = String.Empty;
            //string strruta_server = (Server.MapPath("~/Report/"));
            ////string strruta_server = HostingEnvironment.ApplicationPhysicalPath + "Report";
            ////string strruta_server = AppDomain.CurrentDomain.GetData("Report").ToString();
            //string url = clsrep.GeneraPdf("ITL1",
            //                              "ISAPRE CONSALUD",
            //                              "1.111.111-1",
            //                              "PEDRO FONTOVA 6650",
            //                              "contacto@consalud.cl",
            //                              strruta_server,
            //                              "1,1;2,2;3,5;4,3;7,2;8,3",
            //                              ref Str_Error);

            //var mail2 = TempData["Correo"];
            //TempData.Keep("Correo");
            ////string mail = correo.SendEmail((string)mail2, url);
            ////var mail = await correo.EnviaCorreo((string)mail2, url);
            ////mail2 = "rodrigo.flores01@gmail.com";

            //var appSettings = ConfigurationManager.AppSettings;
            //MailAddress from = new MailAddress(appSettings["UserName_correo"], "IST");
            //var message = new MailMessage();
            ////var body = "";
            //try
            //{

            //    message.To.Add(new MailAddress((string)mail2));  // replace with valid value 
            //    message.From = new MailAddress(appSettings["UserName_correo"], "IST");  // replace with valid value
            //    message.Subject = appSettings["Subject"];
            //    message.Body = appSettings["Body"];
            //    message.Attachments.Add(new Attachment(url));
            //    message.IsBodyHtml = true;

            //    using (var smtp = new SmtpClient())
            //    {
            //        smtp.UseDefaultCredentials = false;
            //        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //        var credential = new NetworkCredential
            //        {
            //            UserName = appSettings["UserName_correo"],  // replace with valid value
            //            Password = appSettings["Password_correo"]  // replace with valid value
            //        };
            //        smtp.UseDefaultCredentials = false;
            //        smtp.Credentials = credential;
            //        smtp.Host = appSettings["server_correo"];
            //        smtp.Port = Convert.ToInt16(appSettings["Puerto_correo"]);
            //        smtp.EnableSsl = Convert.ToBoolean(appSettings["Ssl_correo"]);

            //        smtp.Send(message);                  

            //        return View();
                    
            //    }
            //}
            //catch (SmtpException ex)
            //{
            //    message.Dispose();
            //    return RedirectToAction("Error", "Encuesta", new { desc_err = ex.Message });                
            //}
        }



        [HttpPost]
        public ActionResult Enviar(string encuestaid, string finalizada)
        {

            Session["Click"] = null;
            Session["ContadorSE"] = null;

            //new StreamWriter(Server.MapPath("~/Report/prueba.txt"), true);

            Correo correo = new Correo();
            ClsCreaPdf.ReportPdf clsrep = new ClsCreaPdf.ReportPdf();
            string Str_Error = String.Empty;
            
            string strruta_server = (Server.MapPath(@"~/Report/"));
            //string strruta_server = HostingEnvironment.ApplicationPhysicalPath + "Report";
            //string strruta_server = AppDomain.CurrentDomain.GetData("Report").ToString();
            string _respuestas = Session["Resp"].ToString();
            string url = clsrep.GeneraPdf("ITL1",
                                          "ISAPRE CONSALUD",
                                          "1.111.111-1",
                                          "PEDRO FONTOVA 6650",
                                          "contacto@consalud.cl",
                                          strruta_server,
                                          _respuestas,
                                          ref Str_Error);

            var mail2 = TempData["Correo"];
            TempData.Keep("Correo");
            //string mail = correo.SendEmail((string)mail2, url);
            //var mail = await correo.EnviaCorreo((string)mail2, url);
            //mail2 = "rodrigo.flores01@gmail.com";

            var appSettings = ConfigurationManager.AppSettings;
            MailAddress from = new MailAddress(appSettings["UserName_correo"], "IST");
            var message = new MailMessage();
            //var body = "";
            try
            {

                message.To.Add(new MailAddress((string)mail2));  // replace with valid value 
                message.From = new MailAddress(appSettings["UserName_correo"], "IST");  // replace with valid value
                message.Subject = appSettings["Subject"];
                message.Body = appSettings["Body"];
                message.Attachments.Add(new Attachment(url));
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    var credential = new NetworkCredential
                    {
                        UserName = appSettings["UserName_correo"],  // replace with valid value
                        Password = appSettings["Password_correo"]  // replace with valid value
                    };
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Host = appSettings["server_correo"];
                    smtp.Port = Convert.ToInt16(appSettings["Puerto_correo"]);
                    smtp.EnableSsl = Convert.ToBoolean(appSettings["Ssl_correo"]);

                    smtp.Send(message);

                    return Json(new { result = "Redirect", url = Url.Action("Index", "Encuesta") });

                }
            }
            catch (SmtpException ex)
            {
                message.Dispose();
                return Json(new { result = "Redirect", url = Url.Action("Error", "Encuesta") });
                //return RedirectToAction("Error", "Encuesta", new { desc_err = ex.Message });
            }

            

            //Correo correo = new Correo();
            //ClsCreaPdf.ReportPdf clsrep = new ClsCreaPdf.ReportPdf();
            //string Str_Error = String.Empty;
            //string strruta_server = (Server.MapPath("~/Report/"));
            //string url = clsrep.GeneraPdf("ITL1",
            //                              "ISAPRE CONSALUD",
            //                              "1.111.111-1",
            //                              "PEDRO FONTOVA 6650",
            //                              "contacto@consalud.cl",
            //                              strruta_server,
            //                              "1,1;2,2;3,5;4,3;7,2;8,3",
            //                              ref Str_Error);

            //var mail2 = TempData["Correo"];
            //TempData.Keep("Correo");
            ////string mail = correo.SendEmail((string)mail2, url);
            ////var mail = await correo.EnviaCorreo((string)mail2, url);
            ////mail2 = "rodrigo.flores01@gmail.com";

            //var appSettings = ConfigurationManager.AppSettings;
            //MailAddress from = new MailAddress(appSettings["UserName_correo"], "IST");
            //var message = new MailMessage();
            ////var body = "";
            //try
            //{

            //    message.To.Add(new MailAddress((string)mail2));  // replace with valid value 
            //    message.From = new MailAddress(appSettings["UserName_correo"], "IST");  // replace with valid value
            //    message.Subject = appSettings["Subject"];
            //    message.Body = appSettings["Body"];
            //    //message.Attachments.Add(new Attachment(Url));
            //    message.IsBodyHtml = true;

            //    using (var smtp = new SmtpClient())
            //    {
            //        smtp.UseDefaultCredentials = false;
            //        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //        var credential = new NetworkCredential
            //        {
            //            UserName = appSettings["UserName_correo"],  // replace with valid value
            //            Password = appSettings["Password_correo"]  // replace with valid value
            //        };
            //        smtp.UseDefaultCredentials = false;
            //        smtp.Credentials = credential;
            //        smtp.Host = appSettings["server_correo"];
            //        smtp.Port = Convert.ToInt16(appSettings["Puerto_correo"]);
            //        smtp.EnableSsl = Convert.ToBoolean(appSettings["Ssl_correo"]);


            //        smtp.Send(message);
            //        //smtp.SendMail(message);
            //        //smtp.Send(message);


            //        return Json(new { result = "Redirect", url = Url.Action("Index", "Encuesta") });
            //    }
            //}
            //catch (SmtpException ex)
            //{
            //    //message.Dispose();               
            //    return Json(new { result = "Redirect", url = Url.Action("Error", "Encuesta", new { desc_err = ex.Message }) });
            //}
        }


        // GET: Encuesta
        public ActionResult Error(string desc_err)
        {
            return View(desc_err);
        }


    }

}
