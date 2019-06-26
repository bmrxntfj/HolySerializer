using HolySerializer.Default.Converters;
using HolySerializer.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer.Default
{
    public class DefaultBinarySerializer : BinarySerializerBase
    {
        public static DefaultBinarySerializer Instance = new DefaultBinarySerializer();

        public DefaultBinarySerializer()
        {
            var converters = new List<IBinaryConverter>();
            converters.Add(new ConvertibleConverter());
            converters.Add(new ByteArrayConverter());
            converters.Add(new StringConverter());
            converters.Add(new ObjectConverter());
            converters.Add(new PrimitiveConverter());
            base.Converters = converters;
            base.MetadataProvider = new MetadataAttributeProvider { Strict = true, OrderType = MetadataOrderType.Declare, Asc = true };
            base.ReaderCreator = new BinaryReaderCreator();
            base.WriterCreator = new BinaryWriterCreator();
        }
    }
}
