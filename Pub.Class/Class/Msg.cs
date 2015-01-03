//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Web;
using System.Text;

namespace Pub.Class {
    /// <summary>
    /// �����Ϣ��
    /// 
    /// �޸ļ�¼
    ///     2006.05.08 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Msg {
        //#region Write
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="msg">��Ϣ����</param>
        public static void Write(string msg) { HttpContext.Current.Response.Write(msg); }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="args">��Ϣ����</param>
        public static void Write(params object[] args) { HttpContext.Current.Response.Write(string.Concat(args)); }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="format">��ʽ��</param>
        /// <param name="args">����ֵ</param>
        public static void Write(string format, params object[] args) { HttpContext.Current.Response.Output.Write(format, args); }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void Write(object msg) { Write(msg.ToString()); }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void WriteLine(string msg) { HttpContext.Current.Response.Write(msg + "<br />"); }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void WriteLine(object msg) { WriteLine(msg.ToString()); }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="format">��ʽ��</param>
        /// <param name="args">����ֵ</param>
        public static void WriteLine(string format, params object[] args) { HttpContext.Current.Response.Output.Write(format + "<br />", args); }
        /// <summary>
        /// �����Ϣ ������ִ��
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void WriteEnd(string msg) { Msg.Write(msg); Msg.End(); }
        /// <summary>
        /// �����Ϣ ������ִ��
        /// </summary>
        /// <param name="format">��ʽ��</param>
        /// <param name="args">����ֵ</param>
        public static void WriteEnd(string format, params object[] args) { Msg.Write(format, args); Msg.End(); }
        /// <summary>
        /// �����Ϣ ������ִ��
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void WriteEnd(object msg) { WriteEnd(msg.ToString()); }
        /// <summary>
        /// �����Ϣ��HtmlGenericControl�ؼ�
        /// </summary>
        /// <param name="Control">HtmlGenericControl�ؼ�</param>
        /// <param name="msg">��Ϣ����</param>
        /// <param name="isAdd">�Ƿ��ۼ�</param>
        public static void Write(System.Web.UI.HtmlControls.HtmlGenericControl Control, string msg, bool isAdd) {
            if (isAdd) Control.InnerHtml += msg; else Control.InnerHtml = msg;
        }
        /// <summary>
        /// ��ҳ�����xml���� ������
        /// </summary>
        /// <param name="xmlnode">xml����</param>
        public static void WriteXMLEnd(string xmlnode) {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "text/xml";
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write(xmlnode.ToString());
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// ���json���� ������
        /// </summary>
        /// <param name="json">json����</param>
        public static void WriteJSONEnd(string json) {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "application/json";
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write(json);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// ���jsonp���� ������
        /// </summary>
        /// <param name="json">json����</param>
        public static void WriteJSONPEnd(string json) {
            string callback = Request2.GetQ("callback");
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "application/javascript";
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write("{0}({1});".FormatWith(callback, json));
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// ���html���� ������
        /// </summary>
        /// <param name="html">html����</param>
        public static void WriteHTMLEnd(string html) {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "text/html";
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// ����ı����� ������
        /// </summary>
        /// <param name="text">text����</param>
        public static void WriteTextEnd(string text) {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.End();
        }
        //#endregion
        //#region End/CompleteRequest
        /// <summary>
        /// ��ֹҳ�������
        /// </summary>
        public static void End() { HttpContext.Current.Response.End(); }
        /// <summary>
        /// ��ֹ�̵߳�����
        /// </summary>
        public static void CompleteRequest() { HttpContext.Current.ApplicationInstance.CompleteRequest(); }
        //#endregion
        //#region Redirect/Transfer/NoCache
        /// <summary>
        /// Ŀ¼��ת
        /// </summary>
        /// <param name="url">url</param>
        public static void Redirect(string url) { HttpContext.Current.Response.Redirect(url); }
        /// <summary>
        /// ��������Ŀ¼��ת 
        /// </summary>
        /// <param name="url">url</param>
        public static void Transfer(string url) { HttpContext.Current.Server.Transfer(url); }
        /// <summary>
        /// ��ʹ��ҳ��CACHE
        /// </summary>
        public static void NoCache() {
            System.Web.HttpContext.Current.Response.BufferOutput = false;
            System.Web.HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            System.Web.HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.CacheControl = "no-cache";
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
        }
        /// <summary>
        /// �ֿ����
        /// </summary>
        public static void Flush() {
            HttpContext.Current.Response.Flush();
        }
        //#endregion
    }
}
