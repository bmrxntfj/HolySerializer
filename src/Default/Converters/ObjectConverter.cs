using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer.Default.Converters
{
    public class ObjectConverter : IBinaryConverter
    {
        void IBinaryConverter.WriteBinary(IBinaryWriter writer, object value, SerializeMetadata context, IHolySerializer serializer)
        {
            var buffer = new List<byte>();
            context.Childs.ForEach(b =>
            {
                var val = HelperExtension.GetValue(b.Name, value);
                var childWriter = serializer.WriterCreator.Create();
                foreach (var c in serializer.Converters)
                {
                    if (c.CanConvert(b))
                    {
                        c.WriteBinary(childWriter, val, b, serializer);
                        break;
                    }
                };
                buffer.AddRange(childWriter.ToArray());
            });
            writer.Write(buffer.ToArray(), context.IsReverse);
        }

        object IBinaryConverter.ReadBinary(IBinaryReader reader, SerializeMetadata context, IHolySerializer serializer)
        {
            var array = reader.Read(context.Length == 0 ? reader.ReadRemainderLength() : context.Length, context.IsReverse);
            var value = Activator.CreateInstance(context.ObjectType);
            var childReader = serializer.ReaderCreator.Create(array);
            context.Childs.ForEach(b =>
            {
                foreach (var c in serializer.Converters)
                {
                    if (c.CanConvert(b))
                    {
                        var val = c.ReadBinary(childReader, b, serializer);
                        HelperExtension.SetValue(b.Name, value, new object[] { val });
                        break;
                    }
                };
            });
            return value;
        }

        bool IBinaryConverter.CanConvert(SerializeMetadata context)
        {
            return Type.GetTypeCode(context.ObjectType) == TypeCode.Object;
        }
    }
}
