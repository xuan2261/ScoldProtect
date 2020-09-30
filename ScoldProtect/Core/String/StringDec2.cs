using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ScoldProtect.Core.String
{
    class StringDec2
    {
		public static string Decrypt(string encryptedText)
		{
			byte[] pass = Encoding.UTF8.GetBytes("48235728");
			byte[] text = Convert.FromBase64String(encryptedText);
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();
			ICryptoTransform transform = des.CreateDecryptor(pass, Encoding.ASCII.GetBytes("fmwa3x6k"));
			MemoryStream memoryStream = new MemoryStream(text);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
			byte[] array2 = new byte[text.Length];
			int count = cryptoStream.Read(array2, 0, array2.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(array2, 0, count).TrimEnd("\0".ToCharArray());
		}
	}
}
