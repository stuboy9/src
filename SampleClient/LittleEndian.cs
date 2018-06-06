using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NetUtil
{
    static class LittleEndian
    {
        public static int LittleEndianToInt32(byte[] bytes, int startIndex)
        {
            int result = 0;

            result |= bytes[startIndex];
            result <<= 8;
            result |= bytes[startIndex + 1];
            result <<= 8;
            result |= bytes[startIndex + 2];
            result <<= 8;
            result |= bytes[startIndex + 3];

            return result;
        }

        public static byte[] GetLittleEndianBytes(int value)
        {
            return Correct(BitConverter.GetBytes(value));
        }

        private static byte[] Correct(byte[] data)
        {
            //if (BitConverter.IsLittleEndian)
            Array.Reverse(data);
            return data;
        }
    }
}
