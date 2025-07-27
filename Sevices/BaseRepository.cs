using EjadaEFProject.Data;
using EjadaEFProject.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjadaEFProject.Sevices
{
    internal class BaseRepository<T>(EduDbContext dbContext)
        : IBaseRepository<T> where T : class
    {
        readonly EduDbContext _context = dbContext;
        public async Task AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await SaveChanges();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveChanges();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T GetById(params int[] id)
        {
            return _context.Set<T>().Find(id) ??
                throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found.");
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
