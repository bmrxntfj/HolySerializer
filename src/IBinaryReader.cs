using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer
{
    public interface IBinaryReaderCreator
    {
        IBinaryReader Create(byte[] array);
    }

    public interface IBinaryReader
    {
        int ReadRemainderLength();
        byte[] Read(bool isReverse);
        byte[] Read(int count, bool isReverse);
    }
}
