using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoldProtect.Core.String
{
    class StringDec
    {
        public static string Decrypt(string @string, int key)
        {
            byte[] b = new byte[@string.Length];
            for (int i = 0; i < @string.Length; i++)
                b[i] = (byte)(@string[i] ^ key);
            return Encoding.UTF8.GetString(b);
        }
    }
}
