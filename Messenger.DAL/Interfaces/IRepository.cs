using Messenger.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Messenger.DAL.Interfaces
{
    public interface IRepository<TContext, T>
        where TContext : DbContext
        where T : EntityBase
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T Get(int id);
        Task<T> GetAsync(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Delete(T entityToDelete);

        void Save();
        Task SaveAsync();
    }
}
