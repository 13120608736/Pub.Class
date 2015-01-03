//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.VisualBasic;
using System.Collections;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Security;
using System.Reflection.Emit;
using System.Security.Cryptography;

namespace Pub.Class {
    /// <summary>
    /// string ��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class StringExtensions {
        /// <summary>
        /// �����ַ���
        /// </summary>
        public const string BANWORD = "(ë��|�ܶ���|������|���|��»�|�ֱ�|������|����|����|������|����ǰ|���ٻ�|Ҷ��Ӣ|�����|�¶���|����ɽ|����|������|��Сƽ|����|������|����|���F��|����|ξ����|�����|������|�޸�|�¼ұ�|����|�����|������|�ƾ�|�����|���|����|������|������|������|�ܸմ�|�Ƽ��|������|������|������|�ŵ½�|������|������|����Ȫ|����ɽ|����|���׹�|���|�ع�ǿ|������|��ҫ��|����Ȫ|���׹�|������|��ǻ�|��ս|��ˮ��|�γ��|������|��Ľ��|����ʯ|������|������|��Ӣ�ű�����|�����|����|��������|����|�Ŵ���|Ҧ��Ԫ|������|����Ӣ��|ϣ����|ī������|�Դ�����|�Դ�����|������|������|����|�ֶ�����|���־|���ʦ|������|��Ӿ�ɽ����ʮ��|����|����|�����˶�|���Ĵ���ɱ|6 4�¼�|�й�1989 6 4�¼���Ƶ|6 4�¼���Ƶ|1989 6 4�¼�|�й�1989 6 4�¼�|1989�¼���Ƶ|��������89|1989�걱��ѧ��|1989��ѧ�˶�|����20��|�˾�����|1989��ѧ���¼�|�Ķ�������|426��������|�й�����|����������|���|�����|����|������|Ħ�Ž�|�ɴ�|�ϻ��籨|����|������|����|����|ȳ�Ȱ�|����|���ֹ�|���ִ�|�򵹹�����|̨������|ʥս|ʾ��|̨��|̨������|̨��|̨�����|̨�嵺��|̨�����|̫�ӵ�|�찲���¼�|��ɱ|СȪ|�µ�|�½�����|�½�����|�½���|����|���ض���|���ط���|���ع�|�ض�|�����|�ظ���|ѧ��|ѧ��|һ��ר��|һ��һ̨|�����й�|һ���|����|Բ��|�췴|������|��ѹ|����|����|���η�����|���η�|�й�|����|����|����|������|�й�֮��|ת����|�Է�|����|����|�ռ���|������֯|������|���ǲ���|֧��|������|������|�˹�ҵ����|������|��|ԭ�ӵ�|�ⵯ|����|��Ǳͧ|��ο�|С�ο�|���ڶ�̬����|����|ʥս |������ |������|��|����|���ֹ�|ȳ�Ȱ�|�����|������|ת����|�Է�|����Բ��|�ƴ���|��ˮ|������|��|����|�����|������ |�ض�|��������|�ػ�|����|�ɹ�����|̨��|̨������|̨��|̨�����|���ض���|�½�����|����|��ë��|ϰ��ƽ|����|̨�嵺��|����|�ظ�|������|���ع�|ǿ��|�ּ�|����|�ȼ��ɱ|��ע|Ѻ��|ѺС|��ͷ|��ׯ|����|����|����|�ϻ���|���̶�|��������|����|�ɿ���|������|����|ҡͷ��|���䶡|ѻƬ|���|�Ի�ҩ|�׷�|�ҩ|����|��|����|�ܲ�|÷��|��Һ|��|��|������|������|������|����|����ҩ|��|�ؽ�|����|��ͷ|��ɫ|����|����|����|����|����|����|����|����|����|����|��Ů|��|��|��Һ|����|��|�ڽ�|�Ľ�|�ҽ�|�ּ�|����|ƨ��|���|ǿ��|ǿ�鷸|��ɫ|���|�鷿|���|�齻|��ͷ|����|����|ɫ��|�侫|����|������|������|ΰ��|�Ը߳�|�Խ�|��Ű|����|Ѩ|����|����|һҹ��|����|����|����|����|����|����|����|����|��|����|����|��ˮ|����|��Һ|��֭|��Ѩ|����|Ԯ����|����|����|����|��й|����|����|�Խ�|K����|�׳�|����|��|��|��̬|���|������|�ي���|����|������|������|����|�H|����|����|����|��|����|����|��ɧ|����|������|�Ɋ�|�Ɋ���|����|������|������B|������b|�������|������|������|��������|��|����|����|����|����|��ĸ|����|���|���|���|����ĸ��|�����|�����|��Ь|�ͽ�|ȥ����|ȥ����|ȥ����|ȥ���|ȥ����|ȥ��|ȥ����|����|������|������|������|������|ɧ��|ɵB|ɵ��|ɵ��|�ϊ�|����|�񾭲�|ʺ|ʺ����|ʺ����|�����|���˵�|����|�����|���|��|��|��|��|����|ʪ��|����|����|����|����|����|����|Ƿ��|Ƿ����|��ˬ��|������|����|����|����|����|�ɱ�|�ɻ�|FUCK|����|����|����|����|����|����|���|���|����|������|�ɼ�|����|СѨ|ǿ��|����|����|ˬ��|ˬ��|�ɸ�|��X|����|����|�ɠ�|����|����|����|����|����|����|���ָ�|����|����|�ɵ�|����|��ˬ|Ƿ��|����|�Ҹ�|����|�ָ�|������|��һ��|Ԯ��|����|�ּ�|����|�鱩|�ټ�|�Ҽ�|����|����|����|����|��һ��|��ˮ|��ʪ|����|�ͽ�|����|��|��|�ž�|�ñ�|��Ѫ��|������|������|����|����|����|����|����|����|������ĸ|����|������|������|����|��������|����|����|����|����|����|����|����|ɧB|��������|��ë|������|����|������|��b)";
        /// <summary>
        /// �ַ����Ƿ�Ϊstring.Empty || null || ""
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        static public bool IsNullEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// Guid�Ƿ�Ϊ null || "000000-0000-0000-0000000000"
        /// </summary>
        /// <param name="guid">Guid��չ</param>
        /// <returns></returns>
        static public bool IsNull(this Guid? guid) {
            return guid.IsNull() || guid == Guid.Empty;
        }
        /// <summary>
        /// ����ַ���Ϊstring.Empty || null || "" ����defaultValue ����ԭ�ַ�������
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        static public string IsNullEmpty(this string str, string defaultValue) {
            return str.IsNullEmpty() ? defaultValue : str;
        }
        /// <summary>
        /// ��ֹJS HTML���뱻ִ�� "" &lt; > \n &amp; �ո�
        /// </summary>
        /// <param name="htmlStr">string��չ</param>
        /// <returns></returns>
        public static string UnHtml(this string htmlStr) {
            if (htmlStr.IsNullEmpty()) return string.Empty;
            return htmlStr.Replace("\"", "\\\"").ShowXmlHtml().Replace(" ", "&nbsp;").Replace("\n", "<br />");
        }
        /// <summary>
        /// ��ֹJS HTML���뱻ִ�� "" &lt; > &amp; �ո� ��\n
        /// </summary>
        /// <param name="htmlStr">string��չ</param>
        /// <returns></returns>
        public static string UnHtmlNoBR(this string htmlStr) {
            if (htmlStr.IsNullEmpty()) return string.Empty;
            return htmlStr.Replace("\"", "\\\"").ShowXmlHtml().Replace(" ", "&nbsp;");
        }
        /// <summary>
        /// ת��Ϊ�Ϸ���XML�ļ�
        /// </summary>
        /// <param name="htmlStr">string��չ</param>
        /// <returns></returns>
        public static string ShowXmlHtml(this string htmlStr) {
            if (htmlStr.IsNullEmpty()) return string.Empty;
            string str = htmlStr.Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;");
            return str;
        }
        /// <summary>
        /// ����JS�¼�
        /// </summary>
        /// <param name="htmlStr">string��չ</param>
        /// <returns></returns>
        public static string ShowHtml(this string htmlStr) {
            if (htmlStr.IsNullEmpty()) return string.Empty;
            string str = htmlStr;
            str = Regex.Replace(str, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "_$1.$2", RegexOptions.IgnoreCase);
            str = new Regex("<script", RegexOptions.IgnoreCase).Replace(str, "<_script");
            str = new Regex("<object", RegexOptions.IgnoreCase).Replace(str, "<_object");
            str = new Regex("javascript:", RegexOptions.IgnoreCase).Replace(str, "_javascript:");
            str = new Regex("vbscript:", RegexOptions.IgnoreCase).Replace(str, "_vbscript:");
            str = new Regex("expression", RegexOptions.IgnoreCase).Replace(str, "_expression");
            str = new Regex("@import", RegexOptions.IgnoreCase).Replace(str, "_@import");
            str = new Regex("<iframe", RegexOptions.IgnoreCase).Replace(str, "<_iframe");
            str = new Regex("<frameset", RegexOptions.IgnoreCase).Replace(str, "<_frameset");
            str = Regex.Replace(str, @"(\<|\s+)o([a-z]+\s?=)", "$1_o$2", RegexOptions.IgnoreCase);
            str = new Regex(@" (on[a-zA-Z ]+)=", RegexOptions.IgnoreCase).Replace(str, " _$1=");
            return str;
        }
        /// <summary>
        /// UrlEncode
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string UrlEncode(this string str) {
            if (str.IsNullEmpty()) return string.Empty;
            return HttpUtility.UrlEncode(str);
        }
        /// <summary>
        /// UrlEncode
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="encoding">����</param>
        /// <returns></returns>
        public static string UrlEncode(this string str, Encoding encoding) {
            if (str.IsNullEmpty()) return string.Empty;
            return HttpUtility.UrlEncode(str, encoding);
        }
        /// <summary>
        /// UrlDecode
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string UrlDecode(this string str) {
            if (str.IsNullEmpty()) return string.Empty;
            return HttpUtility.UrlDecode(str);
        }
        /// <summary>
        /// UrlDecode
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="encoding">����</param>
        /// <returns></returns>
        public static string UrlDecode(this string str, Encoding encoding) {
            if (str.IsNullEmpty()) return string.Empty;
            return HttpUtility.UrlDecode(str, encoding);
        }
        /// <summary>
        /// UrlEncodeUnicode��ͬ��JS��escape()
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string UrlEncodeUnicode(this string str) {
            return HttpUtility.UrlEncodeUnicode(str);
        }
        /// <summary>
        /// UrlPathEncode
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string UrlPathEncode(this string str) {
            return HttpUtility.UrlPathEncode(str);
        }
        /// <summary>
        /// HtmlEncode
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string HtmlEncode(this string str) {
            if (str.IsNullEmpty()) return string.Empty;
            return HttpUtility.HtmlEncode(str);
        }
        /// <summary>
        /// HtmlDecode
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string HtmlDecode(this string str) {
            if (str.IsNullEmpty()) return string.Empty;
            return HttpUtility.HtmlDecode(str);
        }
        /// <summary>
        /// �����ַ�������
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static int CnLength(this string str) {
            if (str.IsNullEmpty()) return 0;
            return Encoding.Default.GetBytes(str).Length;
        }
        /// <summary>
        /// ȡָ�����ȵ��ַ��� �����ַ�ռ2���ַ�����
        /// </summary>
        /// <param name="strInput">string��չ</param>
        /// <param name="len">����</param>
        /// <param name="flg">��׺</param>
        /// <returns></returns>
        public static string SubString(this string strInput, int len, string flg) {
            if (strInput.IsNullEmpty()) return string.Empty;
            string myResult = string.Empty;
            if (len >= 0) {
                byte[] bsSrcString = Encoding.Default.GetBytes(strInput);
                if (bsSrcString.Length > len) {
                    int nRealLength = len;
                    int[] anResultFlag = new int[len];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = 0; i < len; i++) {
                        if (bsSrcString[i] > 127) {
                            nFlag++;
                            if (nFlag == 3) nFlag = 1;
                        } else nFlag = 0;
                        anResultFlag[i] = nFlag;
                    }
                    if ((bsSrcString[len - 1] > 127) && (anResultFlag[len - 1] == 1))
                        nRealLength = len + 1;
                    bsResult = new byte[nRealLength];
                    Array.Copy(bsSrcString, bsResult, nRealLength);
                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + (len >= strInput.CnLength() ? "" : flg);
                } else myResult = strInput;
            }
            return myResult;
        }
        /// <summary>
        /// ȡ�ļ���չ�� ��.
        /// </summary>
        /// <param name="filename">string��չ</param>
        /// <returns></returns>
        public static string GetExtension(this string filename) {
            return System.IO.Path.GetExtension(filename);
        }
        /// <summary>
        /// �޸���չ�� ��.
        /// </summary>
        /// <param name="filename">string��չ</param>
        /// <param name="ext">��չ��</param>
        /// <returns></returns>
        public static string ChangeExtension(this string filename, string ext) {
            return System.IO.Path.ChangeExtension(filename, ext);
        }
        /// <summary>
        /// ȡ�ļ��� ����չ��
        /// </summary>
        /// <param name="filename">string��չ</param>
        /// <returns></returns>
        public static string GetFileName(this string filename) {
            return System.IO.Path.GetFileName(filename);
        }
        /// <summary>
        /// ȡ�ļ��� ����չ��
        /// </summary>
        /// <param name="filename">string��չ</param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(this string filename) {
            return System.IO.Path.GetFileNameWithoutExtension(filename);
        }
        /// <summary>
        /// ȡURL�е��ļ��� ����չ��
        /// </summary>
        /// <param name="url">string��չ</param>
        /// <returns></returns>
        public static string GetUrlFileName(this string url) {
            if (url.IsNullEmpty()) return string.Empty;
            string[] strs1 = url.Split(new char[] { '/' });
            return strs1[strs1.Length - 1].Split(new char[] { '?' })[0];
        }
        /// <summary>
        /// ȡ href="�е�����">
        /// </summary>
        /// <param name="HtmlCode">string��չ</param>
        /// <returns></returns>
        public static IList<string> GetHref(this string HtmlCode) {
            IList<string> MatchVale = new List<string>();
            if (HtmlCode.IsNullEmpty()) return MatchVale;
            string Reg = @"(h|H)(r|R)(e|E)(f|F) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)('|""| *|>)?";
            foreach (Match m in Regex.Matches(HtmlCode, Reg)) {
                MatchVale.Add((m.Value).ToLower().Replace("href=", "").Trim().TrimEnd('\'').TrimEnd('"').TrimStart('\'').TrimStart('"'));
            }
            return MatchVale;
        }
        /// <summary>
        /// ȡ src="�е�����">
        /// </summary>
        /// <param name="HtmlCode">string��չ</param>
        /// <returns></returns>
        public static IList<string> GetSrc(this string HtmlCode) {
            IList<string> MatchVale = new List<string>();
            if (HtmlCode.IsNullEmpty()) return MatchVale;
            string Reg = @"(s|S)(r|R)(c|C) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)('|""| *|>)?";
            foreach (Match m in Regex.Matches(HtmlCode, Reg)) {
                MatchVale.Add((m.Value).ToLower().Replace("src=", "").Trim().TrimEnd('\'').TrimEnd('"').TrimStart('\'').TrimStart('"'));
            }
            return MatchVale;
        }
        /// <summary>
        /// ȡEMAIL��ַ�е�@163.com ��@
        /// </summary>
        /// <param name="strEmail">string��չ</param>
        /// <returns></returns>
        public static string GetEmailHostName(this string strEmail) {
            if (strEmail.IsNullEmpty() || strEmail.IndexOf("@") < 0) return string.Empty;
            return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
        }
        /// <summary>
        /// �ַ���ת����
        /// </summary>
        /// <param name="DateTimeStr">string��չ</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string DateTimeStr) {
            if (DateTimeStr.IsNullEmpty()) return DateTime.Now;
            return DateTime.Parse(DateTimeStr);
        }
        /// <summary>
        /// �ַ���ת����
        /// </summary>
        /// <param name="fDateTime">string��չ</param>
        /// <param name="formatStr">��ʽ</param>
        /// <returns></returns>
        public static string ToDateTime(this string fDateTime, string formatStr) {
            DateTime s = fDateTime.ToDateTime();
            return s.ToString(formatStr);
        }
        /// <summary>
        /// �ַ���ת����
        /// </summary>
        /// <param name="DateTimeStr">string��չ</param>
        /// <param name="defDate">Ĭ��ֵ</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string DateTimeStr, DateTime defDate) {
            DateTime.TryParse(DateTimeStr, out defDate);
            return defDate;
        }
        /// <summary>
        /// �ַ���ת����
        /// </summary>
        /// <param name="DateTimeStr">string��չ</param>
        /// <param name="defDate">Ĭ��ֵ</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string DateTimeStr, DateTime? defDate) {
            DateTime dt = DateTime.Now;
            DateTime dt2 = dt;
            DateTime.TryParse(DateTimeStr, out dt);
            if (dt == dt2) return defDate;
            return dt;
        }
        /// <summary>
        /// �ַ���ת�ֽ�
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string value) {
            return value.ToBytes(null);
        }
        /// <summary>
        /// �ַ���ת�ֽ�
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string value, Encoding encoding) {
            if (value.IsNullEmpty()) return null;
            encoding = (encoding ?? Encoding.Default);
            return encoding.GetBytes(value);
        }
        /// <summary>
        /// �ַ���ת�ֽ� UTF8
        /// </summary>
        /// <param name="valueToExpand">string��չ</param>
        /// <returns></returns>
        public static byte[] ToUTF8Bytes(this string valueToExpand) {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(valueToExpand);
            return byteArray;
        }
        /// <summary>
        /// ɾ��HTML���
        /// </summary>
        /// <param name="HtmlCode">string��չ</param>
        /// <returns></returns>
        public static string RemoveHTML(this string HtmlCode) {
            if (HtmlCode.IsNullEmpty()) return string.Empty;
            string MatchVale = HtmlCode;
            MatchVale = new Regex("<br>", RegexOptions.IgnoreCase).Replace(MatchVale, "\n");
            foreach (Match s in Regex.Matches(HtmlCode, "<[^{><}]*>")) { MatchVale = MatchVale.Replace(s.Value, ""); }//"(<[^{><}]*>)"//@"<[\s\S-! ]*?>"//"<.+?>"//<(.*)>.*<\/\1>|<(.*) \/>//<[^>]+>//<(.|\n)+?>
            MatchVale = new Regex("\n", RegexOptions.IgnoreCase).Replace(MatchVale, "<br>");
            return MatchVale;
        }
        /// <summary>
        /// ɾ��HTML���
        /// </summary>
        /// <param name="content">string��չ</param>
        /// <returns></returns>
        public static string RemoveAllHTML(this string content) {
            if (content.IsNullEmpty()) return string.Empty;
            string pattern = "<[^>]*>";
            return Regex.Replace(content, pattern, string.Empty, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// ת��������
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string ToSChinese(this string str) {
            if (str.IsNullEmpty()) return string.Empty;
            return Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0);
        }
        /// <summary>
        /// ת��������
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string ToTChinese(this string str) {
            if (str.IsNullEmpty()) return string.Empty;
            return Strings.StrConv(str, VbStrConv.TraditionalChinese, 0);
        }
        /// <summary>
        /// ������ʾֵ
        /// </summary>
        /// <param name="ExprStr">string��չ</param>
        /// <returns></returns>
        public static double Evel(this string ExprStr) {
            if (ExprStr.IsNullEmpty()) return 0;
            Expr expression = new Expr("return " + ExprStr + ";");
            return expression.Compute(0);
        }
        /// <summary>
        /// �ַ�������
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <returns></returns>
        public static string Reverse(this string value) {
            if (value.IsNullEmpty()) return string.Empty;

            var chars = value.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
        /// <summary>
        /// �����໰
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="word">�磺(TMD|MB)</param>
        /// <returns></returns>
        public static string Filter(this string str, string word = "") {
            if (str.IsNullEmpty()) return string.Empty;
            string k = word.IsNullEmpty() ? BANWORD : word;
            str = new Regex(k, RegexOptions.IgnoreCase).Replace(str, "*");
            return str;
        }
        /// <summary>
        /// UBB
        /// </summary>
        /// <param name="chr">string��չ</param>
        /// <returns></returns>
        public static string UBB(this string chr) {
            if (chr.IsNullEmpty()) return string.Empty;
            chr = chr.HtmlEncode();
            chr = Regex.Replace(chr, @"<script(?<x>[^\>]*)>(?<y>[^\>]*)            \</script\>", @"&lt;script$1&gt;$2&lt;/script&gt;", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[url=(?<x>[^\]]*)\](?<y>[^\]]*)\[/url\]", @"<a href=$1  target=_blank>$2</a>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[url\](?<x>[^\]]*)\[/url\]", @"<a href=$1 target=_blank>$1</a>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[email=(?<x>[^\]]*)\](?<y>[^\]]*)\[/email\]", @"<a href=$1>$2</a>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[email\](?<x>[^\]]*)\[/email\]", @"<a href=$1>$1</a>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[flash](?<x>[^\]]*)\[/flash]", @"<OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=4,0,2,0 classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=500 height=400><PARAM NAME=movie VALUE=""$1""><PARAM NAME=quality VALUE=high><embed src=""$1"" quality=high pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' type='application/x-shockwave-flash' width=500 height=400>$1</embed></OBJECT>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[img](?<x>[^\]]*)\[/img]", @"<IMG SRC=""$1"" border=0>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[color=(?<x>[^\]]*)\](?<y>[^\]]*)\[/color\]", @"<font color=$1>$2</font>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[face=(?<x>[^\]]*)\](?<y>[^\]]*)\[/face\]", @"<font face=$1>$2</font>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[size=1\](?<x>[^\]]*)\[/size\]", @"<font size=1>$1</font>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[size=2\](?<x>[^\]]*)\[/size\]", @"<font size=2>$1</font>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[size=3\](?<x>[^\]]*)\[/size\]", @"<font size=3>$1</font>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[size=4\](?<x>[^\]]*)\[/size\]", @"<font size=4>$1</font>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[align=(?<x>[^\]]*)\](?<y>[^\]]*)\[/align\]", @"<align=$1>$2</align>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[fly](?<x>[^\]]*)\[/fly]", @"<marquee width=90% behavior=alternate scrollamount=3>$1</marquee>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[move](?<x>[^\]]*)\[/move]", @"<marquee scrollamount=3>$1</marquee>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[glow=(?<x>[^\]]*),(?<y>[^\]]*),(?<z>[^\]]*)\](?<w>[^\]]*)\[/glow\]", @"<table width=$1 style='filter:glow(color=$2, strength=$3)'>$4</table>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[shadow=(?<x>[^\]]*),(?<y>[^\]]*),(?<z>[^\]]*)\](?<w>[^\]]*)\[/shadow\]", @"<table width=$1 style='filter:shadow(color=$2, strength=$3)'>$4</table>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[b\](?<x>[^\]]*)\[/b\]", @"<b>$1</b>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[i\](?<x>[^\]]*)\[/i\]", @"<i>$1</i>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[u\](?<x>[^\]]*)\[/u\]", @"<u>$1</u>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[code\](?<x>[^\]]*)\[/code\]", @"<pre id=code><font size=1 face='Verdana, Arial' id=code>$1</font id=code></pre id=code>", RegexOptions.IgnoreCase);

            chr = Regex.Replace(chr, @"\[list\](?<x>[^\]]*)\[/list\]", @"<ul>$1</ul>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[list=1\](?<x>[^\]]*)\[/list\]", @"<ol type=1>$1</ol id=1>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[list=a\](?<x>[^\]]*)\[/list\]", @"<ol type=a>$1</ol id=a>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[\*\](?<x>[^\]]*)\[/\*\]", @"<li>$1</li>", RegexOptions.IgnoreCase);
            chr = Regex.Replace(chr, @"\[quote](?<x>.*)\[/quote]", @"<center>���� ���������� ����<table border='1' width='80%' cellpadding='10' cellspacing='0' ><tr><td>$1</td></tr></table></center>", RegexOptions.IgnoreCase);
            return (chr);
        }
        /// <summary>
        /// ClearUBB
        /// </summary>
        /// <param name="sDetail">string��չ</param>
        /// <returns></returns>
        public static string ClearUBB(this string sDetail) {
            return Regex.Replace(sDetail, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// �ַ�����ʽ��
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="args">ֵ</param>
        /// <returns></returns>
        public static string FormatWith(this string str, params object[] args) {
            return string.Format(str, args);
        }
        /// <summary>
        /// �ַ�����ʽ��
        /// </summary>
        /// <param name="text">string��չ</param>
        /// <param name="arg0">ֵ</param>
        /// <returns></returns>
        public static string FormatWith(this string text, object arg0) {
            return string.Format(text, arg0);
        }
        /// <summary>
        /// �ַ�����ʽ��
        /// </summary>
        /// <param name="text">string��չ</param>
        /// <param name="arg0">ֵ</param>
        /// <param name="arg1">ֵ</param>
        /// <returns></returns>
        public static string FormatWith(this string text, object arg0, object arg1) {
            return string.Format(text, arg0, arg1);
        }
        /// <summary>
        /// �ַ�����ʽ��
        /// </summary>
        /// <param name="text">string��չ</param>
        /// <param name="arg0">ֵ</param>
        /// <param name="arg1">ֵ</param>
        /// <param name="arg2">ֵ</param>
        /// <returns></returns>
        public static string FormatWith(this string text, object arg0, object arg1, object arg2) {
            return string.Format(text, arg0, arg1, arg2);
        }
        /// <summary>
        /// �ַ�����ʽ��
        /// </summary>
        /// <param name="text">string��չ</param>
        /// <param name="provider">IFormatProvider</param>
        /// <param name="args">ֵ</param>
        /// <returns></returns>
        public static string FormatWith(this string text, IFormatProvider provider, params object[] args) {
            return string.Format(provider, text, args);
        }
        /// <summary>
        /// �ַ����滻
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <param name="replaceValue">�滻ֵ</param>
        /// <returns></returns>
        public static string ReplaceWith(this string value, string regexPattern, string replaceValue) {
            if (value.IsNullEmpty()) return string.Empty;
            return ReplaceWith(value, regexPattern, replaceValue, RegexOptions.None);
        }
        /// <summary>
        /// �ַ����滻
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <param name="replaceValue">�滻ֵ</param>
        /// <param name="options">ѡ��</param>
        /// <returns></returns>
        public static string ReplaceWith(this string value, string regexPattern, string replaceValue, RegexOptions options) {
            if (value.IsNullEmpty()) return string.Empty;
            return Regex.Replace(value, regexPattern, replaceValue, options);
        }
        /// <summary>
        /// �ַ����滻
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <param name="evaluator">MatchEvaluator</param>
        /// <returns></returns>
        public static string ReplaceWith(this string value, string regexPattern, MatchEvaluator evaluator) {
            return ReplaceWith(value, regexPattern, RegexOptions.None, evaluator);
        }
        /// <summary>
        /// �ַ����滻
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <param name="options">ѡ��</param>
        /// <param name="evaluator">MatchEvaluator</param>
        /// <returns></returns>
        public static string ReplaceWith(this string value, string regexPattern, RegexOptions options, MatchEvaluator evaluator) {
            if (value.IsNullEmpty()) return string.Empty;
            return Regex.Replace(value, regexPattern, evaluator, options);
        }
        /// <summary>
        /// �ַ����滻
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <param name="ReplaceString">�滻</param>
        /// <param name="IsCaseInsensetive"></param>
        /// <returns></returns>
        public static string ReplaceWith(this string value, string regexPattern, string ReplaceString, bool IsCaseInsensetive) {
            if (value.IsNullEmpty()) return string.Empty;
            return Regex.Replace(value, regexPattern, ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
        /// <summary>
        /// �ַ����滻
        /// </summary>
        /// <param name="RegValue">string��չ</param>
        /// <param name="regStart">��ʼ</param>
        /// <param name="regEnd">����</param>
        /// <returns></returns>
        public static string Replace(this string RegValue, string regStart, string regEnd) {
            if (RegValue.IsNullEmpty()) return string.Empty;
            string s = RegValue;
            if (RegValue != "" && RegValue != null) {
                if (regStart != "" && regStart != null) { s = s.Replace(regStart, ""); }
                if (regEnd != "" && regEnd != null) { s = s.Replace(regEnd, ""); }
            }
            return s;
        }
        /// <summary>
        /// ����ȡֵ to MatchCollection
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <returns></returns>
        public static MatchCollection GetMatches(this string value, string regexPattern) {
            if (value.IsNullEmpty()) return null;
            return GetMatches(value, regexPattern, RegexOptions.None);
        }
        /// <summary>
        /// ����ȡֵ to MatchCollection
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <param name="options">ѡ��</param>
        /// <returns></returns>
        public static MatchCollection GetMatches(this string value, string regexPattern, RegexOptions options) {
            if (value.IsNullEmpty()) return null;
            return Regex.Matches(value, regexPattern, options);
        }
        /// <summary>
        /// ����ȡֵ to MatchCollection
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <param name="startString">��ʼ</param>
        /// <param name="endString">����</param>
        /// <returns></returns>
        public static MatchCollection FindBetween(this string s, string startString, string endString) {
            return s.FindBetween(startString, endString, true);
        }
        /// <summary>
        /// ����ȡֵ to MatchCollection
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <param name="startString">��ʼ</param>
        /// <param name="endString">����</param>
        /// <param name="recursive">�ݹ�</param>
        /// <returns></returns>
        public static MatchCollection FindBetween(this string s, string startString, string endString, bool recursive) {
            if (s.IsNullEmpty()) return null;
            MatchCollection matches;
            startString = Regex.Escape(startString);
            endString = Regex.Escape(endString);
            Regex regex = new Regex("(?<=" + startString + ").*(?=" + endString + ")");
            matches = regex.Matches(s);
            if (!recursive) return matches;
            if (matches.Count > 0) {
                if (matches[0].ToString().IndexOf(Regex.Unescape(startString)) > -1) {
                    s = matches[0].ToString() + Regex.Unescape(endString);
                    return s.FindBetween(Regex.Unescape(startString), Regex.Unescape(endString));
                } else {
                    return matches;
                }
            } else {
                return matches;
            }
        }
        /// <summary>
        /// ����ȡֵ to list
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <returns></returns>
        public static IEnumerable<string> GetMatchingValues(this string value, string regexPattern) {
            return GetMatchingValues(value, regexPattern, RegexOptions.None);
        }
        /// <summary>
        /// ����ȡֵ to list
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <param name="options">ѡ��</param>
        /// <returns></returns>
        public static IEnumerable<string> GetMatchingValues(this string value, string regexPattern, RegexOptions options) {
            foreach (Match match in GetMatches(value, regexPattern, options)) {
                if (match.Success) yield return match.Value;
            }
        }
        /// <summary>
        /// ����ȡֵ to list
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <param name="rep1">�滻1</param>
        /// <param name="rep2">�滻2</param>
        /// <returns></returns>
        public static IList<string> GetMatchingValues(this string value, string regexPattern, string rep1, string rep2) {
            IList<string> txtTextArr = new List<string>();
            if (value.IsNullEmpty()) return txtTextArr;
            string MatchVale = "";
            foreach (Match m in Regex.Matches(value, regexPattern)) {
                MatchVale = m.Value.Trim().Replace(rep1, "").Replace(rep2, "");
                txtTextArr.Add(MatchVale);
            }
            return txtTextArr;
        }
        /// <summary>
        /// �ָ��ַ���
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <param name="options">ѡ��</param>
        /// <returns></returns>
        public static string[] Split(this string value, string regexPattern, RegexOptions options) {
            if (value.IsNullEmpty()) return new string[] { };
            return Regex.Split(value, regexPattern, options);
        }
        /// <summary>
        /// �ָ��ַ���
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="regexPattern">����</param>
        /// <returns></returns>
        public static string[] Split(this string value, string regexPattern) {
            return value.Split(regexPattern, RegexOptions.None);
        }
        /// <summary>
        /// xml�ַ���ת XDocument/XmlDocument/XPathNavigator
        /// </summary>
        /// <param name="xml">string��չ</param>
        /// <returns></returns>
        public static XDocument ToXDocument(this string xml) {
            return XDocument.Parse(xml);
        }
        /// <summary>
        /// xml�ַ���ת XmlDocument
        /// </summary>
        /// <param name="xml">string��չ</param>
        /// <returns></returns>
        public static XmlDocument ToXmlDOM(this string xml) {
            var document = new XmlDocument();
            document.LoadXml(xml);
            return document;
        }
        /// <summary>
        /// xml�ַ���ת XPathNavigator
        /// </summary>
        /// <param name="xml">string��չ</param>
        /// <returns></returns>
        public static XPathNavigator ToXPath(this string xml) {
            var document = new XPathDocument(new StringReader(xml));
            return document.CreateNavigator();
        }
        /// <summary>
        /// תƴ��
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <returns></returns>
        public static string ToPinyin(this string s) {
            if (s.IsNullEmpty()) return string.Empty;
            return PinYin.Instance().Search(s).ToLower();
        }
        /// <summary>
        /// תƴ������ĸ
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <returns></returns>
        public static string ToPinyinChar(this string s) {
            if (s.IsNullEmpty()) return string.Empty;
            string strVal = PinYin.Instance().SearchCap(s);
            if (strVal.ToLower() == strVal.ToUpper()) return "*"; else return strVal.ToLower();
        }
        /// <summary>
        /// תƴ������ĸ
        /// </summary>
        /// <param name="c">string��չ</param>
        /// <returns></returns>
        public static string ToPinyinChar2(this string c) {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "g";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";
            return "*";
        }
        /// <summary>
        /// ����ַ���
        /// </summary>
        /// <param name="string">string��չ</param>
        /// <param name="length">��</param>
        /// <returns></returns>
        public static string Left(this string @string, int length) {
            if (length <= 0 || @string.Length == 0) return string.Empty;
            if (@string.Length <= length) return @string;
            return @string.Substring(0, length);
        }
        /// <summary>
        /// �ҽ��ַ���
        /// </summary>
        /// <param name="string">string��չ</param>
        /// <param name="length">��</param>
        /// <returns></returns>
        public static string Right(this string @string, int length) {
            if (length <= 0 || @string.Length == 0) return string.Empty;
            if (@string.Length <= length) return @string;
            return @string.Substring(@string.Length - length, length);
        }
        /// <summary>
        /// Json�ط��ַ����ˣ��μ�http://www.json.org/
        /// </summary>
        /// <param name="sourceStr">Ҫ���˵�Դ�ַ���</param>
        /// <returns>���ع��˵��ַ���</returns>
        public static string JsonFilter(this string sourceStr) {
            sourceStr = sourceStr.Replace("\\", "\\\\");
            sourceStr = sourceStr.Replace("\b", "\\\b");
            sourceStr = sourceStr.Replace("\t", "\\\t");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\f", "\\\f");
            sourceStr = sourceStr.Replace("\r", "\\\r");
            return sourceStr.Replace("\"", "\\\"");
        }
        /// <summary>
        /// תö������ �� public enum test { test1, test2 } Msg.WriteEnd("0".ToEnum&lt;test>()); ֵ��test1
        /// </summary>
        /// <example>
        /// <code>
        /// public enum test { test1, test2 } 
        /// Msg.WriteEnd("0".ToEnum&lt;test>()); //ֵ��test1
        /// </code>
        /// </example>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value">string��չ</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value) {
            return ToEnum<T>(value, false);
        }
        /// <summary>
        /// תö������
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value">string��չ</param>
        /// <param name="ignorecase">��Сд</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value, bool ignorecase) {
            if (value == null) throw new ArgumentNullException("value");
            value = value.Trim();
            if (value.Length == 0) throw new ArgumentNullException("Must specify valid information for parsing in the string.", "value");
            Type t = typeof(T);
            if (!t.IsEnum) throw new ArgumentException("Type provided must be an Enum.", "T");
            return (T)Enum.Parse(t, value, ignorecase);
        }
        /// <summary>
        /// תö������
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value">string��չ</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static T ToEnum<T>(string value, T defaultValue) where T : struct, IConvertible { if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type"); if (string.IsNullOrEmpty(value)) return defaultValue; foreach (T item in Enum.GetValues(typeof(T))) { if (item.ToString().ToLower().Equals(value.Trim().ToLower())) return item; } return defaultValue; }
        /// <summary>
        /// �ַ���������
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="character">�ַ�</param>
        /// <returns></returns>
        public static int CharacterCount(this string value, char character) {
            int intReturnValue = 0;
            if (value.IsNullEmpty()) return 0;
            for (int intCharacter = 0; intCharacter <= (value.Length - 1); intCharacter++) {
                if (value.Substring(intCharacter, 1) == character.ToString()) intReturnValue += 1;
            }

            return intReturnValue;
        }
        /// <summary>
        /// ��ǰ׺
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <param name="prefix">ǰ׺</param>
        /// <returns></returns>
        public static string ForcePrefix(this string s, string prefix) {
            if (s.IsNullEmpty()) return string.Empty;
            string result = s;
            if (!result.StartsWith(prefix)) result = prefix + result;
            return result;
        }
        /// <summary>
        /// �Ӻ�׺
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <param name="suffix">��׺</param>
        /// <returns></returns>
        public static string ForceSuffix(this string s, string suffix) {
            if (s.IsNullEmpty()) return string.Empty;
            string result = s;
            if (!result.EndsWith(suffix)) result += suffix;
            return result;
        }
        /// <summary>
        /// ɾ��ǰ׺
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <param name="prefix">ǰ׺</param>
        /// <returns></returns>
        public static string RemovePrefix(this string s, string prefix) {
            if (s.IsNullEmpty()) return string.Empty;
            return Regex.Replace(s, "^" + prefix, System.String.Empty, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// ɾ����׺
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <param name="suffix">��׺</param>
        /// <returns></returns>
        public static string RemoveSuffix(this string s, string suffix) {
            if (s.IsNullEmpty()) return string.Empty;
            return Regex.Replace(s, suffix + "$", System.String.Empty, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// ���ұ߲��ַ���
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <param name="pad">����</param>
        /// <returns></returns>
        public static string PadLeft(this string s, string pad) {
            return s.PadLeft(pad, s.Length + pad.Length, false);
        }
        /// <summary>
        /// ���ұ߲��ַ���
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <param name="pad">����</param>
        /// <param name="totalWidth">��</param>
        /// <param name="cutOff">�Ƿ����</param>
        /// <returns></returns>
        public static string PadLeft(this string s, string pad, int totalWidth, bool cutOff) {
            if (s.IsNullEmpty()) return string.Empty;
            if (s.Length >= totalWidth) return s;
            int padCount = pad.Length;
            string paddedString = s;
            while (paddedString.Length < totalWidth) paddedString += pad;
            if (cutOff) paddedString = paddedString.Substring(0, totalWidth);
            return paddedString;
        }
        /// <summary>
        /// ����߲��ַ���
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <param name="pad">����</param>
        /// <returns></returns>
        public static string PadRight(this string s, string pad) {
            return PadRight(s, pad, s.Length + pad.Length, false);
        }
        /// <summary>
        /// ����߲��ַ���
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <param name="pad">����</param>
        /// <param name="length">��</param>
        /// <param name="cutOff">�Ƿ����</param>
        /// <returns></returns>
        public static string PadRight(this string s, string pad, int length, bool cutOff) {
            if (s.IsNullEmpty()) return string.Empty;
            if (s.Length >= length) return s;
            string paddedString = string.Empty;
            while (paddedString.Length < length - s.Length) paddedString += pad;
            if (cutOff) paddedString = paddedString.Substring(0, length - s.Length);
            paddedString += s;
            return paddedString;
        }
        /// <summary>
        /// �ַ���ת��ɫ ��"ffffff".ToColor()
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <returns></returns>
        public static Color ToColor(this string s) {
            if (s.IsNullEmpty()) return new Color() { };
            s = s.Replace("#", string.Empty);
            byte a = System.Convert.ToByte("ff", 16);
            byte pos = 0;
            if (s.Length == 8) {
                a = System.Convert.ToByte(s.Substring(pos, 2), 16);
                pos = 2;
            }
            byte r = System.Convert.ToByte(s.Substring(pos, 2), 16);
            pos += 2;
            byte g = System.Convert.ToByte(s.Substring(pos, 2), 16);
            pos += 2;
            byte b = System.Convert.ToByte(s.Substring(pos, 2), 16);
            return Color.FromArgb(a, r, g, b);
        }
        /// <summary>
        /// �ַ����������Ƿ���� value
        /// </summary>
        /// <param name="value">string��չ</param>
        /// <param name="keywords">�ַ���</param>
        /// <returns></returns>
        public static bool ContainsArray(this string value, params string[] keywords) {
            return keywords.All((s) => value.Contains(s));
        }
        /// <summary>
        /// �ַ���תT���͵�NULL �� "123".ToNullable&lt;long>() ��Ϊnull
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="s">string��չ</param>
        /// <returns></returns>
        public static Nullable<T> ToNullable<T>(this string s) where T : struct {
            T? result = null;
            if (!s.Trim().IsNullEmpty()) {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T?));
                result = (T?)converter.ConvertFrom(s);
            }
            return result;
        }
        /// <summary>
        /// �ַ���תT���͵�NULL �� 0.ToNullable&lt;long>() ��Ϊnull
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? ToNullable<T>(this T value) where T : struct {
            return (value.IsDefault<T>() ? null : (T?)value);
        }
        /// <summary>
        /// ���зָ��ַ���
        /// </summary>
        /// <param name="text">string��չ</param>
        /// <returns></returns>
        public static List<string> GetLines(this string text) {
            return text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }
        /// <summary>
        /// ����ƥ���
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="op">����</param>
        /// <returns></returns>
        public static bool IsMatch(this string str, string op) {
            if (str.IsNullEmpty()) return false;
            Regex re = new Regex(op, RegexOptions.IgnoreCase);
            return re.IsMatch(str);
        }
        /// <summary>
        /// IP��
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns></returns>
        public static bool IsIP(this string input) {
            return input.IsMatch(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"); //@"^(([01]?[\d]{1,2})|(2[0-4][\d])|(25[0-5]))(\.(([01]?[\d]{1,2})|(2[0-4][\d])|(25[0-5]))){3}$";
        }
        /// <summary>
        /// IP6��
        /// </summary>
        /// <param name="ip">string��չ</param>
        /// <returns></returns>
        public static bool IsIPSect(this string ip) {
            return ip.IsMatch(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }
        /// <summary>
        /// IsNumber
        /// </summary>
        /// <param name="strNumber">string��չ</param>
        /// <returns></returns>
        public static bool IsNumber(this string strNumber) {
            string pet = @"^([0-9])[0-9]*(\.\w*)?$"; //^-[0-9]+$|^[0-9]+$
            return strNumber.IsMatch(pet);
        }
        /// <summary>
        /// IsDouble
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns></returns>
        public static bool IsDouble(this string input) {
            string pet = @"^[0-9]*[1-9][0-9]*$";//@"^\d{1,}$"//����У�鳣��//@"^-?(0|\d+)(\.\d+)?$"//��ֵУ�鳣�� 
            return input.IsMatch(pet);
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns></returns>
        public static bool IsInt(this string input) {
            string pet = @"^[0-9]*$"; //@"^([0-9])[0-9]*(\.\w*)?$";
            return input.IsMatch(pet);
        }
        /// <summary>
        /// ������ȫ�����ַ�
        /// </summary>
        /// <param name="strNumber">string��չ</param>
        /// <returns></returns>
        public static bool IsNumberArray(this string[] strNumber) {
            if (strNumber == null) return false;
            if (strNumber.Length < 1) return false;
            foreach (string id in strNumber)
                if (!id.IsNumber()) return false;
            return true;
        }
        /// <summary>
        /// �����
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns></returns>
        public static bool IsEmail(this string input) {
            string pet = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";//@"^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$";
            return input.IsMatch(pet);
        }
        /// <summary>
        /// URL��
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns></returns>
        public static bool IsUrl(this string input) {
            string pet = @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$";//@"^http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            return input.IsMatch(pet);
        }
        /// <summary>
        /// �ʱ��
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns></returns>
        public static bool IsZip(this string input) {
            return input.IsMatch(@"\d{6}");
        }
        /// <summary>
        /// ��ȫSQL�ַ�����
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static bool IsSafeSqlStr(this string str) {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        /// <summary>
        /// ����ʱ��� ����
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns></returns>
        public static bool IsDateTime(this string input) {
            //string pet = @"^(?:(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(\/|-|\.)(?:0?2\1(?:29))$)|(?:(?:1[6-9]|[2-9]\d)?\d{2})(\/|-|\.)(?:(?:(?:0?[13578]|1[02])\2(?:31))|(?:(?:0?[1,3-9]|1[0-2])\2(29|30))|(?:(?:0?[1-9])|(?:1[0-2]))\2(?:0?[1-9]|1\d|2[0-8]))$";
            string pet = @"^(?=\d)(?:(?!(?:1582(?:\.|-|\/)10(?:\.|-|\/)(?:0?[5-9]|1[0-4]))|(?:1752(?:\.|-|\/)0?9(?:\.|-|\/)(?:0?[3-9]|1[0-3])))(?=(?:(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:\d\d)(?:[02468][048]|[13579][26]))\D0?2\D29)|(?:\d{4}\D(?!(?:0?[2469]|11)\D31)(?!0?2(?:\.|-|\/)(?:29|30))))(\d{4})([-\/.])(0?\d|1[012])\2((?!00)[012]?\d|3[01])(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]?\d|2[0-3])(?::[0-5]\d){1,2})?$";
            return input.IsMatch(pet);
        }
        /// <summary>
        /// ����ʱ��� try catch
        /// </summary>
        /// <param name="DateTimeStr">string��չ</param>
        /// <returns></returns>
        public static bool IsDateTime2(this string DateTimeStr) {
            try { DateTime _dt = DateTime.Parse(DateTimeStr); return true; } catch { return false; }
        }
        /// <summary>
        /// ���ڷ� try catch
        /// </summary>
        /// <param name="DateStr">string��չ</param>
        /// <returns></returns>
        public static bool IsDate(this string DateStr) {
            try { DateTime _dt = DateTime.Parse(DateStr); return true; } catch { return false; }
        }
        /// <summary>
        /// ʱ���
        /// </summary>
        /// <param name="TimeStr">string��չ</param>
        /// <returns></returns>
        public static bool IsTime(this string TimeStr) {
            return TimeStr.IsMatch(@"^([0-1]\\d|2[0-3]):[0-5]\\d:[0-5]\\d$");//^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$
        }
        /// <summary>
        /// ��һ����ĸ�Ƿ�a-zA-Z0-9
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns></returns>
        public static bool IsAlpha(this string input) {
            return input.IsMatch(@"[^a-zA-Z0-9]");
        }
        /// <summary>
        /// �绰��
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns>�绰��</returns>
        public static bool IsTelepone(this string input) {
            return input.IsMatch(@"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$");//��"^(\(\d{3,4}-)|\d{3.4}-)?\d{7,8}$
        }
        /// <summary>
        /// �ֻ��ŷ�
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns>�ֻ��ŷ�</returns>
        public static bool IsMobile(this string input) {
            return input.IsMatch(@"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$");
        }
        /// <summary>
        /// ǿ�����
        /// </summary>
        /// <param name="password">string��չ</param>
        /// <returns>ǿ�����</returns>
        public static bool IsStrongPassword(this string password) {
            return Regex.IsMatch(password, @"(?=^.{8,255}$)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*");
        }
        /// <summary>
        /// �ַ����Ƿ���,�ָ����ַ�����
        /// </summary>
        /// <param name="stringarray">string��չ</param>
        /// <param name="str">�ַ���</param>
        /// <returns></returns>
        public static bool IsInArray(this string stringarray, string str) {
            return stringarray.Split(",").IsInArray(str, false);
        }
        /// <summary>
        /// �ַ����Ƿ���,�ָ����ַ�����
        /// </summary>
        /// <param name="stringarray">string��չ</param>
        /// <param name="str">�ַ���</param>
        /// <param name="strsplit">�ָ��</param>
        /// <returns></returns>
        public static bool IsInArray(this string stringarray, string str, string strsplit) {
            return stringarray.Split(strsplit).IsInArray(str, false);
        }
        /// <summary>
        /// �ַ����Ƿ���,�ָ����ַ�����
        /// </summary>
        /// <param name="stringarray">string��չ</param>
        /// <param name="str">�ַ���</param>
        /// <param name="strsplit">�ָ��</param>
        /// <param name="caseInsensetive">���ִ�Сд</param>
        /// <returns></returns>
        public static bool IsInArray(this string stringarray, string str, string strsplit, bool caseInsensetive) {
            return stringarray.Split(strsplit).IsInArray(str, caseInsensetive);
        }
        /// <summary>
        ///  BASE64��
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static bool IsBase64(this string str) {
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        /// <summary>
        /// ��� >=1900 and &lt;=9999
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns></returns>
        public static bool IsYear(this string input) {
            int year = input.ToInt();
            return year >= 1900 && year <= 9999;
        }
        /// <summary>
        /// ͼƬ�� jpg jpeg png bmp gif
        /// </summary>
        /// <param name="filename">string��չ</param>
        /// <returns></returns>
        public static bool IsImgFileName(this string filename) {
            filename = filename.Trim();
            if (filename.EndsWith(".") || (filename.IndexOf(".") == -1)) return false;
            string str = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            if (((str != "jpg") && (str != "jpeg")) && ((str != "png") && (str != "bmp"))) return (str == "gif");
            return true;
        }
        /// <summary>
        /// �ж��Ƿ�IMG�ļ�
        /// </summary>
        /// <param name="filename">string��չ</param>
        /// <returns></returns>
        public static bool IsImgFile(this string filename) {
            if (!FileDirectory.FileExists(filename)) return false;

            ushort code = BitConverter.ToUInt16(File.ReadAllBytes(filename), 0);
            switch (code) {
                case 0x4D42://bmp
                    return true;
                case 0xD8FF://JPEG   
                    return true;
                case 0x4947://GIF   
                    return true;
                case 0x050A://PCX   
                    return true;
                case 0x5089://PNG   
                    return true;
                case 0x4238://PSD   
                    return true;
                case 0xA659://RAS   
                    return true;
                case 0xDA01://SGI   
                    return true;
                case 0x4949://TIFF
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// GUID��
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <returns></returns>
        public static bool IsGuid(this string s) {
            if (s.IsNullEmpty()) return false;
            Regex format = new Regex("^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2},{0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
            Match match = format.Match(s);
            return match.Success;
        }
        /// <summary>
        /// ���֤��
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <returns></returns>
        public static bool IsCreditCard(this string s) {
            return new Regex(@"^(\d{14}|\d{17})(\d|[xX])$").IsMatch(s);
        }
        /// <summary>
        /// �ж��Ƿ�������
        /// </summary>
        /// <param name="s">string��չ</param>
        /// <returns></returns>
        public static bool IsCNStr(this string s) {
            string[] stringMatchs = new string[] {
                @"[\u3040-\u318f]+",
                @"[\u3300-\u337f]+",
                @"[\u3400-\u3d2d]+",
                @"[\u4e00-\u9fff]+",
                @"[\u4e00-\u9fa5]+",
                @"[\uf900-\ufaff]+"
            };
            foreach (string stringMatch in stringMatchs)
                if (Regex.IsMatch(s, stringMatch))
                    return true;
            return false;
        }
        /// <summary>
        /// IsColor
        /// </summary>
        /// <param name="color">string��չ</param>
        /// <returns></returns>
        public static bool IsColor(this string color) {
            if (color.IsNullEmpty()) return false;
            color = color.Trim().Trim('#');
            if (color.Length != 3 && color.Length != 6) return false;
            //������0-9  a-f������ַ�
            if (!Regex.IsMatch(color, "[^0-9a-f]", RegexOptions.IgnoreCase)) return true;
            return false;
        }
        /// <summary>
        /// �ж�(E�� ���� �»���)
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static bool IsUserName(this string str) {
            return new Regex("^[a-zA-Z\\d_]+$", RegexOptions.Compiled).IsMatch(str);
        }
        /// <summary>
        /// �ж�(E�� ���� ���� �»���)
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static bool IsNickName(this string str) {
            return new Regex(@"^[a-zA-Z\u4e00-\u9fa5\d_]+$", RegexOptions.Compiled).IsMatch(str);
        }
        /// <summary>
        /// �ж�����(������/\&lt;>{}:*?|")
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static bool IsGroupName(this string str) {
            return new Regex(@"^[^\/""{}<>:?*|]+$", RegexOptions.Compiled).IsMatch(str);
        }
        /// <summary>
        /// IsAscii > 127
        /// </summary>
        /// <param name="data">string��չ</param>
        /// <returns></returns>
        public static bool IsAscii(this string data) {
            if ((data == null) || (data.Length == 0)) return true;
            foreach (char c in data) {
                if ((int)c > 127) return false;
            }
            return true;
        }
        /// <summary>
        /// IsBinary
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static bool IsBinary(this string str) {
            for (int i = 0; i < str.Length; i++) {
                if (str[i] > '\x00ff') {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// ȡ����·��
        /// </summary>
        /// <param name="strPath">string��չ</param>
        /// <returns></returns>
        public static string GetMapPath(this string strPath) {
            if (HttpContext.Current != null)
                return HttpContext.Current.Server.MapPath(strPath);
            else
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }
        /// <summary>
        /// ȡ����·�� ��Ҫ����Global�ļ���
        /// </summary>
        /// <param name="strPath">string��չ</param>
        /// <returns></returns>
        public static string GetGlobalMapPath(this string strPath) {
            return System.Web.Hosting.HostingEnvironment.MapPath(strPath);
        }
        /// <summary>
        /// ToGUID
        /// </summary>
        /// <param name="target">string��չ</param>
        /// <returns></returns>
        public static Guid ToGuid(this string target) {
            if (target.IsGuid()) return new Guid(target);
            return Guid.Empty;
        }
        /// <summary>
        /// ToGUID
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Guid ToUniqueIdentifier(this string target) {
            return target.ToGuid();
        }
        /// <summary>
        /// "ture"/"1" תΪ true
        /// </summary>
        /// <param name="source">string��չ</param>
        /// <returns></returns>
        public static bool True(this string source) { return string.Compare(source, "true", true) == 0 || string.Compare(source, "1", true) == 0; }
        /// <summary>
        /// "false"/"0" תΪ false
        /// </summary>
        /// <param name="source">string��չ</param>
        /// <returns></returns>
        public static bool False(this string source) { return string.Compare(source, "false", true) == 0 || string.Compare(source, "0", true) == 0; }
        /// <summary>
        /// ���ַ���תT�������� ��"123".As&lt;int>()
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="source">string��չ</param>
        /// <returns></returns>
        public static T As<T>(this string source) {
            if (source == null) return default(T);
            try {
                return (T)Convert.ChangeType(source, typeof(T));
            } catch {
                return default(T);
            }
        }
        /// <summary>
        /// json ת���� ToJson ������
        /// </summary>
        /// <example>
        /// <code>
        /// "{\"MemberID\":1, \"RealName\"}".FormJson&lt;UC_Member>();
        /// </code>
        /// </example>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="json">string��չ</param>
        /// <returns></returns>
        public static T FromJson<T>(this string json) {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Deserialize<T>(json);
        }
        /// <summary>
        /// soapXml ת���� ToSoap������
        /// </summary>
        /// <param name="soapXml">string��չ</param>
        /// <returns></returns>
        public static object FromSoap(this string soapXml) {
            object obj = null;
            using (MemoryStream ms = new MemoryStream((new System.Text.ASCIIEncoding()).GetBytes(soapXml))) {
                ms.Seek(0, SeekOrigin.Begin);
                SoapFormatter sf = new SoapFormatter(null, new StreamingContext(StreamingContextStates.Persistence));
                obj = sf.Deserialize(ms);
            }
            return obj;
        }
        /// <summary>
        /// XMLת���� ToXML������
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="serializedObject">string��չ</param>
        /// <returns></returns>
        public static T FromXml<T>(this string serializedObject) {
            XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add("", "");
            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (MemoryStream memoryStream = new MemoryStream(serializedObject.ToUTF8Bytes())) {
                return (T)xs.Deserialize(memoryStream);
            }
        }
        /// <summary>
        /// �ַ���ת�� \n \r \a \b \t \" \' \\ \u***
        /// </summary>
        /// <param name="text">string��չ</param>
        /// <returns></returns>
        public static string ToOriginal(this string text) {
            StringBuilder sb = new StringBuilder();
            char[] chars = text.ToCharArray();
            foreach (char c in chars) {
                switch ((ushort)c) {
                    case 10:
                        sb.Append("\\n");
                        continue;
                    case 13:
                        sb.Append("\\r");
                        continue;
                    case 7:
                        sb.Append("\\a");
                        continue;
                    case 8:
                        sb.Append("\\b");
                        continue;
                    case 9:
                        sb.Append("\\t");
                        continue;
                    case 0x0b:
                        sb.Append("\\b");
                        continue;
                    case 34:
                        sb.Append("\\\"");
                        continue;
                    case 39:
                        sb.Append("\\\'");
                        continue;
                    case 92:
                        sb.Append("\\\\");
                        continue;
                }
                if ((ushort)c < 32 || (ushort)c == 127) {
                    sb.Append("\\u" + ((ushort)c).ToString("x4"));
                } else {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// ����ĸ��С
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string UpperFirstChar(this string str) {
            if (str.IsNullEmpty()) return string.Empty;
            if (str.Length == 1) return str.ToUpper();
            return str.ToUpper().Substring(0, 1) + str.Substring(1, str.Length - 1);
        }
        /// <summary>
        /// ����ĸ��д ����Ǵ�дǰ��_
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string UpperFirstChar2(this string str) {
            if (str.IsNullEmpty()) return string.Empty;
            string f = str.Substring(0, 1).ToUpper();
            if (f == str.Substring(0, 1)) return "_" + str;
            if (str.Length == 1) return str.ToUpper();
            return f + str.Substring(1, str.Length - 1);
        }
        /// <summary>
        /// ����ÿСд
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string LowerFirstChar(this string str) {
            if (str.IsNullEmpty()) return string.Empty;
            if (str.Length == 1) return str.ToLower();
            return str.ToLower().Substring(0, 1) + str.Substring(1, str.Length - 1);
        }
        /// <summary>
        /// ����ĸСд �����Сдǰ��_
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string LowerFirstChar2(this string str) {
            if (str.IsNullEmpty()) return string.Empty;
            string f = str.Substring(0, 1).ToLower();
            if (f == str.Substring(0, 1)) return "_" + str;
            if (str.Length == 1) return str.ToLower();
            return f + str.Substring(1, str.Length - 1);
        }
        /// <summary>
        /// ����ַ���ID�Ƿ�Ϸ�������0����ȥ��С�ڵ���0��ID
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="c">�ָ��</param>
        /// <param name="isDelZero">�Ƿ�ȥ��С�ڵ���0��ID</param>
        /// <returns></returns>
        public static string FormatToID(this string str, char c = ',', bool isDelZero = true) {
            string newstr = string.Empty;
            if (isDelZero) {
                foreach (string s in str.Split(c)) {
                    long id = s.Trim().ToBigInt();
                    if (id <= 0) continue;
                    newstr += id.ToString() + c.ToString();
                }
            } else {
                foreach (string s in str.Split(c)) {
                    long id = s.Trim().ToBigInt();
                    newstr += id <= 0 ? "0" : id.ToString() + c.ToString();
                }
            }
            if (newstr.Length > 0) newstr = newstr.Remove(newstr.Length - 1, 1);
            return newstr;
        }
        /// <summary>
        /// ����ַ���ID�Ƿ�Ϸ�������0����ȥ��С�ڵ���0��ID
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="c">�ָ��</param>
        /// <returns></returns>
        public static string FormatToID(this string str, char c = ',') {
            return FormatToID(str, c, true);
        }
        /// <summary>
        /// ����ַ���ID�Ƿ�Ϸ�������0����ȥ��С�ڵ���0��ID
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="c">�ָ��</param>
        /// <param name="isDelZero">�Ƿ�ȥ��С�ڵ���0��ID</param>
        /// <returns></returns>
        public static string FormatToID(this string str, string c = ",", bool isDelZero = true) {
            string newstr = string.Empty;
            if (isDelZero) {
                foreach (string s in str.Split(c)) {
                    long id = s.Trim().ToBigInt();
                    if (id <= 0) continue;
                    newstr += id.ToString() + c.ToString();
                }
            } else {
                foreach (string s in str.Split(c)) {
                    long id = s.Trim().ToBigInt();
                    newstr += id <= 0 ? "0" : id.ToString() + c.ToString();
                }
            }
            if (newstr.Length > 0) newstr = newstr.Remove(newstr.Length - 1, 1);
            return newstr;
        }
        /// <summary>
        /// ����ַ���ID�Ƿ�Ϸ�������0����ȥ��С�ڵ���0��ID
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="c">�ָ��</param>
        /// <returns></returns>
        public static string FormatToID(this string str, string c = ",") {
            return FormatToID(str, c, true);
        }
        /// <summary>
        /// ����ַ���ID�Ƿ�Ϸ�������0����ȥ��С�ڵ���0��ID
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="isDelZero">�Ƿ�ȥ��С�ڵ���0��ID</param>
        /// <returns></returns>
        public static string FormatToID(this string str, bool isDelZero = true) {
            return FormatToID(str, ',', isDelZero);
        }
        /// <summary>
        /// ����ַ���ID�Ƿ�Ϸ�������0����ȥ��С�ڵ���0��ID
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns></returns>
        public static string FormatToID(this string str) {
            return FormatToID(str, ',', true);
        }
        /// <summary>
        /// ��ȫSQL
        /// </summary>
        /// <param name="obj">string��չ</param>
        /// <returns></returns>
        static public string SafeSql(this string obj) {
            string str = obj.ToString();
            str = str.IsNullEmpty() ? "" : str.Replace("'", "''");
            str = new Regex("exec", RegexOptions.IgnoreCase).Replace(str, "&#101;xec");
            str = new Regex("xp_cmdshell", RegexOptions.IgnoreCase).Replace(str, "&#120;p_cmdshell");
            str = new Regex("select", RegexOptions.IgnoreCase).Replace(str, "&#115;elect");
            str = new Regex("insert", RegexOptions.IgnoreCase).Replace(str, "&#105;nsert");
            str = new Regex("update", RegexOptions.IgnoreCase).Replace(str, "&#117;pdate");
            str = new Regex("delete", RegexOptions.IgnoreCase).Replace(str, "&#100;elete");

            str = new Regex("drop", RegexOptions.IgnoreCase).Replace(str, "&#100;rop");
            str = new Regex("create", RegexOptions.IgnoreCase).Replace(str, "&#99;reate");
            str = new Regex("rename", RegexOptions.IgnoreCase).Replace(str, "&#114;ename");
            str = new Regex("truncate", RegexOptions.IgnoreCase).Replace(str, "&#116;runcate");
            str = new Regex("alter", RegexOptions.IgnoreCase).Replace(str, "&#97;lter");
            str = new Regex("exists", RegexOptions.IgnoreCase).Replace(str, "&#101;xists");
            str = new Regex("master.", RegexOptions.IgnoreCase).Replace(str, "&#109;aster.");
            str = new Regex("restore", RegexOptions.IgnoreCase).Replace(str, "&#114;estore");
            return str;
        }
        /// <summary>
        /// ��ȫSQL '
        /// </summary>
        /// <param name="obj">string��չ</param>
        /// <returns></returns>
        static public string SafeSqlSimple(this string obj) {
            string str = obj.ToString();
            str = str.IsNullEmpty() ? "" : str.Replace("'", "''");
            return str;
        }
        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ToInt(this string strValue, int defValue) { int def = 0; int.TryParse(strValue, out def); return def == 0 ? defValue : def; }
        /// <summary>
        /// ToTinyInt
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static byte ToTinyInt(this string strValue, byte defValue) { byte def = 0; byte.TryParse(strValue, out def); return def == 0 ? defValue : def; }
        /// <summary>
        /// ToSmallInt
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static short ToSmallInt(this string strValue, short defValue) { short def = 0; short.TryParse(strValue, out def); return def == 0 ? defValue : def; }
        /// <summary>
        /// ToDecimal
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string strValue, decimal defValue) { decimal def = 0; decimal.TryParse(strValue, out def); return def == 0 ? defValue : def; }
        /// <summary>
        /// ToFloat
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string strValue, float defValue) { float def = 0; float.TryParse(strValue, out def); return def == 0 ? defValue : def; }
        /// <summary>
        /// ToBigInt
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static Int64 ToBigInt(this string strValue, Int64 defValue) { Int64 def = 0; Int64.TryParse(strValue, out def); return def == 0 ? defValue : def; }
        /// <summary>
        /// ToMoney
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static decimal ToMoney(this string strValue, decimal defValue) { decimal def = 0; decimal.TryParse(strValue, out def); return def == 0 ? defValue : def; }
        /// <summary>
        /// ToInteger
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ToInteger(this string strValue, int defValue) { int def = 0; int.TryParse(strValue, out def); return def == 0 ? defValue : def; }
        /// <summary>
        /// ToBool
        /// </summary>
        /// <param name="Expression">string��չ</param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static bool ToBool(this string Expression, bool defValue) {
            if (!Expression.IsNullEmpty()) {
                if (string.Compare(Expression, "true", true) == 0) return true;
                if (string.Compare(Expression, "false", true) == 0) return false;
                if (string.Compare(Expression, "1", true) == 0) return true;
                if (string.Compare(Expression, "0", true) == 0) return false;
            }
            return defValue;
        }
        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <returns></returns>
        public static int ToInt(this string strValue) { return strValue.ToInt(0); }
        /// <summary>
        /// ToTinyInt
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <returns></returns>
        public static byte ToTinyInt(this string strValue) { return strValue.ToTinyInt(0); }
        /// <summary>
        /// ToSmallInt
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <returns></returns>
        public static short ToSmallInt(this string strValue) { return strValue.ToSmallInt(0); }
        /// <summary>
        /// ToDecimal
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string strValue) { return strValue.ToDecimal(0); }
        /// <summary>
        /// ToFloat
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <returns></returns>
        public static float ToFloat(this string strValue) { return strValue.ToFloat(0); }
        /// <summary>
        /// ToBigInt
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <returns></returns>
        public static Int64 ToBigInt(this string strValue) { return strValue.ToBigInt(0); }
        /// <summary>
        /// ToMoney
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <returns></returns>
        public static decimal ToMoney(this string strValue) { return strValue.ToMoney(0); }
        /// <summary>
        /// ToInteger
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <returns></returns>
        public static int ToInteger(this string strValue) { return strValue.ToInteger(0); }
        /// <summary>
        /// ToBool
        /// </summary>
        /// <param name="strValue">string��չ</param>
        /// <returns></returns>
        public static bool ToBool(this string strValue) { return strValue.ToBool(false); }
        /// <summary>
        /// ���תȫ��
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns>ȫ��</returns>
        public static string ToSBC(this string input) {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++) {
                if (c[i] == 32) {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// ȫ��ת���
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns>���</returns>
        public static string ToDBC(this string input) {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++) {
                if (c[i] == 12288) {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// SQLite��������תDbType
        /// </summary>
        /// <param name="sqlType">string��չ</param>
        /// <returns>SQLite��������תDbType</returns>
        public static DbType ToDbTypeForSQLite(this string sqlType) {
            switch (sqlType.ToLowerInvariant()) {
                case "longtext":
                case "nchar":
                case "ntext":
                case "text":
                case "sysname":
                case "varchar":
                case "nvarchar": return DbType.String;

                case "bit":
                case "tinyint": return DbType.Boolean;

                case "decimal":
                case "float":
                case "newdecimal":
                case "numeric":
                case "double":
                case "real": return DbType.Decimal;

                case "int":
                case "int32": return DbType.Int32;

                case "integer": return DbType.Int64;

                case "int16":
                case "smallint": return DbType.Int16;

                case "date":
                case "time":
                case "datetime":
                case "smalldatetime": return DbType.DateTime;

                case "image":
                case "varbinary":
                case "binary":
                case "blob":
                case "longblob": return DbType.Binary;

                case "char": return DbType.AnsiStringFixedLength;

                case "currency":
                case "money":
                case "smallmoney": return DbType.Currency;

                case "timestamp": return DbType.DateTime;

                case "uniqueidentifier": return DbType.Guid;

                case "uint16": return DbType.UInt16;

                case "uint32": return DbType.UInt32;
            }
            return DbType.String;
        }
        /// <summary>
        /// Sql2005��������תDbType
        /// </summary>
        /// <param name="sqlType">string��չ</param>
        /// <returns>Sql2005��������תDbType</returns>
        public static DbType ToDbTypeForSql2005(this string sqlType) {
            switch (sqlType) {
                case "varchar": return DbType.AnsiString;
                case "nvarchar": return DbType.String;
                case "int": return DbType.Int32;
                case "uniqueidentifier": return DbType.Guid;
                case "datetime":
                case "datetime2": return DbType.DateTime;
                case "bigint": return DbType.Int64;
                case "binary": return DbType.Binary;
                case "bit": return DbType.Boolean;
                case "char": return DbType.AnsiStringFixedLength;
                case "decimal": return DbType.Decimal;
                case "float": return DbType.Double;
                case "image": return DbType.Binary;
                case "money": return DbType.Currency;
                case "nchar": return DbType.String;
                case "ntext": return DbType.String;
                case "numeric": return DbType.Decimal;
                case "real": return DbType.Single;
                case "smalldatetime": return DbType.DateTime;
                case "smallint": return DbType.Int16;
                case "smallmoney": return DbType.Currency;
                case "sql_variant": return DbType.String;
                case "sysname": return DbType.String;
                case "text": return DbType.AnsiString;
                case "timestamp": return DbType.Binary;
                case "tinyint": return DbType.Byte;
                case "varbinary": return DbType.Binary;
            }
            return DbType.AnsiString;
        }
        /// <summary>
        /// MySql��������תDbType
        /// </summary>
        /// <param name="mySqlType">string��չ</param>
        /// <returns>MySql��������תDbType</returns>
        public static DbType ToDbTypeForMySql(this string mySqlType) {
            switch (mySqlType.ToLowerInvariant()) {
                case "longtext":
                case "nchar":
                case "ntext":
                case "text":
                case "sysname":
                case "varchar":
                case "nvarchar": return DbType.String;

                case "bit":
                case "tinyint": return DbType.Boolean;

                case "decimal":
                case "float":
                case "newdecimal":
                case "numeric":
                case "double":
                case "real": return DbType.Decimal;

                case "bigint": return DbType.Int64;

                case "int":
                case "int32":
                case "integer": return DbType.Int32;

                case "int16":
                case "smallint": return DbType.Int16;

                case "date":
                case "time":
                case "datetime":
                case "smalldatetime": return DbType.DateTime;

                case "image":
                case "varbinary":
                case "binary":
                case "blob":
                case "longblob": return DbType.Binary;

                case "char": return DbType.AnsiStringFixedLength;

                case "currency":
                case "money":
                case "smallmoney": return DbType.Currency;

                case "timestamp": return DbType.DateTime;

                case "uniqueidentifier": return DbType.Binary;

                case "uint16": return DbType.UInt16;

                case "uint32": return DbType.UInt32;
            }
            return DbType.String;
        }
        /// <summary>
        /// ȡ�ļ��ַ�����MIME
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns>ȡ�ļ��ַ�����MIME</returns>
        public static string GetMimeType(this string str) {
            return Encoding.UTF8.GetBytes(str).GetMimeType();
        }
        /// <summary>
        /// �ļ���չ��ȡHttpContentType ��.doc application/msword
        /// </summary>
        /// <param name="extension">�ļ���չ��</param>
        /// <returns>�ļ���չ��ȡHttpContentType ��.doc application/msword</returns>
        public static string GetContentType(string extension) {
            switch (extension.Trim('.')) {
                #region �����ļ�����
                case "jpeg": return "image/jpeg";
                case "jpg": return "image/jpeg";
                case "js": return "application/x-javascript";
                case "jsp": return "text/html";
                case "gif": return "image/gif";
                case "htm": return "text/html";
                case "html": return "text/html";
                case "asf": return "video/x-ms-asf";
                case "avi": return "video/avi";
                case "bmp": return "application/x-bmp";
                case "asp": return "text/asp";
                case "wma": return "audio/x-ms-wma";
                case "wav": return "audio/wav";
                case "wmv": return "video/x-ms-wmv";
                case "ra": return "audio/vnd.rn-realaudio";
                case "ram": return "audio/x-pn-realaudio";
                case "rm": return "application/vnd.rn-realmedia";
                case "rmvb": return "application/vnd.rn-realmedia-vbr";
                case "xhtml": return "text/html";
                case "png": return "image/png";
                case "ppt": return "application/x-ppt";
                case "tif": return "image/tiff";
                case "tiff": return "image/tiff";
                case "xls": return "application/x-xls";
                case "xlw": return "application/x-xlw";
                case "xml": return "text/xml";
                case "xpl": return "audio/scpls";
                case "swf": return "application/x-shockwave-flash";
                case "torrent": return "application/x-bittorrent";
                case "dll": return "application/x-msdownload";
                case "asa": return "text/asa";
                case "asx": return "video/x-ms-asf";
                case "au": return "audio/basic";
                case "css": return "text/css";
                case "doc": return "application/msword";
                case "exe": return "application/x-msdownload";
                case "mp1": return "audio/mp1";
                case "mp2": return "audio/mp2";
                case "mp2v": return "video/mpeg";
                case "mp3": return "audio/mp3";
                case "mp4": return "video/mpeg4";
                case "mpa": return "video/x-mpg";
                case "mpd": return "application/vnd.ms-project";
                case "mpe": return "video/x-mpeg";
                case "mpeg": return "video/mpg";
                case "mpg": return "video/mpg";
                case "mpga": return "audio/rn-mpeg";
                case "mpp": return "application/vnd.ms-project";
                case "mps": return "video/x-mpeg";
                case "mpt": return "application/vnd.ms-project";
                case "mpv": return "video/mpg";
                case "mpv2": return "video/mpeg";
                case "wml": return "text/vnd.wap.wml";
                case "wsdl": return "text/xml";
                case "xsd": return "text/xml";
                case "xsl": return "text/xml";
                case "xslt": return "text/xml";
                case "htc": return "text/x-component";
                case "mdb": return "application/msaccess";
                case "zip": return "application/zip";
                case "rar": return "application/x-rar-compressed";
                #endregion

                case "*": return "application/octet-stream";
                case "001": return "application/x-001";
                case "301": return "application/x-301";
                case "323": return "text/h323";
                case "906": return "application/x-906";
                case "907": return "drawing/907";
                case "a11": return "application/x-a11";
                case "acp": return "audio/x-mei-aac";
                case "ai": return "application/postscript";
                case "aif": return "audio/aiff";
                case "aifc": return "audio/aiff";
                case "aiff": return "audio/aiff";
                case "anv": return "application/x-anv";
                case "awf": return "application/vnd.adobe.workflow";
                case "biz": return "text/xml";
                case "bot": return "application/x-bot";
                case "c4t": return "application/x-c4t";
                case "c90": return "application/x-c90";
                case "cal": return "application/x-cals";
                case "cat": return "application/vnd.ms-pki.seccat";
                case "cdf": return "application/x-netcdf";
                case "cdr": return "application/x-cdr";
                case "cel": return "application/x-cel";
                case "cer": return "application/x-x509-ca-cert";
                case "cg4": return "application/x-g4";
                case "cgm": return "application/x-cgm";
                case "cit": return "application/x-cit";
                case "class": return "java/*";
                case "cml": return "text/xml";
                case "cmp": return "application/x-cmp";
                case "cmx": return "application/x-cmx";
                case "cot": return "application/x-cot";
                case "crl": return "application/pkix-crl";
                case "crt": return "application/x-x509-ca-cert";
                case "csi": return "application/x-csi";
                case "cut": return "application/x-cut";
                case "dbf": return "application/x-dbf";
                case "dbm": return "application/x-dbm";
                case "dbx": return "application/x-dbx";
                case "dcd": return "text/xml";
                case "dcx": return "application/x-dcx";
                case "der": return "application/x-x509-ca-cert";
                case "dgn": return "application/x-dgn";
                case "dib": return "application/x-dib";
                case "dot": return "application/msword";
                case "drw": return "application/x-drw";
                case "dtd": return "text/xml";
                case "dwf": return "application/x-dwf";
                case "dwg": return "application/x-dwg";
                case "dxb": return "application/x-dxb";
                case "dxf": return "application/x-dxf";
                case "edn": return "application/vnd.adobe.edn";
                case "emf": return "application/x-emf";
                case "eml": return "message/rfc822";
                case "ent": return "text/xml";
                case "epi": return "application/x-epi";
                case "eps": return "application/x-ps";
                case "etd": return "application/x-ebx";
                case "fax": return "image/fax";
                case "fdf": return "application/vnd.fdf";
                case "fif": return "application/fractals";
                case "fo": return "text/xml";
                case "frm": return "application/x-frm";
                case "g4": return "application/x-g4";
                case "gbr": return "application/x-gbr";
                case "gcd": return "application/x-gcd";

                case "gl2": return "application/x-gl2";
                case "gp4": return "application/x-gp4";
                case "hgl": return "application/x-hgl";
                case "hmr": return "application/x-hmr";
                case "hpg": return "application/x-hpgl";
                case "hpl": return "application/x-hpl";
                case "hqx": return "application/mac-binhex40";
                case "hrf": return "application/x-hrf";
                case "hta": return "application/hta";
                case "htt": return "text/webviewhtml";
                case "htx": return "text/html";
                case "icb": return "application/x-icb";
                case "ico": return "application/x-ico";
                case "iff": return "application/x-iff";
                case "ig4": return "application/x-g4";
                case "igs": return "application/x-igs";
                case "iii": return "application/x-iphone";
                case "img": return "application/x-img";
                case "ins": return "application/x-internet-signup";
                case "isp": return "application/x-internet-signup";
                case "IVF": return "video/x-ivf";
                case "java": return "java/*";
                case "jfif": return "image/jpeg";
                case "jpe": return "application/x-jpe";
                case "la1": return "audio/x-liquid-file";
                case "lar": return "application/x-laplayer-reg";
                case "latex": return "application/x-latex";
                case "lavs": return "audio/x-liquid-secure";
                case "lbm": return "application/x-lbm";
                case "lmsff": return "audio/x-la-lms";
                case "ls": return "application/x-javascript";
                case "ltr": return "application/x-ltr";
                case "m1v": return "video/x-mpeg";
                case "m2v": return "video/x-mpeg";
                case "m3u": return "audio/mpegurl";
                case "m4e": return "video/mpeg4";
                case "mac": return "application/x-mac";
                case "man": return "application/x-troff-man";
                case "math": return "text/xml";
                case "mfp": return "application/x-shockwave-flash";
                case "mht": return "message/rfc822";
                case "mhtml": return "message/rfc822";
                case "mi": return "application/x-mi";
                case "mid": return "audio/mid";
                case "midi": return "audio/mid";
                case "mil": return "application/x-mil";
                case "mml": return "text/xml";
                case "mnd": return "audio/x-musicnet-download";
                case "mns": return "audio/x-musicnet-stream";
                case "mocha": return "application/x-javascript";
                case "movie": return "video/x-sgi-movie";
                case "mpw": return "application/vnd.ms-project";
                case "mpx": return "application/vnd.ms-project";
                case "mtx": return "text/xml";
                case "mxp": return "application/x-mmxp";
                case "net": return "image/pnetvue";
                case "nrf": return "application/x-nrf";
                case "nws": return "message/rfc822";
                case "odc": return "text/x-ms-odc";
                case "out": return "application/x-out";
                case "p10": return "application/pkcs10";
                case "p12": return "application/x-pkcs12";
                case "p7b": return "application/x-pkcs7-certificates";
                case "p7c": return "application/pkcs7-mime";
                case "p7m": return "application/pkcs7-mime";
                case "p7r": return "application/x-pkcs7-certreqresp";
                case "p7s": return "application/pkcs7-signature";
                case "pc5": return "application/x-pc5";
                case "pci": return "application/x-pci";
                case "pcl": return "application/x-pcl";
                case "pcx": return "application/x-pcx";
                case "pdf": return "application/pdf";
                case "pdx": return "application/vnd.adobe.pdx";
                case "pfx": return "application/x-pkcs12";
                case "pgl": return "application/x-pgl";
                case "pic": return "application/x-pic";
                case "pko": return "application/vnd.ms-pki.pko";
                case "pl": return "application/x-perl";
                case "plg": return "text/html";
                case "pls": return "audio/scpls";
                case "plt": return "application/x-plt";
                case "pot": return "application/vnd.ms-powerpoint";
                case "ppa": return "application/vnd.ms-powerpoint";
                case "ppm": return "application/x-ppm";
                case "pps": return "application/vnd.ms-powerpoint";
                case "pr": return "application/x-pr";
                case "prf": return "application/pics-rules";
                case "prn": return "application/x-prn";
                case "prt": return "application/x-prt";
                case "ps": return "application/x-ps";
                case "ptn": return "application/x-ptn";
                case "pwz": return "application/vnd.ms-powerpoint";
                case "r3t": return "text/vnd.rn-realtext3d";
                case "ras": return "application/x-ras";
                case "rat": return "application/rat-file";
                case "rdf": return "text/xml";
                case "rec": return "application/vnd.rn-recording";
                case "red": return "application/x-red";
                case "rgb": return "application/x-rgb";
                case "rjs": return "application/vnd.rn-realsystem-rjs";
                case "rjt": return "application/vnd.rn-realsystem-rjt";
                case "rlc": return "application/x-rlc";
                case "rle": return "application/x-rle";
                case "rmf": return "application/vnd.adobe.rmf";
                case "rmi": return "audio/mid";
                case "rmj": return "application/vnd.rn-realsystem-rmj";
                case "rmm": return "audio/x-pn-realaudio";
                case "rmp": return "application/vnd.rn-rn_music_package";
                case "rms": return "application/vnd.rn-realmedia-secure";
                case "rmx": return "application/vnd.rn-realsystem-rmx";
                case "rnx": return "application/vnd.rn-realplayer";
                case "rp": return "image/vnd.rn-realpix";
                case "rpm": return "audio/x-pn-realaudio-plugin";
                case "rsml": return "application/vnd.rn-rsml";
                case "rt": return "text/vnd.rn-realtext";
                case "rtf": return "application/msword";
                case "rv": return "video/vnd.rn-realvideo";
                case "sam": return "application/x-sam";
                case "sat": return "application/x-sat";
                case "sdp": return "application/sdp";
                case "sdw": return "application/x-sdw";
                case "sit": return "application/x-stuffit";
                case "slb": return "application/x-slb";
                case "sld": return "application/x-sld";
                case "slk": return "drawing/x-slk";
                case "smi": return "application/smil";
                case "smil": return "application/smil";
                case "smk": return "application/x-smk";
                case "snd": return "audio/basic";
                case "sol": return "text/plain";
                case "sor": return "text/plain";
                case "spc": return "application/x-pkcs7-certificates";
                case "spl": return "application/futuresplash";
                case "spp": return "text/xml";
                case "ssm": return "application/streamingmedia";
                case "sst": return "application/vnd.ms-pki.certstore";
                case "stl": return "application/vnd.ms-pki.stl";
                case "stm": return "text/html";
                case "sty": return "application/x-sty";
                case "svg": return "text/xml";
                case "tdf": return "application/x-tdf";
                case "tg4": return "application/x-tg4";
                case "tga": return "application/x-tga";
                case "tld": return "text/xml";
                case "top": return "drawing/x-top";
                case "tsd": return "text/xml";
                case "txt": return "text/plain";
                case "uin": return "application/x-icq";
                case "uls": return "text/iuls";
                case "vcf": return "text/x-vcard";
                case "vda": return "application/x-vda";
                case "vdx": return "application/vnd.visio";
                case "vml": return "text/xml";
                case "vpg": return "application/x-vpeg005";
                case "vsd": return "application/vnd.visio";
                case "vss": return "application/vnd.visio";
                case "vst": return "application/vnd.visio";
                case "vsw": return "application/vnd.visio";
                case "vsx": return "application/vnd.visio";
                case "vtx": return "application/vnd.visio";
                case "vxml": return "text/xml";
                case "wax": return "audio/x-ms-wax";
                case "wb1": return "application/x-wb1";
                case "wb2": return "application/x-wb2";
                case "wb3": return "application/x-wb3";
                case "wbmp": return "image/vnd.wap.wbmp";
                case "wiz": return "application/msword";
                case "wk3": return "application/x-wk3";
                case "wk4": return "application/x-wk4";
                case "wkq": return "application/x-wkq";
                case "wks": return "application/x-wks";
                case "wm": return "video/x-ms-wm";
                case "wmd": return "application/x-ms-wmd";
                case "wmf": return "application/x-wmf";
                case "wmx": return "video/x-ms-wmx";
                case "wmz": return "application/x-ms-wmz";
                case "wp6": return "application/x-wp6";
                case "wpd": return "application/x-wpd";
                case "wpg": return "application/x-wpg";
                case "wpl": return "application/vnd.ms-wpl";
                case "wq1": return "application/x-wq1";
                case "wr1": return "application/x-wr1";
                case "wri": return "application/x-wri";
                case "wrk": return "application/x-wrk";
                case "ws": return "application/x-ws";
                case "ws2": return "application/x-ws";
                case "wsc": return "text/scriptlet";
                case "wvx": return "video/x-ms-wvx";
                case "xdp": return "application/vnd.adobe.xdp";
                case "xdr": return "text/xml";
                case "xfd": return "application/vnd.adobe.xfd";
                case "xfdf": return "application/vnd.adobe.xfdf";
                case "xq": return "text/xml";
                case "xql": return "text/xml";
                case "xquery": return "text/xml";
                case "xwd": return "application/x-xwd";
                case "x_b": return "application/x-x_b";
                case "x_t": return "application/x-x_t";
            }
            return "application/octet-stream";
        }
        /// <summary>
        /// תSecureString
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns>SecureString</returns>
        public static SecureString ToSecureString(this string str) {
            SecureString secureString = new SecureString();
            foreach (Char c in str) secureString.AppendChar(c);
            return secureString;
        }
        /// <summary>
        /// ����DLL�ļ���ȫ·��
        /// </summary>
        /// <example>
        /// <code>
        /// Msg.Write("test".GetBinFileFullPath() + "&lt;br />");
        /// Msg.Write("test.dll".GetBinFileFullPath() + "&lt;br />");
        /// Msg.Write("test.dllx".GetBinFileFullPath() + "&lt;br />&lt;br />");
        /// Msg.Write("c:\\test".GetBinFileFullPath() + "&lt;br />");
        /// Msg.Write("c:\\test.dll".GetBinFileFullPath() + "&lt;br />");
        /// Msg.Write("c:\\test.dllx".GetBinFileFullPath() + "&lt;br />&lt;br />");
        /// Msg.Write("~/test".GetBinFileFullPath() + "&lt;br />");
        /// Msg.Write("~/test.dll".GetBinFileFullPath() + "&lt;br />");
        /// Msg.Write("~/test.dllx".GetBinFileFullPath() + "&lt;br />&lt;br />");
        /// </code>
        /// </example>
        /// <param name="fileName">�ļ���/�ļ�����·��/�ļ����·��</param>
        /// <returns></returns>
        public static string GetBinFileFullPath(this string fileName) {
            if (fileName.IndexOf(".") == -1) fileName = fileName + ".dll";
            if (fileName.IndexOf("\\") != -1) return fileName;

            if (fileName.IndexOf("/") != -1) fileName = fileName.GetMapPath();
            else fileName = HttpContext.Current != null ? ("~/bin/" + fileName).GetMapPath() : fileName.GetMapPath();

            return fileName;
        }
        /// <summary>
        /// ���ļ���
        /// </summary>
        /// <param name="dllFullPath">dll·�����ļ���</param>
        /// <returns></returns>
        public static byte[] LoadFileStream(this string dllFullPath) {
            return new FileStream(dllFullPath.GetBinFileFullPath(), FileMode.Open).ToBytes();
        }

        private static Dictionary<string, Assembly> assemblyCache = new Dictionary<string, Assembly>();
        private static object cacheLock = new object();
        /// <summary>
        /// ��̬���س��� CACHE
        /// </summary>
        /// <param name="dllFullPath">dll·�����ļ���</param>
        /// <returns></returns>
        public static Assembly LoadDynamicAssembly(this string dllFullPath) {
            lock (cacheLock) {
                if (assemblyCache.ContainsKey(dllFullPath)) return assemblyCache[dllFullPath];

                Assembly assembly = null;
                byte[] addinStream = dllFullPath.GetBinFileFullPath().LoadFileStream();//�Ƚ�����������ڴ滺��
                assembly = Assembly.Load(addinStream); //�����ڴ��е�Dll
                assemblyCache.Add(dllFullPath, assembly);
                return assembly;
            }
        }
        /// <summary>
        /// ���س��� CACHE
        /// </summary>
        /// <param name="dllFullPath">dll·�����ļ���</param>
        /// <returns></returns>
        public static Assembly LoadAssembly(this string dllFullPath) {
            lock (cacheLock) {
                if (assemblyCache.ContainsKey(dllFullPath)) return assemblyCache[dllFullPath];

                Assembly assembly = Assembly.LoadFrom(dllFullPath.GetBinFileFullPath());
                assemblyCache.Add(dllFullPath, assembly);
                return assembly;
            }
        }

        private static Dictionary<string, object> objectCache = new Dictionary<string, object>();
        private static object objLock = new object();
        /// <summary>
        /// ��̬����binĿ¼�µ�DLL�� Activator.CreateInstance CACHE
        /// </summary>
        /// <param name="dllFileName">binĿ¼�µ�dll�ļ��� ����·��</param>
        /// <param name="className">�����ռ������</param>
        /// <returns></returns>
        public static object LoadDLLClass(this string dllFileName, string className = "") {
            dllFileName = dllFileName.GetBinFileFullPath();
            className = className.Length == 0 ? dllFileName.GetFileNameWithoutExtension() : className;
            lock (objLock) {
                if (objectCache.ContainsKey(dllFileName + className)) return objectCache[dllFileName + className];
                object obj = Activator.CreateInstance(dllFileName.LoadAssembly().GetType(className, false, true));
                objectCache.Add(dllFileName + className, obj);
                return obj;
            }
        }
        /// <summary>
        /// ��̬����binĿ¼�µ�DLL�� Activator.CreateInstance CACHE
        /// </summary>
        /// <param name="dllFileName">binĿ¼�µ�dll�ļ��� ����·��</param>
        /// <param name="className">�����ռ������</param>
        /// <returns></returns>
        public static object LoadDynamicDLLClass(this string dllFileName, string className = "") {
            dllFileName = dllFileName.GetBinFileFullPath();
            className = className.Length == 0 ? dllFileName.GetFileNameWithoutExtension() : className;
            lock (objLock) {
                if (objectCache.ContainsKey(dllFileName + className)) return objectCache[dllFileName + className];
                object obj = Activator.CreateInstance(dllFileName.LoadDynamicAssembly().GetType(className, false, true));
                objectCache.Add(dllFileName + className, obj);
                return obj;
            }
        }
        /// <summary>
        /// ��̬����binĿ¼�µ�DLL�� Activator.CreateInstance CACHE
        /// </summary>
        /// <param name="classNameAndNameSpace">�����ռ�.����,�����ռ�</param>
        /// <returns></returns>
        public static object LoadDynamicDLLClass(this string classNameAndNameSpace) {
            lock (objLock) {
                if (objectCache.ContainsKey(classNameAndNameSpace)) return objectCache[classNameAndNameSpace];
                return Activator.CreateInstance(Type.GetType(classNameAndNameSpace, false, true));
            }
        }
        ///// <summary>
        ///// ��̬����binĿ¼�µ�DLL�� IL + CACHE ��ε���ʹ��
        ///// </summary>
        ///// <param name="dllFileName">binĿ¼�µ�dll�ļ��� ����·��</param>
        ///// <param name="className">�����ռ������</param>
        ///// <returns></returns>
        //public static object CreateInstanceForBinCache(this string dllFileName, string className) {
        //    string assemblyPath = (HttpContext.Current != null ? "~/bin/".GetMapPath() : "".GetMapPath());
        //    Type t = Assembly.LoadFrom(assemblyPath + dllFileName).GetType(className.Length == 0 ? dllFileName.Substring(0, dllFileName.Length - 4) : className);

        //    return new FastCreateMethod(t).CreateInstance();
        //}

        /// <summary>
        /// Hashֵ
        /// </summary>
        /// <param name="content">string��չ</param>
        /// <returns>Hashֵ</returns>
        public static string Hash(this string content) {
            return Encoding.UTF8.GetBytes(content).MD5();
        }
        /// <summary>
        /// Unicode��
        /// </summary>
        /// <param name="Input">string��չ</param>
        /// <returns>Unicode��</returns>
        public static bool IsUnicode(this string Input) {
            if (string.IsNullOrEmpty(Input))
                return true;
            UnicodeEncoding Encoding = new UnicodeEncoding();
            string UniInput = Encoding.GetString(Encoding.GetBytes(Input));
            ASCIIEncoding Encoding2 = new ASCIIEncoding();
            string ASCIIInput = Encoding2.GetString(Encoding2.GetBytes(Input));
            if (UniInput == ASCIIInput)
                return false;
            return true;
        }
        /// <summary>
        /// \r\n�滻�ɿո�
        /// </summary>
        /// <param name="input">Ҫȥ�����е��ַ���</param>
        /// <param name="replace">�Ƿ���ӿո�</param>
        /// <returns>�Ѿ�ȥ�����е��ַ���</returns>
        public static string ReplaceRN(this string input, string replace = " ") {
            string pattern = @"[\r|\n]";
            Regex regEx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(input, replace);
        }
        /// <summary>
        /// \r\n�滻��br
        /// </summary>
        /// <param name="input">string��չ</param>
        /// <returns>\r\n�滻��br</returns>
        public static string ReplaceRNToBR(this string input) {
            return input.Replace("\r\n", "<br />").Replace("\n", "<br />");
        }
        /// <summary>
        /// �ض�HTML���볤��
        /// </summary>
        /// <param name="htmlString">string��չ</param>
        /// <param name="maxLength">���</param>
        /// <param name="flg">��չ�ַ�</param>
        /// <returns>�ض�HTML���볤��</returns>
        public static string SubHtmlString(this string htmlString, int maxLength, string flg) {
            bool isText = true;
            StringBuilder r = new StringBuilder();
            int i = 0;

            char currentChar = ' ';
            int lastSpacePosition = -1;
            char lastChar = ' ';

            Dictionary<int, string> tagsArray = new Dictionary<int, string>();
            string currentTag = "";
            int tagLevel = 0;

            int noTagLength = 0;
            int j = 0;

            for (j = 0; j < htmlString.Length; j++) {
                currentChar = htmlString[j];
                if (currentChar == '<') {
                    isText = false;
                }
                if (isText) {
                    noTagLength++;
                }
                if (currentChar == '>') {
                    isText = true;
                }
            }
            for (j = 0; j < htmlString.Length; j++) {
                currentChar = htmlString[j];
                r.Append(currentChar);
                if (currentChar == '<') {
                    isText = false;
                }
                if (isText) {
                    if (currentChar == ' ') {
                        lastSpacePosition = j;
                    } else {
                        lastChar = currentChar;
                    }
                    i++;
                } else {
                    currentTag += currentChar;
                }
                if (currentChar == '>') {
                    isText = true;
                    if (currentTag.IndexOf("<") != -1 && currentTag.IndexOf("/>") == -1 && currentTag.IndexOf("</") == -1) {
                        if (currentTag.IndexOf(" ") != -1) {
                            currentTag = currentTag.Substring(1, currentTag.IndexOf(" ") - 1);
                        } else {
                            currentTag = currentTag.Substring(1, currentTag.Length - 2);
                        }
                        tagsArray[tagLevel] = currentTag;
                        tagLevel++;
                    } else if (currentTag.IndexOf("</") != -1) {
                        tagsArray[tagLevel - 1] = null;
                        tagLevel--;
                    }
                    currentTag = "";
                }

                if (i == maxLength) { break; }
            }
            if (maxLength < noTagLength) {
                if (lastSpacePosition != -1) {
                    r = new StringBuilder(htmlString.Substring(0, lastSpacePosition));
                } else {
                    r = new StringBuilder(htmlString.Substring(0, j));
                }
            }
            for (int a = tagsArray.Count - 1; a >= 0; a--) {
                if (tagsArray[a] != null) {
                    r.Append("</" + tagsArray[a] + ">");
                }
            }
            if (maxLength < noTagLength) {
                r.Append(flg);
                //if (lastChar != '.') {
                //    r.Append("...");
                //} else {
                //    r.Append("..");
                //}
            }
            return r.ToString();
        }
        /// <summary>
        /// UTF8ToGB2312
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns>UTF8ToGB2312</returns>
        public static string UTF8ToGB2312(this string str) {
            Encoding utf8 = Encoding.GetEncoding(65001);
            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            byte[] temp = utf8.GetBytes(str);
            byte[] temp1 = Encoding.Convert(utf8, gb2312, temp);
            string result = gb2312.GetString(temp1);
            return result;
        }
        /// <summary>
        /// GB2312ToUTF8
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <returns>GB2312ToUTF8</returns>
        public static string GB2312ToUTF8(this string str) {
            Encoding uft8 = Encoding.GetEncoding(65001);
            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            byte[] temp = gb2312.GetBytes(str);
            byte[] temp1 = Encoding.Convert(gb2312, uft8, temp);
            string result = uft8.GetString(temp1);
            return result;
        }
        /// <summary>
        /// ȡ��·��
        /// </summary>
        /// <param name="path">string��չ</param>
        /// <returns>ȡ��·��</returns>
        public static string GetParentPath(this string path) {
            if (string.IsNullOrEmpty(path)) return null;
            int length = path.LastIndexOf('/');
            if (length == -1) return null;
            return path.Substring(0, length);
        }
        /// <summary>
        /// ת�����ļ���Ϊ���ļ���
        /// </summary>
        /// <example>
        /// <code>
        /// string name = "http://www.testxt.test.com/test/test/test/test/test.aspx".GetSimpleFileName("...", 20, 10, 30)
        /// </code>
        /// </example>
        /// <param name="fullname">string��չ</param>
        /// <param name="repstring">�滻���ַ���</param>
        /// <param name="leftnum">��</param>
        /// <param name="rightnum">�ҳ�</param>
        /// <param name="charnum">�ܳ�</param>
        /// <returns>ת�����ļ���Ϊ���ļ���</returns>
        public static string GetSimpleFileName(this string fullname, string repstring, int leftnum, int rightnum, int charnum) {
            string simplefilename = "", leftstring = "", rightstring = "", filename = "";
            string extname = fullname.GetExtension();

            if (extname.IsNullEmpty()) return fullname;

            int filelength = 0, dotindex = 0;

            dotindex = fullname.LastIndexOf('.');
            filename = fullname.Substring(0, dotindex);
            filelength = filename.Length;
            if (dotindex > charnum) {
                leftstring = filename.Substring(0, leftnum);
                rightstring = filename.Substring(filelength - rightnum, rightnum);
                if (repstring == "" || repstring == null)
                    simplefilename = leftstring + rightstring + extname;
                else
                    simplefilename = leftstring + repstring + rightstring + extname;
            } else
                simplefilename = fullname;

            return simplefilename;
        }
        /// <summary>
        /// ��ҳ�����xml����
        /// </summary>
        /// <param name="xmlnode">xml����</param>
        public static void WriteXML(this string xmlnode) {
            Msg.WriteXML(xmlnode);
        }
        /// <summary>
        /// ���json����
        /// </summary>
        /// <param name="json">string��չ</param>
        public static void WriteJSON(this string json) {
            Msg.WriteJSON(json);
        }
        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="text">string��չ</param>
        public static void WriteEnd(this string text) {
            Msg.WriteEnd(text);
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="text">string��չ</param>
        public static void Write(this string text) {
            Msg.Write(text);
        }
        /// <summary>
        /// ɾ�����N���ַ�
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="lastchar">���һ���ַ���</param>
        public static string TrimEnd(this string str, string lastchar) {
            int length = str.LastIndexOf(lastchar);
            if (length > 0) { return str.Substring(0, length); }
            return str;
        }
        /// <summary>
        /// ɾ�����N���ַ�
        /// </summary>
        /// <param name="str">string��չ</param>
        public static string TrimEnd(this string str) {
            if (!str.IsNullEmpty()) return str.Remove(str.Length - 1, 1);
            return string.Empty;
        }
        /// <summary>
        /// ɾ�����N���ַ�
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="len">����</param>
        public static string TrimEnd(this string str, int len) {
            if (str.IsNullEmpty()) return string.Empty;
            if (str.Length >= len) return str.Remove(str.Length - len, len);
            return str;
        }
        /// <summary>
        /// ɾ��ǰ��N���ַ�
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="prevchar">���һ���ַ���</param>
        public static string TrimStart(this string str, string prevchar) {
            int length = str.IndexOf(prevchar);
            if (length > 0) { return str.Substring(length); }
            return str;
        }
        /// <summary>
        /// ɾ��ǰ��N���ַ�
        /// </summary>
        /// <param name="str">string��չ</param>
        public static string TrimStart(this string str) {
            if (!str.IsNullEmpty()) return str.Remove(0, 1);
            return string.Empty;
        }
        /// <summary>
        /// ɾ��ǰ��N���ַ�
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="len">����</param>
        public static string TrimStart(this string str, int len) {
            if (str.IsNullEmpty()) return string.Empty;
            if (str.Length >= len) return str.Remove(0, len);
            return str;
        }
        /// <summary>
        /// ɾ��ǰ��ͺ���N���ַ�
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="len">����</param>
        public static string Trim(this string str, int len) {
            str = str.TrimStart(len);
            str = str.TrimEnd(len);
            return str;
        }
        /// <summary>
        /// ɾ��ǰ��ͺ���N���ַ�
        /// </summary>
        /// <param name="str">string��չ</param>
        /// <param name="nchar">���һ���ַ���</param>
        public static string Trim(this string str, string nchar) {
            str = str.TrimStart(nchar);
            str = str.TrimEnd(nchar);
            return str;
        }
        /// <summary>
        /// ���浽�ļ�
        /// </summary>
        /// <param name="str">����</param>
        /// <param name="fileName">�ļ�·��</param>
        /// <param name="encoding">����</param>
        /// <param name="isOver">�Ƿ���д</param>
        public static void ToFile(this string str, string fileName, Encoding encoding, bool isOver = true) {
            if (isOver) FileDirectory.FileDelete(fileName);
            Log.Write(fileName, str, encoding);
        }
        /// <summary>
        /// ���浽�ļ�
        /// </summary>
        /// <param name="str">����</param>
        /// <param name="fileName">�ļ�·��</param>
        public static void ToFile(this string str, string fileName) {
            str.ToFile(fileName, Encoding.UTF8, true);
        }
        /// <summary>
        /// ���浽�ļ�
        /// </summary>
        /// <param name="str">����</param>
        /// <param name="fileName">�ļ�·��</param>
        /// <param name="encoding">����</param>
        public static void ToFile(this string str, string fileName, Encoding encoding) {
            str.ToFile(fileName, encoding, true);
        }
        /// <summary>
        /// CheckOnNullEmpty ����Ƿ�Ϊ�գ����Ϊ����ʾ�쳣
        /// </summary>
        /// <param name="this">object��չ</param>
        /// <param name="parameterName">������</param>
        public static void CheckOnNullEmpty(this string @this, string parameterName) {
            if (@this.IsNullEmpty()) throw new ArgumentNullException(parameterName);
        }
        /// <summary>
        /// CheckOnNullEmpty ����Ƿ�Ϊ�գ����Ϊ����ʾ�쳣
        /// </summary>
        /// <param name="this">object��չ</param>
        /// <param name="parameterName">������</param>
        /// <param name="message">��Ϣ</param>
        public static void CheckOnNullEmpty(this string @this, string parameterName, string message) {
            if (@this.IsNullEmpty()) throw new ArgumentNullException(parameterName, message);
        }
        /// <summary>
        /// ȡHTML���ݱ��� ���� utf8 gb_2312 utf_8 zh_cn
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static Encoding GetHtmlEncoding(this string contentType, Encoding defaultEncoding) {
            Encoding encoding = defaultEncoding;
            System.Text.RegularExpressions.Match m = new System.Text.RegularExpressions.Regex("charset\\s*=\\s*(?<encode>(\\w*(-|_|\\.|)*\\w*)+)\\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled).Match(contentType);
            string encodename = m.Groups["encode"].Value.ToLower().Trim();
            Hashtable encodingNames = new Hashtable();
            encodingNames.Add("utf8", "utf-8");
            encodingNames.Add("gb_2312", "gb2312");
            encodingNames.Add("'utf_8'", "utf-8");
            encodingNames.Add("zh_cn", "gbk");
            encodingNames.Add("utf_8", "utf-8");

            if (encodingNames.Contains(encodename)) encodename = encodingNames[encodename].ToString();
            if (encodename.Length > 0) encoding = Encoding.GetEncoding(encodename); else encoding = defaultEncoding;
            return encoding;
        }
        /// <summary>
        /// ȥ��json�Ŀ�����
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string RemoveJsonNull(this string json) {
            //return System.Text.RegularExpressions.Regex.Replace(json, @",?""\w*"":null,?", string.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @",""\w*"":null", string.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @"""\w*"":null,", string.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @"""\w*"":null", string.Empty);
            return json;
        }
        /// <summary>
        /// ȥ��xml�еĿսڵ�
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>�������xml�ַ���</returns>
        public static string RemoveEmptyNodes(string xml) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList nodes = doc.SelectNodes("//node()");

            foreach (XmlNode node in nodes)
                if (node.ChildNodes.Count == 0 && node.InnerText == string.Empty)
                    node.ParentNode.RemoveChild(node);
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xw.Formatting = Formatting.Indented;
            xw.Indentation = 2;
            doc.PreserveWhitespace = true;
            doc.WriteTo(xw);
            xml = sw.ToString();
            sw.Close();
            xw.Close();
            return xml;
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="length">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static string IfLengthEqual(this string obj, int length, string defaultValue) {
            return obj.IsNullEmpty() || obj.Length == length ? defaultValue : obj;
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="length">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static string IfLengthNotEqual(this string obj, int length, string defaultValue) {
            return obj.IsNullEmpty() || obj.Length != length ? defaultValue : obj;
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="length">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static string IfLengthMoreThan(this string obj, int length, string defaultValue) {
            return obj.IsNullEmpty() || obj.Length > length ? defaultValue : obj;
        }
        /// <summary>
        /// �����С��
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="length">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static string IfLengthLessThan(this string obj, int length, string defaultValue) {
            return obj.IsNullEmpty() || obj.Length < length ? defaultValue : obj;
        }
        /// <summary>
        /// ��������ڵ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="length">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static string IfLengthMoreThanOrEqual(this string obj, int length, string defaultValue) {
            return obj.IsNullEmpty() || obj.Length >= length ? defaultValue : obj;
        }
        /// <summary>
        /// �����С�ڵ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="length">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static string IfLengthLessThanOrEqual(this string obj, int length, string defaultValue) {
            return obj.IsNullEmpty() || obj.Length <= length ? defaultValue : obj;
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static string IfEqual(this string obj, string value, string defaultValue) {
            return obj.IsNullEmpty() || obj == value ? defaultValue : obj;
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static string IfNotEqual(this string obj, string value, string defaultValue) {
            return obj != value ? defaultValue : obj;
        }
        /// <summary>
        /// ɾ����β��ָ���ַ����Ժ���ַ�
        /// </summary>
        /// <param name="obj">Դ�ַ���</param>
        /// <param name="end">���ڵ��ַ���</param>
        /// <returns>ɾ����β��ָ���ַ����Ժ���ַ�</returns>
        public static string TrimIndexEnd(this string obj, string end) {
            int len = obj.LastIndexOf(end);
            if (len == -1) return obj;
            return obj.Substring(0, len);
        }
        /// <summary>
        /// ɾ�����״�ָ���ַ�����ǰ���ַ�
        /// </summary>
        /// <param name="obj">Դ�ַ���</param>
        /// <param name="start">���ڵ��ַ���</param>
        /// <returns>ɾ�����״�ָ���ַ�����ǰ���ַ�</returns>
        public static string TrimIndexStart(this string obj, string start) {
            int len = obj.IndexOf(start);
            if (len == -1) return obj;
            if (obj.Length == start.Length) return string.Empty;
            return obj.Substring(len + start.Length);
        }
        /// <summary>
        /// unicode ascii��ת���� Native2Ascii������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>ת������ַ���</returns>
        public static string Ascii2Native(this string str) {
            string outStr = "";
            if (!string.IsNullOrEmpty(str)) {
                string[] strlist = str.Replace("\\", "").Split('u');
                for (int i = 1; i < strlist.Length; i++) outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
            }
            return outStr.Trim();
        }
        /// <summary>
        /// ����תunicode ascii�� Ascii2Native������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>ת������ַ���</returns>
        public static string Native2Ascii(this string str) {
            string outStr = "";
            if (!string.IsNullOrEmpty(str)) {
                for (int i = 1; i < str.Length; i++) outStr += "\\u" + ((int)str[i]).ToString("x");
            }
            return outStr.Trim();
        }
    }
}
