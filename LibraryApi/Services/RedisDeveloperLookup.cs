using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class RedisDeveloperLookup : ILookupDevelopers
    {
        IDistributedCache Cache;

        public RedisDeveloperLookup(IDistributedCache cache)
        {
            Cache = cache;
        }

        public async Task<string> GetCurrentOnCallDeveloper()
        {
            // look in the cache to see if the value is already there and it isn't stale.

            var developer = await Cache.GetAsync("developer");

            var result = "";

            if (developer == null)

            {
                // if it isn't, make the API call or whatever to get it, then
                // stick in the cache, with a expiration time

                // then return the new value.
                result = "Leland Palmer" + Guid.NewGuid().ToString();
                var encodedDeveloper = Encoding.UTF8.GetBytes(result);
                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddSeconds(15));
                await Cache.SetAsync("developer", encodedDeveloper, options);
            }
            else
            {
                // if it is there, just return that.
                result = Encoding.UTF8.GetString(developer);
            }

            return result;
        }
    }
}
