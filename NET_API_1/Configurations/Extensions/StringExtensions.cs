using System.Net.Mail;
using System.Text.RegularExpressions;

namespace NET_API_1.Configurations.Extensions
{
    public static class StringExtensions
    {
        public static bool ValidatePhoneNumber(this string phone)
        {
            return Regex.Match(phone, @"^([0-9]{9,11})$").Success;
        }

        public static bool ValidateEmailAddress(this string emailAddress)
        {
            MailAddress.TryCreate(emailAddress, out var email);
            return email != null;
        }
    }
}
