//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Text;
using System.Web;
using System.IO;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace Pub.Class {
    /// <summary>
    /// ��־������
    /// 
    /// �޸ļ�¼
    ///     2006.05.03 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class SimpleLog: ILog {
        private readonly static string _logPath = WebConfig.GetApp("LogPath");
        private readonly static string LogPath = _logPath.IndexOf("/") == -1 ? _logPath : _logPath.GetMapPath();

        /// <summary>
        /// д��־
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="encoding">����</param>
        /// <returns>true/false</returns>
        public bool Write(string msg, Encoding encoding = null) { 
            string LogFile = LogPath.TrimEnd('\\') + @"\Log_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("/*******************************************************************************************************");
            sb.AppendLine(string.Format("* DateTime��{0}{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), HttpContext.Current.IsNotNull() ? ("	IP��{0}	OS��{1}	Brower��{2}".FormatWith(Request2.GetIP(), Request2.GetOS(), Request2.GetBrowser())) : ""));
            if (HttpContext.Current.IsNotNull()) {
                sb.AppendLine("* Url��" + Request2.GetUrl());
                sb.AppendLine("* Data��" + Request2.GetRequestInputStream());
            }
            sb.AppendLine("* Message��" + msg);
            sb.AppendLine("*******************************************************************************************************/");
            sb.AppendLine("");
            return FileDirectory.FileWrite(LogFile, sb.ToString(), encoding ?? Encoding.UTF8);
        }
    }
}
