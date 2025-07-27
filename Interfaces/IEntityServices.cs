using EjadaEFProject.Models;
using EjadaEFProject.Sevices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjadaEFProject.Interfaces
{
    internal interface IEntityServices<T> where T : class
    {
        //public Task<Result<T>> ExecuteOperation(Func<Task<Result<T>>> operation);
        public Task<List<T>> GetAll();
        public Task<Result<T>> GetById(params int[] id);
        public Task<Result<T>> GetById(int id);
        public Task<Result<T>> Add(T entity);
        public Task<Result<T>> Update(T entity);
        public Task<Result<T>> Delete(params int[] id);
        public Task<Result<T>> Delete(int id);

    }
}
