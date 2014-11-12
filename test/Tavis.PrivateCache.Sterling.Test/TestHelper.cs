namespace Tavis.PrivateCache.Sterling.Test
{
    using System.Collections.Generic;
    using Wintellect.Sterling.Core;

    internal static class TestHelper
    {
        public static ISterlingDatabaseInstance CreateDatabase()
        {
            var engine = new SterlingEngine(new SterlingPlatformAdapter());
            engine.SterlingDatabase.RegisterSerializer<SterlingContentSerializer>();
            engine.Activate();
            return engine.SterlingDatabase.RegisterDatabase<ContentStoreDB>("TestDB", new MemoryDriver());
        }

        internal static CacheContentKey CreateContentKey()
        {
            var varyHeaders = new[] { "Accept-Language", "Accept-Encoding" };
            var headerValues = new Dictionary<string, IEnumerable<string>>()
            {
                {"Accept-Language", new[]{"de"}},
                {"Content-Type", new[]{"text/plain"}}
            };
            var contentHeaderValues = new Dictionary<string, IEnumerable<string>>();

            return new CacheContentKey(varyHeaders, headerValues, contentHeaderValues);
        }
    }
}
