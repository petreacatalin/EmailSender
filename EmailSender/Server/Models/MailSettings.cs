namespace EmailSender.Models
{
	public class MailSettings
	{
		public string? Mail { get; set; }
		public string? DisplayName { get; set; }
		public string? Password { get; set; }
		public string? Host { get; set; }
		public int Port { get; set; }
        public string? Server { get; set; }
        public string? Username { get; set; }
        public bool UseSSL { get; set; }
	}
}
