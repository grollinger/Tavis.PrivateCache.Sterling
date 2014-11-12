namespace Tavis.PrivateCache.Sterling
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    internal class SterlingCacheEntry : CacheEntry
    {
        public PrimaryCacheKey StoreKey
        {
            get { return PrimaryKey; }
            set { PrimaryKey = value; }
        }
        public IEnumerable<string> StoreVaryHeaders
        {
            get { return VaryHeaders; }
            set { VaryHeaders = value; }
        }

        public SterlingCacheEntry(PrimaryCacheKey key, IEnumerable<string> varyHeaders)
            : base(key, varyHeaders) { }

        public SterlingCacheEntry()
        {
        }

        public SterlingCacheEntry(CacheEntry cacheEntry)
            : base(cacheEntry.PrimaryKey, cacheEntry.VaryHeaders)
        {
        }
    }
}
