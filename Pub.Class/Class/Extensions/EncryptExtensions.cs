//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
using System.Web.Script.Serialization;
#endif
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace Pub.Class {
    /// <summary>
    /// �����뷽��
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class EncryptExtensions {
        private static readonly byte[] AESKeys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };
        /// <summary>
        /// AES����
        /// </summary>
        /// <param name="encryptString">����</param>
        /// <param name="encryptKey">KEY</param>
        /// <returns>AES����</returns>
        public static string AESEncode(this string encryptString, string encryptKey) {
            encryptKey = encryptKey.SubString(32, "");
            encryptKey = encryptKey.PadRight(32, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            rijndaelProvider.IV = AESKeys;
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }
        /// <summary>
        /// AES����
        /// </summary>
        /// <param name="decryptString">AES����</param>
        /// <param name="decryptKey">KEY</param>
        /// <returns>����</returns>
        public static string AESDecode(this string decryptString, string decryptKey) {
            try {
                decryptKey = decryptKey.SubString(32, "");
                decryptKey = decryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = AESKeys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            } catch { return string.Empty; }
        }

        private static readonly byte[] DESKeys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary>
        /// DES����
        /// </summary>
        /// <param name="encryptString">����</param>
        /// <param name="encryptKey">KEY</param>
        /// <returns>����</returns>
        public static string DESEncode(this string encryptString, string encryptKey = null) {
            //if (encryptKey.IsNullEmpty()) encryptKey = IV;
            //encryptKey = encryptKey.SubString(8, "");
            //encryptKey = encryptKey.PadRight(8, ' ');
            //byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            //byte[] rgbIV = DESKeys;
            //byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            //DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            //MemoryStream mStream = new MemoryStream();
            //CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            //cStream.Write(inputByteArray, 0, inputByteArray.Length);
            //cStream.FlushFinalBlock();
            //return Convert.ToBase64String(mStream.ToArray());
            return Convert.ToBase64String(DESEncode(Encoding.UTF8.GetBytes(encryptString), encryptKey));
        }
        public static byte[] DESEncode(this byte[] inputByteArray, string encryptKey = null) {
            if (encryptKey.IsNullEmpty()) encryptKey = IV;
            encryptKey = encryptKey.SubString(8, "");
            encryptKey = encryptKey.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = DESKeys;
            //byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            //return Convert.ToBase64String(mStream.ToArray());
            return mStream.ToArray();
        }
        /// <summary>
        /// DES����
        /// </summary>
        /// <param name="decryptString">����</param>
        /// <param name="decryptKey">KEY</param>
        /// <returns>����</returns>
        public static string DESDecode(this string decryptString, string decryptKey = null) {
            byte[] b = DESDecode(Convert.FromBase64String(decryptString), decryptKey);
            return b.IsNull() ? string.Empty : Encoding.UTF8.GetString(b);
            //try {
            //    if (decryptKey.IsNullEmpty()) decryptKey = IV;
            //    decryptKey = decryptKey.SubString(8, "");
            //    decryptKey = decryptKey.PadRight(8, ' ');
            //    byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
            //    byte[] rgbIV = DESKeys;
            //    byte[] inputByteArray = Convert.FromBase64String(decryptString);
            //    DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

            //    MemoryStream mStream = new MemoryStream();
            //    CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            //    cStream.Write(inputByteArray, 0, inputByteArray.Length);
            //    cStream.FlushFinalBlock();
            //    return Encoding.UTF8.GetString(mStream.ToArray());
            //} catch { return string.Empty; }
        }
        public static byte[] DESDecode(this byte[] inputByteArray, string decryptKey = null) {
            try {
                if (decryptKey.IsNullEmpty()) decryptKey = IV;
                decryptKey = decryptKey.SubString(8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = DESKeys;
                //byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                //return Encoding.UTF8.GetString(mStream.ToArray());
                return mStream.ToArray();
            } catch { return null; }
        }

        private static readonly string IV = "eF274e95aC95";
        /// <summary>
        /// 3DES����(��utf-8����)
        /// </summary>
        /// <param name="data">Ҫ���ܵ�����</param>
        /// <param name="key">��Կ</param>
        /// <returns>���ܺ�����</returns>
        public static string DES3Encode(this string data, string key = null) {
            return Convert.ToBase64String(DES3Encode(Encoding.UTF8.GetBytes(data), key));
        }
        public static byte[] DES3Encode(this byte[] arData, string key = null) {
            if (key.IsNullEmpty()) key = IV;
            TripleDESCryptoServiceProvider desCSP = new TripleDESCryptoServiceProvider();
            ICryptoTransform ctf = desCSP.CreateEncryptor(Convert.FromBase64String(key), Convert.FromBase64String(IV));
            byte[] encData = ctf.TransformFinalBlock(arData, 0, arData.Length);
            desCSP.Clear();
            return encData;
        }
        /// <summary>
        /// 3DES����(��utf-8����)
        /// </summary>
        /// <param name="data">Ҫ���ܵ�����</param>
        /// <param name="key">��Կ</param>
        /// <returns>���ܺ�����</returns>
        public static string DES3Decode(this string data, string key = null) {
            return Encoding.UTF8.GetString(DES3Decode(Convert.FromBase64String(data), key));
        }
        public static byte[] DES3Decode(this byte[] encData, string key = null) {
            if (key.IsNullEmpty()) key = IV;
            TripleDESCryptoServiceProvider desCSP = new TripleDESCryptoServiceProvider();
            ICryptoTransform ctf = desCSP.CreateDecryptor(Convert.FromBase64String(key), Convert.FromBase64String(IV));
            byte[] arData = ctf.TransformFinalBlock(encData, 0, encData.Length);
            desCSP.Clear();
            return arData;
        }

        public static string Base64Encode(this string encryptString, Encoding encoding = null) {
            byte[] encbuff = (encoding.IsNull() ? System.Text.Encoding.UTF8 : encoding).GetBytes(encryptString);
            return Convert.ToBase64String(encbuff);
        }
        /// <summary>
        /// Base64����
        /// </summary>
        /// <param name="decryptString">����</param>
        /// <param name="encoding">����</param>
        /// <returns>����</returns>
        public static string Base64Decode(this string decryptString, Encoding encoding = null) {
            byte[] decbuff = Convert.FromBase64String(decryptString);
            return (encoding.IsNull() ? System.Text.Encoding.UTF8 : encoding).GetString(decbuff);
        }

        ///// <summary>
        ///// RSA����
        ///// </summary>
        ///// <param name="s">����</param>
        ///// <param name="key">KEY</param>
        ///// <returns>����</returns>
        //public static string RSADecrypt(this string s, string key) {
        //    string result = null;
        //    if (string.IsNullOrEmpty(s)) throw new ArgumentException("An empty string value cannot be encrypted.");
        //    if (string.IsNullOrEmpty(key)) throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");

        //    CspParameters cspp = new CspParameters();
        //    cspp.KeyContainerName = key;

        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp);
        //    rsa.PersistKeyInCsp = true;

        //    string[] decryptArray = s.Split(new string[] { "-" }, StringSplitOptions.None);
        //    byte[] decryptByteArray = Array.ConvertAll<string, byte>(decryptArray, (a => Convert.ToByte(byte.Parse(a, System.Globalization.NumberStyles.HexNumber))));
        //    byte[] bytes = rsa.Decrypt(decryptByteArray, true);
        //    result = System.Text.UTF8Encoding.UTF8.GetString(bytes);

        //    return result;
        //}
        ///// <summary>
        ///// RSA����
        ///// </summary>
        ///// <param name="s">����</param>
        ///// <param name="key">KEY</param>
        ///// <returns>����</returns>
        //public static string RSAEncrypt(this string s, string key) {
        //    if (string.IsNullOrEmpty(s)) throw new ArgumentException("An empty string value cannot be encrypted.");
        //    if (string.IsNullOrEmpty(key)) throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");

        //    CspParameters cspp = new CspParameters();
        //    cspp.KeyContainerName = key;

        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp);
        //    rsa.PersistKeyInCsp = true;
        //    byte[] bytes = rsa.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(s), true);
        //    return BitConverter.ToString(bytes);
        //}
        /// <summary>
        /// ��ȡRSA��Կ������
        /// </summary>
        /// <param name="PrivateKey">��Կ</param>
        /// <param name="PublicKey">��Կ</param>
        public static void GetRSAPublicAndPrivateKey(ref string PrivateKey, ref string PublicKey) {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            PrivateKey = rsa.ToXmlString(true);
            PublicKey = rsa.ToXmlString(false);
            rsa.Clear();
        }
        /// <summary>
        /// RSA�����ַ���
        /// </summary>
        /// <param name="data">��������</param>
        /// <param name="publickey">��Կ</param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(this byte[] data, string publickey) {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickey);
            int keySize = rsa.KeySize / 8;
            int bufferSize = keySize - 11;
            byte[] buffer = new byte[bufferSize];
            MemoryStream msInput = new MemoryStream(data);
            MemoryStream msOutput = new MemoryStream();
            int readLen = msInput.Read(buffer, 0, bufferSize);
            byte[] dataToEnc, encData;
            while (readLen > 0) {
                dataToEnc = new byte[readLen];
                Array.Copy(buffer, 0, dataToEnc, 0, readLen);
                encData = rsa.Encrypt(dataToEnc, false);
                msOutput.Write(encData, 0, encData.Length);
                readLen = msInput.Read(buffer, 0, bufferSize);
            }
            msInput.Close();
            byte[] result = msOutput.ToArray();    //�õ����ܽ��
            msOutput.Close();
            rsa.Clear();
            return result;
        }
        /// <summary>
        /// RSA�����ַ���
        /// </summary>
        /// <param name="encryptString">���ܴ�</param>
        /// <param name="publickey">��Կ</param>
        /// <returns></returns>
        public static string RSAEncrypt(this string encryptString, string publickey) {
            return Convert.ToBase64String(RSAEncrypt(Encoding.UTF8.GetBytes(encryptString), publickey));
        }
        /// <summary>
        /// RSA�����ַ���
        /// </summary>
        /// <param name="data">���ܴ�</param>
        /// <param name="privatekey">��Կ</param>
        /// <returns></returns>
        public static byte[] RSADecrypt(this byte[] data, string privatekey) {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privatekey);
            int keySize = rsa.KeySize / 8;
            byte[] buffer = new byte[keySize];
            MemoryStream msInput = new MemoryStream(data);
            MemoryStream msOutput = new MemoryStream();
            int readLen = msInput.Read(buffer, 0, keySize);
            byte[] dataToDec, decData;
            while (readLen > 0) {
                dataToDec = new byte[readLen];
                Array.Copy(buffer, 0, dataToDec, 0, readLen);
                decData = rsa.Decrypt(dataToDec, false);
                msOutput.Write(decData, 0, decData.Length);
                readLen = msInput.Read(buffer, 0, keySize);
            }
            msInput.Close();
            byte[] result = msOutput.ToArray();    //�õ����ܽ��
            msOutput.Close();
            rsa.Clear();
            return result;
        }
        /// <summary>
        /// RSA�����ַ���
        /// </summary>
        /// <param name="decryptString">���ܴ�</param>
        /// <param name="privatekey">��Կ</param>
        /// <returns></returns>
        public static string RSADecrypt(this string decryptString, string privatekey) {
            return Encoding.UTF8.GetString(RSADecrypt(Convert.FromBase64String(decryptString), privatekey));
        }

        /// <summary>
        /// ��ȡ���ݵ�MD5ժҪ
        /// </summary>
        /// <param name="data">ԭʼ����</param>
        /// <param name="encoding">��������</param>
        /// <returns>MD5ժҪ</returns>
        public static string MD5(this string data, Encoding encoding = null) {
            //����ַ���Ϊ�գ��򷵻�
            if (data == null) data = string.Empty;
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] arr = md5Hasher.ComputeHash((encoding == null ? Encoding.UTF8 : encoding).GetBytes(data));
            md5Hasher.Clear();
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < arr.Length; i++) sBuilder.Append(arr[i].ToString("x2"));
            return sBuilder.ToString();
        }

        /// <summary>
        /// SHA256����
        /// </summary>
        /// <param name="str">����</param>
        /// <returns>����</returns>
        public static string SHA256(this string str) {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result);  //���س���Ϊ44�ֽڵ��ַ���
        }
        /// <summary>
        /// SHA1����
        /// </summary>
        /// <param name="str">����</param>
        /// <returns>����</returns>
        public static string SHA1(this string str) {
            byte[] SHA1Data = Encoding.UTF8.GetBytes(str);
            SHA1Managed Sha1 = new SHA1Managed();
            byte[] Result = Sha1.ComputeHash(SHA1Data);
            return Convert.ToBase64String(Result);  //���س���Ϊ44�ֽڵ��ַ���
        }
        /// <summary>
        /// SHA512����
        /// </summary>
        /// <param name="str">����</param>
        /// <returns>����</returns>
        public static string SHA512(this string str) {
            byte[] SHA512Data = Encoding.UTF8.GetBytes(str);
            SHA512Managed Sha512 = new SHA512Managed();
            byte[] Result = Sha512.ComputeHash(SHA512Data);
            return Convert.ToBase64String(Result);  //���س���Ϊ44�ֽڵ��ַ���
        }

        /// <summary>
        /// SHA1Hash����
        /// </summary>
        /// <param name="stringToHash">����</param>
        /// <returns>����</returns>
        public static string SHA1Hash(this string stringToHash) {
            if (string.IsNullOrEmpty(stringToHash)) throw new ArgumentException("An empty string value cannot be hashed.");
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(stringToHash);
            Byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(data);
            return Convert.ToBase64String(hash);
        }
