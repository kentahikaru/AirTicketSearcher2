namespace AirTicketSearcher.Configuration
{

    public class Config
    {
        public string chromePath;

        public EmailConfig emailConfig = new EmailConfig();
        public int monthsToLookFor;
        public bool headless;
        public int maxPrice;

        public KiwiConfig kiwiConfig = new KiwiConfig();
        public KiwiWebConfig kiwiWebConfig = new KiwiWebConfig();
        public PelikanConfig pelikanConfig = new PelikanConfig();
    }
}