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
using System.Globalization;
using System.Collections;
using System.IO;
using System.Drawing;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Threading;

namespace Pub.Class {
    /// <summary>
    /// ������չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.27 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class ArrayExtensions {
        /// <summary>
        /// Array�����ǿ� null or length=0
        /// </summary>
        /// <param name="array">Array����</param>
        /// <returns>true/false</returns>
        public static bool IsNullEmpty(this Array array) { return array.IsNull() || array.Length == 0; }
        /// <summary>
        /// ArrayList�����ǿ� null or length=0
        /// </summary>
        /// <param name="list">ArrayList����</param>
        /// <returns>true/false</returns>
        public static bool IsNullEmpty(this ArrayList list) { return (list.IsNull()) || (list.Count == 0); }
        /// <summary>
        /// ȡ�ַ����������е�λ��
        /// </summary>
        /// <example>���ִ�Сд��
        /// <code>
        /// Msg.WriteEnd("1,2,T,4".Split(',').GetInArrayID("T", false));
        /// </code>
        /// </example>
        /// <param name="stringArray">����</param>
        /// <param name="strSearch">�ַ���</param>
        /// <param name="caseInsensetive">�Ƿ�ע���Сд</param>
        /// <returns>λ��</returns>
        public static int GetInArrayID(this string[] stringArray, string strSearch, bool caseInsensetive) {
            for (int i = 0; i < stringArray.Length; i++) {
                if (caseInsensetive) {
                    if (strSearch.ToLower() == stringArray[i].ToLower()) return i;
                } else {
                    if (strSearch == stringArray[i]) return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// ȡ�ַ����������е�λ�� ���ں���Сд
        /// </summary>
        /// <example>
        /// <code>
        /// Msg.WriteEnd("1,2,3,4".Split(',').GetInArrayID("2"));
        /// </code>
        /// </example>
        /// <param name="stringArray">����</param>
        /// <param name="strSearch">�ַ���</param>
        /// <returns>λ��</returns>
        public static int GetInArrayID(this string[] stringArray, string strSearch) {
            return GetInArrayID(stringArray, strSearch, true);
        }
        /// <summary>
        /// ����ȥ���ظ�
        /// </summary>
        /// <example>
        /// <code>
        /// Msg.WriteEnd("1,2,T,2".Split(',').RemoveDuplicates&lt;string>().Length);
        /// </code>
        /// </example>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="array">����</param>
        /// <returns>ȥ���ظ�������</returns>
        public static T[] RemoveDuplicates<T>(this T[] array) {
            ArrayList al = new ArrayList();
            for (int i = 0; i < array.Length; i++) {
                if (!al.Contains(array[i])) al.Add(array[i]);
            }
            return (T[])al.ToArray(typeof(T));
        }
        /// <summary>
        /// ֻȡ��������
        /// </summary>
        /// <example>
        /// <code>
        /// Msg.WriteEnd("1,2,T,2".Split(',').Slice&lt;string>(2,3).Length);
        /// </code>
        /// </example>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="array">����</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="end">����λ��</param>
        /// <returns>����</returns>
        public static T[] Slice<T>(this T[] array, int start, int end) {
            if (start >= array.Length) { start = 0; end = 0; }
            if (end < 0) end = array.Length - start - end;
            if (end <= start) end = start;
            if (end >= array.Length) end = array.Length - 1;
            int len = end - start + 1;
            T[] res = new T[len];
            for (int i = 0; i < len; i++) res[i] = array[i + start];
            return res;
        }
        /// <summary>
        /// ����ϲ����ַ���
        /// </summary>
        /// <example>
        /// <code>
        /// string str = "1,2,3,4".Split(',').Join&lt;string>("|");
        /// </code>
        /// </example>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="array">����</param>
        /// <param name="splitStr">�ָ���</param>
        /// <returns>�ϲ����ַ���</returns>
        public static string Join<T>(this T[] array, string splitStr) {
            StringBuilder sb = new StringBuilder();
            foreach (T info in array) {
                sb.AppendFormat("{0}{1}", info.ToString(), splitStr);
            }
            return sb.ToString().Left(sb.Length - splitStr.Length);
        }
        /// <summary>
        /// ȡHTML���ݱ��� ���� utf8 gb_2312 utf_8 zh_cn ��ǰ��λ�ֽ���
        /// </summary>
        /// <param name="bytes">�ֽ���</param>
        /// <param name="defaultEncoding">Ĭ�ϱ���</param>
        /// <returns>�ֽ���ʹ�õı���</returns>
        public static Encoding GetHtmlEncoding(this byte[] bytes, Encoding defaultEncoding) {
            Encoding encoding = Encoding.ASCII;
            string content = encoding.GetString(bytes);

            encoding = content.GetHtmlEncoding(null);
            if (encoding.IsNull()) {
                if (bytes.Length > 3) {
                    if (bytes[0] == 255 && bytes[1] == 254) {
                        encoding = Encoding.Unicode;
                    } else if (bytes[0] == 239 && bytes[1] == 187 && bytes[2] == 191) {
                        encoding = Encoding.UTF8;
                    } else {
                        encoding = defaultEncoding;
                    }
                } else {
                    encoding = defaultEncoding;
                }
            }
            return encoding;
        }
        ///// <summary>
        ///// һά��������Ԫ��ִ�ж���
        ///// </summary>
        ///// <example>
        ///// <code>
        ///// Action&lt;string, int> MsgW3 = (s, i) => { Msg.Write(s + i); Msg.Write("&lt;br />"); };
        ///// "test1,test2,test3".Split(',').Action&lt;string>(MsgW3);
        ///// </code>
        ///// </example>
        ///// <typeparam name="T">��������</typeparam>
        ///// <param name="inArray">����</param>
        ///// <param name="inAction">����</param>
        //public static void Action<T>(this T[] inArray, Action<T, Int32> inAction) {
        //    for (int i0 = 0; i0 < inArray.GetLength(0); i0++) {
        //        inAction(inArray[i0], i0);
        //    }
        //}
        ///// <summary>
        ///// ��ά��������Ԫ��ִ�ж���
        ///// </summary>
        ///// <example>
        ///// <code>
        ///// int[,] arr = new int[2, 2] { { 11, 12 }, { 21, 22 } };
        ///// arr.Action&lt;int>((t, i, j)=>{ Msg.Write(i + "|" + j + "|" + t + "&lt;br />"); });
        ///// </code>
        ///// </example>
        ///// <typeparam name="T">��������</typeparam>
        ///// <param name="inArray">����</param>
        ///// <param name="inAction">����</param>
        //public static void Action<T>(this T[,] inArray, Action<T, Int32, Int32> inAction) {
        //    for (int i0 = 0; i0 < inArray.GetLength(0); i0++) {
        //        for (int i1 = 0; i1 < inArray.GetLength(1); i1++) inAction(inArray[i0, i1], i0, i1);
        //    }
        //}
        ///// <summary>
        ///// ��ά��������Ԫ��ִ�ж���
        ///// </summary>
        ///// <typeparam name="T">��������</typeparam>
        ///// <param name="inArray">����</param>
        ///// <param name="inAction">����</param>
        //public static void Action<T>(this T[, ,] inArray, Action<T, Int32, Int32, Int32> inAction) {
        //    for (int i0 = 0; i0 < inArray.GetLength(0); i0++) {
        //        for (int i1 = 0; i1 < inArray.GetLength(1); i1++) {
        //            for (int i2 = 0; i2 < inArray.GetLength(2); i2++) inAction(inArray[i0, i1, i2], i0, i1, i2);
        //        }
        //    }
        //}
        ///// <summary>
        ///// ��ά��������Ԫ��ִ�ж���
        ///// </summary>
        ///// <typeparam name="T">��������</typeparam>
        ///// <param name="inArray">����</param>
        ///// <param name="inDimension">ά��</param>
        ///// <param name="inIndex"></param>
        ///// <param name="inAction">����</param>
        //public static void Action<T>(this T[,] inArray, Int32 inDimension, Int32 inIndex, Action<T, Int32> inAction) {
        //    if (inDimension == 0) {
        //        for (int i = 0; i < inArray.GetLength(1); i++) inAction(inArray[inIndex, i], i);
        //    } else if (inDimension == 1) {
        //        for (int i = 0; i < inArray.GetLength(0); i++) inAction(inArray[i, inIndex], i);
        //    } else {
        //        throw new ArgumentException("inDimension must be zero or one");
        //    }
        //}
        ///// <summary>
        ///// ��ά��������Ԫ��ִ�ж���
        ///// </summary>
        ///// <typeparam name="T">��������</typeparam>
        ///// <param name="inArray">����</param>
        ///// <param name="inDimension">ά��</param>
        ///// <param name="inIndex"></param>
        ///// <param name="inAction">����</param>
        //public static void Action<T>(this T[, ,] inArray, Int32 inDimension, Int32 inIndex, Action<T, Int32, Int32> inAction) {
        //    if (inDimension == 0) {
        //        for (int i0 = 0; i0 < inArray.GetLength(1); i0++) {
        //            for (int i1 = 0; i1 < inArray.GetLength(2); i1++) inAction(inArray[inIndex, i0, i1], i0, i1);
        //        }
        //    } else if (inDimension == 1) {
        //        for (int i0 = 0; i0 < inArray.GetLength(0); i0++) {
        //            for (int i1 = 0; i1 < inArray.GetLength(2); i1++) inAction(inArray[i0, inIndex, i1], i0, i1);
        //        }
        //    } else if (inDimension == 2) {
        //        for (int i0 = 0; i0 < inArray.GetLength(0); i0++) {
        //            for (int i1 = 0; i1 < inArray.GetLength(1); i1++) inAction(inArray[i0, i1, inIndex], i0, i1);
        //        }
        //    } else {
        //        throw new ArgumentException("inDimension must be zero or one or two");
        //    }
        //}
#if !MONO40
        /// <summary>
        /// �ֽ�תͼƬ
        /// </summary>
        /// <param name="bytes">ͼƬ�ֽ�</param>
        /// <returns>Image</returns>
        public static Image ToImage(this byte[] bytes) {
            if (bytes.IsNotNull()) {
                MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
                ms.Write(bytes, 0, bytes.Length);
                return Image.FromStream(ms, true);
            }

            return null;
        }
#endif
        /// <summary>
        /// �ֽ�д���ļ�
        /// </summary>
        /// <param name="bytes">�ֽ�</param>
        /// <param name="fileName">������</param>
        /// <param name="fileMode">FileMode</param>
        /// <returns>true/false</returns>
        public static bool ToFile(this byte[] bytes, string fileName, FileMode fileMode = FileMode.CreateNew) {
            return FileDirectory.FileWrite(fileName, bytes, fileMode);
        }
        /// <summary>
        /// �ֽڷ����л��ɶ���
        /// </summary>
        /// <typeparam name="T">����/����</typeparam>
        /// <param name="data">�ֽ�</param>
        /// <returns>����</returns>
        public static T FromBytes<T>(this byte[] data, bool compress = false) {
            return SerializeBytes.FromBytes<T>(compress ? data.DeflateDecompress() : data);
            //var formatter = new BinaryFormatter();
            //using (MemoryStream ms = new MemoryStream(data)) return (T)formatter.Deserialize(ms);
        }
        /// <summary>
        /// �ַ�����������
        /// </summary>
        /// <example>
        /// <code>
        /// Msg.WriteEnd("1,2,T,4".Split(',').IsInArray("T", false));
        /// </code>
        /// </example>
        /// <param name="stringArray">�ַ�������</param>
        /// <param name="strSearch">�ַ���</param>
        /// <param name="caseInsensetive">���ִ�Сд</param>
        /// <returns>true/false</returns>
        public static bool IsInArray(this string[] stringArray, string strSearch, bool caseInsensetive) {
            return stringArray.GetInArrayID(strSearch, caseInsensetive) >= 0;
        }
        /// <summary>
        /// �ַ����������� �����ִ�Сд
        /// </summary>
        /// <example>
        /// <code>
        /// Msg.WriteEnd("1,2,T,4".Split(',').IsInArray("t"));
        /// </code>
        /// </example>
        /// <param name="stringarray">�ַ�������</param>
        /// <param name="str">�ַ���</param>
        /// <returns>true/false</returns>
        public static bool IsInArray(this string[] stringarray, string str) {
            return stringarray.IsInArray(str, false);
        }
        /// <summary>
        /// �ֽ�MD5
        /// </summary>
        /// <param name="bts">�ֽ�</param>
        /// <returns>MD5�ַ���</returns>
        public static string MD5(this byte[] bts) {
            if (bts.IsNull() || bts.Length == 0) return null;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding enc = new UTF8Encoding();
            byte[] hash = md5.ComputeHash(bts);
            StringBuilder buff = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) buff.Append(String.Format("{0:x2}", hash[i]));
            return buff.ToString();
        }
        /// <summary>
        /// UTF8�ֽ�ת�ַ���
        /// </summary>
        /// <param name="characters">�ֽ�</param>
        /// <returns>�ַ���</returns>
        public static String ToUTF8(this byte[] characters) {
            return Encoding.UTF8.GetString(characters);
        }
        public static String ToGB2312(this byte[] characters) {
            return Encoding.GetEncoding("gb2312").GetString(characters);
        }
#if !MONO40
        /// <summary>
        /// FindMimeFromData urlmon.dll API
        /// </summary>
        /// <param name="pBC"></param>
        /// <param name="pwzUrl"></param>
        /// <param name="pBuffer"></param>
        /// <param name="cbSize"></param>
        /// <param name="pwzMimeProposed"></param>
        /// <param name="dwMimeFlags"></param>
        /// <param name="ppwzMimeOut"></param>
        /// <param name="dwReserved"></param>
        /// <returns></returns>
        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        private static extern int FindMimeFromData(IntPtr pBC, [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)] byte[] pBuffer, int cbSize, [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed, int dwMimeFlags, out IntPtr ppwzMimeOut, int dwReserved);
        /// <summary>
        /// ȡ�����ļ���MIME
        /// </summary>
        /// <param name="data">�ֽ�</param>
        /// <returns>MIME��application/unknown</returns>
        public static string GetMimeType(this byte[] data) {
            if (data.Length == 0) return "application/unknown";

            IntPtr mimeOut = default(IntPtr);
            int result = FindMimeFromData(IntPtr.Zero, string.Empty, data, data.Length, null, 0, out mimeOut, 0);
            if (result != 0) throw Marshal.GetExceptionForHR(result);

            string mime = Marshal.PtrToStringUni(mimeOut);
            Marshal.FreeCoTaskMem(mimeOut);
            return mime;
        }
#endif
        /// <summary>
        /// GZipѹ��
        /// </summary>
        /// <param name="Bytes">�ֽ�</param>
        /// <returns>�ֽ�</returns>
        public static byte[] GZipCompress(this byte[] Bytes) {
            if (Bytes.IsNull()) throw new ArgumentNullException("Bytes");
            using (MemoryStream Stream = new MemoryStream()) {
                using (GZipStream ZipStream = new GZipStream(Stream, CompressionMode.Compress, true)) {
                    ZipStream.Write(Bytes, 0, Bytes.Length);
                    ZipStream.Close();
                    return Stream.ToArray();
                }
            }
        }
        /// <summary>
        /// GZip��ѹ
        /// </summary>
        /// <param name="Bytes">�ֽ�</param>
        /// <returns>�ֽ�</returns>
        public static byte[] GZipDecompress(this byte[] Bytes) {
            if (Bytes.IsNull()) throw new ArgumentNullException("Bytes");
            using (MemoryStream Stream = new MemoryStream()) {
                using (GZipStream ZipStream = new GZipStream(new MemoryStream(Bytes), CompressionMode.Decompress, true)) {
                    byte[] Buffer = new byte[4096];
                    while (true) {
                        int Size = ZipStream.Read(Buffer, 0, Buffer.Length);
                        if (Size > 0) Stream.Write(Buffer, 0, Size);
                        else break;
                    }
                    ZipStream.Close();
                    return Stream.ToArray();
                }
            }
        }
        /// <summary>
        /// Deflateѹ���ֽ�
        /// </summary>
        /// <param name="Bytes">�ֽ�</param>
        /// <returns>�ֽ�</returns>
        public static byte[] DeflateCompress(this byte[] Bytes) {
            if (Bytes.IsNull()) throw new ArgumentNullException("Bytes");
            using (MemoryStream Stream = new MemoryStream()) {
                using (DeflateStream ZipStream = new DeflateStream(Stream, CompressionMode.Compress, true)) {
                    ZipStream.Write(Bytes, 0, Bytes.Length);
                    ZipStream.Close();
                    return Stream.ToArray();
                }
            }
        }
        /// <summary>
        /// Deflate��ѹ�ֽ�
        /// </summary>
        /// <param name="Bytes">�ֽ�</param>
        /// <returns>�ֽ�</returns>
        public static byte[] DeflateDecompress(this byte[] Bytes) {
            if (Bytes.IsNull()) throw new ArgumentNullException("Bytes");
            using (MemoryStream Stream = new MemoryStream()) {
                using (DeflateStream ZipStream = new DeflateStream(new MemoryStream(Bytes), CompressionMode.Decompress, true)) {
                    byte[] Buffer = new byte[4096];
                    while (true) {
                        int Size = ZipStream.Read(Buffer, 0, Buffer.Length);
                        if (Size > 0) Stream.Write(Buffer, 0, Size);
                        else break;
                    }
                    ZipStream.Close();
                    return Stream.ToArray();
                }
            }
        }
        /// <summary>
        /// ѹ���������л��ɶ���
        /// </summary>
        /// <typeparam name="T">����/����</typeparam>
        /// <param name="compressedData">�ֽ�</param>
        /// <returns>����</returns>
        public static T FromDeflateDecompressBinary<T>(this byte[] compressedData) where T : class {
            return compressedData.DeflateDecompress().FromBytes<T>();
        }
        /// <summary>
        /// Unicode�����
        /// </summary>
        /// <param name="Input">�ֽ�</param>
        /// <returns>true/false</returns>
        public static bool IsUnicode(this byte[] Input) {
            if (Input.IsNull()) return true;
            UnicodeEncoding Encoding = new UnicodeEncoding();
            byte[] UniInput = Encoding.GetBytes(Encoding.GetString(Input));
            ASCIIEncoding Encoding2 = new ASCIIEncoding();
            byte[] ASCIIInput = Encoding2.GetBytes(Encoding2.GetString(Input));
            if (UniInput[0] == ASCIIInput[0]) return false;
            return true;
        }
        /// <summary>
        /// ��ȡ�ļ�����ʵ��׺����Ŀǰֻ֧��JPG, GIF, PNG, BMP����ͼƬ�ļ���
        /// </summary>
        /// <param name="fileData">�ļ��ֽ���</param>
        /// <returns>JPG, GIF, PNG or null</returns>
        public static string GetImageExtension(this byte[] fileData) {
            if (fileData.IsNull() || fileData.Length < 10) return null;

            if (fileData[0] == 'G' && fileData[1] == 'I' && fileData[2] == 'F') {
                return "GIF";
            } else if (fileData[1] == 'P' && fileData[2] == 'N' && fileData[3] == 'G') {
                return "PNG";
            } else if (fileData[6] == 'J' && fileData[7] == 'F' && fileData[8] == 'I' && fileData[9] == 'F') {
                return "JPG";
            } else if (fileData[0] == 'B' && fileData[1] == 'M') {
                return "BMP";
            } else {
                return string.Empty;
            }
        }
        /// <summary>
        /// �Ƿ�ͼƬ�ļ�
        /// </summary>
        /// <param name="fileData">ͼƬ�ֽ�</param>
        /// <returns></returns>
        public static bool IsImage(this byte[] fileData) {
            return fileData.GetImageExtension().IsInArray("GIF,PNG,JPG,BMP", ",");
        }
        /// <summary>
        /// ת16�����ַ���
        /// </summary>
        /// <param name="data">�ֽ�</param>
        /// <returns>16�����ַ���</returns>
        public static string ToHexString(this byte[] data) {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data) sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }
        /// <summary>
        /// base 64 string
        /// </summary>
        /// <param name="data">�ֽ���</param>
        /// <returns></returns>
        public static string ToBase64(this byte[] data) {
            return Convert.ToBase64String(data);
        }
        /// <summary>
        /// ����Encoding
        /// </summary>
        /// <param name="data">�ֽ���</param>
        /// <returns>Encoding</returns>
        public static Encoding GetEncoding(this byte[] data) {
            if (data[0] >= 0xEF) {
                if (data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
                    return System.Text.Encoding.UTF8;
                else if (data[0] == 0xFE && data[1] == 0xFF)
                    return System.Text.Encoding.BigEndianUnicode;
                else if (data[0] == 0xFF && data[1] == 0xFE)
                    return System.Text.Encoding.Unicode;
                else
                    return System.Text.Encoding.Default;
            } else return System.Text.Encoding.Default;
        }
        /// <summary>
        /// �ֽ�����ת�ṹ��
        /// </summary>
        /// <typeparam name="T">�ṹ��</typeparam>
        /// <param name="bytes">�ֽ�����</param>
        /// <returns></returns>
        public static T BytesToStruct<T>(this byte[] bytes) {
            Type type = typeof(T);
            int size = Marshal.SizeOf(type);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, buffer, size);
            object obj = Marshal.PtrToStructure(buffer, type);
            Marshal.FreeHGlobal(buffer);
            return (T)obj;
        }
        /// <summary>
        /// char[] ת string
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string CharsToString(this char[] chars) { return new string(chars).TrimEnd('\0'); }
        /// <summary>
        /// byte ת bool
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool ToBool(this byte b) { return b == 1; }
    }
}
