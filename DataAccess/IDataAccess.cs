using MailLog.LogModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailLog
{
    public interface IDataAccess
    {
        Task PostMailAsync(MailDAO entity);   
        Task<ICollection<MailDAO>> GetAllMailsAsync();
    }
}
