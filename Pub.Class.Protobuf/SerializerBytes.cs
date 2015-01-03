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
using ProtoBuf;

namespace Pub.Class.Protobuf {
    /// <summary>
    /// Protobuf���л�
    /// 
    /// �޸ļ�¼
    ///     2013.02.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class SerializerBytes : ISerializeBytes {
        public void RegisterTypes(params Type[] types) { }
        /// <summary>
        /// ���г�bytes
        /// </summary>
        /// <param name="o">����</param>
        /// <returns>bytes</returns>
        public byte[] Serialize<T>(T o) {
            using (MemoryStream ms = new MemoryStream()) {
                Serializer.Serialize<T>(ms, o);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// bytes�����л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">bytes</param>
        /// <returns>����</returns>
        public T Deserialize<T>(byte[] data) {
            using (MemoryStream ms = new MemoryStream(data)) {
                return Serializer.Deserialize<T>(ms);
            }
        }
        /// <summary>
        /// ���г�bytes�ļ�
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="fileName">�ļ���</param>
        public void SerializeFile<T>(T o, string fileName) {
            FileDirectory.FileDelete(fileName);
            FileDirectory.FileWrite(fileName, Serialize(o).ToUTF8());
        }
        /// <summary>
        /// bytes�ļ������л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="fileName">�ļ���</param>
        /// <returns>����</returns>
        public T DeserializeFile<T>(string fileName) {
            byte[] data = FileDirectory.FileReadAll(fileName, Encoding.UTF8).FromBase64();
            return Deserialize<T>(data);
        }
        /// <summary>
        /// ���г�bytes��DES����
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="key">����KEY</param>
        /// <returns>bytes����</returns>
        public byte[] SerializeEncode<T>(T o, string key = "") {
            return key.IsNullEmpty() ? Serialize(o) : Serialize(o).ToUTF8().DESEncode(key).FromBase64();
        }
        /// <summary>
        /// DES���ܺ����гɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">bytes����</param>
        /// <param name="key">����KEY</param>
        /// <returns>����</returns>
        public T DecodeDeserialize<T>(byte[] data, string key = "") {
            return key.IsNullEmpty() ? Deserialize<T>(data) : Deserialize<T>(data.ToUTF8().DESDecode(key).FromBase64());
        }
    }
}
