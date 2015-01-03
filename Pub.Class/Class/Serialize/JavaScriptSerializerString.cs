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
#if !NET20
using System.Web.Script.Serialization;
#endif

namespace Pub.Class {
    /// <summary>
    /// json���л��ͷ����л�
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// <code>
    /// <example>
    /// User u1 = new User() { UserID = 1000, Name = "�ܻ���" };
    /// var serialize = new JavaScriptSerializerString();
    /// string s = serialize.Serialize(u1);
    /// serialize.Deserialize&lt;User>(s);
    /// </example>
    /// </code>
    /// </summary>
    public class JavaScriptSerializerString : ISerializeString {
        /// <summary>
        /// ���г�json
        /// </summary>
        /// <param name="o">����</param>
        /// <returns>json</returns>
        public string Serialize<T>(T o) {
#if NET20
            return Json.ToJsonString(o);
#else
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            return serializer.Serialize(o);
#endif
        }
        /// <summary>
        /// json�����л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">xml</param>
        /// <returns>����</returns>
        public T Deserialize<T>(string data) {
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //if (typeof(T) == typeof(DateTime)) {
            //    object obj = jsonSerializer.Deserialize<DateTime>(data).ToLocalTime();
            //    return (T)obj;
            //} else return jsonSerializer.Deserialize<T>(data);
#if NET20
            return Json.ToObject<T>(data);
#else
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Deserialize<T>(data);
#endif
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
