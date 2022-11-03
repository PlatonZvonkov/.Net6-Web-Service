using MailLog.LogModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailLog.Context;

namespace MailLog
{
    public class DataAccess : IDataAccess
    {
        private readonly SQLiteContext context;
        public DataAccess(SQLiteContext context)
        {
            this.context = context;
        }
        /**
         * <summary> Asynchronously creates ICollection<MailDAO> from database log </summary>      
        */
        public async Task<ICollection<MailDAO>> GetAllMailsAsync()
        {
            ICollection<MailDAO> result;
            result = await context.LoggedMail.OrderBy(x => x).ToListAsync();
            return result;
        }

        /**
         * <summary> Adding sended or failed mail to DB log. </summary>      
        */
        public async Task PostMailAsync(MailDAO entity)
        {            
            context.LoggedMail.Add(entity);
            await context.SaveChangesAsync();
        }
    }
}
