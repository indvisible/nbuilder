namespace Indvisible.DataGen.Models
{
    public class Mail
    {
        public User Sender { get; set; }

        public User Receiver { get; set; }

        public string MailText { get; set; }
    }
}