namespace Tavis.PrivateCache.Sterling
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Wintellect.Sterling.Core.Serialization;

    internal class SterlingContentSerializer : BaseSerializer
    {
        private static readonly List<Type> SupportedTypes = new List<Type>()
        {
            typeof(PrimaryCacheKey),
            typeof(HttpResponseMessage)
        };

        public override bool CanSerialize(Type targetType)
        {
            return SupportedTypes.Contains(targetType);
        }

        public override object Deserialize(Type type, System.IO.BinaryReader reader)
        {
            if (typeof(PrimaryCacheKey) == type)
            {
                var uri = reader.ReadString();
                var method = reader.ReadString();

                return new PrimaryCacheKey(uri, method);
            }
            else if (typeof(HttpResponseMessage) == type)
            {
                var content = new StreamContent(reader.BaseStream);

                var message = content.ReadAsHttpResponseMessageAsync().Result;

                return message;
            }

            throw new NotImplementedException();
        }

        public override void Serialize(object target, System.IO.BinaryWriter writer)
        {
            if (target is PrimaryCacheKey)
            {
                var key = target as PrimaryCacheKey;

                writer.Write(key.Uri ?? string.Empty);
                writer.Write(key.Method ?? string.Empty);
            }
            else if (target is HttpResponseMessage)
            {
                var msg = target as HttpResponseMessage;

                var content = new HttpMessageContent(msg);

                content.CopyToAsync(writer.BaseStream).Wait();
            }
        }
    }
}
