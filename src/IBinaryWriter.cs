using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer
{
    public interface IBinaryWriterCreator
    {
        IBinaryWriter Create();
    }

    public interface IBinaryWriter
    {
        void Write(byte[] array, bool isReverse);
        byte[] ToArray();
    }
}
