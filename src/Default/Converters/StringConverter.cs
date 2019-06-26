using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer.Default.Converters
{
    public class StringConverter : IBinaryConverter
    {
        void IBinaryConverter.WriteBinary(IBinaryWriter writer, object value, SerializeMetadata context, IHolySerializer serializer)
        {
            var encoding = (context == null || string.IsNullOrWhiteSpace(context.Options)) ? Encoding.UTF8 : Encoding.GetEncoding(context.Options);
            var array = value == null ? Array.Empty<byte>() : encoding.GetBytes(value.ToString());
            if (context.Length > 0 && array.Length != context.Length)
            {
                var fillArray = new byte[context.Length];
                Array.Copy(array, fillArray, array.Length > fillArray.Length ? fillArray.Length : array.Length);
                array = fillArray;
            }
            writer.Write(array, context.IsReverse);
        }

        object IBinaryConverter.ReadBinary(IBinaryReader reader, SerializeMetadata context, IHolySerializer serializer)
        {
            var array = reader.Read(context.Length, context.IsReverse);
            var encoding = (context == null || string.IsNullOrWhiteSpace(context.Options)) ? Encoding.UTF8 : Encoding.GetEncoding(context.Options);
            return encoding.GetString(array).TrimEnd('\0');
        }

        bool IBinaryConverter.CanConvert(SerializeMetadata context)
        {
            return context.ObjectType == typeof(string);
        }
    }
}
