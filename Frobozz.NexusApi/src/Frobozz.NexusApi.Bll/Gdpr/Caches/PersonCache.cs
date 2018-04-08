﻿using System.Threading;
using System.Threading.Tasks;
using Frobozz.CapabilityContracts.Gdpr;
using Xlent.Lever.Libraries2.MoveTo.Core.Cache;
using Xlent.Lever.Libraries2.MoveTo.Core.Cache.Microsoft.Extensions.Caching.Distributed;
using Xlent.Lever.Libraries2.MoveTo.Core.ClientTranslators;
using Xlent.Lever.Libraries2.MoveTo.Core.Translation;

namespace Frobozz.NexusApi.Bll.Gdpr.Caches
{
    /// <summary>
    /// Client translator
    /// </summary>
    public class PersonCache : CrudAutoCache<Person, string>, IPersonService
    {
        private readonly IGdprCapability _gdprCapability;

        /// <summary>
        /// Constructor 
        /// </summary>
        public PersonCache(IGdprCapability gdprCapability, IDistributedCache cache)
        :base(gdprCapability.PersonService, cache)
        {
            _gdprCapability = gdprCapability;
        }

        /// <inheritdoc />
        public async Task<Person> FindFirstOrDefaultByNameAsync(string name, CancellationToken token = default(CancellationToken))
        {
            var key = $"ByName-{name}";
            var item = await CacheGetByKeyAsync(key, token);
            if (item != null) return item;
            item = await _gdprCapability.PersonService.FindFirstOrDefaultByNameAsync(name, token);
            if (item == null) return null;
            await CacheSetByKeyAsync(key, item, token);
            return item;
;        }
    }
}
