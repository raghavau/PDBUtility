using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PDBUtility
{
    public static class Encrypt
    {

        public static string EncryptText(string EncText)
        {
            return EncryptString(EncText, "&%#@?,:*");
        }

        private static string EncryptString(string strText, string strEncrKey)
        {
            byte[] byKey = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                DESCryptoServiceProvider Des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream Ms = new MemoryStream();
                CryptoStream Cs = new CryptoStream(Ms, Des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                Cs.Write(inputByteArray, 0, inputByteArray.Length);
                Cs.FlushFinalBlock();
                return Convert.ToBase64String(Ms.ToArray());
            }
            catch (Exception)
            {
                return "Error Occured while Encrypting the '" + strText + "'";
            }
        }

    }
}