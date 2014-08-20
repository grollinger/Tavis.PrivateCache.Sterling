﻿namespace Tavis.PrivateCache.Sterling.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using Wintellect.Sterling.Core;

    class SterlingPlatformAdapter : ISterlingPlatformAdapter
    {
        public SterlingPlatformAdapter()
        {
        }

        public bool IsAssignableFrom(Type target, Type test)
        {
            return target.IsAssignableFrom(test);
        }

        public bool IsSubclassOf(Type target, Type test)
        {
            return target.IsSubclassOf(test);
        }

        public bool IsEnum(Type target)
        {
            return target.IsEnum;
        }

        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields();
        }

        public IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties();
        }

        public MethodInfo GetGetMethod(PropertyInfo property)
        {
            return property.GetGetMethod();
        }

        public MethodInfo GetSetMethod(PropertyInfo property)
        {
            return property.GetSetMethod();
        }

        public IEnumerable<Attribute> GetCustomAttributes(Type target, Type attributeType, bool inherit)
        {
            return target.GetCustomAttributes(attributeType, inherit).Cast<Attribute>();
        }

        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public Tuple<Type, Action<BinaryWriter, object>, Func<BinaryReader, object>> GetBitmapSerializer()
        {
            return null;
        }
    }
}
