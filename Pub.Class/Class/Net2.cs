//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Web.UI;

namespace Pub.Class {
    /// <summary>
    /// ���������
    /// 
    /// �޸ļ�¼
    ///     2006.05.08 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Net2 {
        private static readonly object lockHelper = new object();
        //#region DownFile/ResponseFile/GetHttpFile
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="fileFile">�ļ�·��������·����</param>
        /// <param name="newFileName">���ļ���.��չ������·����</param>
        /// <returns >�����ļ��Ƿ���ڣ��������سɹ�</returns>
        public static bool ResponseFile(string fileFile, string newFileName = "") {
            if (System.IO.File.Exists(fileFile)) {
                FileInfo _DownloadFile = new FileInfo(fileFile);
                newFileName = string.IsNullOrEmpty(newFileName) ? _DownloadFile.FullName : newFileName;
                if (Request2.GetBrowser().ToLower().IndexOf("ie") != -1) newFileName = HttpUtility.UrlEncode(newFileName, System.Text.Encoding.UTF8);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + newFileName);
                HttpContext.Current.Response.AppendHeader("Content-Length", _DownloadFile.Length.ToString());
                HttpContext.Current.Response.WriteFile(_DownloadFile.FullName);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                return true;
            }

            return false;
        }
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="fileName">�ļ���</param>
        /// <returns></returns>
        public static string DownFile(string url, string fileName) {
            using (WebClient wc = new WebClient()) wc.DownloadFile(url, fileName);
            return fileName;
        }
        /// <summary>
        /// ��ָ����ContentType���ָ���ļ��ļ�
        /// </summary>
        /// <param name="filepath">�ļ�·��</param>
        /// <param name="filename">������ļ���</param>
        /// <param name="filetype">���ļ����ʱ���õ�ContentType</param>
        public static void ResponseFile(string filepath, string filename, string filetype) {
            byte[] buffer = new Byte[10000];// ������Ϊ10k
            int length;// �ļ�����
            long dataToRead;// ��Ҫ�������ݳ���

            using (Stream iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                dataToRead = iStream.Length;// ��Ҫ�������ݳ���
                HttpContext.Current.Response.ContentType = filetype;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename.Trim().UrlEncode().Replace("+", " "));
                while (dataToRead > 0) {
                    if (HttpContext.Current.Response.IsClientConnected) {// ���ͻ����Ƿ񻹴�������״̬
                        length = iStream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    } else {
                        dataToRead = -1;// �������������������ѭ��
                    }
                }
            }
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// ����Զ�̵��ļ�
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="sSavePath">���浽</param>
        /// <returns>�ɹ���</returns>
        /// <example>
        /// <code>
        /// TimeSpan oStartTime=DateTime.Now.TimeOfDay;
        /// Response.Write(GetHttpFile("http://www.spbdev.com/download/DotNetInfo1.0.rar",Request.MapPath("RecievedFile.rar")));
        /// Response.Write("&lt;br>&lt;br>\r\nִ��ʱ�䣺" + DateTime.Now.TimeOfDay.Subtract(oStartTime).TotalMilliseconds.ToString() + " ����");
        /// </code>
        /// </example>
        public bool GetHttpFile(string url, string sSavePath) {
            string sException = null;
            bool bRslt = false;
            WebResponse oWebRps = null;
            WebRequest oWebRqst = WebRequest.Create(url);
            oWebRqst.Timeout = 50000;
            try {
                oWebRps = oWebRqst.GetResponse();
            } catch (WebException e) {
                sException = e.Message.ToString();
            } catch (Exception e) {
                sException = e.ToString();
            } finally {
                if (oWebRps.IsNotNull()) {
                    BinaryReader oBnyRd = new BinaryReader(oWebRps.GetResponseStream(), System.Text.Encoding.GetEncoding("GB2312"));
                    int iLen = Convert.ToInt32(oWebRps.ContentLength);
                    FileStream oFileStream;
                    try {
                        if (File.Exists(HttpContext.Current.Request.MapPath("RecievedData.tmp")))
                            oFileStream = File.OpenWrite(sSavePath);
                        else
                            oFileStream = File.Create(sSavePath);
                        oFileStream.SetLength((Int64)iLen);
                        oFileStream.Write(oBnyRd.ReadBytes(iLen), 0, iLen);
                        oFileStream.Close();
                    } finally {
                        oBnyRd.Close();
                        oWebRps.Close();
                    }
                    bRslt = true;
                }
            }
            return bRslt;
        }
        //#endregion
        //#region GetRemoteHtmlCode/GetRemoteHtmlCode2/GetRemoteHtmlCode3
        /// <summary>
        /// ��ȡԶ���ļ�Դ���� �Զ�ȡ���� WebClient DownloadData
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">����</param>
        /// <returns>��ȡԶ���ļ�Դ����</returns>
        public static string GetRemoteHtmlCode(string url, System.Text.Encoding encoding = null) {
            url += (url.IndexOf("?") >= 0 ? "&time=" : "?time=") + Rand.RndDateStr();
            using (WebClient wc = new WebClient()) {
#if !MONO40
                wc.Credentials = CredentialCache.DefaultCredentials;
#endif
                Byte[] pageData = wc.DownloadData(url);
                string content = pageData.GetHtmlEncoding(encoding.IfNull(Encoding.UTF8)).GetString(pageData);
                return content;
            }
        }
        /// <summary>
        /// ��ȡԶ���ļ�Դ���� �Զ�ȡ���� WebClient DownloadData
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">����</param>
        /// <returns>��ȡԶ���ļ�Դ����</returns>
        public static string GetRemoteHtmlCodeLock(string url, System.Text.Encoding encoding = null) {
            lock (lockHelper) {
                return GetRemoteHtmlCode(url, encoding);
            }
        }
        /// <summary>
        /// ��ȡԶ���ļ�Դ���� good HttpWebRequest
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">����</param>
        /// <param name="timeout">��ʱʱ��</param>
        /// <returns>��ȡԶ���ļ�Դ���� �̰߳�ȫ</returns>
        public static string GetRemoteHtmlCode2(string url, System.Text.Encoding encoding = null, int timeout = 0) {
            url += (url.IndexOf("?") >= 0 ? "&time=" : "?time=") + Rand.RndDateStr();
            string s = ""; HttpWebResponse response = null; StreamReader stream = null;
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                if (timeout > 1) request.Timeout = timeout;
                response = (HttpWebResponse)request.GetResponse();
                stream = new StreamReader(response.GetResponseStream(), encoding.IfNull(Encoding.UTF8));
                s = stream.ReadToEnd();
            } catch {
            } finally {
                if (stream.IsNotNull()) stream.Close();
                if (response.IsNotNull()) response.Close();
            }
            return s;
        }
        /// <summary>
        /// ��ȡԶ���ļ�Դ���� HttpWebRequest
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">����</param>
        /// <param name="timeout">��ʱʱ��</param>
        /// <returns>��ȡԶ���ļ�Դ���� �̰߳�ȫ</returns>
        public static string GetRemoteHtmlCode2Lock(string url, System.Text.Encoding encoding = null, int timeout = 0) {
            lock (lockHelper) {
                return GetRemoteHtmlCode2(url, encoding, timeout);
            }
        }
        /// <summary>
        /// ��ȡԶ���ļ�Դ���� HttpWebRequest UserAgent + Referer + AllowAutoRedirect
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">����</param>
        /// <param name="timeout">��ʱʱ��</param>
        /// <returns>��ȡԶ���ļ�Դ����</returns>
        public static string GetRemoteHtmlCode3(string url, System.Text.Encoding encoding = null, int timeout = 0) {
            url += (url.IndexOf("?") >= 0 ? "&time=" : "?time=") + Rand.RndDateStr();
            string s = ""; HttpWebResponse response = null; StreamReader stream = null;
            try {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.AllowAutoRedirect = true;
                if (timeout > 1) request.Timeout = timeout;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                request.Referer = url;
                response = (HttpWebResponse)request.GetResponse();
                stream = new StreamReader(response.GetResponseStream(), encoding.IfNull(Encoding.UTF8));
                s = stream.ReadToEnd();
            } catch {
            } finally {
                if (stream.IsNotNull()) stream.Close();
                if (response.IsNotNull()) response.Close();
            }
            return s;
        }
        /// <summary>
        /// ��ȡԶ���ļ�Դ���� HttpWebRequest UserAgent + Referer + AllowAutoRedirect
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">����</param>
        /// <param name="timeout">��ʱʱ��</param>
        /// <returns>��ȡԶ���ļ�Դ����</returns>
        public static string GetRemoteHtmlCode3Lock(string url, System.Text.Encoding encoding = null, int timeout = 0) {
            lock (lockHelper) {
                return GetRemoteHtmlCode3(url, encoding, timeout);
            }
        }
        /// <summary>
        /// ��ȡԶ���ļ�Դ���� WebClient DownloadData
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">����</param>
        /// <returns>��ȡԶ���ļ�Դ����</returns>
        public static string GetRemoteHtmlCode4(string url, System.Text.Encoding encoding = null) {
            url += (url.IndexOf("?") >= 0 ? "&time=" : "?time=") + Rand.RndDateStr();
            using (WebClient wc = new WebClient()) {
#if !MONO40
                wc.Credentials = CredentialCache.DefaultCredentials;
#endif
                Byte[] pageData = wc.DownloadData(url);
                string content = encoding.IfNull(Encoding.UTF8).GetString(pageData);
                return content;
            }
        }
        /// <summary>
        /// ��ȡԶ���ļ�Դ���� WebClient DownloadData
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">����</param>
        /// <returns>��ȡԶ���ļ�Դ����</returns>
        public static string GetRemoteHtmlCode4Lock(string url, System.Text.Encoding encoding = null) {
            lock (lockHelper) {
                return GetRemoteHtmlCode4(url, encoding);
            }
        }
        /// <summary>
        /// ��ȡԶ���ļ�Դ���� WebClient UploadData
        /// </summary>
        /// <param name="url">Զ��url</param>
        /// <param name="encoding">����</param>
        /// <returns>��ȡԶ���ļ�Դ����</returns>
        public static string GetRemoteHtmlCode5(string url, System.Text.Encoding encoding = null) {
            url += (url.IndexOf("?") >= 0 ? "&time=" : "?time=") + Rand.RndDateStr();
            string postString = "";
            using (WebClient webClient = new WebClient()) {
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] postData = Encoding.ASCII.GetBytes(postString);
                byte[] responseData = webClient.UploadData(url, "POST", postData);
                string srcString = encoding.IfNull(Encoding.UTF8).GetString(responseData);
                return srcString;
            }
        }
        /// <summary>
        /// ��ȡԶ���ļ�Դ���� WebClient UploadData
        /// </summary>
        /// <param name="url">Զ��url</param>
        /// <param name="encoding">����</param>
        /// <returns>��ȡԶ���ļ�Դ����</returns>
        public static string GetRemoteHtmlCode5Lock(string url, System.Text.Encoding encoding = null) {
            lock (lockHelper) {
                return GetRemoteHtmlCode5(url, encoding);
            }
        }

        //#endregion
        //#region TransHtml
        /// <summary>
        /// ת��Ϊ��̬html
        /// </summary>
        /// <param name="path">��ַ</param>
        /// <param name="outpath">���·��</param>
        /// <param name="encoding">����</param>
        public static void TransHtml(string path, string outpath, System.Text.Encoding encoding) {
            Page page = new Page();
            StringWriter writer = new StringWriter();
            page.Server.Execute(path, writer);
            outpath = outpath.IndexOf("\\") > 0 ? outpath : outpath.GetMapPath();
            FileDirectory.FileDelete(outpath);
            FileDirectory.FileWrite(outpath, writer.ToString(), encoding);

            //Page page = new Page();
            //StringWriter writer = new StringWriter();
            //page.Server.Execute(path, writer);
            //FileStream fs;
            //if (File.Exists(page.Server.MapPath("~/") + "\\" + outpath)) {
            //    File.Delete(page.Server.MapPath("~/") + "\\" + outpath);
            //    fs = File.Create(page.Server.MapPath("~/") + "\\" + outpath);
            //} else {
            //    fs = File.Create(page.Server.MapPath("~/") + "\\" + outpath);
            //}
            //byte[] bt = encoding.GetBytes(writer.ToString());
            //fs.Write(bt, 0, bt.Length);
            //fs.Close();
        }
        //#endregion
    }
}
