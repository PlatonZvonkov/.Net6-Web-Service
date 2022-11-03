using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailLog;
using MailLog.LogModel;
using MimeKit;
using MimeKit.Text;
using MonkMailService.Models;
using System.Text;

namespace MonkMailService.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;
        private readonly IDataAccess dataAccess;
        private IMapper _mapper;
        private MapperConfiguration mapperConfiguration;
        private string errorMessage;
        private string status = "Failed";

        public EmailService(IConfiguration config, IDataAccess dataAccess)
        {
            this.config = config;
            this.dataAccess = dataAccess;
            /**
             * Automapper also allows us to ignore some of the fields 
            * when mapped in one of the direction if you want to hide some info            
            */
            mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MailBody, MailDAO>().ForMember(x => x.Id, opt => opt.Ignore());
                cfg.CreateMap<MailDAO, MailBody>();
            });
            _mapper = new Mapper(mapperConfiguration);
        }

        /** <summary> Forming new email's according to config file  </summary>      
        */
        public async Task SendEmailAsync(MailView request)
        {
            MimeMessage email = new();
            email.From.Add(MailboxAddress.Parse(config.GetSection("EmailUsername").Value));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.CheckCertificateRevocation = false;
                await smtp.ConnectAsync(config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(config.GetSection("EmailUsername").Value, config.GetSection("EmailPassword").Value);
                try
                {
                    foreach (string item in request.Recipients)
                    {
                        email.To.Clear();
                        email.To.Add(MailboxAddress.Parse(item));
                        await smtp.SendAsync(email);
                        Task.Delay(100).Wait();
                        status = "Ok";
                    }                    
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    status = "Failed";                    
                }
                finally
                {                    
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }            
        }

        /** <summary>
        * Returns new Collection of logged mails from database  </summary>  */
        public async Task<ICollection<MailBody>> GetAllMailAsync()
        {
            ICollection<MailDAO> temp = await dataAccess.GetAllMailsAsync();
            ICollection<MailBody> result = _mapper.Map<ICollection<MailDAO>, ICollection<MailBody>>(temp);
            return result;
        }

        /** <summary>
        * Log requested mail to DB, sucsessful of not.  </summary>  */
        public async Task LogMessageAsync(MailView mail)
        {
            MailBody body = new()
            {
                Subject = mail.Subject,
                Recipients = ArrayToString(mail.Recipients),
                Body = mail.Body,
                Date = DateTime.Now,
                Result = status,
                ErrorMessage = errorMessage
            };
            MailDAO mailDAO = _mapper.Map<MailDAO>(body);
            await dataAccess.PostMailAsync(mailDAO);
        }

        private string ArrayToString(string[] arr)
        {
            StringBuilder result = new();
            if (arr.Length == 1)
            {
                result.Append($"{arr[0]}");
            }
            else
            {
                foreach (var i in arr)
                {
                    result.Append($"{i},");
                }
            }           
            return result.ToString();
        }
    }
}
