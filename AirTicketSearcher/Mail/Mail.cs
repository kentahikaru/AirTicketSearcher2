namespace AirTicketSearcher.Mail
{
    using System.Collections.Generic;
    using System.Linq;
	using System.Text;
    using MailKit;
    using MimeKit;
    using MailKit.Security;
	using MailKit.Net.Smtp;
	using AirTicketSearcher.Configuration;

    public class Mail
    {
		EmailConfig emailConfig;

		public Mail(EmailConfig config)
		{
			this.emailConfig = config;
		}

		public void SendEmail(string subject, string messageText)
		{
			MimeMessage message = MakeMessage(subject, messageText);
			SendEmailMessage(message);
		}

		private MimeMessage MakeMessage(string subject, string messageText)
		{
			// BodyBuilder bb = new BodyBuilder();
			// bb.HtmlBody = string.Format(messageText);
			// bb.TextBody = "-";

			var message = new MimeMessage ();
				message.From.Add (new MailboxAddress (this.emailConfig.fromName, this.emailConfig.fromAddress));
				message.To.Add (new MailboxAddress (this.emailConfig.toName, this.emailConfig.toAddress));
				message.Subject = subject;

				//message.Body = new TextPart ("plain") {
				message.Body = new TextPart ("html") {
					//Text = "@\"" + messageText + "\"" 
					Text = messageText
				};
				// message.Body = bb.ToMessageBody();

				return message;
		}

		

		private void SendEmailMessage(MimeMessage message)
		{
			//((TextPart)(message.Body)).Text
			//Be careful that the SmtpClient class is the one from Mailkit not the framework!
			using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
			{
                emailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(this.emailConfig.smtpServer, this.emailConfig.port, SecureSocketOptions.SslOnConnect);

				//Remove any OAuth functionality as we won't be using it. 
				emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
		
				emailClient.Authenticate(this.emailConfig.userName, this.emailConfig.Password);
		
				emailClient.Send(message);
		
				emailClient.Disconnect(true);
			}
		}

    }
}