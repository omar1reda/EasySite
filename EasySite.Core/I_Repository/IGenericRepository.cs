using Core.Entites;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.I_Repository
{
    public interface IGenericRepository< T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        
        Task<T> AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        Task DeletRangeAsync(IEnumerable<T> items);

        void Delete(T item);

        void Update(T item);

        Task<T> GetByIdAsync(int id);



        Task<IEnumerable<T>> GetAllWithSpesificationAsync(ISpesification<T> spesification);
        Task<T> GetByIdWithSpesificationAsync(ISpesification<T> spesification);
        Task<int> GetCountByIdWithSpesificationAsync(ISpesification<T> spesification);

    }
}
