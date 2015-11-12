using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace istEncuestasMVC.Helpers
{
    public class Correo
    {

        public string SendEmail(string Recip, string Url)
        {

            System.Net.Mail.MailMessage Email = new System.Net.Mail.MailMessage();
            try
            {

                var appSettings = ConfigurationManager.AppSettings;
                bool BSsl = Convert.ToBoolean(appSettings["Ssl_correo"]);


                MailAddress from = new MailAddress(appSettings["UserName_correo"], "IST");
                Email.To.Add(Recip);

                Email.Subject = appSettings["Subject"];
                Email.SubjectEncoding = System.Text.Encoding.UTF8;
                Email.Body = appSettings["Body"];
                Email.BodyEncoding = System.Text.Encoding.UTF8;
                Email.IsBodyHtml = false;
                Email.Attachments.Add(new Attachment(Url));
                Email.From = from;
                System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
                cliente.Port = Convert.ToInt16(appSettings["Puerto_correo"]);
                cliente.UseDefaultCredentials = true;
                cliente.Credentials = new System.Net.NetworkCredential(appSettings["UserName_correo"], appSettings["Password_correo"]);
                cliente.Host = appSettings["server_correo"];
                cliente.EnableSsl = BSsl;
                cliente.Send(Email);

                Email.Dispose();
                return "OK";
            }
            catch (SmtpException ex)
            {
                Email.Dispose();
                return (ex.Message + "Smtp.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Email.Dispose();
                return "Sending Email Failed. Check Port Number.";
            }
            catch (InvalidOperationException Ex)
            {
                Email.Dispose();
                return "Sending Email Failed. Check Port Number.";
            }

           
        }


        public async System.Threading.Tasks.Task<string> EnviaCorreo(string Recip, string Url, string repemp, string cargemp, string razsoc)
        {
            var appSettings = ConfigurationManager.AppSettings;
            MailAddress from = new MailAddress(appSettings["UserName_correo"], "IST");
            MailAddress StrCC = new MailAddress(appSettings["ConCopia"], "IST");
            var message = new MailMessage();
            //var body = "";
            try
            {
                const string quote = "\"";
                var StrCuerpoMailIni = "<!DOCTYPE html PUBLIC " + quote + "-//W3C//DTD XHTML 1.0 Transitional//EN" + quote + " " + quote + "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" + quote + "><html xmlns=" + quote + "http://www.w3.org/1999/xhtml" + quote + "><head><title></title><style TYPE=" + quote + "text/css" + quote + "><!--PageIntro{font-family: Verdana, Arial, Helvetica, sans-serif;font-SIZE: 11px;font-weight: NORMAL;color: #666666;text-align: left;}--></style></head><body>";
                var StrCuerpo = appSettings["CorreoBody"];
                var StrCuerpoMailFin = "</body></html>";
                
                StrCuerpo = StrCuerpo.Replace("representante_empresa", repemp).Replace("cargo_empresa", cargemp).Replace("razon_social", razsoc);

              
                message.To.Add(new MailAddress((string)Recip));  // replace with valid value 
                message.From = new MailAddress(appSettings["UserName_correo"], "IST");  // replace with valid value

                message.CC.Add(StrCC);

                message.Subject = appSettings["Subject"];
                message.Body = StrCuerpoMailIni + StrCuerpo + StrCuerpoMailFin;
                message.Attachments.Add(new Attachment(Url));
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

                    await smtp.SendMailAsync(message);

                    return "S";

                    //return Json(new { result = "Redirect", url = Url.Action("Index", "Encuesta") });

                }
            }
            catch (SmtpException ex)
            {
                message.Dispose();
                return (ex.Message);

                //return Json(new { result = "Redirect", url = Url.Action("Error", "Encuesta") });
                //return RedirectToAction("Error", "Encuesta", new { desc_err = ex.Message });
            }
            //var appSettings = ConfigurationManager.AppSettings;
            //MailAddress from = new MailAddress(appSettings["UserName_correo"], "IST");
            //var message = new MailMessage();
            ////var body = "";
            //try
            //{

            //    message.To.Add(new MailAddress(Recip));  // replace with valid value 
            //    message.From = new MailAddress(appSettings["UserName_correo"], "IST");  // replace with valid value
            //    message.Subject = appSettings["Subject"];
            //    message.Body = appSettings["Body"];
            //    //message.Attachments.Add(new Attachment(Url));
            //    message.IsBodyHtml = false;

            //    using (var smtp = new SmtpClient())
            //    {
            //        smtp.UseDefaultCredentials = false;
            //        var credential = new NetworkCredential
            //        {
            //            UserName = appSettings["UserName_correo"],  // replace with valid value
            //            Password = appSettings["Password_correo"]  // replace with valid value
            //        };
            //        smtp.Credentials = credential;
            //        smtp.Host = appSettings["server_correo"];
            //        smtp.Port = Convert.ToInt16(appSettings["Puerto_correo"]);
            //        smtp.EnableSsl = Convert.ToBoolean(appSettings["Ssl_correo"]);

            //        await smtp.SendMailAsync(message);
            //        //smtp.SendMail(message);
            //        //smtp.Send(message);


            //        return "S";
            //    }
            //}
            //catch (SmtpException ex)
            //{
            //    message.Dispose();
            //    return (ex.Message + "Smtp.");
            //}
        }
    }

}
