using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjadaEFProject.Interfaces
{
    internal interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        T GetById(params int[] id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
