using System;
using System.Collections.Generic;
using System.Text;

namespace HolySerializer
{
    public interface IHolySerializer : ISerializer
    {
        IEnumerable<IBinaryConverter> Converters { get; }
        IMetadataProvider MetadataProvider { get; }
        IBinaryReaderCreator ReaderCreator { get; }
        IBinaryWriterCreator WriterCreator { get; }
    }
}
