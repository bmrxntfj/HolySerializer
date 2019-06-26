using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MetadataIgnoreAttribute : Attribute { }
}
