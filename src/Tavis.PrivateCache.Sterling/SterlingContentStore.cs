namespace Tavis.PrivateCache.Sterling
{
    using System;
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

        public async Task<CacheEntry> GetEntryAsync(PrimaryCacheKey cacheKey)
        {
            return await Instance.LoadAsync<SterlingCacheEntry>(cacheKey);
        }

        public Task<CacheContent> GetContentAsync(CacheEntry entry, string secondaryKey)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEntryAsync(CacheContent content)
        {
            throw new NotImplementedException();
        }
    }
}
