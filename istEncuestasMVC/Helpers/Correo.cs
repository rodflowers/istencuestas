using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

    }
}