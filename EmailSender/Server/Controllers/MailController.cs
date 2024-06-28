using EmailSender.Models;
using EmailSender.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;

namespace EmailSenderAPI.Controllers
{
	[ApiController]
	[Route("api")]
	public class EmailCommandsController : ControllerBase
	{
		private readonly IEmailService _emailService;
		private readonly ILogger<EmailCommandsController> _logger;

		public EmailCommandsController(IEmailService emailService, ILogger<EmailCommandsController> logger)
		{
			_emailService = emailService;
			_logger = logger;
		}
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		[HttpPost("send")]
     
        //		}
        public async Task<EmailSendResult> SendEmailAsync(MailRequest mailreq)
        {
            var result = new EmailSendResult();

            try
            {
                if (mailreq.NumberOfEmails == 0)
                {
                    result.IsSuccess = false;
                    result.ResponseMessage = "No emails to send.";
                    return result;
                }

                await _emailService.SendEmailAsync(mailreq);

                if (Response.StatusCode == 500)
                {
                    _logger.LogError("Failed to send email");
                    result.IsSuccess = false;
                    result.ResponseMessage = "Failed to send email.";
                    return result;
                }

                result.IsSuccess = true;
                result.ResponseMessage = "Email sent successfully.";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
#if !DEBUG
                result.IsSuccess = false;
                result.ResponseMessage = ex.Message;
                return result;
#else
                throw;
#endif
            }
        }


    }
}
