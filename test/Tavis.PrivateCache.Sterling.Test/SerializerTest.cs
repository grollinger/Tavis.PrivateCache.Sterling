namespace Tavis.PrivateCache.Sterling.Test
{
    using System.IO;
    using System.Text;
    using Xunit;

    public class SerializerTest
    {
        SterlingContentSerializer Serializer;

        public SerializerTest()
        {
            Serializer = new SterlingContentSerializer();
        }

        [Fact]
        public void Serializes_a_PrimaryCacheKey()
        {
            var memory = new MemoryStream();
            try
            {
                // Arrange
                var key = new PrimaryCacheKey(
                        "https://test/uri",
                        "POST"
                    );

                // Act
                PrimaryCacheKey restored = null;
                using (var writer = new BinaryWriter(memory, Encoding.UTF8, true))
                {
                    Serializer.Serialize(key, writer);
                }
                memory.Seek(0, SeekOrigin.Begin);
                using (var reader = new BinaryReader(memory, Encoding.UTF8, true))
                {
                    restored = Serializer.Deserialize<PrimaryCacheKey>(reader);
                }

                // Assert
                Assert.True(Serializer.CanSerialize(typeof(PrimaryCacheKey)));
                Assert.NotNull(restored);
                Assert.Equal(key.Uri, restored.Uri);
                Assert.Equal(key.Method, restored.Method);
            }
            finally
            {
                memory.Dispose();
            }
        }
    }
}
