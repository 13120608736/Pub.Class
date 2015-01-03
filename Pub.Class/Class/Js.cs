//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Pub.Class {
    /// <summary>
    /// Js������
    /// 
    /// �޸ļ�¼
    ///     2006.05.02 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Js {
        //#region Run
        /// <summary>
        /// ����JS����
        /// </summary>
        /// <param name="Page">ָ��Page</param>
        /// <param name="strCode">Ҫע��Ĵ���</param>
        /// <param name="isTop">�Ƿ���ͷ��/������β��</param>
        public static void Run(System.Web.UI.Page Page, string strCode, bool isTop) {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">\n");
            sb.Append(strCode.Trim());
            sb.Append("\n</script>\n");
            if (isTop) Page.RegisterClientScriptBlock("RunTopJs", sb.ToString()); else Page.RegisterStartupScript("RunBottomJs", sb.ToString());
        }
        /// <summary>
        /// ����JS����
        /// </summary>
        /// <param name="Page">ָ��Page</param>
        /// <param name="strCode">Ҫע��Ĵ���</param>
        /// <param name="isTop">�Ƿ���ͷ��/������β��</param>
        /// <param name="IDStr">Key</param>
        public static void Run(System.Web.UI.Page Page, string strCode, bool isTop, string IDStr) {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">\n");
            sb.Append(strCode.Trim());
            sb.Append("\n</script>\n");
            if (isTop) Page.RegisterClientScriptBlock(IDStr, sb.ToString()); else Page.RegisterStartupScript(IDStr, sb.ToString());
        }
        /// <summary>
        /// ���ָ��ע���JS����
        /// </summary>
        /// <param name="Page">ָ��Page</param>
        /// <param name="isTop">�Ƿ���ͷ��/������β��</param>
        /// <param name="IDStr">Key</param>
        public static void Run(System.Web.UI.Page Page, bool isTop, string IDStr) {
            if (isTop) Page.RegisterClientScriptBlock(IDStr, ""); else Page.RegisterStartupScript(IDStr, "");
        }
        //#endregion
        //#region Alert
        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void Alert(string msg) {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + msg.Trim() + "\"); \n");
            sb.Append("</script>\n");
            HttpContext.Current.Response.Write(sb.ToString());
        }
        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void AlertEnd(string msg) {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + msg.Trim() + "\"); \n");
            sb.Append("</script>\n");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        /// <param name="Page">ָ��ҳ</param>
        /// <param name="msg">��Ϣ</param>
        public static void Alert(System.Web.UI.Page Page, string msg) {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + msg.Trim() + "\"); \n");
            sb.Append("</script>\n");
            Page.RegisterClientScriptBlock("AlertJs", sb.ToString());
        }
        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        /// <param name="Page">ָ��ҳ</param>
        /// <param name="msg">��Ϣ</param>
        /// <param name="isTop">�Ƿ���ͷ��/������β��</param>
        public static void Alert(System.Web.UI.Page Page, string msg, bool isTop) {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + msg.Trim() + "\"); \n");
            sb.Append("</script>\n");
            if (isTop) Page.RegisterClientScriptBlock("AlertTopJs", sb.ToString()); else Page.RegisterStartupScript("AlertBottomJs", sb.ToString());
        }
        //#endregion
        //#region Import/loadCss/AddAttr/chkFormData
        /// <summary>
        /// ע��һ������JS�ļ�/��CSS�ļ�
        /// </summary>
        /// <param name="Page">ָ��ҳ</param>
        /// <param name="filePath">�ļ�</param>
        /// <param name="isTop">�Ƿ���ͷ��/������β��</param>
        public static void Import(System.Web.UI.Page Page, string filePath, bool isTop) {
            StringBuilder sb = new StringBuilder();
            if (filePath.ToLower().Substring(filePath.Length - 3, 3) == ".js") {
                sb.Append("<script language=\"JavaScript\" src=\"" + filePath + "\" type=\"text/javascript\"></script>\n");
                if (isTop) Page.RegisterClientScriptBlock("TopJs", sb.ToString()); else Page.RegisterStartupScript("BottomJs", sb.ToString());
            }
            if (filePath.ToLower().Substring(filePath.Length - 4, 4) == ".css") {
                LoadCss(Page, filePath);
            }
        }
        /// <summary>
        /// ע��һ������CSS�ļ�
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="cssFile">CSS�ļ�</param>
        public static void JsLoadCss(System.Web.UI.Page page, string cssFile) {
            Run(page, "setStyle(\"" + cssFile + "\");\n", true);
        }
        /// <summary>
        /// ע��һ������CSS�ļ�
        /// </summary>
        /// <param name="placeHolder">PlaceHolder���</param>
        /// <param name="cssFile">CSS�ļ�</param>
        public static void LoadCss(System.Web.UI.WebControls.PlaceHolder placeHolder, string cssFile) {
            HtmlGenericControl objLink = new HtmlGenericControl("LINK");
            objLink.Attributes["rel"] = "stylesheet";
            objLink.Attributes["type"] = "text/css";
            objLink.Attributes["href"] = cssFile;
            placeHolder.Controls.Add(objLink);
            //<asp:placeholder id="MyCSS" runat="server"></asp:placeholder> 
        }
        /// <summary>
        /// ע��һ������CSS�ļ�
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="cssFile">CSS�ļ�</param>
        public static void LoadCss(System.Web.UI.Page page, string cssFile) {
            HtmlLink myHtmlLink = new HtmlLink();
            myHtmlLink.Href = cssFile;
            Js.AddAttr(myHtmlLink, "rel", "stylesheet");
            Js.AddAttr(myHtmlLink, "type", "text/css");
            page.Header.Controls.Add(myHtmlLink);
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="Control">WebControl</param>
        /// <param name="eventStr">����</param>
        /// <param name="MsgStr">����</param>
        public static void AddAttr(System.Web.UI.WebControls.WebControl Control, string eventStr, string MsgStr) {
            Control.Attributes.Add(eventStr, MsgStr);
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="Control">HtmlGenericControl</param>
        /// <param name="eventStr">����</param>
        /// <param name="MsgStr">����</param>
        public static void AddAttr(System.Web.UI.HtmlControls.HtmlGenericControl Control, string eventStr, string MsgStr) {
            Control.Attributes.Add(eventStr, MsgStr);
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="Control">HtmlGenericControl</param>
        /// <param name="eventStr">����</param>
        /// <param name="MsgStr">����</param>
        public static void AddAttr(System.Web.UI.HtmlControls.HtmlControl Control, string eventStr, string MsgStr) {
            Control.Attributes.Add(eventStr, MsgStr);
        }
        /// <summary>
        /// ��֤����������
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="dataValue">����</param>
        /// <param name="divObjStr">div���� ���Ϊ��ʱ��alert������ʾ</param>
        /// <param name="minLength">��С����</param>
        /// <param name="maxLength">��󳤶�</param>
        /// <param name="titleStr">����</param>
        /// <param name="isNVarchar">�Ƿ�Nvarchar����</param>
        /// <returns>��/��</returns>
        /// <example>
        /// <code>
        /// private bool doSave()
        /// { 
        ///     if (!Cmn.Js.ChkFormData(this, "", "", 1, 20, "����", false)) return false;
        ///     if (!Cmn.Js.ChkFormData(this, "��", "", 4, 20, "����", false)) return false;
        ///     if (!Cmn.Js.ChkFormData(this, "�ܻ���123456789123451", "", 4, 20, "����", false)) return false;
        ///     return true;
        /// }
        /// if (doSave()) Cmn.Js.Alert(this,"�ɹ�");
        /// </code>
        /// </example>
        public static bool ChkFormData(System.Web.UI.Page page, string dataValue, string divObjStr, int minLength, int maxLength, string titleStr, bool isNVarchar) {
            int txtObjLength = (isNVarchar) ? dataValue.Length : dataValue.CnLength();
            StringBuilder sb = new StringBuilder();
            bool _result = true;
            if (txtObjLength == 0 && minLength != 0) {
                if (divObjStr != "") {
                    sb.Append("document.getElementById(\"" + divObjStr + "\").innerHTML = \"<div class=ErrorMsg>" + titleStr + "����Ϊ�գ�</div>\";");
                } else {
                    sb.Append("alert('" + titleStr + "����Ϊ�գ�');");
                }
                _result = false;
            } else if (txtObjLength < minLength) {
                if (divObjStr != "") {
                    sb.Append("document.getElementById(\"" + divObjStr + "\").innerHTML = \"<div class=ErrorMsg>" + titleStr + "����С��" + minLength + "���ַ���</div>\";");
                } else {
                    sb.Append("alert('" + titleStr + "����С��" + minLength + "���ַ���');");
                }
                _result = false;
            } else if (txtObjLength > maxLength) {
                if (divObjStr != "") {
                    sb.Append("document.getElementById(\"" + divObjStr + "\").innerHTML = \"<div class=ErrorMsg>" + titleStr + "���ܴ���" + maxLength + "���ַ���</div>\";");
                } else {
                    sb.Append("alert('" + titleStr + "���ܴ���" + maxLength + "���ַ���');");
                }
                _result = false;
            } else {
                if (divObjStr != "") { sb.Append("document.getElementById(\"" + divObjStr + "\").innerHTML = \"\");"); }
                _result = true;
            }
            Js.Run(page, sb.ToString(), false, titleStr);
            return _result;
        }
        //#endregion
    }
}
