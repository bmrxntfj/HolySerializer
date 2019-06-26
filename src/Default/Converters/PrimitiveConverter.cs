using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer.Default.Converters
{
    public class PrimitiveConverter : IBinaryConverter
    {
        void IBinaryConverter.WriteBinary(IBinaryWriter writer, object value, SerializeMetadata context, IHolySerializer serializer)
        {
            byte[] array = null;
            var typeCode = value == null ? TypeCode.Empty : Type.GetTypeCode(value.GetType());
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    array = BitConverter.GetBytes((bool)value);
                    break;
                case TypeCode.Byte:
                    array = new byte[] { (byte)value };
                    break;
                case TypeCode.Char:
                    array = BitConverter.GetBytes((char)value);
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.DateTime:
                    array = BitConverter.GetBytes(((DateTime)value).Ticks);
                    break;
                case TypeCode.Decimal:
                    {
                        var bits = decimal.GetBits((decimal)value);
                        var bytes = new List<byte>();
                        foreach (Int32 i in bits)
                        {
                            bytes.AddRange(BitConverter.GetBytes(i));
                        }
                        array= bytes.ToArray();
                    }
                    break;
                case TypeCode.Double:
                    array = BitConverter.GetBytes((double)value);
                    break;
                case TypeCode.Empty:
                    break;
                case TypeCode.Int16:
                    array = BitConverter.GetBytes((short)value);
                    break;
                case TypeCode.Int32:
                    array = BitConverter.GetBytes((int)value);
                    break;
                case TypeCode.Int64:
                    array = BitConverter.GetBytes((long)value);
                    break;
                case TypeCode.SByte:
                    array = new byte[] { (byte)value };
                    break;
                case TypeCode.Single:
                    array = BitConverter.GetBytes((float)value);
                    break;
                case TypeCode.UInt16:
                    array = BitConverter.GetBytes((ushort)value);
                    break;
                case TypeCode.UInt32:
                    array = BitConverter.GetBytes((uint)value);
                    break;
                case TypeCode.UInt64:
                    array = BitConverter.GetBytes((ulong)value);
                    break;
                default:
                    break;
            }
            writer.Write(array, context.IsReverse);
        }

        object IBinaryConverter.ReadBinary(IBinaryReader reader, SerializeMetadata context, IHolySerializer serializer)
        {
            var array = reader.Read(context.Length, context.IsReverse);
            object value = null;
            switch (Type.GetTypeCode(context.ObjectType))
            {
                case TypeCode.Boolean:
                    value = BitConverter.ToBoolean(array, 0);
                    break;
                case TypeCode.Byte:
                    value = array[0];
                    break;
                case TypeCode.Char:
                    value = BitConverter.ToChar(array, 0);
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.DateTime:
                    value = new DateTime(BitConverter.ToInt64(array, 0));
                    break;
                case TypeCode.Decimal:
                    {
                        if (array.Count() != 16) { throw new Exception("A decimal must be created from exactly 16 bytes"); }
                        var bits = new Int32[4];
                        for (int i = 0; i <= 15; i += 4)
                        {
                            bits[i / 4] = BitConverter.ToInt32(array, i);
                        }
                        value= new decimal(bits);
                    }
                    break;
                case TypeCode.Double:
                    value = BitConverter.ToDouble(array, 0);
                    break;
                case TypeCode.Empty:
                    break;
                case TypeCode.Int16:
                    value = BitConverter.ToInt16(array, 0);
                    break;
                case TypeCode.Int32:
                    value = BitConverter.ToInt32(array, 0);
                    break;
                case TypeCode.Int64:
                    value = BitConverter.ToInt64(array, 0);
                    break;
                case TypeCode.SByte:
                    value = (sbyte)array[0];
                    break;
                case TypeCode.Single:
                    value = BitConverter.ToSingle(array, 0);
                    break;
                case TypeCode.UInt16:
                    value = BitConverter.ToUInt16(array, 0);
                    break;
                case TypeCode.UInt32:
                    value = BitConverter.ToUInt32(array, 0);
                    break;
                case TypeCode.UInt64:
                    value = BitConverter.ToUInt64(array, 0);
                    break;
                default:
                    break;
            }
            return value;
        }

        bool IBinaryConverter.CanConvert(SerializeMetadata context)
        {
            return true;
        }
    }
}
