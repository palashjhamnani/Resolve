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

        public string SendEmail(LocalUser luser, string case_cid, string case_id, string template, LocalUser comment_by = null, string comment_on_case = null, string group_name = null, Case related_case = null)
        {
            try
            {
                //Sending notification
                string from_add = _config.GetValue<string>("Smtp:FromAddress");
                string server_add = _config.GetValue<string>("Smtp:Server");
                string email_pass = _config.GetValue<string>("Smtp:Password");
                int email_port = _config.GetValue<int>("Smtp:Port");
                string host_add = _config.GetValue<string>("Launch:Host_Name");
                string host_port = _config.GetValue<string>("Launch:Host_Port");
                MimeMessage message = new MimeMessage();
                MailboxAddress from = new MailboxAddress("SOD RequestManager", from_add);
                message.From.Add(from);
                MailboxAddress to = new MailboxAddress(luser.FirstName, luser.LocalUserID);
                message.To.Add(to);

                // Copy Admin (anyal@uw.edu) to every email send out by Resolve
                //MailboxAddress copy = new MailboxAddress("palashj@uw.edu");                
                //message.Cc.Add(copy);

                BodyBuilder bodyBuilder = new BodyBuilder();
                // Picking Template
                if (template == "assignment")
                {
                    var fileName = $"wwwroot/email_templates/case_assignment.html";
                    message.Subject = "New Case [" + case_cid + "] Assigned";
                    var body = File.ReadAllText(fileName);
                    body = body.Replace("{first_name}", luser.FirstName)
                    .Replace("{last_name}", luser.LastName)
                    .Replace("{Resolve_Hostname}", host_add)
                    .Replace("{Resolve_Port}", host_port)
                    .Replace("{Resolve_CASEID}", case_id)
                    .Replace("{Resolve_CASECID}", case_cid)
                    .Replace("{Resolve_UserID}", luser.LocalUserID)
                    .Replace("{Case_Type}", related_case.CaseType.CaseTypeTitle)
                    .Replace("{Created_By}", related_case.LocalUserID)
                    .Replace("{Created_On}", related_case.CaseCreationTimestamp.ToString())
                    .Replace("{Case_Description}", related_case.Description);
                    bodyBuilder.HtmlBody = body;
                    //bodyBuilder.TextBody = "Hello World!";
                    message.Body = bodyBuilder.ToMessageBody();
                }
                else
                    if (template == "g_assignment")
                    {
                        var fileName = $"wwwroot/email_templates/group_case_assignment.html";
                        message.Subject = "New Case [" + case_cid + "] Assigned";
                        var body = File.ReadAllText(fileName);
                        body = body.Replace("{first_name}", luser.FirstName)
                        .Replace("{last_name}", luser.LastName)
                        .Replace("{Resolve_Hostname}", host_add)
                        .Replace("{group_name}", group_name)
                        .Replace("{Resolve_Port}", host_port)
                        .Replace("{Resolve_CASEID}", case_id)
                        .Replace("{Resolve_CASECID}", case_cid)
                        .Replace("{Resolve_UserID}", luser.LocalUserID);
                        bodyBuilder.HtmlBody = body;
                        //bodyBuilder.TextBody = "Hello World!";
                        message.Body = bodyBuilder.ToMessageBody();
                    }
                else
                    if (template == "comment")
                    {
                        var fileName = $"wwwroot/email_templates/comment_creation.html";
                        message.Subject = "New Comment on [" + case_cid + "]";
                        var body = File.ReadAllText(fileName);
                        body = body.Replace("{first_name}", luser.FirstName)
                        .Replace("{last_name}", luser.LastName)
                        .Replace("{Resolve_Hostname}", host_add)
                        .Replace("{Resolve_Port}", host_port)
                        .Replace("{Resolve_CASEID}", case_id)
                        .Replace("{Resolve_CASECID}", case_cid)
                        .Replace("{Resolve_UserID}", luser.LocalUserID)
                        .Replace("{commenter_fname}", comment_by.FirstName)
                        .Replace("{commenter_lname}", comment_by.LastName)
                        .Replace("{comment_on_case}", comment_on_case);
                        bodyBuilder.HtmlBody = body;
                        //bodyBuilder.TextBody = "Hello World!";
                        message.Body = bodyBuilder.ToMessageBody();
                    }
                else
                    if (template == "approved")
                    {
                        var fileName = $"wwwroot/email_templates/case_approval.html";
                        message.Subject = "Case Approved - [" + case_cid + "]";
                        var body = File.ReadAllText(fileName);
                        body = body.Replace("{first_name}", luser.FirstName)
                        .Replace("{last_name}", luser.LastName)
                        .Replace("{Resolve_Hostname}", host_add)
                        .Replace("{Resolve_Port}", host_port)
                        .Replace("{Resolve_CASEID}", case_id)
                        .Replace("{Resolve_CASECID}", case_cid)
                        .Replace("{Resolve_UserID}", luser.LocalUserID);
                        bodyBuilder.HtmlBody = body;
                        //bodyBuilder.TextBody = "Hello World!";
                        message.Body = bodyBuilder.ToMessageBody();
                    }
                else
                    if (template == "rejected")
                    {
                        var fileName = $"wwwroot/email_templates/case_rejection.html";
                        message.Subject = "Case Rejected - [" + case_cid + "]";
                        var body = File.ReadAllText(fileName);
                        body = body.Replace("{first_name}", luser.FirstName)
                        .Replace("{last_name}", luser.LastName)
                        .Replace("{Resolve_Hostname}", host_add)
                        .Replace("{Resolve_Port}", host_port)
                        .Replace("{Resolve_CASEID}", case_id)
                        .Replace("{Resolve_CASECID}", case_cid)
                        .Replace("{Resolve_UserID}", luser.LocalUserID);
                        bodyBuilder.HtmlBody = body;
                        //bodyBuilder.TextBody = "Hello World!";
                        message.Body = bodyBuilder.ToMessageBody();
                    }

                SmtpClient client = new SmtpClient();
                client.Connect(server_add, email_port, false);
                client.Authenticate(from_add, email_pass);
                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
                return ("Sent");
            }
            catch
            {
                return ("Failed");
            }
            
        }
        
    }
}
