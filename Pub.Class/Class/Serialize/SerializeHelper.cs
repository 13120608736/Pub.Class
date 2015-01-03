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

namespace Pub.Class {
    /// <summary>
    /// ���л������л�������
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// <code>
    /// <example>
    ///     using (SerializeHelper s = new SerializeHelper(SerializeEnum.json)) { 
    ///         s.Serialize(u);
    ///         s.Deserialize&lt;User>(s.Serialize(u))
    ///     }
    ///     using (SerializeHelper s = new SerializeHelper(SerializeEnum.xml)) { 
    ///         s.SerializeFile(u, "c:\test.txt");
    ///         s.DeserializeFile&lt;User>(s.SerializeFile(u, "c:\test.txt"))
    ///     }
    ///     using (SerializeHelper s = new SerializeHelper(SerializeEnum.binary)) { 
    ///         s.SerializeFile(u, "c:\test.txt");
    ///         s.DeserializeFile&lt;User>(s.SerializeFile(u, "c:\test.txt"))
    ///     }
    ///     using (SerializeHelper s = new SerializeHelper("json")) { 
    ///         s.SerializeEncode(u, "test");
    ///         s.DecodeDeserialize&lt;User>(s.SerializeEncode(u, "test"))
    ///     }
    ///     using (SerializeHelper s = new SerializeHelper("xml")) { 
    ///         s.SerializeEncode(u, "");
    ///         s.DecodeDeserialize&lt;User>(s.SerializeEncode(u, ""))
    ///     }
    ///     using (SerializeHelper s = new SerializeHelper("binary")) { 
    ///         s.SerializeEncode(u, "");
    ///         s.DecodeDeserialize&lt;User>(s.SerializeEncode(u, ""))
    ///     }
    /// </example>
    /// </code>
    /// </summary>
    public class SerializeHelper : Disposable {
        private SerializeEnum serializeEnum;
        private ISerialize serialize = null;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="serializeEnum">���л����� string</param>
        public SerializeHelper(string serializeEnum) {
            this.serializeEnum = serializeEnum.ToEnum<SerializeEnum>();
            init();
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="serializeEnum">���л����� enum</param>
        public SerializeHelper(SerializeEnum serializeEnum) {
            this.serializeEnum = serializeEnum;
            init();
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void init() { 
            switch (this.serializeEnum) {
                case SerializeEnum.xml: this.serialize = new XmlSerialize(); break;
                case SerializeEnum.json: this.serialize = new JsonSerialize(); break;
                case SerializeEnum.binary: this.serialize = new BinarySerialize(); break;
                default: this.serialize = new JsonSerialize(); break;
            }
        }
        /// <summary>
        /// ��using �Զ��ͷ�
        /// </summary>
        protected override void InternalDispose() {
            serialize = null;
            base.InternalDispose();
        }
        /// <summary>
        /// ���л�
        /// </summary>
        /// <param name="o">����</param>
        /// <returns>�ַ���</returns>
        public string Serialize(object o) {
            return this.serialize.Serialize(o);
        }
        /// <summary>
        /// data�����л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">����</param>
        /// <returns>����</returns>
        public T Deserialize<T>(string data) {
            return this.serialize.Deserialize<T>(data);
        }
        /// <summary>
        /// ���г��ļ�
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="fileName">�ļ���</param>
        public void SerializeFile(string o, string fileName) {
            this.serialize.SerializeFile(o, fileName);
        }
        /// <summary>
        /// �ļ������л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="fileName">�ļ���</param>
        /// <returns>����</returns>
        public T DeserializeFile<T>(string fileName) {
            return this.serialize.DeserializeFile<T>(fileName);
        }
        /// <summary>
        /// ���л���DES����
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="key">����KEY</param>
        /// <returns>����</returns>
        public string SerializeEncode(object o, string key = "") {
            return this.serialize.SerializeEncode(o, key);
        }
        /// <summary>
        /// DES���ܺ����л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">����</param>
        /// <param name="key">����KEY</param>
        /// <returns>����</returns>
        public T DecodeDeserialize<T>(string data, string key = "") {
            return this.serialize.DecodeDeserialize<T>(data, key);
        }
    }
}
