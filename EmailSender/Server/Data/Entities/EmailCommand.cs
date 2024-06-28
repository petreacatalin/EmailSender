namespace EmailSender.Data.Entities
{
	public enum EmailStatus
	{
		Trimis = 1,
		Nespecificat = 2,
		EroareTrimitere = 3,
	}
	public class EmailCommand
	{
		public Guid Id { get; set; }
		public string? EmailAddress { get; set; }
		public int CommandNumberOfEmails { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public DateTime Timestamp { get; set; }
        public EmailStatus Status { get; set; } = EmailStatus.Nespecificat;
		public Guid EmailCountId { get; set; }  
		public EmailCount? EmailCount { get; set; } = new EmailCount();
        public Guid EmailLogId { get; set; }
		public EmailLog EmailLog { get; set; }
        public Guid CampaignCommandId { get; set; }
        public CampaignCommand CampaignCommand { get; set; }
	}

	

}
