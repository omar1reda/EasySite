using EasySite.Core.I_Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySite.Services
{
    public class ResponseCachingService : IResponseCachingService
    {
        private readonly IDatabase redise;

        public ResponseCachingService(IConnectionMultiplexer redise)
        {
            this.redise = redise.GetDatabase();
        }
        public  async Task CachResponsAsync(string CachKey, object Response, TimeSpan ExpireTime)
        {
            if (Response is null) return;
            var Option = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var ResponseString = JsonSerializer.Serialize(Response, Option);
            await  redise.StringSetAsync(CachKey, ResponseString,ExpireTime);

            await redise.SetAddAsync("allKeys", CachKey);
        }

        public async Task DeleteCachAsync(string word)
        {
            var keys = await redise.SetMembersAsync("allKeys");
            foreach (var key in keys)
            {
                if (key.ToString().Contains(word))
                {
                    
                   await redise.KeyDeleteAsync(key.ToString());
                }
            }
            
        }

        public async Task<string?> GetCachAsync(string CachKey)
        {
            var CachedRespons =await redise.StringGetAsync(CachKey);
            if(CachedRespons.IsNullOrEmpty) return null;

            return CachedRespons;
        }
    }
}
