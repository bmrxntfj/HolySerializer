using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HolySerializer.Metadata
{
    public abstract class AbstractMetadataProvider : IMetadataProvider
    {
        public abstract SerializeMetadata GetMetadataFrom(object value);

        protected bool IsRecursive(Type type)
        {
            return !(type.IsPrimitive || type.IsEnum || type == typeof(Guid)
                        || type == typeof(string) || type == typeof(DateTime)
                        || type.IsInherit(typeof(IEnumerable)));
        }

        protected PropertyInfo[] GetAvailableProperty(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.GetProperty | BindingFlags.SetProperty)
                .Where(c => c.CanWrite && c.CanRead && c.GetCustomAttribute<MetadataIgnoreAttribute>(true) == null).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">instance or type</param>
        /// <returns></returns>
        protected int SizeOf(object value)
        {
            if (value == null) { return 0; }
            //if (!(value is Type)) { return System.Runtime.InteropServices.Marshal.SizeOf(value); }
            var type = value as Type;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return sizeof(bool);
                case TypeCode.Byte:
                    return sizeof(byte);
                case TypeCode.Char:
                    return sizeof(char);
                //case TypeCode.DBNull:
                //    break;
                case TypeCode.DateTime:
                    return sizeof(long);
                case TypeCode.Decimal:
                    return sizeof(decimal);
                case TypeCode.Double:
                    return sizeof(double);
                //case TypeCode.Empty:
                //    break;
                case TypeCode.Int16:
                    return sizeof(short);
                case TypeCode.Int32:
                    return sizeof(int);
                case TypeCode.Int64:
                    return sizeof(char);
                case TypeCode.Object:
                    {
                        if (type == typeof(Guid)) { return Guid.Empty.ToByteArray().Length; }
                        return 0;
                    }
                case TypeCode.SByte:
                    return sizeof(sbyte);
                case TypeCode.Single:
                    return sizeof(float);
                //case TypeCode.String:
                //    break;
                case TypeCode.UInt16:
                    return sizeof(ushort);
                case TypeCode.UInt32:
                    return sizeof(uint);
                case TypeCode.UInt64:
                    return sizeof(ulong);
                default:
                    return 0;
            }
        }
    }
}
