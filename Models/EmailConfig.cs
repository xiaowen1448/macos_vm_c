using System;

namespace VMCloneApp.Models
{
    public class EmailTemplate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }

        public EmailTemplate()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
            Name = "新模板";
            Subject = "邮件主题";
            Body = "邮件内容";
            CreatedTime = DateTime.Now;
            ModifiedTime = DateTime.Now;
        }
    }

    public class EmailRecord
    {
        public string Id { get; set; }
        public string TemplateId { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public DateTime SendTime { get; set; }
        public string ErrorMessage { get; set; }

        public EmailRecord()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
            TemplateId = "";
            Recipient = "";
            Subject = "";
            Status = "Pending";
            SendTime = DateTime.Now;
            ErrorMessage = "";
        }
    }

    public class EmailConfig
    {
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }

        public EmailConfig()
        {
            SMTPHost = "smtp.example.com";
            SMTPPort = 587;
            Username = "user@example.com";
            Password = "";
            EnableSSL = true;
        }
    }
}