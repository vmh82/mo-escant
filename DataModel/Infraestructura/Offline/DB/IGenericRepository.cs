using LiteDB;
using DataModel.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataModel.Infraestructura.Offline.DB
{
    public interface IGenericRepository<T> where T : BaseModel, new()
    {
        Task<string> AddAsync(T entity);
        Task InsertWithChildrenAsync(T entity);
        Task UpdateWithChildrenAsync(T entity);
        Task<int> AddRangeAsync(IEnumerable<T> entities);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAsync(int skip, int take);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, int skip, int take);
        Task<List<T>> GetTableWithAllChildrenAsync(Expression<Func<T, bool>> expression);
        Task RemoveAsync(T entity);
        Task RemoveAllAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task<int> ExecuteScriptsAsync(string query);
        Task<int> ExecuteScriptsScalarAsync(string query, object[] args);
        Task<List<T>> ExecuteScriptsWithParameterAsync(string query, object[] args);
    }
}