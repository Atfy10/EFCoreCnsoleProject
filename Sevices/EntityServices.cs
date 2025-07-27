using EjadaEFProject.Interfaces;
using EjadaEFProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EjadaEFProject.Sevices
{
    internal abstract class EntityServices<T> : IEntityServices<T> where T : class
    {
        public async Task<Result<T>> ExecuteOperation(Func<Task<Result<T>>> operation)
        {
            Result<T> result = default;
            try
            {
                result = await operation();
                return result;
            }
            catch (KeyNotFoundException ex)
            {
                return new Result<T>
                {
                    IsSuccess = false,
                    Message = result?.Message ?? ex.Message,
                    Data = result?.Data
                };
            }
            catch (DbUpdateException ex)
            {
                return new Result<T>
                {
                    IsSuccess = false,
                    Message = result?.Message ?? ex.Message,
                    Data = result?.Data
                };
            }
            catch (ArgumentNullException ex)
            {
                return new Result<T>
                {
                    IsSuccess = false,
                    Message = result?.Message ?? $"Argument null error: {ex.Message}",
                    Data = result?.Data
                };
            }
            catch (Exception ex)
            {
                return new Result<T>
                {
                    IsSuccess = false,
                    Message = result?.Message ?? $"Unexpected error occurred: {ex.Message}",
                    Data = result?.Data
                };
            }
        }
        public virtual Task<Result<T>> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<Result<T>> Delete(params int[] id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<Result<T>> Delete(int id)
        {
            throw new NotImplementedException();
        }


        public virtual Task<List<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual Task<Result<T>> GetById(params int[] id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<Result<T>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<Result<T>> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}