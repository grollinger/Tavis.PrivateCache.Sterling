namespace Tavis.PrivateCache.Sterling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Wintellect.Sterling.Core.Database;

    internal class ContentStoreDB : BaseDatabaseInstance
    {
        public const string SECONDARYKEY_INDEX = "SecondaryKey";

        public ContentStoreDB()
        {
            
        }

        protected override List<ITableDefinition> RegisterTables()
        {
            return new List<ITableDefinition>()
            {
                CreateTableDefinition<SterlingCacheEntry, string>(x => x.Key.ToString()),
                CreateTableDefinition<CacheContent, string>(x => string.Format("{0}->{1}", x.CacheEntry.Key.ToString(), x.Key))
            };
        }
    }
}
