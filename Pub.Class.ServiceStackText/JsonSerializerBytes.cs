//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ServiceStack.Text;

namespace Pub.Class.ServiceStackText {
    /// <summary>
    /// ServiceStackText���л�
    /// 
    /// �޸ļ�¼
    ///     2013.02.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class JsonSerializerBytes : ISerializeBytes {
        public void RegisterTypes(params Type[] types) { }
        /// <summary>
        /// ���г�json
        /// </summary>
        /// <param name="o">����</param>
        /// <returns>json</returns>
        public byte[] Serialize<T>(T o) {
            using (var s = new MemoryStream()) {
                JsonSerializer.SerializeToStream<T>(o, s);
                return s.ToArray();
            }
        }
        /// <summary>
        /// json�����л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">xml</param>
        /// <returns>����</returns>
        public T Deserialize<T>(byte[] data) {
            using (var s = new MemoryStream(data)) {
                return JsonSerializer.DeserializeFromStream<T>(s);
            }
        }
        /// <summary>
        /// ���г�json�ļ�
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="fileName">�ļ���</param>
        public void SerializeFile<T>(T o, string fileName) {
            FileDirectory.FileDelete(fileName);
            FileDirectory.FileWrite(fileName, Serialize(o).ToUTF8());
        }
        /// <summary>
        /// json�ļ������л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="fileName">�ļ���</param>
        /// <returns>����</returns>
        public T DeserializeFile<T>(string fileName) {
            byte[] data = FileDirectory.FileReadAll(fileName, Encoding.UTF8).FromBase64();
            return Deserialize<T>(data);
        }
        /// <summary>
        /// ���г�XML��DES����
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="key">����KEY</param>
        /// <returns>XML����</returns>
        public byte[] SerializeEncode<T>(T o, string key = "") {
            return key.IsNullEmpty() ? Serialize(o) : Serialize(o).ToUTF8().DESEncode(key).FromBase64();
        }
        /// <summary>
        /// DES���ܺ����гɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">XML����</param>
        /// <param name="key">����KEY</param>
        /// <returns>����</returns>
        public T DecodeDeserialize<T>(byte[] data, string key = "") {
            return key.IsNullEmpty() ? Deserialize<T>(data) : Deserialize<T>(data.ToUTF8().DESDecode(key).FromBase64());
        }
    }
}
