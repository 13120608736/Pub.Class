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
using System.Net.Mime;
using System.Net;

namespace Pub.Class {
    /// <summary>
    /// ����EMAIL
    /// 
    /// �޸ļ�¼
    ///     2012.02.20 �汾��1.0 livexy ��������
    /// 
    /// <example>
    /// <code>
    /// var email = new Email("Pub.Class.SmtpClient.SendEmail,Pub.Class.SmtpClient")
    ///     .Host("smtp.163.com").Port(25)
    ///     .From("�ܻ���", "cexo255@163.com")
    ///     .Body("��������")
    ///     .Subject("����")
    ///     .IsBodyHtml(true)
    ///     .Credentials("cexo255@163.com", "cexo851029")
    ///     .To(to => to.Add("hcxiong@elibiz.com"))
    ///     .Cc(cc => cc.Add("cexo255@163.com"))
    ///     .Send();
    /// </code>
    /// </example>
    /// </summary>
    public class Email : Disposable {
        private readonly IEmail email = null;
        private readonly bool isSend = false;
        private readonly SmtpClient smtpClient = null;
        private readonly MailMessage message = null;
        private readonly IList<LinkedResource> linkeds = null;
        /// <summary>
        /// ������ ָ��DLL�ļ���ȫ����
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        public Email(string dllFileName, string className) {
            errorMessage = string.Empty;
            if (email.IsNull()) {
                email = (IEmail)dllFileName.LoadClass(className);
                smtpClient = new SmtpClient();
                message = new MailMessage();
                linkeds = new List<LinkedResource>();
            }
        }
        /// <summary>
        /// ������ ָ��classNameDllName(EmailProviderName) Ĭ��Pub.Class.SmtpClient.SendEmail,Pub.Class.SmtpClient
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        public Email(string classNameAndAssembly) {
            errorMessage = string.Empty;
            if (email.IsNull()) {
                email = (IEmail)classNameAndAssembly.IfNullOrEmpty("Pub.Class.Email.SmtpClient.SendEmail,Pub.Class.Email.SmtpClient").LoadClass();
                smtpClient = new SmtpClient();
                message = new MailMessage();
                linkeds = new List<LinkedResource>();
            }
        }
        /// <summary>
        /// ������ ��Web.config�ж�EmailProviderName Ĭ��Pub.Class.SmtpClient.SendEmail,Pub.Class.SmtpClient
        /// </summary>
        public Email() {
            errorMessage = string.Empty;
            if (email.IsNull()) {
                email = (IEmail)(WebConfig.GetApp("EmailProviderName") ?? "Pub.Class.Email.SmtpClient.SendEmail,Pub.Class.Email.SmtpClient").LoadClass();
                smtpClient = new SmtpClient();
                message = new MailMessage();
                linkeds = new List<LinkedResource>();
            }
        }
        /// <summary>
        /// ������ ֱ�ӷ��� ����Ҫָ��EmailProviderName��classNameDllName
        /// </summary>
        /// <param name="host">SMTP ��������������ƻ� IP ��ַ</param>
        /// <param name="port">host ��ʹ�õĶ˿�</param>
        public Email(string host, int port) {
            errorMessage = string.Empty;
            smtpClient = new SmtpClient(host, port);
            message = new MailMessage();
            linkeds = new List<LinkedResource>();
            isSend = true;
        }
        /// <summary>
        /// ��using �Զ��ͷ�
        /// </summary>
        protected override void InternalDispose() {
            base.InternalDispose();
        }
        ///<summary>
        /// ʵ����MailMessage
        ///</summary>
        public MailMessage Message {
            get { return message; }
        }
        ///<summary>
        /// From��ʾ���ƺ͵�ַ
        ///</summary>
        ///<param name="fromMail">From��ʾ��ַ</param>
        ///<returns>Email(this)</returns>
        public virtual Email From(string fromMail) {
            Message.From = new MailAddress(fromMail);
            return this;
        }
        ///<summary>
        /// From��ʾ���ƺ͵�ַ
        ///</summary>
        ///<param name="fromDisplayName">From��ʾ����</param>
        ///<param name="fromMail">From��ʾ��ַ</param>
        ///<returns>Email(this)</returns>
        public virtual Email From(string fromDisplayName, string fromMail) {
            Message.From = new MailAddress(fromMail, fromDisplayName);
            return this;
        }
        ///<summary>
        /// ��ӷ����ʼ���ַ
        ///</summary>
        ///<param name="mailAddresses">�ʼ���ַ</param>
        ///<returns>Email(this)</returns>
        public virtual Email To(Func<MailAddresses, MailAddresses> mailAddresses) {
            foreach (var address in mailAddresses(new MailAddresses()).AddressCollection)
                Message.To.Add(address);

            return this;
        }
        /// <summary>
        /// �Ƴ����з��͵�EMAIL��ַ
        /// </summary>
        /// <returns></returns>
        public virtual Email ClearTo() {
            Message.To.Clear();
            return this;
        }
        /// <summary>
        /// ��Ӹ���
        /// </summary>
        /// <param name="filePath">����·��</param>
        /// <returns>Email(this)</returns>
        public virtual Email AddAttachment(string filePath) {
            Attachment data = new Attachment(filePath, MediaTypeNames.Application.Octet);
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(filePath);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(filePath);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(filePath);
            Message.Attachments.Add(data);
            return this;
        }
        ///<summary>
        /// ��ӷ����ʼ���ַ
        ///</summary>
        ///<param name="mailAddresses">�ʼ���ַ</param>
        ///<returns>Email(this)</returns>
        public virtual Email Cc(Func<MailAddresses, MailAddresses> mailAddresses) {
            foreach (var address in mailAddresses(new MailAddresses()).AddressCollection)
                Message.CC.Add(address);

            return this;
        }
        /// <summary>
        /// �Ƴ����г��͵�EMAIL��ַ
        /// </summary>
        /// <returns></returns>
        public virtual Email ClearCc() {
            Message.CC.Clear();
            return this;
        }
        ///<summary>
        /// ��ӷ����ʼ���ַ
        ///</summary>
        ///<param name="mailAddresses">�ʼ���ַ</param>
        ///<returns>Email(this)</returns>
        public virtual Email Bcc(Func<MailAddresses, MailAddresses> mailAddresses) {
            foreach (var address in mailAddresses(new MailAddresses()).AddressCollection)
                Message.Bcc.Add(address);

            return this;
        }
        public virtual Email AddLinkedResource(string fileName, string contentType, string cid) { 
            LinkedResource link = new System.Net.Mail.LinkedResource(fileName, contentType);
            link.ContentId = cid;
            linkeds.Add(link);
            return this;
        }
        public virtual Email AddImageResource(string fileName, string cid) {
            return AddLinkedResource(fileName, "image/gif", cid);
        }
        /// <summary>
        /// �Ƴ������ܼ����͵�EMAIL��ַ
        /// </summary>
        /// <returns></returns>
        public virtual Email ClearBcc() {
            Message.Bcc.Clear();
            return this;
        }
        /// <summary>
        /// �Ƴ�����To/Cc/Bcc��EMAIL��ַ
        /// </summary>
        /// <returns></returns>
        public virtual Email Clear() {
            ClearBcc();
            ClearCc();
            ClearTo();
            return this;
        }
        ///<summary>
        /// ���ͱ���
        ///</summary>
        ///<param name="subject">����</param>
        ///<returns>Email(this)</returns>
        public virtual Email Subject(string subject) {
            Message.Subject = subject;
            return this;
        }
        ///<summary>
        /// ��������
        ///</summary>
        ///<param name="body">����</param>
        ///<returns>Email(this)</returns>
        public virtual Email Body(string body) {
            Message.Body = body;
            return this;
        }
        ///<summary>
        /// ���ͱ������
        ///</summary>
        ///<param name="subjectEncoding">�������</param>
        ///<returns>Email(this)</returns>
        public virtual Email SubjectEncoding(Encoding subjectEncoding) {
            Message.SubjectEncoding = subjectEncoding;
            return this;
        }
        ///<summary>
        /// �������ݱ���
        ///</summary>
        ///<param name="bodyEncoding">���ݱ���</param>
        ///<returns>Email(this)</returns>
        public virtual Email BodyEncoding(Encoding bodyEncoding) {
            Message.BodyEncoding = bodyEncoding;
            return this;
        }
        public virtual Email Priority(MailPriority mailPriority) {
            Message.Priority = mailPriority;
            return this;
        }
        ///<summary>
        /// �����Ƿ���HTML��ʽ����
        ///</summary>
        ///<param name="isBodyHtml">�Ƿ���HTML��ʽ���� true/false</param>
        ///<returns>Email(this)</returns>
        public virtual Email IsBodyHtml(bool isBodyHtml) {
            Message.IsBodyHtml = isBodyHtml;
            return this;
        }
#if !MONO40
        ///<summary>
        /// ���ʹ��Ĭ��ƾ�ݣ���Ϊ true������Ϊ false��Ĭ��ֵΪ false��
        ///</summary>
        ///<param name="useDefaultCredentials">���ʹ��Ĭ��ƾ�ݣ���Ϊ true������Ϊ false��Ĭ��ֵΪ true��</param>
        ///<returns>Email(this)</returns>
        public virtual Email UseDefaultCredentials(bool useDefaultCredentials = true) {
            smtpClient.UseDefaultCredentials = useDefaultCredentials;
            return this;
        }
#endif
        ///<summary>
        /// �ʼ�����������
        ///</summary>
        ///<param name="host">�ʼ�����������</param>
        ///<returns>Email(this)</returns>
        public virtual Email Host(string host) {
            smtpClient.Host = host;
            return this;
        }
        ///<summary>
        /// �ʼ�������
        ///</summary>
        ///<param name="host">�ʼ�����������</param>
        ///<param name="port">ָ���Ķ˿�</param>
        ///<returns>Email(this)</returns>
        public virtual Email Server(string host, int port = 25) {
            smtpClient.Host = host;
            smtpClient.Port = port;
            return this;
        }
        ///<summary>
        /// ָ���Ķ˿�
        ///</summary>
        ///<param name="port">ָ���Ķ˿�</param>
        ///<returns>Email(this)</returns>
        public virtual Email Port(int port) {
            smtpClient.Port = port;
            return this;
        }
        ///<summary>
        /// �Ƿ�����SSL
        ///</summary>
        ///<param name="enableSsl">�Ƿ�����SSL</param>
        ///<returns>Email(this)</returns>
        public virtual Email Ssl(bool enableSsl) {
            smtpClient.EnableSsl = enableSsl;
            return this;
        }
        ///<summary>
        /// ��ʱʱ��
        ///</summary>
        ///<param name="timeout">��ʱʱ��</param>
        ///<returns>Email(this)</returns>
        public virtual Email Timeout(int timeout) {
            smtpClient.Timeout = timeout;
            return this;
        }
        ///<summary>
        /// ����Ҫʹ�õ�ƾ�ݷ����ʼ���NetworkCredentials��
        ///</summary>
        ///<param name="username">�û���</param>
        ///<param name="password">����</param>
        ///<returns>Email(this)</returns>
        public virtual Email Credentials(string username, string password) {
#if !MONO40
            if (username.IsNullEmpty() || password.IsNullEmpty()) UseDefaultCredentials(true);
            else smtpClient.Credentials = new NetworkCredential(username, password);
#else
            smtpClient.Credentials = new NetworkCredential(username, password);
#endif
            return this;
        }
        ///<summary>
        /// ����Ҫʹ�õ�ƾ�ݷ����ʼ���NetworkCredentials��
        ///</summary>
        ///<param name="username">�û���</param>
        ///<param name="password">����</param>
        ///<param name="domain">��</param>
        ///<returns>Email(this)</returns>
        public virtual Email Credentials(string username, string password, string domain) {
            smtpClient.Credentials = new NetworkCredential(username, password, domain);
            return this;
        }
        ///<summary>
        /// ����
        ///</summary>
        ///<example>
        /// <code>
        ///     var Email = new Email("smtp.gmail.com", 587);
        ///     Email
        ///         .From("Andre Carrilho", "me@mymail.com")
        ///         .To(to => to.Add("Andre Carrilho", "anotherme@mymail.com"))
        ///         .Bcc(bcc => bcc.Add(mailsWithDisplayNames))
        ///         .Cc(cc => cc.Add(justMails))
        ///         .Body("Trying out the Email class with some Html: &lt;p style='font-weight:bold;color:blue;font-size:32px;'>html&lt;/p>")
        ///         .Subject("Testing Fluent Email")
        ///         .IsBodyHtml(true)
        ///         .Credentials("someUser", "somePass")
        ///         .Port(1234)
        ///         .Ssl(true)
        ///         .Send();
        /// </code>
        ///</example>
        ///<returns>true/false</returns>
        public bool Send() {
            if (linkeds.Count > 0) {
                AlternateView htmlBody = AlternateView.CreateAlternateViewFromString(Message.Body, null, "text/html");
                foreach(var info in linkeds) htmlBody.LinkedResources.Add(info);
                Message.AlternateViews.Add(htmlBody);
                linkeds.Clear();
            }
            errorMessage = string.Empty;
            if (isSend) {
                try {
                    smtpClient.Send(message);
                    return true;
                } catch (Exception ex) {
                    errorMessage = ex.ToExceptionDetail();
                    return false;
                }
            }
            bool isTrue = email.Send(message, smtpClient);
            if (!isTrue) errorMessage = email.ErrorMessage;
            return isTrue;
        }
        public bool SendAsync(object state) {
            if (linkeds.Count > 0) {
                AlternateView htmlBody = AlternateView.CreateAlternateViewFromString(Message.Body, null, "text/html");
                foreach (var info in linkeds) htmlBody.LinkedResources.Add(info);
                Message.AlternateViews.Add(htmlBody);
                linkeds.Clear();
            }
            errorMessage = string.Empty;
            if (isSend) {
                try {
                    smtpClient.SendAsync(message, state);
                    return true;
                } catch (Exception ex) {
                    errorMessage = ex.ToExceptionDetail();
                    return false;
                }
            }
            bool isTrue = email.Send(message, smtpClient);
            if (!isTrue) errorMessage = email.ErrorMessage;
            return isTrue;
        }
        ///<summary>
        /// MailAddresses��
        ///</summary>
        public class MailAddresses {
            private readonly MailAddressCollection addressCollection = null;
            internal MailAddressCollection AddressCollection { get { return addressCollection; } }
            /// <summary>
            /// ������
            /// </summary>
            public MailAddresses() {
                addressCollection = new MailAddressCollection();
            }
            ///<summary>
            /// ���һ���µ��ʼ���ַ
            ///</summary>
            ///<param name="mail">�ʼ���ַ</param>
            ///<returns>MailAddresses(this)</returns>
            public MailAddresses Add(string mail) {
                AddressCollection.Add(new MailAddress(mail));
                return this;
            }
            ///<summary>
            /// ���һ���µ��ʼ���ַ
            ///</summary>
            ///<param name="displayName">��ʾ����</param>
            ///<param name="mail">�ʼ���ַ</param>
            ///<returns>MailAddresses(this)</returns>
            public MailAddresses Add(string displayName, string mail) {
                AddressCollection.Add(new MailAddress(mail, displayName));
                return this;
            }
            ///<summary>
            /// ���һ���µ��ʼ���ַ
            ///</summary>
            ///<param name="mails">�ʼ��б�</param>
            ///<returns>MailAddresses(this)</returns>
            public MailAddresses Add(IEnumerable<string> mails) {
                foreach (var mail in mails) {
                    AddressCollection.Add(new MailAddress(mail));
                }
                return this;
            }
            ///<summary>
            /// ���һ���µ��ʼ���ַ
            ///</summary>
            ///<param name="contacts">�ʼ��б�</param>
            ///<returns>MailAddresses(this)</returns>
            public MailAddresses Add(Dictionary<string, string> contacts) {
                foreach (var contact in contacts) {
                    AddressCollection.Add(new MailAddress(contact.Value, contact.Key));
                }
                return this;
            }
        }
        private string errorMessage = string.Empty;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string ErrorMessage { get { return errorMessage; } }
    }
}
