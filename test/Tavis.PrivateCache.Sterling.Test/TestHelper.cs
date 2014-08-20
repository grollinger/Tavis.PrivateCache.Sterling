namespace Tavis.PrivateCache.Sterling.Test
{
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
    }
}
