namespace EmailSender.Data.Entities
{
	public class EmailLog
	{
        public Guid Id { get; set; } 
		public string? EmailCommandLogMessage { get; set; }
		public string? LogMessage { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid? EmailCommandId { get; set; }
        public EmailCommand? EmailCommand{ get; set; }
		public Guid? CampaignCommandId { get; set; }
		public CampaignCommand? CampaignCommand { get; set; }	
	}
}
