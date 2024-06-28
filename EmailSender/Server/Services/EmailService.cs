using EmailSender.Data.EmailContext;
using EmailSender.Data.Entities;
using EmailSender.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EmailSender.Services
{
	public class EmailService : IEmailService
	{
		private readonly IDbContextFactory<EmailDbContext> _dbContextFactory;
		private readonly MailSettings _mailSettings;
		private readonly ILogger<EmailService> _logger;


		public EmailService(
			IOptions<MailSettings> mailSettings,
			ILogger<EmailService> logger,
			IDbContextFactory<EmailDbContext> dbContextFactory)
		{
			_mailSettings = mailSettings.Value;
			_dbContextFactory = dbContextFactory;
			_logger = logger;
		}

		public async Task SendEmailAsync(MailRequest mailRequest)
		{
			try
			{
				var context = _dbContextFactory.CreateDbContext();
				int campaignNo = await GetCampaignCommandsCount(context);
				campaignNo++;
				var request = new CampaignCommand()
				{
					Id = Guid.NewGuid(),
					CampaignCount = campaignNo,
					CampaignName = $"Campania #{campaignNo}",
					EmailLogId = Guid.NewGuid(),
				};
				context.CampaignCommands.Add(request);
				await context.SaveChangesAsync();

				if (mailRequest.StartTime.HasValue)
				{
					var currentTime = DateTime.Now;
					var startTime = mailRequest.StartTime.Value.ToLocalTime();
					if ((currentTime >= startTime) || (startTime > currentTime))
					{
						if (startTime > currentTime)
						{
							var delay = startTime - currentTime;
							await Task.Delay(delay);
						}

						await SendRandomEmailsAsync(mailRequest, mailRequest.ToEmail, mailRequest.NumberOfEmails, startTime, request);

					}

				}
				else
				{
					_ = SendBulkEmailsAsync(mailRequest, request);

				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}


		}


		public async Task SendBulkEmailsAsync(MailRequest mailRequest, CampaignCommand request)
		{
			try
			{
                var context = _dbContextFactory.CreateDbContext();
                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL).ConfigureAwait(false);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password).ConfigureAwait(false);

                foreach (var item in mailRequest.ToEmail)
                {
                    for (int i = 0; i < mailRequest.NumberOfEmails; i++)
                    {
                        await SendEmailAsync(context, smtp, mailRequest, item, mailRequest.StartTime ?? DateTime.Now, request);
                    }
                }

                await smtp.DisconnectAsync(true).ConfigureAwait(false);

                await context.SaveChangesAsync();
            }
			catch (Exception ex)
			{
				Console.WriteLine($"{ex.Message}");
				throw;
			}


		}

		public async Task SendRandomEmailsAsync(MailRequest mailRequest, List<string> emailAddresses, int numberOfEmails, DateTime startTime, CampaignCommand request)
		{
			try
			{
                var context = _dbContextFactory.CreateDbContext();
                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL).ConfigureAwait(false);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password).ConfigureAwait(false);

                var random = new Random();
                foreach (var emailAddress in emailAddresses)
                {
                    var totalEmailsSent = 0;
                    while (totalEmailsSent < numberOfEmails)
                    {
                        var emailsToSend = random.Next(1, Math.Min(4, numberOfEmails - totalEmailsSent + 1));

                        for (int i = 0; i < emailsToSend; i++)
                        {
                            await SendEmailAsync(context, smtp, mailRequest, emailAddress, startTime, request);
                            totalEmailsSent++;

                            if (totalEmailsSent < numberOfEmails)
                            {
                                var delayMilliseconds = mailRequest.SecondsBetweenEmails * 1000;
                                await Task.Delay(delayMilliseconds);
                            }
                        }
                    }
                }

                await context.SaveChangesAsync();
                await smtp.DisconnectAsync(true).ConfigureAwait(false);
            }
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());	
				throw ex;
			}
		}


		private int GenerateDelay()
		{
			var random = new Random();
			return random.Next(2000, 10001);
		}

		public async Task SendEmailAsync(EmailDbContext context, SmtpClient smtp, MailRequest mailRequest, string emailAddress, DateTime startTime, CampaignCommand request)
		{

			var command = new EmailCommand
			{
				Id = Guid.NewGuid(),
				EmailAddress = emailAddress,
				CommandNumberOfEmails = mailRequest.NumberOfEmails,
				StartTime = startTime,
				Timestamp = DateTime.Now,
				CampaignCommandId = request.Id,
			};

			var emailLog = new EmailLog() { Id = Guid.NewGuid() };
			try
			{
				var email = new MimeMessage();
				email.From.Add(new MailboxAddress("Email Sender", _mailSettings.Mail));
				email.To.Add(new MailboxAddress("Email Sender", emailAddress));

                int emailCounter = await GetAndUpdateEmailCount(context);
				int emailCounterx = 0;
				string subject;
				subject = $"Email #{(emailCounter == 0 ? ++emailCounter : emailCounter)} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
				emailCounter++;
				email.Subject = subject;

				var builder = new BodyBuilder();
				string randomText = GenerateRandomText();
				builder.HtmlBody = randomText;
				email.Body = builder.ToMessageBody();

				await smtp.SendAsync(email).ConfigureAwait(false);
          
                command.EmailCount.Count = emailCounter + 1;
				command.Status = EmailStatus.Trimis;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Eroare de trimitere email către adresa: {emailAddress}");
				command.EmailLog.EmailCommandLogMessage = $"Eroare de trimitere email către adresa: {emailAddress}";
				command.Status = EmailStatus.EroareTrimitere;
			}
			finally
			{
				var requestCount = await GetCampaignCommandsCount(context);
				var emailAddresses = string.Join(", ", mailRequest.ToEmail);
				var logMessage = $"A fost inițiată comanda cu numărul '{requestCount}' pentru a trimite '{mailRequest.NumberOfEmails}' " +
					$"email-uri către următoarele adrese: {$"{emailAddresses}"}" + $" în intervalul de timp {$"{startTime}"}";
				var commandLogMessage = $"Campanie #{requestCount}: {logMessage}";
				_logger.LogInformation(commandLogMessage);

				emailLog.EmailCommandId = command.Id;
				emailLog.EmailCommandLogMessage = commandLogMessage;
				emailLog.Timestamp = DateTime.Now;
				emailLog.LogMessage = $"Trimitere de email nr. #'' din '{mailRequest.NumberOfEmails}'" + $"către adresa: '{emailAddress}, status: {command.Status}";

				context.EmailLogs.Add(emailLog);
				command.EmailLogId = emailLog.Id;
				context.EmailCommands.Add(command);

				await context.SaveChangesAsync();
			}

		}

		public string GenerateRandomText()
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var random = new Random();
			return new string(Enumerable.Repeat(chars, 50).Select(s => s[random.Next(s.Length)]).ToArray());
		}

		private async Task<int> GetAndUpdateEmailCount(EmailDbContext context)
		{
			
			return await context.EmailCounts.CountAsync();
		}


		private async Task<int> GetCampaignCommandsCount(EmailDbContext context)
		{
			return await context.CampaignCommands.CountAsync();
		}


	}
}
