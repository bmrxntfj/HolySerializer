using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HolySerializer
{
    public abstract class BinarySerializerBase : IHolySerializer
    {

        public IEnumerable<IBinaryConverter> Converters { get; protected set; }

        public IMetadataProvider MetadataProvider { get; protected set; }

        public IBinaryReaderCreator ReaderCreator { get; protected set; }
        public IBinaryWriterCreator WriterCreator { get; protected set; }

        #region ISerializer

        public O Serialize<I, O>(I i)
        {
            if (((IHolySerializer)this).MetadataProvider == null) { throw new NullReferenceException("MetadataProvider is null."); }
            var writer = ((IHolySerializer)this).WriterCreator.Create();
            var context = ((IHolySerializer)this).MetadataProvider.GetMetadataFrom(i);
            foreach (var c in this.Converters)
            {
                if (c.CanConvert(context))
                {
                    c.WriteBinary(writer, i, context, this);
                    break;
                }
            };
            return (O)(object)(writer.ToArray());
        }

        public O Deserialize<I, O>(I i)
        {
            return (O)(((ISerializer)this).Deserialize(i, typeof(O)));
        }

        public object Deserialize(object i, Type targetType)
        {
            if (((IHolySerializer)this).MetadataProvider == null) { throw new NullReferenceException("MetadataProvider is null."); }
            if (i == null) { return null; }
            var buffer = (byte[])i;
            object instance = null;
            var reader = ((IHolySerializer)this).ReaderCreator.Create(buffer);
            var context = ((IHolySerializer)this).MetadataProvider.GetMetadataFrom(targetType);
            foreach (var c in this.Converters)
            {
                if (c.CanConvert(context))
                {
                    instance = c.ReadBinary(reader, context, this);
                    break;
                }
            };
            return instance;
        }
        #endregion

        public byte[] Serialize(object obj)
        {
            return ((ISerializer)this).Serialize<object, byte[]>(obj);
        }

        public T Deserialize<T>(byte[] array)
        {
            return ((ISerializer)this).Deserialize<byte[], T>(array);
        }

        public object Deserialize(byte[] array, Type targetType)
        {
            return ((ISerializer)this).Deserialize(array, targetType);
        }
    }
}
