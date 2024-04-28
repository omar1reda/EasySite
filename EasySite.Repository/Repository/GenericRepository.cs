using Core.Entites;
using EasySite.Core.I_Repository;
using EasySite.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntites
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(T item)
        {
            await _context.AddAsync(item);
        }

        public void Delete(T item)
        {
            _context.Remove(item);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
          
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(dynamic id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
 
        public void Update(T item)
        {
            _context.Update(item);
        }
    }
}
