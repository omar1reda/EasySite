using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.I_Repository
{
    public interface IResponseCachingService
    {
        Task CachResponsAsync(string CachKey , object Response , TimeSpan ExpireTime);
        Task<string?> GetCachAsync (string CachKey );
        Task DeleteCachAsync (string CachKey );
    }
}
