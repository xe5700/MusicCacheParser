using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCacheParser.Padding
{
    class ZERO
    {
        public static byte[] Encoding(byte[] data, int padlen)
        {
            int padsize = data.Length % padlen;
            if (padsize == 0)
            {
                return data;
            }
            var output = new byte[data.Length + padsize];
            Buffer.BlockCopy(data, 0, output, 0, data.Length);
            for (int i = 0; i < padsize; i++)
            {
                output[data.Length + i] = (byte)0;
            }
            return output;
        }
    }
    class PKCS7
    {
        public static byte[] Encoding(byte[] data, int padlen)
        {
            int padsize = data.Length % padlen;
            if (padsize == 0)
            {
                return data;
            }
            var output = new byte[data.Length + padsize];
            Buffer.BlockCopy(data, 0, output, 0, data.Length);
            for(int i = 0; i < padsize; i++)
            {
                output[data.Length + i] = (byte)padsize;
            }
            return output;
        }
    }
}
