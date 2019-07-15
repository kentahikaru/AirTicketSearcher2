namespace AirTicketSearcher.Configuration
{

    public class Config
    {
        public string chromePath;
        public EmailConfig emailConfig = new EmailConfig();
        public KiwiConfig kiwiConfig = new KiwiConfig();
        public KiwiWebConfig kiwiWebConfig = new KiwiWebConfig();
    }
}