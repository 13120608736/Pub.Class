//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pub.Class.Log4Net {
    /// <summary>
    /// д��־
    /// 
    /// �޸ļ�¼
    ///     2013.02.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Log : ILog {
        private readonly static string logName = WebConfig.GetApp("log4net.LoggerName");
        private readonly static log4net.ILog log = log4net.LogManager.GetLogger(logName.IsNullEmpty() ? "loginfo": logName);
        /// <summary>
        /// д��־
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="encoding">����</param>
        /// <returns>true/false</returns>
        public bool Write(string msg, Encoding encoding = null) {
            log.Info(msg);
            return true;
        }
    }
}
