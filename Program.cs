using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExcelDataReader;

namespace MailBomber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Read email details from excel file
            var mailsDictionary = new Dictionary<int, object[]>();

            using (FileStream fileStream = File.OpenRead("Mails.xlsx"))
            {
                using (var reader = ExcelReaderFactory.CreateReader(fileStream))
                {
                    var config = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = false
                        }
                    };

                    var mailsDataSet = reader.AsDataSet(config);
                    var mailDataTable = mailsDataSet.Tables[0];

                    for (int i = 1; i < mailDataTable.Rows.Count; i++)
                    {
                        mailsDictionary.Add(i, mailDataTable.Rows[i].ItemArray);
                    }
                }
            }

            //Create a new emails
            string fromMail = "thedotnetchannelsender22@gmail.com";
            string fromPassword = "lgioenhkvchemfkrw";

            List<MailMessage> messages = new List<MailMessage>();
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);

            foreach (var mailDetails in mailsDictionary.Values)
            {
                message.To.Add(new MailAddress(mailDetails[0].ToString()));
                message.Subject = mailDetails[1].ToString();
                message.Body = mailDetails[2].ToString();

                messages.Add(message);
            }

            //SMTP configuration
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
            smtpClient.EnableSsl = true;

            //Sending messages
            try
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    smtpClient.Send(message);
                    Console.WriteLine("Email message was sent succesfully.");

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
