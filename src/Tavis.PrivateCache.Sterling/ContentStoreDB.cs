namespace Tavis.PrivateCache.Sterling
{
    using System.Collections.Generic;
    using Wintellect.Sterling.Core.Database;

    internal class ContentStoreDB : BaseDatabaseInstance
    {
        public ContentStoreDB()
        {
        }

        protected override List<ITableDefinition> RegisterTables()
        {
            return new List<ITableDefinition>()
            {
                CreateTableDefinition<SterlingCacheEntry, string>(x => x.Key.ToString()),
                CreateTableDefinition<SterlingCacheContent, string>(x => string.Format("{0}->{1}", x.CacheEntry.Key.ToString(), x.Key.ToString()))
            };
        }
    }
}
