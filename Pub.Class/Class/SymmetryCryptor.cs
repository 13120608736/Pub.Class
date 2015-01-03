using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;

namespace Pub.Class {
    /// <summary>
    /// �ԳƼ��ܽӿ�
    /// 
    /// �޸ļ�¼
    ///     2010.01.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface ISymmetryCryptor {
        /// <summary>
        /// �ԳƼ��ܱ���
        /// </summary>
        Encoding Encoding { get; set; }
        /// <summary>
        /// SymmetricAlgorithmType ���õļ����㷨���͡�
        /// �����DES���ܣ���Ҫ��64λ�ܳס�
        /// �����Rijndael���ܣ���֧�� 128��192 �� 256 λ����Կ���ȡ�
        /// �����RC2���ܣ���֧�ֵ���Կ����Ϊ�� 40 λ�� 128 λ���� 8 λ������
        /// �����TripleDES���ܣ���֧�ִ� 128 λ�� 192 λ���� 64 λ����������Կ���ȡ�
        /// </summary>
        SymmetricAlgorithmType SymmetricAlgorithmType { get; set; }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="mode">CipherMode</param>
        /// <param name="padding">PaddingMode</param>
        void Initialize(CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7);
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        byte[] EncryptStream(byte[] source, string key);
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="toEncrpyLen"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        byte[] EncryptStream(byte[] source, int offset, int toEncrpyLen, string key);
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="bytesEncoded"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        byte[] DecryptStream(byte[] bytesEncoded, string key);
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="bytesEncoded"></param>
        /// <param name="offset"></param>
        /// <param name="toDecrpyLen"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        byte[] DecryptStream(byte[] bytesEncoded, int offset, int toDecrpyLen, string key);
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string EncryptString(string source, string key);
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="strEncoded"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string DecryptString(string strEncoded, string key);
    }
    /// <summary>
    /// ֧��DES, RC2, Rijndael, TripleDES
    /// </summary>
    public enum SymmetricAlgorithmType {
        /// <summary>
        /// DES
        /// </summary>
        DES,
        /// <summary>
        /// RC2
        /// </summary>
        RC2,
        /// <summary>
        /// Rijndael
        /// </summary>
        Rijndael,
        /// <summary>
        /// TripleDES
        /// </summary>
        TripleDES
    }
    /// <summary>
    /// �ԳƼ����㷨
    /// </summary>
    public class SymmetryCryptor : ISymmetryCryptor {
        private SymmetricAlgorithm symmetricAlgorithm;
        private string strInitialVector = "SymmetryCryptor";
        //private byte[] initialVector = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF}; 
        private byte[] initialVector;
        /// <summary>
        /// ������
        /// </summary>
        public SymmetryCryptor() {
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="iv">iv</param>
        public SymmetryCryptor(string iv) {
            if (!iv.IsNullEmpty()) strInitialVector = iv;
        }
        #region Encoding
        private Encoding encoding = Encoding.Default;
        /// <summary>
        /// ����
        /// </summary>
        public Encoding Encoding {
            get { return encoding; }
            set { encoding = value; }
        }
        #endregion

        #region SymmetricAlgorithmType
        private SymmetricAlgorithmType symmetricAlgorithmType = SymmetricAlgorithmType.DES;
        /// <summary>
        /// SymmetricAlgorithmType
        /// </summary>
        public SymmetricAlgorithmType SymmetricAlgorithmType {
            get { return symmetricAlgorithmType; }
            set { symmetricAlgorithmType = value; }
        }
        #endregion

        #region Initialize
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="mode">CipherMode</param>
        /// <param name="padding">PaddingMode</param>
        public void Initialize(CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7) {
            this.initialVector = this.GetExactBytes(strInitialVector, 16);
            switch (this.symmetricAlgorithmType) {
                case SymmetricAlgorithmType.DES: {
                    this.symmetricAlgorithm = new DESCryptoServiceProvider();
                    break;
                }
                case SymmetricAlgorithmType.Rijndael: {
                    this.symmetricAlgorithm = new RijndaelManaged();
                    break;
                }
                case SymmetricAlgorithmType.RC2: {
                    this.symmetricAlgorithm = new RC2CryptoServiceProvider();
                    break;
                }
                case SymmetricAlgorithmType.TripleDES: {
                    this.symmetricAlgorithm = new TripleDESCryptoServiceProvider();
                    break;
                }
                default: {
                    this.symmetricAlgorithm = new DESCryptoServiceProvider();
                    break;
                }
            }

            this.symmetricAlgorithm.Mode = mode;
            this.symmetricAlgorithm.Padding = padding;
        }
        #endregion

        #region EncryptStream
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] EncryptStream(byte[] source, string key) {
            if (source.IsNull()) {
                return null;
            }

            byte[] bytesKey = this.encoding.GetBytes(key);

            MemoryStream memStream = new MemoryStream();
            CryptoStream crytoStream = new CryptoStream(memStream, this.symmetricAlgorithm.CreateEncryptor(bytesKey, this.initialVector), CryptoStreamMode.Write);

            try {
                crytoStream.Write(source, 0, source.Length);//��ԭʼ�ַ������ܺ�д��memStream
                crytoStream.FlushFinalBlock();
                byte[] bytesEncoded = memStream.ToArray();

                return bytesEncoded;

            } finally {
                memStream.Close();
                crytoStream.Close();
            }
        }
        #endregion

        #region EncryptStream
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="toEncrpyLen"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] EncryptStream(byte[] source, int offset, int toEncrpyLen, string key) {
            if (toEncrpyLen == 0) {
                return source;
            }

            byte[] temp = this.GetPartOfStream(source, offset, toEncrpyLen);

            return this.EncryptStream(temp, key);
        }

        private byte[] GetPartOfStream(byte[] source, int offset, int length) {
            if ((source.IsNull()) || offset >= source.Length) {
                return null;
            }

            if (length + offset > source.Length) {
                length = source.Length - offset;
            }

            byte[] temp = new byte[length];
            for (int i = 0; i < length; i++) {
                temp[i] = source[offset + i];
            }

            return temp;
        }

        #endregion

        #region DecryptStream
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="bytesEncoded"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] DecryptStream(byte[] bytesEncoded, string key) {
            if (bytesEncoded.IsNull()) {
                return null;
            }

            byte[] bytesKey = this.encoding.GetBytes(key);
            MemoryStream memStream = new MemoryStream(bytesEncoded);
            CryptoStream crytoStream = new CryptoStream(memStream, this.symmetricAlgorithm.CreateDecryptor(bytesKey, this.initialVector), CryptoStreamMode.Read);

            try {
                StreamReader streamReader = new StreamReader(crytoStream, this.encoding);
                string ss = streamReader.ReadToEnd();
                return this.encoding.GetBytes(ss);
            } finally {
                memStream.Close();
                crytoStream.Close();
            }
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="bytesEncoded"></param>
        /// <param name="offset"></param>
        /// <param name="toDecrpyLen"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] DecryptStream(byte[] bytesEncoded, int offset, int toDecrpyLen, string key) {
            byte[] temp = this.GetPartOfStream(bytesEncoded, offset, toDecrpyLen);
            return this.DecryptStream(temp, key);
        }
        #endregion

        #region EncryptString ,DecryptString
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string EncryptString(string source, string key) {
            byte[] bytes_source = this.encoding.GetBytes(source);
            byte[] bytesEncoded = this.EncryptStream(bytes_source, key);
            return Convert.ToBase64String(bytesEncoded);
        }
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="strEncoded"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string DecryptString(string strEncoded, string key) {
            byte[] bytesEncoded = Convert.FromBase64String(strEncoded); //����ʹ��this.encoding.GetBytes(str_encoded);
            byte[] bytesDecoded = this.DecryptStream(bytesEncoded, key);

            return this.encoding.GetString(bytesDecoded, 0, bytesDecoded.Length);
        }
        #endregion

        #region private
        private byte[] GetExactBytes(string source, int length) {
            byte[] result = new byte[length];
            byte[] buff = this.encoding.GetBytes(source);


            int buff_len = buff.Length;

            if (buff_len >= length) {
                for (int i = 0; i < length; i++) {
                    result[i] = buff[i];
                }
            } else {
                for (int i = 0; i < length; i++) {
                    result[i] = buff[i % buff_len];
                }
            }

            return result;
        }

        #endregion
    }

}
