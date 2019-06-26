using HolySerializer.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HolySerializer;

namespace HolySerializer.Default.Converters
{
    public class ByteArrayConverter : IBinaryConverter
    {
        void IBinaryConverter.WriteBinary(IBinaryWriter writer, object value, SerializeMetadata context, IHolySerializer serializer)
        {
            writer.Write((value as IEnumerable<byte>).ToArray(), context.IsReverse);
        }

        object IBinaryConverter.ReadBinary(IBinaryReader reader, SerializeMetadata context, IHolySerializer serializer)
        {
            var array = reader.Read(context.Length, context.IsReverse);
            return array;
        }

        bool IBinaryConverter.CanConvert(SerializeMetadata context)
        {
            return context.ObjectType != null && context.ObjectType.IsInherit(typeof(IEnumerable<byte>));
        }
    }
}
