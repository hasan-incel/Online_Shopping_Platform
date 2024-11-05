using Microsoft.EntityFrameworkCore.Storage;
using Online_Shopping_Platform.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineShoppingPlatformDbContext _db;
        private IDbContextTransaction _transaction;

        public UnitOfWork(OnlineShoppingPlatformDbContext db)
        {
            _db = db;
        }


        public async Task BeginTransaction()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
            //The place where we give permission to the Garbage Collector to clean up.
            //It doesn't delete at that moment; it marks it as deletable.

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //These codes will run the Garbage Collector directly.
        }

        public async Task RollBackTransaction()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
