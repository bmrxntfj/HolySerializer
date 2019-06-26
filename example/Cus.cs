using HolySerializer;
using HolySerializer.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace HolySerializerExample
{
    [BinaryMember]
    public class Cus
    {
        [BinaryMember]
        public byte Version { get; set; }
        [BinaryMember]
        public int Id { get; set; }
        [BinaryMember(Length = 20, Options = "utf-8")]
        public string Key { get; set; }
        [BinaryMember(Length = 50, Options = "utf-8")]
        public string Remark { get; set; }
        [BinaryMember]
        public Cuso Cuso { get; set; }
    }

    [BinaryMember]
    public class Cuso
    {
        [BinaryMember]
        public int CusId { get; set; }
        [BinaryMember(Length = 20, Options = "utf-8")]
        public string Name { get; set; }
        [BinaryMember(Length = 100, Options = "utf-8")]
        public string Address { get; set; }
        [BinaryMember]
        public DateTime CreatedTime { get; set; }
        [BinaryMember(Converter = typeof(GuidConverter))]
        public Guid UUId { get; set; }
    }

    public class GuidConverter : IBinaryConverter
    {
        void IBinaryConverter.WriteBinary(IBinaryWriter writer, object value, SerializeMetadata context, IHolySerializer serializer)
        {
            var guid = (Guid)value;
            guid.ToByteArray();
            writer.Write(guid.ToByteArray(), context.IsReverse);
        }

        object IBinaryConverter.ReadBinary(IBinaryReader reader, SerializeMetadata context, IHolySerializer serializer)
        {
            var array = reader.Read(context.Length, context.IsReverse);
            return new Guid(array);
        }

        bool IBinaryConverter.CanConvert(SerializeMetadata context)
        {
            return context.ObjectType != null && context.ObjectType.IsInherit(typeof(IEnumerable<byte>));
        }
    }
}
