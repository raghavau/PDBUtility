using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PDBUtility
{
    public static class Decrypt
    {

        public static string DecryptText(string DecText)
        {
            return DecryptString(DecText, "&%#@?,:*");
        }

        private static string DecryptString(string strText, string sDecrKey)
        {
            byte[] byKey = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
            byte[] inputByteArray = new byte[strText.Length + 1];
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
                DESCryptoServiceProvider Des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream Ms = new MemoryStream();
                CryptoStream Cs = new CryptoStream(Ms, Des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                Cs.Write(inputByteArray, 0, inputByteArray.Length);
                Cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(Ms.ToArray());
            }
            catch (Exception)
            {
                return "Error Occured while Decrypting the '" + strText + "'";
            }
        }

    }
}