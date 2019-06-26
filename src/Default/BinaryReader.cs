using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolySerializer.Default
{
    public class BinaryReaderCreator : IBinaryReaderCreator
    {

        IBinaryReader IBinaryReaderCreator.Create(byte[] array)
        {
            return new BinaryReader(array);
        }
    }

    public class BinaryReader : IBinaryReader
    {

        byte[] buffer = null;
        int offset = 0;

        /// <summary>
        /// ArgumentNullException
        /// </summary>
        /// <param name="array"></param>
        public BinaryReader(byte[] array)
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            buffer = array;
        }

        byte[] IBinaryReader.Read(bool isReverse)
        {
            return ((IBinaryReader)this).Read(((IBinaryReader)this).ReadRemainderLength(), isReverse);
        }

        byte[] IBinaryReader.Read(int count, bool isReverse)
        {
            if (count == 0) { count = buffer.Length - offset; }
            var array = buffer.Skip(offset).Take(count).ToArray();
            if (array.Length != count) { throw new Exception(string.Format("length of data not equal {0}.", count)); }
            offset += count;
            if (isReverse) { Array.Reverse(array); }
            return array;
        }

        int IBinaryReader.ReadRemainderLength()
        {
            return buffer.Skip(offset).Count();
        }
    }
}
