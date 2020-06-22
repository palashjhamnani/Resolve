using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Resolve.Data;
using Resolve.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Resolve.Helpers
{

    public class Notifications
    {
        private readonly IConfiguration _config;

        public Notifications(IConfiguration config)
        {
            _config = config;
        }

        public string SendEmail(LocalUser luser, string case_cid, string case_id)
        {
            //Sending notification
            string from_add = _config.GetValue<string>("Smtp:FromAddress");
            string server_add = _config.GetValue<string>("Smtp:Server");
            string email_pass = _config.GetValue<string>("Smtp:Password");
            int email_port = _config.GetValue<int>("Smtp:Port");
            string host_add = _config.GetValue<string>("Launch:Host_Name");
            string host_port = _config.GetValue<int>("Launch:Host_Port").ToString();
            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress("UW Resolve", from_add);
            message.From.Add(from);
            MailboxAddress to = new MailboxAddress(luser.FirstName, luser.LocalUserID);
            message.To.Add(to);
            message.Subject = "New Case [" + case_cid + "] Assigned";
            BodyBuilder bodyBuilder = new BodyBuilder();

            // Picking Template
            var fileName = $"Helpers/email_templates/case_creation.html";
            var body = File.ReadAllText(fileName);
            body = body.Replace("{first_name}", luser.FirstName)
            .Replace("{last_name}", luser.LastName)
            .Replace("{Resolve_Hostname}", host_add)
            .Replace("{Resolve_Port}", host_port)
            .Replace("{Resolve_CASEID}", case_id)
            .Replace("{Resolve_CASECID}", case_cid);
            bodyBuilder.HtmlBody = body;
            //bodyBuilder.TextBody = "Hello World!";
            message.Body = bodyBuilder.ToMessageBody();
            SmtpClient client = new SmtpClient();
            client.Connect(server_add, email_port, false);
            client.Authenticate(from_add, email_pass);
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
            return ("Sent");
        }
        
    }
}
