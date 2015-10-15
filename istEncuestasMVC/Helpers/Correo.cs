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


        public async System.Threading.Tasks.Task<string> EnviaCorreo(string Recip, string Url)
        {
            var appSettings = ConfigurationManager.AppSettings;
            MailAddress from = new MailAddress(appSettings["UserName_correo"], "IST");
            var message = new MailMessage();
            //var body = "";
            try
            {
                
                message.To.Add(new MailAddress(Recip));  // replace with valid value 
                message.From = new MailAddress(appSettings["UserName_correo"], "IST");  // replace with valid value
                message.Subject = appSettings["Subject"];
                message.Body = appSettings["Body"];
                message.Attachments.Add(new Attachment(Url));
                message.IsBodyHtml = false;

                using (var smtp = new SmtpClient())
                {
                    smtp.UseDefaultCredentials = false;
                    var credential = new NetworkCredential
                    {
                        UserName = appSettings["UserName_correo"],  // replace with valid value
                        Password = appSettings["Password_correo"]  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = appSettings["server_correo"];
                    smtp.Port = Convert.ToInt16(appSettings["Puerto_correo"]);
                    smtp.EnableSsl = Convert.ToBoolean(appSettings["Ssl_correo"]);
   
                    await smtp.SendMailAsync(message);
                    //smtp.SendMail(message);
                    //smtp.Send(message);


                    return "S";
                }
            }
            catch (SmtpException ex)
            {
                message.Dispose();
                return (ex.Message + "Smtp.");
            }
        }
    }

}
