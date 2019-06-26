using HolySerializer.Metadata;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer.Default.Converters
{
    public class ConvertibleConverter : IBinaryConverter
    {
        private static ConcurrentDictionary<Type, IBinaryConverter> ConverterCache = new ConcurrentDictionary<Type, IBinaryConverter>();

        private IBinaryConverter CreateConverter(SerializeMetadata context)
        {
            IBinaryConverter converter = null;
            if (!ConverterCache.ContainsKey(context.Converter))
            {
                converter = Activator.CreateInstance(context.Converter) as IBinaryConverter;
                ConverterCache.TryAdd(context.Converter, converter);
            }
            else
            {
                converter = ConverterCache[context.Converter];
            }
            return converter;
        }

        void IBinaryConverter.WriteBinary(IBinaryWriter writer, object value, SerializeMetadata context, IHolySerializer serializer)
        {
            CreateConverter(context).WriteBinary(writer, value, context, serializer);
        }

        object IBinaryConverter.ReadBinary(IBinaryReader reader, SerializeMetadata context, IHolySerializer serializer)
        {
            return CreateConverter(context).ReadBinary(reader, context, serializer);
        }

        bool IBinaryConverter.CanConvert(SerializeMetadata context)
        {
            return context.Converter != null;
        }
    }
}
