using HolySerializer.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer
{
    public interface IBinaryConverter
    {
        void WriteBinary(IBinaryWriter writer, object value, SerializeMetadata context, IHolySerializer serializer);
        object ReadBinary(IBinaryReader reader, SerializeMetadata context, IHolySerializer serializer);
        bool CanConvert(SerializeMetadata context);
    }
}
