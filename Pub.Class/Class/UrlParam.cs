//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Cryptography;

namespace Pub.Class {
    /// <summary>
    /// ������
    /// 
    /// �޸ļ�¼
    ///     2006.05.14 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class UrlParam : IComparable {
        private string name;
        private object value;
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public string Name { get { return name; } }
        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        public string Value {
            get {
                if (value is Array) return ConvertArrayToString(value as Array);
                else return value.ToString();
            }
        }
        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        public string EncodedValue {
            get {
                if (value is Array) return HttpUtility.UrlEncode(ConvertArrayToString(value as Array));
                else return HttpUtility.UrlEncode(value.ToString());
            }
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="value">����ֵ</param>
        protected UrlParam(string name, object value) {
            this.name = name;
            this.value = value;
        }
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <returns>�����ַ�������ֵ��</returns>
        public override string ToString() {
            return string.Format("{0}={1}", Name, Value);
        }
        /// <summary>
        /// ����encode�ַ���
        /// </summary>
        /// <returns>����encode�ַ���</returns>
        public string ToEncodedString() {
            return string.Format("{0}={1}", Name, EncodedValue);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="value">����ֵ</param>
        /// <returns>���ز���</returns>
        public static UrlParam Create(string name, object value) {
            return new UrlParam(name, value);
        }
        /// <summary>
        /// �Ƚϲ����Ƿ���ͬ
        /// </summary>
        /// <param name="obj">Ҫͬ��ǰ�����ȽϵĲ���</param>
        /// <returns>0��ͬ,��0��ͬ</returns>
        public int CompareTo(object obj) {
            if (!(obj is UrlParam)) return -1;
            return this.name.CompareTo((obj as UrlParam).name);
        }
        /// <summary>
        /// ����������ת��Ϊ��ֵ��
        /// </summary>
        /// <param name="a">��������</param>
        /// <returns>ת������ֵ��,��ֵ��֮���ö��ŷָ�</returns>
        private static string ConvertArrayToString(Array a) {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < a.Length; i++) {
                if (i > 0) builder.Append(",");
                builder.Append(a.GetValue(i).ToString());
            }
            return builder.ToString();
        }
        /// <summary>
        /// php time()
        /// </summary>
        /// <returns></returns>
        public static long Time() {
            DateTime timeStamp = new DateTime(1970, 1, 1);
            return (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000000;
        }
        /// <summary>
        /// php microtime()
        /// </summary>
        /// <returns></returns>
        private static string MicroTime() {
            long sec = Time();
            int msec = DateTime.UtcNow.Millisecond;
            string strMsec = "0." + msec.ToString().PadRight(8, '0');
            return strMsec + " " + sec.ToString();
        }
        /// <summary>
        /// �������󶨵�url
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="secret"></param>
        /// <param name="action"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static string GetUrl(string apiUrl, string secret, string action, UrlParam[] parameters) {
            List<UrlParam> list = new List<UrlParam>(parameters);
            list.Add(UrlParam.Create("time", Time()));
            list.Add(UrlParam.Create("action", action));
            list.Sort();
            StringBuilder values = new StringBuilder();
            foreach (UrlParam param in list) {
                if (!string.IsNullOrEmpty(param.Value)) values.Append(param.ToString());
            }
            values.Append(secret);
            byte[] md5_result = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(values.ToString()));
            StringBuilder sig_builder = new StringBuilder();
            foreach (byte b in md5_result) sig_builder.Append(b.ToString("x2"));
            list.Add(UrlParam.Create("sig", sig_builder.ToString()));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++) {
                if (i > 0) builder.Append("&");
                builder.Append(list[i].ToEncodedString());
            }
            return string.Format("{0}?{1}", apiUrl, builder.ToString());
        }
    }
}
