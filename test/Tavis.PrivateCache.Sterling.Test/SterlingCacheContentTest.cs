namespace Tavis.PrivateCache.Sterling.Test
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Tavis.PrivateCache.Sterling;
    using Xunit;

    public class SterlingCacheContentTest
    {
        PrimaryCacheKey primaryKey;
        string secondaryKey;
        CacheEntry entry;
        HttpResponseMessage response;
        CacheContent content;

        public SterlingCacheContentTest()
        {
            primaryKey = new PrimaryCacheKey("https://localhost/test", "POST");
            secondaryKey = "secondary";
            entry = new CacheEntry(primaryKey, new string[0]);
            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("42")
            };

            response.Headers.CacheControl = new CacheControlHeaderValue() { MaxAge = TimeSpan.FromHours(1) };
            response.Content.Headers.ContentLanguage.Add("de");

            // We need to add this here, since the header will be added during reconstruction in any case
            // That is (hopefully) not a problem, however, since it does not contribute to the "content" as such. 
            response.Content.Headers.ContentLength = 2;

            content = new CacheContent()
            {
                Key = secondaryKey,
                CacheEntry = entry,
                Response = response
            };
        }

        private void AssertCorrectReconstruction(HttpResponseMessage restoredResponse)
        {
            Assert.NotNull(restoredResponse);
            Assert.Equal(response.StatusCode, restoredResponse.StatusCode);
            Assert.Equal(response.Headers, restoredResponse.Headers, new HttpHeadersComparer());
            Assert.NotNull(restoredResponse.Content);
            Assert.Equal(response.Content.Headers, restoredResponse.Content.Headers, new HttpHeadersComparer());
            Assert.Equal("42", restoredResponse.Content.ReadAsStringAsync().Result);
        }


        [Fact]
        public async Task Can_be_created_from_plain_CacheContent()
        {
            // Arrange
            // Ctor

            // Act
            var sterlingContent = await SterlingCacheContent.CreateAsync(content);

            // Assert
            Assert.NotNull(sterlingContent);
            Assert.Equal(primaryKey, sterlingContent.CacheEntry.Key);
            var restoredResponse = sterlingContent.Response;
            AssertCorrectReconstruction(restoredResponse);
        }

        [Fact]
        public async Task Creation_does_not_fail_with_empty_message()
        {
            // Arrange
            var primaryKey = new PrimaryCacheKey("https://localhost/test", "POST");
            var secondaryKey = "secondary";
            var entry = new CacheEntry(primaryKey, new string[0]);
            var response = new HttpResponseMessage();

            var content = new CacheContent()
            {
                Key = secondaryKey,
                CacheEntry = entry,
                Response = response
            };

            // Act
            var sterlingContent = await SterlingCacheContent.CreateAsync(content);

            // Assert
            Assert.NotNull(sterlingContent);
        }

        [Fact]
        public void Creation_fails_for_invalid_CacheContent()
        {
            // Arrange
            var primaryKey = new PrimaryCacheKey("https://localhost/test", "POST");
            var secondaryKey = "secondary";
            var entry = new CacheEntry(primaryKey, new string[0]);
            var response = new HttpResponseMessage();

            var noEntry = new CacheContent()
            {
                Key = secondaryKey,
                Response = response
            };

            var noKey = new CacheContent()
            {
                CacheEntry = entry,
                Response = response
            };

            var emptyKey = new CacheContent()
            {
                CacheEntry = entry,
                Key = "",
                Response = response
            };

            var noResponse = new CacheContent()
            {
                Key = secondaryKey,
                CacheEntry = entry
            };

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => SterlingCacheContent.CreateAsync(noEntry).Result);
            Assert.Throws<ArgumentException>(() => SterlingCacheContent.CreateAsync(noKey).Result);
            Assert.Throws<ArgumentException>(() => SterlingCacheContent.CreateAsync(emptyKey).Result);
            Assert.Throws<ArgumentException>(() => SterlingCacheContent.CreateAsync(noResponse).Result);
        }


        [Fact]
        public async Task Database_can_save_a_CacheContent()
        {
            // Arrange
            var Database = TestHelper.CreateDatabase();
            var sterlingContent = await SterlingCacheContent.CreateAsync(content);

            // Act
            var objKey = await Database.SaveAsAsync(sterlingContent);
            var savedContent = await Database.LoadAsync<SterlingCacheContent>(objKey);

            // Assert
            Assert.NotNull(objKey);
            Assert.NotNull(savedContent);
            AssertCorrectReconstruction(savedContent.Response);
        }
    }
}
