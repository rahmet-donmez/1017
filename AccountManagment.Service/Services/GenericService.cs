using AccountManagment.Core.Repositories;
using AccountManagment.Core.Services;
using AccountManagment.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagment.Service.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly AppDbContext _context;


        public GenericService(IGenericRepository<T> repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {        
            return await _repository.GetByIdAsync(id);
        }

        public async Task RemoveAsync(T entity)
        {

            _repository.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _repository.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _repository.Where(expression).AsQueryable();
        }
    }
}
