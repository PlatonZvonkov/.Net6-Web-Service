using MailLog.Context;
using MailLog.LogModel;
using Microsoft.AspNetCore.Mvc;
using MonkMailService.Models;
using MonkMailService.Service;

namespace MonkMailService.Controllers
{
    [ApiController]
    [Route("api/mails")]
    public class MailController : ControllerBase
    {
        private readonly IEmailService service;
        
        public MailController(IEmailService service)
        {
            this.service = service;           
        }

        /** <summary>
        * Form new Mail With one or multiple recipients    
        * </summary> <param name="request"></param>   
        */
        [HttpPost]
        public async Task<IActionResult> SendEmailAsync(MailView request)
        {
            if (request.Subject == null || request.Body == null || request.Recipients == null)
            {
                string message = "Failed to fill all Required Fields!";
                await service.LogMessageAsync(request);
                return BadRequest(
                    new { Message = message });
            }

            await service.SendEmailAsync(request);
            await service.LogMessageAsync(request);
            return Ok();
        }
    
        /** 
         * <summary>To Get All Messages that was sent earlier. </summary>        
         */
        [HttpGet]
        public async Task<IActionResult> GetAllMailAsync()
        {
            ICollection<MailBody> result = await service.GetAllMailAsync();
            return Ok(result);
        }
    }
}
