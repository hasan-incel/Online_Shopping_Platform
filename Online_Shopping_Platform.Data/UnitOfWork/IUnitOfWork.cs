using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        //It returns the number of saves affected.For that reason I use int.

        Task BeginTransaction();
        Task CommitTransaction();
        Task RollBackTransaction();
    }
}
