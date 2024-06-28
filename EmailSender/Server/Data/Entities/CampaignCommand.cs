namespace EmailSender.Data.Entities
{
	public class CampaignCommand
	{
        public string CampaignName { get; set; }
        public int CampaignCount { get; set; }
		public Guid Id { get; set; }
        public Guid EmailLogId { get; set; }
		public EmailLog EmailLog { get; set; }
		public ICollection<EmailCommand> EmailCommands { get; set; }
    }
}
