using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Collections;
using System.Threading;
using System.Xml;
using System.IO;
using System.Net.Mail;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using Pub.Class;
using System.Diagnostics;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
#endif

namespace EmailService {
    [RunInstaller(true)]
    public partial class MService : ServiceBase {
        private IList<EmailList> emailList = new List<EmailList>();
        private Thread readEmailThread;
        private string[] strList = new string[] { "����EmailService����", "EmailService ֹͣ��" };
        private readonly ConfigInfo config = new ConfigInfo();
        private Email email;

        /// <summary>
        /// ��ʼ������
        /// </summary>
        public MService() {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        /// <summary>
        /// ����ʼ
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args) {
            WriteLog(strList[0]);

            config.LogPath = Pub.Class.WebConfig.GetApp("LogPath").GetMapPath().TrimEnd('\\') + "\\";
            config.SmtpServer = Pub.Class.WebConfig.GetApp("SmtpServer");
            config.UserName = Pub.Class.WebConfig.GetApp("UserName");
            config.Password = Pub.Class.WebConfig.GetApp("Password");
            config.FromAddress = Pub.Class.WebConfig.GetApp("FromAddress");
            config.AmountThread = Pub.Class.WebConfig.GetApp("AmountThread").ToInt(1).IfLessThan(1, 1);
            config.RecordCount = Pub.Class.WebConfig.GetApp("RecordCount").ToInt(5).IfLessThan(5, 5);
            config.TimeInterval = Pub.Class.WebConfig.GetApp("TimeInterval").ToInt(5).IfLessThan(5, 5);
            config.SmtpPort = Pub.Class.WebConfig.GetApp("SmtpPort").ToInt(25).IfLessThan(0, 25);
            config.IsBodyHtml = Pub.Class.WebConfig.GetApp("IsBodyHtml").ToBool(true);
            config.UseLog = Pub.Class.WebConfig.GetApp("UseLog").ToBool(true);
            config.Ssl = Pub.Class.WebConfig.GetApp("Ssl").ToBool(false);
            config.Retries = Pub.Class.WebConfig.GetApp("Retries").ToInt(1).IfLessThan(1, 1);
            config.Timeout = Pub.Class.WebConfig.GetApp("Timeout").ToInt(1000).IfLessThan(1000, 1000);
            config.ExpireDay = Pub.Class.WebConfig.GetApp("ExpireDay").ToInt(0).IfLessThan(0, 0);
            config.SelectListByTop = Pub.Class.WebConfig.GetApp("SelectListByTop");
            config.DeleteByIDList = Pub.Class.WebConfig.GetApp("DeleteByIDList");
            config.InsertSendHistry = Pub.Class.WebConfig.GetApp("InsertSendHistry");
            config.ClearExpireEmail = Pub.Class.WebConfig.GetApp("ClearExpireEmail").Replace("&lt;", "<");
            config.EmailProviderName = Pub.Class.WebConfig.GetApp("EmailProviderName");

            config.LogPath += "log\\";
            FileDirectory.DirectoryCreate(config.LogPath);
            config.LogPath += DateTime.Now.ToDate() + ".log";

            if (config.ExpireDay > 0) {
                WriteLog("��ʼ����{0}��ǰ�Ĺ����ʼ���".FormatWith(config.ExpireDay));
                WriteLog("����{0}����¼�ɹ���".FormatWith(ClearExpireEmail()));
            }

            if (config.EmailProviderName.IsNullEmpty())
                email = new Email(config.SmtpServer, config.SmtpPort);
            else
                email = new Email(config.EmailProviderName).Server(config.SmtpServer, config.SmtpPort);

            email = email.From(config.FromAddress)
                .IsBodyHtml(config.IsBodyHtml)
                .Ssl(config.Ssl)
                .Credentials(config.UserName, config.Password);

            readEmailThread = new Thread(new ThreadStart(ReadEmailList));
            readEmailThread.Name = "ReadDBThread";
            readEmailThread.Start();

            this.timer1.Interval = config.TimeInterval * 1000;
            this.timer1.Enabled = true;
        }
        /// <summary>
        /// ����ֹͣ
        /// </summary>
        protected override void OnStop() {
            this.timer1.Enabled = false;
            this.timer1.Dispose();
            WriteLog(strList[1]);
        }
        /// <summary>
        /// ��ʱִ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            if (readEmailThread.ThreadState == System.Threading.ThreadState.SuspendRequested || readEmailThread.ThreadState == System.Threading.ThreadState.Suspended) {
                WriteLog("�̼߳��");
                readEmailThread.Resume();
            }
        }
        /// <summary>
        /// ������
        /// </summary>
        private void ReadEmailList() {
            while (true) {
                try {
                    WriteLog("��ʼ��ǰ{0}����¼��".FormatWith(config.RecordCount));
                    emailList = SelectListByTop(config.RecordCount);
                    WriteLog("����{0}����¼�ɹ���".FormatWith(emailList.Count));

                    if (emailList.IsNull() || emailList.Count == 0) {
                        if (readEmailThread.ThreadState == System.Threading.ThreadState.Running) {
                            WriteLog("�̹߳���");
                            readEmailThread.Suspend();
                        }
                    } else {
                        string idlist = emailList.Select(p => p.EmailID).Join<int?>(",");
                        if (!idlist.IsNullEmpty()) {
                            WriteLog("��ʼɾ��{0}��¼��".FormatWith(idlist));
                            WriteLog("ɾ��{0}����¼�ɹ���".FormatWith(DeleteByIDList(idlist)));

                            foreach (var mail in emailList) {
                                string to = mail.Email;
                                string subject = mail.Subject;
                                string body = mail.Body;
                                if (SendEmail(to, subject, body)) {
                                    WriteLog("����{0}�ɹ���".FormatWith(to));
                                } else {
                                    WriteLog("����{0}ʧ�ܡ�".FormatWith(to));
                                    InsertSendHistry(new EmailList() { Email = to, Subject = subject, Body = body });
                                    WriteLog("����ʧ�ܼ�¼��д�뵽��ʷ��");
                                }

                            }
                        }
                    }
                } catch (Exception ex) {
                    WriteLog("�����ݴ���" + ex.ToExceptionDetail(), true);
                }
            }
        }
        /// <summary>
        /// ����EMAIL
        /// </summary>
        /// <param name="to">����email��ַ</param>
        /// <param name="subject">����</param>
        /// <param name="body">����</param>
        /// <returns></returns>
        public bool SendEmail(string to, string subject, string body) {
            if (!to.IsEmail()) return false;
            bool isTrue = false;
            ActionExtensions.Retry(() => {
                if (!email.ClearTo().Body(body).Subject(subject).To(t => t.Add(to)).Send()) {
                    WriteLog("{0}����ʧ�ܣ�{1}".FormatWith(to, email.ErrorMessage), true);
                } else isTrue = true;
            }, config.Retries, config.Timeout, false, ex => {
                WriteLog("{0}����ʧ�ܣ�{1}".FormatWith(to, ex.ToExceptionDetail()), true);
            });
            return isTrue;
        }
        /// <summary>
        /// д��־
        /// </summary>
        /// <param name="message"></param>
        public void WriteLog(string message, bool iswrite = false) {
            if (config.UseLog || iswrite) {
                FileDirectory.FileWrite(config.LogPath, DateTime.Now.ToDateTime() + " - " + message);
            }
        }
        /// <summary>
        /// ȡǰ��������¼
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public IList<EmailList> SelectListByTop(int top) {
            string strSql = config.SelectListByTop.FormatWith(top);
            WriteLog(strSql);
            return Data.GetDbDataReader(strSql).ToList<EmailList>();
        }
        /// <summary>
        /// ��ID����ɾ������
        /// </summary>
        /// <param name="idlist">idlist</param>
        /// <returns></returns>
        public int DeleteByIDList(string idlist) {
            if (idlist.IsNullEmpty()) return 0;
            string strSql = config.DeleteByIDList.FormatWith(idlist);
            WriteLog(strSql);
            return Data.ExecSql(strSql);
        }
        /// <summary>
        /// ����ʧ�ܵļ�¼����EmailSendHistory��
        /// </summary>
        /// <param name="mail">EmailList</param>
        /// <returns></returns>
        public bool InsertSendHistry(EmailList mail) {
            string strSql = config.InsertSendHistry.FormatWith(mail.Email, mail.Body, mail.Subject);
            WriteLog(strSql);
            return Data.ExecSql(strSql) > 0 ? true : false;
        }
        /// <summary>
        /// ��������ʼ�
        /// </summary>
        /// <returns></returns>
        public int ClearExpireEmail() {
            string strSql = config.ClearExpireEmail.FormatWith(DateTime.Now.AddDays(-config.ExpireDay).ToDateTime());
            WriteLog(strSql);
            return Data.ExecSql(strSql);
        }
    }
    /// <summary>
    /// �ʼ�ʵ����
    /// </summary>
    public class EmailList {
        private int? emailID = null;
        /// <summary>
        /// �ʼ����
        /// </summary>
        public new int? EmailID { get { return emailID; } set { emailID = value; } }
        private string email = null;
        /// <summary>
        /// �ʼ���ַ
        /// </summary>
        public new string Email { get { return email; } set { email = value; } }
        private string subject = null;
        /// <summary>
        /// �ʼ�����
        /// </summary>
        public new string Subject { get { return subject; } set { subject = value; } }
        private string body = null;
        /// <summary>
        /// �ʼ�����
        /// </summary>
        public new string Body { get { return body; } set { body = value; } }
    }
    /// <summary>
    /// Web.config ������Ϣ
    /// </summary>
    public class ConfigInfo {
        /// <summary>
        /// ��־�ļ�·��
        /// </summary>
        public string LogPath { get; set; }
        /// <summary>
        /// smtp��������ַ
        /// </summary>
        public string SmtpServer { get; set; }
        /// <summary>
        /// smtp�˺�
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// smtp����
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// �����˵�ַ
        /// </summary>
        public string FromAddress { get; set; }
        /// <summary>
        /// �߳���
        /// </summary>
        public int AmountThread { get; set; }
        /// <summary>
        /// ÿ�ζ���¼��
        /// </summary>
        public int RecordCount { get; set; }
        /// <summary>
        /// ʱ����
        /// </summary>
        public int TimeInterval { get; set; }
        /// <summary>
        /// �˿�
        /// </summary>
        public int SmtpPort { get; set; }
        /// <summary>
        /// ��HTML����
        /// </summary>
        public bool IsBodyHtml { get; set; }
        /// <summary>
        /// Ssl
        /// </summary>
        public bool Ssl { get; set; }
        /// <summary>
        /// ������־
        /// </summary>
        public bool UseLog { get; set; }
        /// <summary>
        /// �ط�����
        /// </summary>
        public int Retries { get; set; }
        /// <summary>
        /// �ط���ʱ�����룩
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// ֻ�������������ʼ���ɾ�����ڵ��ʼ�
        /// </summary>
        public int ExpireDay { get; set; }
        public string SelectListByTop { get; set; }
        public string DeleteByIDList { get; set; }
        public string InsertSendHistry { get; set; }
        public string ClearExpireEmail { get; set; }
        public string EmailProviderName { get; set; }
    }
}