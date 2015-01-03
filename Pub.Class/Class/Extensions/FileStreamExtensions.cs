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
using System.Runtime.Serialization.Formatters.Binary;

namespace Pub.Class {
    /// <summary>
    /// FileStream��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class FileStreamExtensions {
        /// <summary>
        /// FileStream to �ֽ�
        /// </summary>
        /// <param name="stream">FileStream</param>
        /// <returns>�ֽ�</returns>
        public static byte[] ToBytes(this FileStream stream) {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, System.Convert.ToInt32(stream.Length));
            stream.Close();
            return bytes;
        }
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="file">FileStream</param>
        /// <param name="path">·��</param>
        /// <returns>·��</returns>
        public static string Save(this FileStream file, string path) { return file.Save(path, false); }
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="file">FileStream</param>
        /// <param name="path">·��</param>
        /// <param name="overwrite">�Ƿ���д</param>
        /// <returns>·��</returns>
        public static string Save(this FileStream file, string path, bool overwrite) {
            int count = 1;
            string folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            int fileSize = Convert.ToInt32(file.Length);
            string fileName = Path.GetFileName(file.Name);
            Byte[] bytes = new Byte[fileSize];
            file.Read(bytes, 0, fileSize);
            string root = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path);

            while (!overwrite && File.Exists(path)) path = root + "[" + count++.ToString() + "]" + Path.GetExtension(path);

            File.WriteAllBytes(path, bytes);
            return Path.GetFileName(path);
        }
        /// <summary>
        /// ���ļ�����
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="encoding">����</param>
        /// <returns>���ļ�����</returns>
        public static string ReadAll(this Stream stream, Encoding encoding) {
            string value; stream.Seek(0, SeekOrigin.Begin);
            using (StreamReader sr = new StreamReader(stream, encoding)) value = sr.ReadToEnd();
            return value;
        }
        /// <summary>
        /// ���ļ�����
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>�ļ�����</returns>
        public static string ReadAll(this Stream stream) {
            return stream.ReadAll(Encoding.UTF8);
        }
        /// <summary>
        /// IsUTF8
        /// </summary>
        /// <param name="sbInputStream">FileStream</param>
        /// <returns>IsUTF8</returns>
        public static bool IsUTF8(this FileStream sbInputStream) {
            int i;
            byte cOctets;  // octets to go in this UTF-8 encoded character 
            byte chr;
            bool bAllAscii = true;
            long iLen = sbInputStream.Length;
            cOctets = 0;
            for (i = 0; i < iLen; i++) {
                chr = (byte)sbInputStream.ReadByte();
                if ((chr & 0x80) != 0) bAllAscii = false;
                if (cOctets == 0) {
                    if (chr >= 0x80) {
                        do {
                            chr <<= 1;
                            cOctets++;
                        } while ((chr & 0x80) != 0);
                        cOctets--;
                        if (cOctets == 0) return false;
                    }
                } else {
                    if ((chr & 0xC0) != 0x80) return false;
                    cOctets--;
                }
            }
            if (cOctets > 0) return false;
            if (bAllAscii) return false;
            return true;
        }
        /// <summary>
        /// ת�ֽ�
        /// </summary>
        /// <param name="target">Stream</param>
        /// <returns>�ֽ�</returns>
        public static byte[] ToBytes(this Stream target) {
            byte[] content;

            if (target.IsNull()) content = new byte[0];
            else if (target is MemoryStream) content = ((MemoryStream)target).ToArray();
            else {
                content = new byte[target.Length];
                target.Position = 0;
                target.Read(content, 0, (int)target.Length);
            }
            return content;
        }
        /// <summary>
        /// ȡ�ļ�����
        /// </summary>
        /// <param name="stream">FileStream</param>
        /// <returns>�ļ�����</returns>
        public static Encoding GetEncoding(this FileStream stream) { return GetEncoding(stream, Encoding.Default); }
        /// <summary>
        /// ȡ�ļ�����
        /// </summary>
        /// <param name="stream">FileStream</param>
        /// <param name="defaultEncoding">Ĭ�ϱ���</param>
        /// <returns>�ļ�����</returns>
        public static Encoding GetEncoding(this FileStream stream, Encoding defaultEncoding) {
            Encoding targetEncoding = defaultEncoding;

            if (stream.IsNotNull() && stream.Length >= 2) {
                //�����ļ�����ǰ4���ֽ�
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;

                //���浱ǰSeekλ��
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());

                if (stream.Length >= 3) byte3 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 4) byte4 = Convert.ToByte(stream.ReadByte());

                if (byte1 == 0xFE && byte2 == 0xFF) targetEncoding = Encoding.BigEndianUnicode;//UnicodeBe
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF) targetEncoding = Encoding.Unicode;//Unicode
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF) targetEncoding = Encoding.UTF8;//UTF8
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }
        /// <summary>
        /// �����л��ɶ���
        /// </summary>
        /// <param name="memStream">MemoryStream</param>
        /// <returns>�����л��ɶ���</returns>
        public static T FromMemoryStream<T>(this MemoryStream memStream) {
            memStream.Position = 0;
            BinaryFormatter deserializer = new BinaryFormatter();
            object newobj = deserializer.Deserialize(memStream);
            memStream.Close();
            return (T)newobj;
        }
        /// <summary>
        /// �ļ�MD5
        /// </summary>
        /// <param name="strm">Stream</param>
        /// <returns>MD5</returns>
        public static string MD5(this Stream strm) {
            if (strm == Stream.Null || strm.Length == 0) return null;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding enc = new UTF8Encoding();
            byte[] hash = md5.ComputeHash(strm);
            StringBuilder buff = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) buff.Append(String.Format("{0:x2}", hash[i]));
            return buff.ToString();
        }
        /// <summary>
        /// Hash
        /// </summary>
        /// <param name="strm">Stream</param>
        /// <returns>Hashֵ</returns>
        public static string Hash(this Stream strm) { return strm.MD5(); }
    }
}
