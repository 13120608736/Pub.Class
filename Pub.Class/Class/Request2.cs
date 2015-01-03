//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace Pub.Class {
    /// <summary>
    /// Requests������
    /// 
    /// �޸ļ�¼
    ///     2006.05.09 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Request2 {
        //#region Get/GetInt/GetFloat
        /// <summary>
        /// ���մ�ֵ ����POST/GET����
        /// </summary>
        /// <param name="VarName">��������</param>
        /// <returns>������Ӧ��ֵ</returns>
        public static String Get(String VarName) {
            string varValue = string.Empty;
            if (HttpContext.Current.Request[VarName].IsNotNull()) varValue = HttpContext.Current.Request[VarName].ToString();
            return varValue;
        }
        /// <summary>
        /// ���մ�ֵ ����POST/GET���� ����int����
        /// </summary>
        /// <param name="varName">��������</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns>������Ӧ��ֵint����</returns>
        public static int GetInt(string varName, int defValue) { return Get(varName).ToInt(defValue); }
        /// <summary>
        /// ���մ�ֵ ����POST/GET���� ����float����
        /// </summary>
        /// <param name="varName">��������</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns>������Ӧ��ֵfloat����</returns>
        public static float GetFloat(string varName, int defValue) { return Get(varName).ToFloat(defValue); }
        //#endregion
        //#region GetQ/GetQInt/GetQFloat
        /// <summary>
        /// ȡURL�ϵĲ���
        /// </summary>
        /// <param name="VarName">������</param>
        /// <returns>���ز���</returns>
        public static String GetQ(String VarName) {
            string varValue = string.Empty;
            if (HttpContext.Current.Request.QueryString[VarName].IsNotNull()) varValue = HttpContext.Current.Request.QueryString[VarName].ToString();
            return varValue;
        }
        /// <summary>
        /// ȡURL�ϵĲ��� ����int����
        /// </summary>
        /// <param name="varName">������</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns>���ز���int����</returns>
        public static int GetQInt(string varName, int defValue) { return GetQ(varName).ToInt(defValue); }
        /// <summary>
        /// ȡURL�ϵĲ��� ����long����
        /// </summary>
        /// <param name="varName">������</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns>���ز���long����</returns>
        public static long GetQLong(string varName, long defValue) { return GetQ(varName).ToBigInt(defValue); }
        /// <summary>
        /// ȡURL�ϵĲ��� ����float����
        /// </summary>
        /// <param name="varName">������</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns>���ز���float����</returns>
        public static float GetQFloat(string varName, int defValue) { return GetQ(varName).ToFloat(defValue); }
        //#endregion
        //#region GetF/GetFInt/GetFFloat
        /// <summary>
        /// ȡPOST�ύ������ 
        /// </summary>
        /// <param name="VarName">����</param>
        /// <returns>����ֵ</returns>
        public static String GetF(String VarName) {
            string varValue = string.Empty;
            if (HttpContext.Current.Request.Form[VarName].IsNotNull()) varValue = HttpContext.Current.Request.Form[VarName].ToString();
            return varValue;
        }
        /// <summary>
        /// ȡPOST�ύ������ int����
        /// </summary>
        /// <param name="varName">������</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns>���ز���int����</returns>
        public static int GetFInt(string varName, int defValue) { return GetF(varName).ToInt(defValue); }
        /// <summary>
        /// ȡPOST�ύ������ long����
        /// </summary>
        /// <param name="varName">������</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns>���ز���long����</returns>
        public static long GetFLong(string varName, long defValue) { return GetF(varName).ToBigInt(defValue); }
        /// <summary>
        /// ȡPOST�ύ������ float����
        /// </summary>
        /// <param name="varName">������</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns>���ز���long����</returns>
        public static float GetFFloat(string varName, int defValue) { return GetF(varName).ToFloat(defValue); }
        //#endregion
        //#region IsPost/IsGet
        /// <summary>
        /// �жϵ�ǰҳ���Ƿ���յ���Post����
        /// </summary>
        /// <returns>�Ƿ���յ���Post����</returns>
        public static bool IsPost() { return HttpContext.Current.Request.HttpMethod.Equals("POST"); }
        /// <summary>
        /// �жϵ�ǰҳ���Ƿ���յ���Get����
        /// </summary>
        /// <returns>�Ƿ���յ���Get����</returns>
        public static bool IsGet() { return HttpContext.Current.Request.HttpMethod.Equals("GET"); }
        //#endregion
        //#region ������������
        /// <summary>
        /// ����ָ���ķ�����������Ϣ
        /// </summary>
        /// <param name="strName">������������</param>
        /// <returns>������������Ϣ</returns>
        public static string GetServerString(string strName) {
            if (HttpContext.Current.Request.ServerVariables[strName].IsNull()) return string.Empty;
            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }
        //#endregion
        //#region GetRawUrl/IsBrowserGet/IsSearchEnginesGet/GetPageName/GetQParamCount/GetFParamCount/GetParamCount/
        /// <summary>
        /// ��ȡ��ǰ�����ԭʼ URL(URL ������Ϣ֮��Ĳ���,������ѯ�ַ���(�������))
        /// </summary>
        /// <returns>ԭʼ URL</returns>
        public static string GetRawUrl() { return HttpContext.Current.Request.RawUrl; }
        /// <summary>
        /// �жϵ�ǰ�����Ƿ�������������
        /// </summary>
        /// <returns>��ǰ�����Ƿ�������������</returns>
        public static bool IsBrowserGet() {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++) {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0) return true;
            }
            return false;
        }
        /// <summary>
        /// �ж��Ƿ�����������������
        /// </summary>
        /// <returns>�Ƿ�����������������</returns>
        public static bool IsSearchEnginesGet() {
            if (HttpContext.Current.Request.UrlReferrer.IsNotNull()) {
                string[] strArray = new string[] { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
                string str = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
                for (int i = 0; i < strArray.Length; i++) {
                    if (str.IndexOf(strArray[i]) >= 0) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// ��õ�ǰҳ�������
        /// </summary>
        /// <returns>��ǰҳ�������</returns>
        public static string GetPageName() {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }
        /// <summary>
        /// ���ر���Url�������ܸ���
        /// </summary>
        /// <returns>���ر���Url�������ܸ���</returns>
        public static int GetParamCount() {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }
        /// <summary>
        /// URL��������
        /// </summary>
        /// <returns>URL��������</returns>
        public static int GetQParamCount() { return (HttpContext.Current.Request.QueryString.Count); }
        /// <summary>
        /// �����ݸ���
        /// </summary>
        /// <returns>�����ݸ���</returns>
        public static int GetFParamCount() { return (HttpContext.Current.Request.Form.Count); }
        //#endregion
        //#region GetCurrentFullHost/GetHost/GetIP/GetUrl/GetReferrer/SaveRequestFile/GetOS/GetBrowser/GetRequest
        /// <summary>
        /// ȡȫIP�����˿�
        /// </summary>
        /// <returns>IP�Ͷ˿�</returns>
        public static string GetCurrentFullHost() {
            HttpRequest request = HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort) return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            return request.Url.Host;
        }
        /// <summary>
        /// ȡ������
        /// </summary>
        /// <returns>ȡ������</returns>
        public static string GetHost() { return HttpContext.Current.Request.Url.Host; }
        /// <summary>
        /// ǰ̨URL
        /// </summary>
        /// <returns>ǰ̨URL</returns>
        public static string GetUrl() { return HttpContext.Current.Request.Url.ToString(); }
        /// <summary>
        /// ������Ը�·�� /��ͷ
        /// </summary>
        /// <returns></returns>
        public static string GetRelativeRoot() {
            return "~/".GetAbsolutePath();
        }
        /// <summary>
        /// ��ȡվ���Ŀ¼URL {0}://{1}{2}{3}
        /// </summary>
        /// <param name="forumPath">վ��Ŀ¼��ӦURL</param>
        /// <returns>��ȡվ���Ŀ¼URL</returns>
        public static string GetRootUrl(string forumPath) {
            int port = HttpContext.Current.Request.Url.Port;
            return string.Format("{0}://{1}{2}{3}",
                                 HttpContext.Current.Request.Url.Scheme,
                                 HttpContext.Current.Request.Url.Host.ToString(),
                                 (port == 80 || port == 0) ? "" : ":" + port,
                                 forumPath);
        }
        /// <summary>
        /// ��ԴURL
        /// </summary>
        /// <returns>��ԴURL</returns>
        public static string GetReferrer() {
            string str = GetServerString("HTTP_REFERER").Trim();
            if (null == str) str = HttpContext.Current.Request.UrlReferrer.ToString();
            if (null == str) return string.Empty;
            return str;
        }
        /// <summary>
        /// ����Request�ļ�
        /// </summary>
        /// <param name="path">���浽�ļ���</param>
        public static void SaveRequestFile(string path) { if (HttpContext.Current.Request.Files.Count > 0) HttpContext.Current.Request.Files[0].SaveAs(path); }
        //#region GetIP
        /// <summary>
        /// ȡIP
        /// </summary>
        /// <returns>����IP</returns>
        public static string GetIP() {
            if (HttpContext.Current.IsNull()) return string.Empty;

            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (result != null && result != String.Empty) { //�����д��� 
                if (result.IndexOf(".") == -1) result = null;
                else {
                    if (result.IndexOf(",") != -1) {//�С�,�������ƶ������ȡ��һ������������IP�� 

                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++) {
                            if (temparyip[i].IsIP()
                                && temparyip[i].Substring(0, 3) != "10."
                                && temparyip[i].Substring(0, 7) != "192.168"
                                && temparyip[i].Substring(0, 7) != "172.16.") {
                                return temparyip[i];    //�ҵ����������ĵ�ַ 
                            }
                        }
                    } else if (result.IsIP()) //������IP��ʽ 
                        return result;
                    else
                        result = null;    //�����е����� ��IP��ȡIP 
                }
            }

            string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IsNotNull()
                && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty)
                ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (null == result) result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (null == result) result = HttpContext.Current.Request.UserHostAddress;
            return result;
        }
        //#endregion
        //#region GetOS
        /// <summary>
        /// ȡ����ϵͳ
        /// </summary>
        /// <returns>���ز���ϵͳ</returns>
        public static string GetOS() {
            HttpBrowserCapabilities bc = new HttpBrowserCapabilities();
            bc = System.Web.HttpContext.Current.Request.Browser;
            return bc.Platform;
        }
        /// <summary>
        /// ��ò���ϵͳ��Ϣ
        /// </summary>
        /// <returns></returns>
        public static string GetClientOS() {
            string os = string.Empty;
            string agent = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (agent == null) return "Other";

            if (agent.IndexOf("Win") > -1) os = "Windows";
            else if (agent.IndexOf("Mac") > -1) os = "Mac";
            else if (agent.IndexOf("Linux") > -1) os = "Linux";
            else if (agent.IndexOf("FreeBSD") > -1) os = "FreeBSD";
            else if (agent.IndexOf("SunOS") > -1) os = "SunOS";
            else if (agent.IndexOf("OS/2") > -1) os = "OS/2";
            else if (agent.IndexOf("AIX") > -1) os = "AIX";
            else if (System.Text.RegularExpressions.Regex.IsMatch(agent, @"(Bot|Crawl|Spider)")) os = "Spiders";
            else os = "Other";
            return os;
        }
        //#endregion
        //#region GetBrowser
        /// <summary>
        /// ȡ������
        /// </summary>
        /// <returns>����������</returns>
        public static string GetBrowser() {
            HttpBrowserCapabilities bc = new HttpBrowserCapabilities();
            bc = System.Web.HttpContext.Current.Request.Browser;
            return bc.Type;
        }
        private static string[] browerNames = { "MSIE", "Firefox", "Opera", "Netscape", "Safari", "Lynx", "Konqueror", "Mozilla" };
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <returns></returns>
        public static string GetClientBrower() {
            string agent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (!string.IsNullOrEmpty(agent)) {
                foreach (string name in browerNames) {
                    if (agent.Contains(name))
                        return name;
                }
            }
            return "Other";
        }
        //#endregion
        /// <summary>
        /// ���� get/post ����
        /// </summary>
        /// <returns>���� get/post ����</returns>
        public static string GetRequest() {
            StringBuilder sb = new StringBuilder();
            sb.Append("form:");
            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++) sb.Append(HttpContext.Current.Request.Form.Keys[i].ToString() + "=" + HttpContext.Current.Request.Form[i].ToString() + "&");
            sb.RemoveLastChar("&");
            sb.Append("querystring:");
            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++) sb.Append(HttpContext.Current.Request.QueryString.Keys[i].ToString() + "=" + HttpContext.Current.Request.QueryString[i].ToString() + "&");
            sb.RemoveLastChar("&");
            return sb.ToString();
        }
        /// <summary>
        /// GetRequestInputStream
        /// </summary>
        /// <returns></returns>
        public static string GetRequestInputStream() {
            return GetRequestInputStream(Encoding.UTF8);
        }
        /// <summary>
        /// GetRequestInputStream
        /// </summary>
        /// <param name="encoding">����</param>
        /// <returns></returns>
        public static string GetRequestInputStream(Encoding encoding) {
            if (HttpContext.Current.IsNull()) return string.Empty;

            System.IO.Stream s = HttpContext.Current.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0) {
                builder.Append(encoding.GetString(buffer, 0, count));
            }
            return builder.ToString();
        }
        /// <summary>
        /// GetRequest
        /// </summary>
        /// <returns></returns>
        public static ISafeDictionary<string, string> GetRequestUTF8() {
            return GetRequest(Encoding.UTF8);
        }
        /// <summary>
        /// GetRequest
        /// </summary>
        /// <param name="encoding">����</param>
        /// <returns></returns>
        public static ISafeDictionary<string, string> GetRequest(Encoding encoding) {
            ISafeDictionary<string, string> list = new SafeDictionary<string, string>();
            string data = GetRequestInputStream(encoding);
            string[] arr = data.Split('&');
            foreach (string info in arr) {
                if (info.IndexOf("=") != -1) {
                    string[] arr2 = info.Split('=');
                    list.AddOrUpdate(arr2[0], arr2[1]);
                } else {
                    list.AddOrUpdate("", info);
                }
            }
            return list;
        }
        /// <summary>
        /// �ж��Ƿ����ϴ����ļ�
        /// </summary>
        /// <returns>�Ƿ����ϴ����ļ�</returns>
        public static bool IsPostFile() {
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++) {
                if (HttpContext.Current.Request.Files[i].FileName != "")
                    return true;
            }
            return false;
        }
        /// <summary>
        /// �ֻ���
        /// </summary>
        /// <returns>true/false</returns>
        public static bool IsMobile() {
            var request = System.Web.HttpContext.Current.Request;
            if (request.IsNull()) return false;
            if (request.Browser.IsMobileDevice) return true;
            if (!string.IsNullOrEmpty(request.UserAgent) && new System.Text.RegularExpressions.Regex(
                "(iemobile|iphone|ipod|android|nokia|sonyericsson|blackberry|samsung|sec\\-|windows ce|motorola|mot\\-|up.b|midp\\-)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled).IsMatch(request.UserAgent)) return true;
            return false;
        }
        /// <summary>
        /// �ֻ���
        /// </summary>
        /// <returns>true/false</returns>
        public static bool IsMobile2() {
            var Request = System.Web.HttpContext.Current.Request;
            if (Request.Browser.IsMobileDevice
                || !Request.ServerVariables["HTTP_X_WAP_PROFILE"].IsNullEmpty()
                || (!Request.ServerVariables["HTTP_ACCEPT"].IsNullEmpty()
                    && (Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap")
                       || Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wml+xml"))))
                return true;
            Regex Regex1 = new Regex(@"android.+mobile|avantgo|bada\\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\\/|plucker|pocket|psp|symbian|treo|up\\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex Regex2 = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\\-(n|u)|c55\\/|capi|ccwa|cdm\\-|cell|chtm|cldc|cmd\\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\\-s|devi|dica|dmob|do(c|p)o|ds(12|\\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\\-|_)|g1 u|g560|gene|gf\\-5|g\\-mo|go(\\.w|od)|gr(ad|un)|haie|hcit|hd\\-(m|p|t)|hei\\-|hi(pt|ta)|hp( i|ip)|hs\\-c|ht(c(\\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\\-(20|go|ma)|i230|iac( |\\-|\\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\\/)|klon|kpt |kwc\\-|kyo(c|k)|le(no|xi)|lg( g|\\/(k|l|u)|50|54|e\\-|e\\/|\\-[a-w])|libw|lynx|m1\\-w|m3ga|m50\\/|ma(te|ui|xo)|mc(01|21|ca)|m\\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\\-2|po(ck|rt|se)|prox|psio|pt\\-g|qa\\-a|qc(07|12|21|32|60|\\-[2-7]|i\\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\\-|oo|p\\-)|sdk\\/|se(c(\\-|0|1)|47|mc|nd|ri)|sgh\\-|shar|sie(\\-|m)|sk\\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\\-|v\\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\\-|tdg\\-|tel(i|m)|tim\\-|t\\-mo|to(pl|sh)|ts(70|m\\-|m3|m5)|tx\\-9|up(\\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|xda(\\-|2|g)|yas\\-|your|zeto|zte\\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (Regex1.IsMatch(Request.ServerVariables["HTTP_USER_AGENT"]) || Regex2.IsMatch(Request.ServerVariables["HTTP_USER_AGENT"].Substring(0, 4)))
                return true;
            return false;
        }
        /// <summary>
        /// IsMono
        /// </summary>
        /// <returns>true/false</returns>
        public static bool IsMono() {
            return (Type.GetType("Mono.Runtime").IsNotNull());
        }
        /// <summary>
        /// IsLinux
        /// </summary>
        /// <returns>true/false</returns>
        public static bool IsLinux() {
            var p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 128); ;
        }

        //#endregion
    }
}
