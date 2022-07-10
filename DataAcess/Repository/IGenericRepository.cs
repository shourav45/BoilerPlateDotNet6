using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> Insert(T Entity);
        public Task<List<T>> InsertBulk(List<T> Entity);
        public Task<T> Update(T Entity, Expression<Func<T, bool>> predicate);
        public Task<bool> Delete(Expression<Func<T, bool>> predicate);
        public Task<List<T>> Find(Expression<Func<T, bool>> predicate);
        public Task<T> FindOne(Expression<Func<T, bool>> predicate);
        public Task<bool> ExecuteCommand(string query, params object[] parameters);
        public Task<List<T>> FindUsingSPAsync(string storedProcedureName, SqlParameter[] parameters = null);
        public Task<List<T>> GetAll();
    }
}
