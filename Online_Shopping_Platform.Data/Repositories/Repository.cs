using Microsoft.EntityFrameworkCore;
using Online_Shopping_Platform.Data.Context;
using Online_Shopping_Platform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Data.Repositories
{
    // Generic Repository pattern implementation for CRUD operations
    public class Repository<TEntity> : IRepository<TEntity>
         where TEntity : BaseEntity  // Restrict TEntity to classes that inherit from BaseEntity
    {
        private readonly OnlineShoppingPlatformDbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        // Constructor to initialize DB context and DbSet for the entity
        public Repository(OnlineShoppingPlatformDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();  // Get DbSet for the entity type
        }

        // Add a new entity to the database
        public void Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;  // Set creation date
            _dbSet.Add(entity);  // Add the entity to the DbSet
            //_db.SaveChanges();  
        }

        // Delete an entity, with optional soft delete (mark as deleted)
        public void Delete(TEntity entity, bool softDelete = true)
        {
            if (softDelete)
            {
                entity.ModifiedDate = DateTime.Now;  // Set modification date
                entity.IsDeleted = true;  // Mark entity as deleted
                _dbSet.Update(entity);  // Update the entity in the DbSet
            }
            else
            {
                _dbSet.Remove(entity);  // Permanently remove the entity from DbSet
            }
            //_db.SaveChanges(); 
        }

        // Delete an entity by ID
        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);  // Find the entity by ID
            Delete(entity);  // Call Delete method to delete the entity
        }

        // Get a single entity based on a condition (predicate)
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);  // Return the first matching entity or null
        }

        // Get all entities, optionally filtered by a condition (predicate)
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is null ? _dbSet : _dbSet.Where(predicate);  // Apply filter if predicate is provided
        }

        // Get an entity by its ID
        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);  // Find the entity by its primary key (ID)
        }

        // Update an existing entity in the database
        public void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;  // Set modification date
            _dbSet.Update(entity);  // Update the entity in the DbSet
            //_db.SaveChanges();  // Optional: Save changes to the database (commented out)
        }
    }
}


// _db.SaveChanges() will be managed within a pattern called UnitOfWork, taking into account transaction states.
