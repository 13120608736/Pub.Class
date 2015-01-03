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

namespace Pub.Class {
    /// <summary>
    /// ����EMAIL�ӿ�
    /// 
    /// �޸ļ�¼
    ///     2012.02.20 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface IEmail : IAddIn {
        /// <summary>
        /// ����EMAIL
        /// </summary>
        /// <param name="message">MailMessage</param>
        /// <param name="smtp">SmtpClient</param>
        /// <returns>true/false</returns>
        bool Send(MailMessage message, SmtpClient smtp);
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <returns></returns>
        string ErrorMessage { get; }
    }
}
