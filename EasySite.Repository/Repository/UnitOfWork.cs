using Core.Entites;
using EasySite.Core.I_Repository;
using EasySite.Repository.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Hashtable _Hashtable;

        public UnitOfWork(AppDbContext context)
        {
            this._context = context;
            _Hashtable = new Hashtable();
        }


        public async Task<int> CompletedAsynk()
        {
            return await _context.SaveChangesAsync();        
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntites
        {
            

            var Type = typeof(TEntity); 
            if(!_Hashtable.ContainsKey(Type))
            {
                var repo = new GenericRepository<TEntity>(_context);
                _Hashtable.Add(Type, repo);
            }

            return (IGenericRepository<TEntity>) _Hashtable[Type];
            
        }
    }
}