#if !NET20
        /// <summary>
        /// SHA256Hash����
        /// </summary>
        /// <param name="stringToHash">����</param>
        /// <returns>����</returns>
        public static string SHA256Hash(this string stringToHash) {
            if (string.IsNullOrEmpty(stringToHash)) throw new ArgumentException("An empty string value cannot be hashed.");
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(stringToHash);
            Byte[] hash = new SHA256CryptoServiceProvider().ComputeHash(data);
            return Convert.ToBase64String(hash);
        }
        /// <summary>
        /// SHA512Hash����
        /// </summary>
        /// <param name="stringToHash">����</param>
        /// <returns>����</returns>
        public static string SHA512Hash(this string stringToHash) {
            if (string.IsNullOrEmpty(stringToHash)) throw new ArgumentException("An empty string value cannot be hashed.");
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(stringToHash);
            Byte[] hash = new SHA512CryptoServiceProvider().ComputeHash(data);
            return Convert.ToBase64String(hash);
        }
#endif

        /// <summary>
        /// Rijndael����
        /// </summary>
        /// <param name="s">����</param>
        /// <param name="key">KEY</param>
        /// <param name="mode">CipherMode</param>
        /// <param name="iv">iv</param>
        /// <param name="padding">PaddingMode</param>
        /// <returns>����</returns>
        public static string RijndaelEncrypt(this string s, string key, string iv = "", CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7) {
            if (s.IsNullEmpty()) return string.Empty;
            SymmetryCryptor sc = new SymmetryCryptor(iv);
            sc.SymmetricAlgorithmType = SymmetricAlgorithmType.Rijndael;
            sc.Initialize(mode, padding);
            return sc.EncryptString(s, key);
        }
        /// <summary>
        /// Rijndael����
        /// </summary>
        /// <param name="s">����</param>
        /// <param name="key">KEY</param>
        /// <param name="mode">CipherMode</param>
        /// <param name="iv">iv</param>
        /// <param name="padding">PaddingMode</param>
        /// <returns>����</returns>
        public static string RijndaelDecrypt(this string s, string key, string iv = "", CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7) {
            if (s.IsNullEmpty()) return string.Empty;
            SymmetryCryptor sc = new SymmetryCryptor(iv);
            sc.SymmetricAlgorithmType = SymmetricAlgorithmType.Rijndael;
            sc.Initialize(mode, padding);
            return sc.DecryptString(s, key);
        }

        /// <summary>
        /// RC2����
        /// </summary>
        /// <param name="s">����</param>
        /// <param name="key">KEY</param>
        /// <param name="mode">CipherMode</param>
        /// <param name="iv">iv</param>
        /// <param name="padding">PaddingMode</param>
        /// <returns>����</returns>
        public static string RC2Encrypt(this string s, string key, string iv = "", CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7) {
            if (s.IsNullEmpty()) return string.Empty;
            SymmetryCryptor sc = new SymmetryCryptor(iv);
            sc.SymmetricAlgorithmType = SymmetricAlgorithmType.RC2;
            sc.Initialize(mode, padding);
            return sc.EncryptString(s, key);
        }
        /// <summary>
        /// RC2����
        /// </summary>
        /// <param name="s">����</param>
        /// <param name="key">KEY</param>
        /// <param name="mode">CipherMode</param>
        /// <param name="iv">iv</param>
        /// <param name="padding">PaddingMode</param>
        /// <returns>����</returns>
        public static string RC2Decrypt(this string s, string key, string iv = "", CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7) {
            if (s.IsNullEmpty()) return string.Empty;
            SymmetryCryptor sc = new SymmetryCryptor(iv);
            sc.SymmetricAlgorithmType = SymmetricAlgorithmType.RC2;
            sc.Initialize(mode, padding);
            return sc.DecryptString(s, key);
        }

        /// <summary>
        /// TripleDES����
        /// </summary>
        /// <param name="s">����</param>
        /// <param name="key">KEY</param>
        /// <param name="mode">CipherMode</param>
        /// <param name="iv">iv</param>
        /// <param name="padding">PaddingMode</param>
        /// <returns>����</returns>
        public static string TripleDESEncrypt(this string s, string key, string iv = "", CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7) {
            if (s.IsNullEmpty()) return string.Empty;
            SymmetryCryptor sc = new SymmetryCryptor(iv);
            sc.SymmetricAlgorithmType = SymmetricAlgorithmType.TripleDES;
            sc.Initialize(mode, padding);
            return sc.EncryptString(s, key);
        }
        /// <summary>
        /// TripleDES����
        /// </summary>
        /// <param name="s">����</param>
        /// <param name="key">KEY</param>
        /// <param name="mode">CipherMode</param>
        /// <param name="iv">iv</param>
        /// <param name="padding">PaddingMode</param>
        /// <returns>����</returns>
        public static string TripleDESDecrypt(this string s, string key, string iv = "", CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7) {
            if (s.IsNullEmpty()) return string.Empty;
            SymmetryCryptor sc = new SymmetryCryptor(iv);
            sc.SymmetricAlgorithmType = SymmetricAlgorithmType.TripleDES;
            sc.Initialize(mode, padding);
            return sc.DecryptString(s, key);
        }

        /// <summary>
        /// VernamEncrypt
        /// </summary>
        /// <param name="Input">����</param>
        /// <param name="Key">KEY</param>
        /// <returns>����</returns>
        public static string VernamEncrypt(this string Input, string Key) {
            return vernamProcess(Input, Key);
        }
        /// <summary>
        /// VernamDecrypt
        /// </summary>
        /// <param name="Input">����</param>
        /// <param name="Key">KEY</param>
        /// <returns>����</returns>
        public static string VernamDecrypt(this string Input, string Key) {
            return vernamProcess(Input, Key);
        }
        private static string vernamProcess(string Input, string Key) {
            if (string.IsNullOrEmpty(Input)) throw new ArgumentNullException("Input");
            if (string.IsNullOrEmpty(Key)) throw new ArgumentNullException("Key");
            if (Input.Length != Key.Length) throw new ArgumentException("Key is not the same length as the input string");

            ASCIIEncoding Encoding = new ASCIIEncoding();
            byte[] InputArray = Encoding.GetBytes(Input);
            byte[] KeyArray = Encoding.GetBytes(Key);
            byte[] OutputArray = new byte[InputArray.Length];
            for (int x = 0; x < InputArray.Length; ++x) OutputArray[x] = (byte)(InputArray[x] ^ Key[x]);
            return Encoding.GetString(OutputArray);
        }

        /// <summary>
        /// CaesarDecrypt
        /// </summary>
        /// <param name="Input">����</param>
        /// <param name="Key">Key</param>
        /// <returns>����</returns>
        public static string CaesarEncrypt(this string Input, string Key) {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Key)) throw new ArgumentNullException("The input/key string can not be empty.");
            return caesarProcess(Input, Key);
        }
        /// <summary>
        /// CaesarDecrypt
        /// </summary>
        /// <param name="Input">����</param>
        /// <param name="Key">Key</param>
        /// <returns>����</returns>
        public static string CaesarDecrypt(this string Input, string Key) {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Key)) throw new ArgumentNullException("The input/key string can not be empty.");
            return caesarProcess(Input, Key);
        }
        private static string caesarProcess(string Input, string Key) {
            ASCIIEncoding Encoding = new ASCIIEncoding();
            byte[] InputArray = Encoding.GetBytes(Input);
            byte[] KeyArray = Encoding.GetBytes(Key);
            byte[] OutputArray = new byte[InputArray.Length];
            int Position = 0;
            for (int x = 0; x < InputArray.Length; ++x) {
                OutputArray[x] = (byte)(InputArray[x] ^ Key[Position]);
                ++Position;
                if (Position >= Key.Length) Position = 0;
            }
            return Encoding.GetString(OutputArray);
        }
    }
}
