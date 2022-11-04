using MailLog;
using MailLog.Context;
using MailLog.LogModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonkMailService.Controllers;
using MonkMailService.Models;
using MonkMailService.Service;
using Moq;

namespace MailServiceTests
{
    public class ContrellersIntegrationTests
    {
        private ICollection<MailBody> GetTestSessions()
        {
            var session = new List<MailBody>();
            session.Add(new MailBody {  Subject = "testEmail", Body = "Test", Recipients = "Test@mail.com", Date = DateTime.Now, Result = "Ok", ErrorMessage = null });
            session.Add(new MailBody { Subject = "testEmail2", Body = "Test", Recipients = "Test@mail.com", Date = DateTime.Now, Result = "Ok", ErrorMessage = null });
            return session;
        }
        [Fact]
        public void SendEmail_GivenWrongModel_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IEmailService>();
            mockRepo.Setup(repo => repo.GetAllMailAsync())
                .ReturnsAsync(GetTestSessions());
            var options = new DbContextOptionsBuilder<SQLiteContext>()
            .UseInMemoryDatabase(databaseName: "Logic Test")
            .EnableSensitiveDataLogging()
            .Options;

            var request = new MailView { Subject = "TestEmail", Body = null, Recipients = new[] { "Test@mail.com" } };

            using (var context = new SQLiteContext(options))
            {
                var controller = new MailController(mockRepo.Object, context);
                controller.ModelState.AddModelError("SessionName", "Required");

                // Act
                var result = controller.SendEmailAsync(request);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }
        }

        [Fact]
        public void GetAllMail_ReturnsCollection()
        {
            // Arrange
            var mockRepo = new Mock<IEmailService>();
            mockRepo.Setup(repo => repo.GetAllMailAsync())
                .ReturnsAsync(GetTestSessions());
            var options = new DbContextOptionsBuilder<SQLiteContext>()
            .UseInMemoryDatabase(databaseName: "Logic Test")
            .EnableSensitiveDataLogging()
            .Options;

            using (var context = new SQLiteContext(options))
            {
                var controller = new MailController(mockRepo.Object);
                controller.ModelState.AddModelError("SessionName", "Required");

                // Act
                var result = controller.GetAllMailAsync();                
                // Assert
                Assert.IsType<OkObjectResult>(result.Result);                
            }
        }
    }
}
