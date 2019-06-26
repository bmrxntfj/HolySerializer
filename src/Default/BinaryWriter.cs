using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer.Default
{
    public class BinaryWriterCreator : IBinaryWriterCreator
    {

        IBinaryWriter IBinaryWriterCreator.Create()
        {
            return new BinaryWriter();
        }
    }

    public class BinaryWriter : IBinaryWriter
    {
        List<byte> buffer = new List<byte>();

        void IBinaryWriter.Write(byte[] array, bool isReverse)
        {
            if (array != null)
            {
                if (isReverse)
                {
                    Array.Reverse(array);
                }
                buffer.AddRange(array);
            }
        }

        byte[] IBinaryWriter.ToArray()
        {
            return buffer.ToArray();
        }
    }
}
