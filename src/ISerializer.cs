using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer
{
    public interface ISerializer
    {
        O Serialize<I, O>(I i);
        O Deserialize<I, O>(I i);
        object Deserialize(object i, Type targetType);
    }
}
