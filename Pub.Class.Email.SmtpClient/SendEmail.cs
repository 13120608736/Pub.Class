//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Net.Mail;

namespace Pub.Class.Email.SmtpClient {
    /// <summary>
    /// ʹ��SmtpClient�����ʼ�
    /// 
    /// �޸ļ�¼
    ///     2012.02.20 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class SendEmail : IEmail {
        private string errorMessage = string.Empty;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <returns></returns>
        public string ErrorMessage { get { return errorMessage; } }
        /// <summary>
        /// ����EMAIL
        /// </summary>
        /// <param name="message">MailMessage</param>
        /// <param name="smtp">SmtpClient</param>
        /// <returns>true/false</returns>
        public bool Send(MailMessage message, System.Net.Mail.SmtpClient smtp) {
            try {
                smtp.Send(message);
                return true;
            } catch(Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return false;
            } finally {
                message = null;
                smtp = null;
            }
        }
    }
}
