using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailBomber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] toMails = {"mehdiyevsubhan1@gmail.com","mehdiyevsubhan00@gmail.com","kamran.azizov004@gmail.com" };

            string fromMail = "thedotnetchannelsender22@gmail.com";
            string fromPassword = "lgioenhkvchemfkrw";

            //Create a new email
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Test mail from Subhan";
            foreach (string mail in toMails)
            {
                message.To.Add(new MailAddress(mail));
            }
            message.Body = "Test body from Subhan";

            //SMTP configuration
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
            smtpClient.EnableSsl = true;

            //Sending message
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    smtpClient.Send(message);
                    var rnd = new Random();
                    int delayLimit = rnd.Next(5000,10000);
                    Thread.Sleep(delayLimit);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
