namespace Tavis.PrivateCache.Sterling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    internal class SterlingCacheContent : CacheContent
    {
        public string Key { get; set; }

        public SterlingCacheEntry CacheEntry { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public IDictionary<string, IEnumerable<string>> ResponseHeaders { get; set; }

        public IDictionary<string, IEnumerable<string>> ContentHeaders { get; set; }

        public byte[] Content { get; set; }

        private Lazy<HttpResponseMessage> _Response;

        new public HttpResponseMessage Response
        {
            get { return _Response.Value; }
        }

        public SterlingCacheContent()
        {
            _Response = new Lazy<HttpResponseMessage>(() => CreateResponse(this));
            ResponseHeaders = new Dictionary<string, IEnumerable<string>>();
            ContentHeaders = new Dictionary<string, IEnumerable<string>>();
        }

        public static HttpResponseMessage CreateResponse(SterlingCacheContent content)
        {
            Contract.Requires<ArgumentNullException>(content != null, "content");

            var response = new HttpResponseMessage(content.StatusCode);

            foreach (var keyValue in content.ResponseHeaders)
            {
                response.Headers.Add(keyValue.Key, keyValue.Value);
            }

            if (content.Content != null)
            {
                response.Content = new ByteArrayContent(content.Content);
                response.Content.Headers.Clear();

                foreach (var keyValue in content.ContentHeaders)
                {
                    response.Content.Headers.Add(keyValue.Key, keyValue.Value);
                }
            }

            return response;
        }

        public static async Task<SterlingCacheContent> CreateAsync(CacheContent content)
        {
            Contract.Requires<ArgumentException>(content.CacheEntry != null, "content");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(content.Key), "content");
            Contract.Requires<ArgumentException>(content.Response != null, "content");

            var sterlingContent = new SterlingCacheContent();

            if (content.CacheEntry is SterlingCacheEntry)
            {
                sterlingContent.CacheEntry = content.CacheEntry as SterlingCacheEntry;
            }
            else
            {
                sterlingContent.CacheEntry = new SterlingCacheEntry(content.CacheEntry);
            }

            sterlingContent.Key = content.Key;

            var response = content.Response;

            sterlingContent.StatusCode = response.StatusCode;

            foreach (var keyValue in response.Headers)
            {
                sterlingContent.ResponseHeaders.Add(keyValue.Key, keyValue.Value);
            }

            if (response.Content != null)
            {
                foreach (var keyValue in response.Content.Headers)
                {
                    sterlingContent.ContentHeaders.Add(keyValue.Key, keyValue.Value);
                }

                sterlingContent.Content = await response.Content.ReadAsByteArrayAsync();
            }

            return sterlingContent;
        }
    }
}
