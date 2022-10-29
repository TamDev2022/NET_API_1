using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NET_API_1.Interfaces.IServices;
using static NET_API_1.Configurations.AppSettings;

namespace NET_API_1.Services
{
    public class MailContent
    {
        public string? To { get; set; }              // Địa chỉ gửi đến
        public string? Subject { get; set; }         // Chủ đề (tiêu đề email)
        public string? Body { get; set; }            // Nội dung (hỗ trợ HTML) của email
    }

    // + ============================== +

    public class SendMailService : ISendMailService
    {
        private readonly ILogger<SendMailService> _logger;
        private readonly MailSettings _mailSettings;

        public SendMailService(ILogger<SendMailService> logger, IOptions<MailSettings> mailSettings)
        {
            _logger = logger;
            _mailSettings = mailSettings.Value;
        }

        public async Task SendMailAsync(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", mailContent.To).Replace("[emaiUserNamel]", mailContent.To);

            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();

            using var stmp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                stmp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                stmp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await stmp.SendAsync(email);
                stmp.Disconnect(true);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);

                _logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                _logger.LogError(ex.Message);
            }

        }
    }
}
