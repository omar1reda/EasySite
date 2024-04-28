using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.I_Repository
{
    public interface IGenericRepository< T> where T : BaseEntites
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task AddAsync(T item);

        void Delete(T item);

        void Update(T item);

        Task<T> GetByIdAsync(dynamic id);

    }
}
