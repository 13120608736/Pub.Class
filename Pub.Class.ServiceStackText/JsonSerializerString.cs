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
    public class JsonSerializerString : ISerializeString {
        /// <summary>
        /// ���г�json
        /// </summary>
        /// <param name="o">����</param>
        /// <returns>json</returns>
        public string Serialize<T>(T o) {
            return JsonSerializer.SerializeToString(o);
        }
        /// <summary>
        /// json�����л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">xml</param>
        /// <returns>����</returns>
        public T Deserialize<T>(string data) {
            return JsonSerializer.DeserializeFromString<T>(data);
        }
        /// <summary>
        /// ���г�json�ļ�
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="fileName">�ļ���</param>
        public void SerializeFile<T>(T o, string fileName) {
            FileDirectory.FileDelete(fileName);
            FileDirectory.FileWrite(fileName, Serialize(o));
        }
        /// <summary>
        /// json�ļ������л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="fileName">�ļ���</param>
        /// <returns>����</returns>
        public T DeserializeFile<T>(string fileName) {
            string data = FileDirectory.FileReadAll(fileName, Encoding.UTF8);
            return Deserialize<T>(data);
        }
        /// <summary>
        /// ���г�XML��DES����
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="key">����KEY</param>
        /// <returns>XML����</returns>
        public string SerializeEncode<T>(T o, string key = "") {
            return key.IsNullEmpty() ? Serialize(o) : Serialize(o).DESEncode(key);
        }
        /// <summary>
        /// DES���ܺ����гɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">XML����</param>
        /// <param name="key">����KEY</param>
        /// <returns>����</returns>
        public T DecodeDeserialize<T>(string data, string key = "") {
            return key.IsNullEmpty() ? Deserialize<T>(data) : Deserialize<T>(data.DESDecode(key));
        }
    }
}
