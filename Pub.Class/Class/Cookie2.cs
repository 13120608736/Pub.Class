//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Text;
using System.Web;
using System.Collections.Generic;

namespace Pub.Class {
    /// <summary>
    /// Cookie����
    /// 
    /// �޸ļ�¼
    ///     2012.01.20 �汾��1.5 livexy ɾ��Set2/Get2
    ///     2010.02.11 �汾��1.4 livexy ������Cookie
    ///     2009.07.20 �汾��1.3 livexy ���Set2/Get2
    ///     2009.06.01 �汾��1.2 livexy �޸Ķ�COOKIES���ݽ���AES����
    ///     2009.04.20 �汾��1.1 livexy ��Ӷ�P3P֧��
    ///     2006.05.01 �汾��1.0 livexy ��������
    /// 
    /// <code>
    /// <example>
    /// Cookie2.Set("UserName", "cexo255");
    /// Cookie2.Get("UserName");
    /// Cookie2.SetP3PHeader();
    /// Cookie2.Set("HJ.CurrentUser", "UserName", "cexo255");
    /// Cookie2.Get("HJ.CurrentUser", "UserName");
    /// Cookie2.Clear();
    /// </example>
    /// </code>
    /// </summary>
    public class Cookie2 {
        //#region Set
        /// <summary>
        /// ���� Cookie P3P Header ���������� Cookie ��
        /// </summary>
        public static void SetP3PHeader() {
            HttpContext.Current.Response.AddHeader("P3P", "CP=CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");
        }
        /// <summary>
        /// ���� Cookie
        /// </summary>
        /// <param name="key">����</param>
        /// <param name="value">ֵ</param>
        public static void Set(string key, string value) {
            Set("Pub.Class.Cookies", key, value);
        }
        /// <summary>
        /// ���� Cookie
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="expires">��������</param>
        public static void Set(string key, string value, DateTime expires) {
            Set("Pub.Class.Cookies", key, value, expires, null);
        }
        /// <summary>
        /// ����Cookiesֵ
        /// </summary>
        /// <param name="key">Cookies����</param>
        /// <param name="value">Cookies���ƶ�Ӧ��ֵ</param>
        /// <param name="days">��Ч�� ����</param>
        public static void Set(string key, string value, int days) { Set("Pub.Class.Cookies", key, value, days > 0 ? DateTime.Now.AddDays(days) : DateTime.MinValue); }
        /// <summary>
        /// ���� Cookie
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="expires">��������</param>
        /// <param name="domain">��</param>
        public static void Set(string key, string value, DateTime expires, string domain) {
            Set("Pub.Class.Cookies", key, value, expires, domain);
        }
        /// <summary>
        /// ���� Cookie
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        public static void Set(string name, string key, string value) {
            Set(name, key, value, DateTime.MinValue);
        }
        /// <summary>
        /// ���� Cookie
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="domain">��</param>
        public static void Set(string name, string key, string value, string domain) {
            Set(name, key, value, DateTime.MinValue, domain);
        }
        /// <summary>
        /// ���� Cookie
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="expires">��������</param>
        public static void Set(string name, string key, string value, DateTime expires) {
            Set(name, key, value, expires, null);
        }
        /// <summary>
        /// ����Cookiesֵ
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="days">��Ч�� ����</param>
        public static void Set(string name, string key, string value, int days) { Set(name, key, value, days > 0 ? DateTime.Now.AddDays(days) : DateTime.MinValue); }
        /// <summary>
        /// ���� Cookie
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="expires">��������</param>
        /// <param name="domain">��</param>
        public static void Set(string name, string key, string value, DateTime expires, string domain) {
            Set(name, key, value, expires, domain, null);
        }
        /// <summary>
        /// ���� Cookie
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="expires">��������</param>
        /// <param name="domain">��</param>
        /// <param name="path">����·��</param>
        /// <param name="httpOnly">�Ƿ��ͨ���ͻ��˽ű�����</param>
        public static void Set(string name, string key, string value, DateTime expires, string domain, string path, bool httpOnly) {
            Set(name, key, value, expires, domain, path, httpOnly, false);
        }
        /// <summary>
        /// ���� Cookie
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="expires">��������</param>
        /// <param name="domain">��</param>
        /// <param name="path">����·��</param>
        public static void Set(string name, string key, string value, DateTime expires, string domain, string path) {
            Set(name, key, value, expires, domain, path, false);
        }
        /// <summary>
        /// ���� Cookie
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="expires">��������</param>
        /// <param name="domain">��</param>
        /// <param name="path">����·��</param>
        /// <param name="httpOnly">�Ƿ��ͨ���ͻ��˽ű�����</param>
        /// <param name="secure">�Ƿ�ʹ�ð�ȫ�׽��ֲ� (SSL)������ͨ�� HTTPS������ Cookie</param>
        public static void Set(string name, string key, string value, DateTime expires, string domain, string path, bool httpOnly, bool secure) {
            string _key = "9cf8d21d394a8919d2f9706dfdc6421e";
            string encryptName = (_key + name).MD5();
            string encryptKey = (_key + key).MD5();
            string encryptValue = value.AESEncode(_key);
            HttpCookie cookie = HttpContext.Current.Request.Cookies[encryptName];

            if (cookie.IsNull()) cookie = new HttpCookie(encryptName);
            cookie.Values[encryptKey] = encryptValue;
            if (expires > DateTime.MinValue) cookie.Expires = expires;
            if (!string.IsNullOrEmpty(domain)) cookie.Domain = domain;
            if (!string.IsNullOrEmpty(path)) cookie.Path = path;
            cookie.HttpOnly = httpOnly;
            cookie.Secure = secure;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        //#endregion
        //#region Get
        /// <summary>
        /// ȡCookiesֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public static string Get(string key) {
            return Get("Pub.Class.Cookies", key);
        }
        /// <summary>
        /// ȡCookiesֵ
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="key">��</param>
        /// <returns></returns>
        public static string Get(string name, string key) {
            string _key = "9cf8d21d394a8919d2f9706dfdc6421e";
            string encryptName = (_key + name).MD5();
            string encryptKey = (_key + key).MD5();
            string decryptValue = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[encryptName];

            if (cookie.IsNotNull()) {
                if (!string.IsNullOrEmpty(cookie.Values[encryptKey])) {
                    decryptValue = cookie.Values[encryptKey].ToString().AESDecode(_key);
                }
            }

            return decryptValue.IsNullEmpty() ? string.Empty : decryptValue;
        }
        //#endregion
        //#region Clear
        /// <summary>
        /// ���Cookies
        /// </summary>
        public static void Clear() {
            IList<string> cookies = new List<string>();
            foreach (string name in HttpContext.Current.Request.Cookies) {
                cookies.Add(name);
            }
            foreach (string name in cookies) {
                HttpCookie cookie = new HttpCookie(name);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }
        //#endregion
    }
}
