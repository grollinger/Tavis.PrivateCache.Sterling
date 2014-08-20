namespace Tavis.PrivateCache.Sterling
{
    using System;
    using System.Collections.Generic;
    using Wintellect.Sterling.Core.Serialization;

    internal class SterlingContentSerializer : BaseSerializer
    {
        private static readonly List<Type> SupportedTypes = new List<Type>()
        {
            typeof(PrimaryCacheKey)
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
        }
    }
}
