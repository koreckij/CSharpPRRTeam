using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BitcoinExplorer.Core
{
    public class HashHelper
    {
       public byte[] HashString(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            SHA256Managed hashstring = new SHA256Managed();
            var hash = hashstring.ComputeHash(bytes);
            return hash;
        }

        public static string Bytes2String(byte[] hash)
        {
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
    }
}
