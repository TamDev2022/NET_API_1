using NET_API_1.Services;

namespace NET_API_1.Interfaces.IServices
{
    public interface ISendMailService
    {
        public Task SendMailAsync(MailContent mailContent);
    }
}
