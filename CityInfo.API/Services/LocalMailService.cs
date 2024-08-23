namespace CityInfo.API.Services
{
    public class LocalMailService:IMailService
    {
        private readonly string _MailFrom = string.Empty;
        private readonly string _MailTo = string.Empty;

        public LocalMailService(IConfiguration configuration)
        {
            _MailFrom = configuration["mailSettings:mailFromAddress"];
            _MailTo = configuration["mailSettings:mailToAddress"];
        }

        public void Send(string subject,string message)
        {
            Console.WriteLine($"Mail From {_MailFrom} To {_MailTo}"
                + $"With {nameof(LocalMailService)}");
            Console.WriteLine($"Subject {subject}");
            Console.WriteLine($"Message {message}");
        }
    }
}
