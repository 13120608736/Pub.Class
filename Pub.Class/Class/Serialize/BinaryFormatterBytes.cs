//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2011 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
#endif

namespace Pub.Class {
    /// <summary>
    /// binary���л��ͷ����л�
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// <code>
    /// <example>
    /// User u1 = new User() { UserID = 1000, Name = "�ܻ���" };
    /// var serialize = new BinarySerializeString();
    /// string s = serialize.Serialize(u1);
    /// serialize.Deserialize&lt;User>(s);
    /// </example>
    /// </code>
    /// </summary>
    public class BinaryFormatterBytes : ISerializeBytes {
        public void RegisterTypes(params Type[] types) { }
        /// <summary>
        /// ���г�16�����ַ���
        /// </summary>
        /// <param name="o">����</param>
        /// <returns>16�����ַ���</returns>
        public byte[] Serialize<T>(T o) {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream()) {
                formatter.Serialize(ms, o);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// 16�����ַ��������л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">16�����ַ���</param>
        /// <returns>����</returns>
        public T Deserialize<T>(byte[] data) {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data)) return (T)formatter.Deserialize(ms);
        }
        /// <summary>
        /// ���г�16�����ַ����ļ�
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="fileName">�ļ���</param>
        public void SerializeFile<T>(T o, string fileName) {
            FileDirectory.FileDelete(fileName);
            FileDirectory.FileWrite(fileName, Serialize(o).ToUTF8());
        }
        /// <summary>
        /// 16�����ַ����ļ������л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="fileName">�ļ���</param>
        /// <returns>����</returns>
        public T DeserializeFile<T>(string fileName) {
            byte[] data = FileDirectory.FileReadAll(fileName, Encoding.UTF8).FromBase64();
            return Deserialize<T>(data);
        }
        /// <summary>
        /// ���г�16�����ַ�����DES����
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="key">����KEY</param>
        /// <returns>16�����ַ�������</returns>
        public byte[] SerializeEncode<T>(T o, string key = "") {
            return key.IsNullEmpty() ? Serialize(o) : Serialize(o).ToUTF8().DESEncode(key).FromBase64();
        }
        /// <summary>
        /// DES���ܺ����гɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">16�����ַ�������</param>
        /// <param name="key">����KEY</param>
        /// <returns>����</returns>
        public T DecodeDeserialize<T>(byte[] data, string key = "") {
            return key.IsNullEmpty() ? Deserialize<T>(data) : Deserialize<T>(data.ToUTF8().DESDecode(key).FromBase64());
        }
    }
}
