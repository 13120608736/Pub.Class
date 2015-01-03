//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
#endif
using System.Text;

namespace Pub.Class {
    /// <summary>
    /// ����
    /// 
    /// �޸ļ�¼
    ///     2011.12.01 �汾��1.0 livexy ��������
    /// 
    /// <code>
    /// <example>
    /// List&lt;UrlParameter> param = new List&lt;UrlParameter>();
    /// param.Add(new UrlParameter("oauth_callback", config.RedirectUrl.UrlEncode2()));
    /// param.Add(new UrlParameter("oauth_consumer_key", config.AppKey));
    /// param.Add(new UrlParameter("oauth_nonce", OAuthCommon.GetGUID32()));
    /// param.Add(new UrlParameter("oauth_signature_method", "HMAC-SHA1"));
    /// param.Add(new UrlParameter("oauth_timestamp", OAuthCommon.GetTimestamp()));
    /// param.Add(new UrlParameter("oauth_version", "1.0"));
    /// param.Add(new UrlParameter("scope", "create_records"));
    /// param.Sort(new UrlParameterCompre());
    /// </example>
    /// </code>
    /// </summary>
    public class UrlParameter {
        /// <summary>
        /// ��������
        /// </summary>
        public string ParameterName;
        /// <summary>
        /// ����ֵ
        /// </summary>
        public string ParameterValue;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="Name">��������</param>
        /// <param name="Value">����ֵ</param>
        public UrlParameter(string Name, string Value) {
            this.ParameterName = Name;
            this.ParameterValue = Value;
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="Name">��������</param>
        /// <param name="Value">����ֵ</param>
        public UrlParameter(string Name, object Value) {
            this.ParameterName = Name;
            this.ParameterValue = Value.ToString();
        }
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <returns>�����ַ���</returns>
        public override string ToString() {
            return string.Format("{0}={1}", this.ParameterName, this.ParameterValue);
        }
        /// <summary>
        /// ����Url Encode�ַ���
        /// </summary>
        /// <returns>�����ַ���</returns>
        public string ToEncodeString() {
            return string.Format("{0}={1}", this.ParameterName, this.ParameterValue.UrlEncode());
        }
    }
    /// <summary>
    /// ��������
    /// 
    /// �޸ļ�¼
    ///     2011.12.01 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class UrlParameterCompre : IComparer<UrlParameter> {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(UrlParameter x, UrlParameter y) {
            if (x.ParameterName == y.ParameterName) {
                return string.Compare(x.ParameterValue, y.ParameterValue);
            } else {
                return string.Compare(x.ParameterName, y.ParameterName);
            }
        }
    }
}
