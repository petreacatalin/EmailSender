using EmailSender.Data.Entities;

namespace EmailSender.Models
{
	public class MailRequest
	{
        // public int MailRequestNumber { get; set; }
        public List<string> ToEmail { get; set; }
        public int NumberOfEmails { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
		public DateTime Timestamp { get; set; }
        public int SecondsBetweenEmails { get; set; }
    }
}
