using System.Net.Mail;
using System.Net;
using Store.Model;

namespace App.Services
{
    public class EmailService
    {
        public static int SendMail(Email email, string sender, string password)
        {
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            m.From = new MailAddress(sender, email.DisplayName);

            foreach (var recipient in email.Recipients)
            {
                m.To.Add(recipient);
            }

            m.Subject = email.Subject;
            m.Body = email.Message;
            m.IsBodyHtml = true;

            sc.Host = "smtp.gmail.com";
            string str1 = "gmail.com";
            string str2 = sender.ToLower();

            if (str2.Contains(str1))
            {
                try
                {
                    sc.Port = 587;
                    sc.Credentials = new NetworkCredential(sender, password);
                    sc.EnableSsl = false;
                    sc.Send(m);
                    return 1;
                }
                catch (Exception)
                {
                    //Response.Write("<BR><BR>* Please double check the From Address and Password to confirm that both of them are correct. <br>");
                    //Response.Write("<BR><BR>If you are using gmail smtp to send email for the first time, please refer to this KB to setup your gmail account: http://www.smarterasp.net/support/kb/a1546/send-email-from-gmail-with-smtp-authentication-but-got-5_5_1-authentication-required-error.aspx?KBSearchID=137388");
                    //Response.End();
                    return 0;
                }
            }
            else
            {
                try
                {
                    sc.Port = 25;
                    sc.Credentials = new NetworkCredential(sender, password);
                    sc.EnableSsl = false;
                    sc.Send(m);
                    return 1;
                    //Response.Write("Email Send successfully");
                }
                catch (Exception)
                {
                    //Response.Write("<BR><BR>* Please double check the From Address and Password to confirm that both of them are correct. <br>");
                    //Response.End();
                    return 0;
                }
            }
        }

        public static void FastMail(string receiverEmail, string subject, string body)
        {
            // Sender's email address and password
            string senderEmail = "salmatraglobal@gmail.com";
            string senderPassword = "Salm1!0#009";

            // Receiver's email address

            // Create and configure the SMTP client
            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                // Create the email message
                using (var message = new MailMessage(senderEmail, receiverEmail))
                {
                    message.Subject = subject;
                    message.Body = body;

                    try
                    {
                        // Send the email
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to send email. Error message: " + ex.Message);
                    }
                }
            }
        }
    }

}