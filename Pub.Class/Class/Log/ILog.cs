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
using System.Net.Mail;
using System.Text;

namespace Pub.Class {
    /// <summary>
    /// д��־
    /// 
    /// �޸ļ�¼
    ///     2013.02.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface ILog : IAddIn {
        /// <summary>
        /// д��־
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="encoding">����</param>
        /// <returns>true/false</returns>
        bool Write(string msg, Encoding encoding = null);
    }
}
