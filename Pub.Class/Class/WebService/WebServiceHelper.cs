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
    /// WebService������
    /// 
    /// �޸ļ�¼
    ///     2011.11.09 �汾��1.0 livexy ��������
    /// 
    /// <code>
    /// <example>
    ///     Hashtable pas = new Hashtable(); pas["i"] = 100;
    ///     new WebServiceHelper(WebServiceEnum.get).Call("http://www.test.com/default.asmx", "WebService", "test2", pas)
    ///     new WebServiceHelper(WebServiceEnum.post).Call("http://www.test.com/default.asmx", "WebService", "test2", pas)
    ///     new WebServiceHelper(WebServiceEnum.soap).Call("http://www.test.com/default.asmx", "WebService", "test2", pas)
    ///     new WebServiceHelper(WebServiceEnum.dynamic).Call("http://www.test.com/default.asmx", "WebService", "test2", pas)
    /// </example>
    /// </code>
    /// </summary>
    public class WebServiceHelper : Disposable {
        private WebServiceEnum WebServiceEnum;
        private IWebService WebService = null;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="WebServiceEnum">WebService �������� Enum string</param>
        public WebServiceHelper(string WebServiceEnum) {
            this.WebServiceEnum = WebServiceEnum.ToEnum<WebServiceEnum>();
            init();
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="WebServiceEnum">WebService �������� Enum</param>
        public WebServiceHelper(WebServiceEnum WebServiceEnum) {
            this.WebServiceEnum = WebServiceEnum;
            init();
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void init() { 
            switch (this.WebServiceEnum) {
                case WebServiceEnum.get: this.WebService = new GetWebService(); break;
                case WebServiceEnum.post: this.WebService = new PostWebService(); break;
                case WebServiceEnum.soap: this.WebService = new SoapWebService(); break;
                case WebServiceEnum.dynamic: this.WebService = new DynamicWebService(); break;
                default: this.WebService = new GetWebService(); break;
            }
        }
        /// <summary>
        /// ��using �Զ��ͷ�
        /// </summary>
        protected override void InternalDispose() {
            WebService = null;
            base.InternalDispose();
        }
        /// <summary>
        /// WebService���÷���
        /// </summary>
        /// <param name="url">WebService �ӿڵ�ַ</param>
        /// <param name="className">����</param>
        /// <param name="methodName">������</param>
        /// <param name="parms">����</param>
        /// <returns>�����ַ���</returns>
        public string Call(string url, string className, string methodName, Hashtable parms) {
            return this.WebService.Call(url, className, methodName, parms);
        }
        /// <summary>
        /// WebService���÷���
        /// </summary>
        /// <param name="url">WebService �ӿڵ�ַ</param>
        /// <param name="className">����</param>
        /// <param name="methodName">������</param>
        /// <param name="parms">����</param>
        /// <returns>�����ַ���</returns>
        public string Call(string url, string className, string methodName, IList<UrlParameter> parms) {
            return this.WebService.Call(url, className, methodName, parms);
        }
    }
}
