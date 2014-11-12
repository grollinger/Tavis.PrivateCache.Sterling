namespace Tavis.PrivateCache.Sterling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Wintellect.Sterling.Core;

    public class SterlingContentStore : IContentStore
    {
        private readonly ISterlingDatabaseInstance Instance;

        public SterlingContentStore(ISterlingDatabase db, ISterlingDriver driver)
        {
            Contract.Requires<ArgumentNullException>(db != null, "db");
            Contract.Requires<ArgumentNullException>(driver != null, "driver");

            Instance = db.RegisterDatabase<ContentStoreDB>("ContentStore", driver);
        }

        public async Task<CacheEntry> GetEntryAsync(PrimaryCacheKey primaryKey, CacheEntryKey entryKey)
        {
            return null;
        }

        public Task<IEnumerable<CacheEntry>> GetEntriesAsync(PrimaryCacheKey primaryKey)
        {
            throw new NotImplementedException();
        }

        public Task<ICacheContent> GetContentAsync(PrimaryCacheKey primaryKey, CacheContentKey contentKey)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEntryAsync(CacheContent content)
        {
            throw new NotImplementedException();
        }
    }
}
