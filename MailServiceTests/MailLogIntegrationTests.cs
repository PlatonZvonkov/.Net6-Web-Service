using MailLog;
using MailLog.Context;
using MailLog.LogModel;
using Microsoft.EntityFrameworkCore;

namespace MailServiceTests
{
    public class MailLogIntegrationTests
    {
        [Fact]
        public async void PostMailAsync_Add_WorkingAsIntended()
        {            
            var options = new DbContextOptionsBuilder<SQLiteContext>()
            .UseInMemoryDatabase(databaseName: "Logic Test")
            .EnableSensitiveDataLogging()
            .Options;
            using (var context = new SQLiteContext(options))
            {
                // Arrange
                context.LoggedMail.Add(new MailDAO { Id = 1, Subject = "testEmail", Body = "Test", Recipients = "Test@mail.com", Date = DateTime.Now, Result = "Ok", ErrorMessage = null });
                context.SaveChanges();
            }
            using (var context = new SQLiteContext(options))
            {
                var st = new MailDAO { Id = 2, Subject = "testEmail2", Body = "Test2", Recipients = "Test@mail.com", Date = DateTime.Now, Result = "Ok", ErrorMessage = null };
                var repo = new DataAccess(context);

                // Act
                await repo.PostMailAsync(st);
                // Assert
                Assert.Equal(2, context.LoggedMail.Count());
            }
        }
        [Fact]
        public async void GetAllMailsAsync_Call_ReturnsCollection()
        {
            var options = new DbContextOptionsBuilder<SQLiteContext>()
            .UseInMemoryDatabase(databaseName: "Logic Test")
            .EnableSensitiveDataLogging()
            .Options;

            using (var context = new SQLiteContext(options))
            {
                // Arrange 
                context.LoggedMail.AddRange(new MailDAO[]
                 {
                new MailDAO { Id = 1, Subject = "testEmail", Body = "Test", Recipients = "Test@mail.com", Date = DateTime.Now, Result = "Ok", ErrorMessage = null },
                new MailDAO { Id = 2, Subject = "testEmail", Body = "Test", Recipients = "Test@mail.com", Date = DateTime.Now, Result = "Ok", ErrorMessage = null }
                 });
                context.SaveChanges();
            }
            using (var context = new SQLiteContext(options))
            {                
                var repo = new DataAccess(context);
                // Act
                var actual = await repo.GetAllMailsAsync();
                // Assert
                Assert.Equal(typeof(List<MailDAO>), actual.GetType());
                Assert.Equal(actual.Count , context.LoggedMail.Count());
            }
        }
    }
}
