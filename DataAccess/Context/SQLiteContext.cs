using MailLog.LogModel;
using Microsoft.EntityFrameworkCore;

namespace MailLog.Context
{
    public class SQLiteContext:DbContext
    {
        public SQLiteContext(DbContextOptions<SQLiteContext> options) : base(options) { }
        public SQLiteContext() { }

        /** 
         1st table for logging sucsessfull or failed mail
        */
        public DbSet<MailDAO> LoggedMail => Set<MailDAO>();
    }
}
