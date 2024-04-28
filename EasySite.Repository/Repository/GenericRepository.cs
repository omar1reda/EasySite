using Core.Entites;
using EasySite.Core.I_Repository;
using EasySite.Repository.Context;
using EasySite.Repository.Spesifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<T> AddAsync(T item)
        {
            
          var addedItem = await _context.AddAsync(item);
            return addedItem.Entity;

        }

        public async Task AddRangeAsync(IEnumerable<T> items)
        {
             await _context.AddRangeAsync(items);
        }

        public async Task DeletRangeAsync(IEnumerable<T> items)
        {
              _context.RemoveRange(items);
        }
        public void Delete(T item)
        {
            _context.Remove(item);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
          
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

              

        public void Update(T item)
        {
            _context.Update(item);
        }




        public  async Task<IEnumerable<T>> GetAllWithSpesificationAsync(ISpesification<T> spesification)
        {
            return await  SpesificationEvalutor<T>.GetQuery(_context.Set<T>(),spesification).ToListAsync();
        }

        public async Task<T> GetByIdWithSpesificationAsync(ISpesification<T> spesification)
        {
            return await SpesificationEvalutor<T>.GetQuery(_context.Set<T>(), spesification).FirstOrDefaultAsync();
        }


        public async Task<int> GetCountByIdWithSpesificationAsync(ISpesification<T> spesification)
        {
          return  await SpesificationEvalutor<T>.GetQuery(_context.Set<T>(), spesification).CountAsync();
        }


    }
}
