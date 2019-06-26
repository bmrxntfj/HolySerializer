using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HolySerializer
{
    public static class HelperExtension
    {
        public static bool IsInherit(this Type type, Type baseType)
        {
            return type.IsSubclassOf(baseType)
                || type.GetInterfaces().Contains(baseType)
                || (baseType.IsGenericType && type.GetInterfaces().Where(c => c.IsGenericType).Select(c => c.GetGenericTypeDefinition()).Contains(baseType));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) { return; }
            foreach (var s in source) { action(s); }
        }

        private static ConcurrentDictionary<string, MethodInfo> cache = new ConcurrentDictionary<string, MethodInfo>();

        public static object GetValue(string property, object target)
        {
            var type = target.GetType();
            var key = string.Format("get_{0}_{1}", type.FullName, property);
            MethodInfo dmh = null;
            if (!cache.TryGetValue(key, out dmh))
            {
                var pro = type.GetProperty(property);
                if (pro == null) { return null; }
                dmh = pro.GetGetMethod();
                cache.TryAdd(key, dmh);
            }
            return dmh.Invoke(target, null);
        }
        public static void SetValue(string property, object target, params object[] paramters)
        {
            var type = target.GetType();
            var key = string.Format("set_{0}_{1}", type.FullName, property);
            MethodInfo dmh = null;
            if (!cache.TryGetValue(key, out dmh))
            {
                var pro = type.GetProperty(property);
                if (pro == null) { return; }
                dmh = pro.GetSetMethod();
                cache.TryAdd(key, dmh);
            }
            dmh.Invoke(target, paramters);
        }
    }
}
