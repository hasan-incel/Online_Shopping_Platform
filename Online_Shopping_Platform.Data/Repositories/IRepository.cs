using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Data.Repositories
{
    public interface IRepository<TEntity>
       where TEntity : class
    {
        void Add(TEntity entity);
        void Delete(TEntity entity, bool softDelete = true);
        void Delete(int id);
        void Update(TEntity entity);
        TEntity GetById(int id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);
    }
}
