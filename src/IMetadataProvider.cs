using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer
{
    public interface IMetadataProvider
    {
        SerializeMetadata GetMetadataFrom(object value);
    }
}
