using EmailSender.Data.Entities;
using EmailSender.Models;

namespace EmailSender.Services
{
	public interface IEmailService
	{
		Task SendEmailAsync(MailRequest mailRequest);
		Task SendRandomEmailsAsync(MailRequest mailRequest,List<string> emailAddresses, int numberOfEmails, DateTime startTime, CampaignCommand request);
		Task SendBulkEmailsAsync(MailRequest mailRequest,CampaignCommand request);
	}
}
