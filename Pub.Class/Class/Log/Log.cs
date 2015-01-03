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
    /// д��־
    /// 
    /// �޸ļ�¼
    ///     2013.02.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Log {
        private readonly ILog log;
        /// <summary>
        /// ������ ָ��DLL�ļ���ȫ����
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        public Log(string dllFileName, string className) {
            errorMessage = string.Empty;
            if (log.IsNull()) {
                log = (ILog)dllFileName.LoadClass(className);
            }
        }
        /// <summary>
        /// ������ ָ��classNameDllName(LogProviderName) Ĭ��Pub.Class.SimpleLog,Pub.Class
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        public Log(string classNameAndAssembly) {
            errorMessage = string.Empty;
            if (log.IsNull()) {
                if (classNameAndAssembly.IsNullEmpty())
                    log = Singleton<SimpleLog>.Instance();
                else
                    log = (ILog)classNameAndAssembly.LoadClass();
            }
        }
        /// <summary>
        /// ������ ��Web.config�ж�LogProviderName Ĭ��Pub.Class.SimpleLog,Pub.Class
        /// </summary>
        public Log() {
            errorMessage = string.Empty;
            if (log.IsNull()) {
                string classNameAndAssembly = WebConfig.GetApp("LogProviderName");
                if (classNameAndAssembly.IsNullEmpty())
                    log = Singleton<SimpleLog>.Instance();
                else
                    log = (ILog)classNameAndAssembly.LoadClass();
            }
        }
        private string errorMessage = string.Empty;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string ErrorMessage { get { return errorMessage; } }
        ///<summary>
        /// д��־
        ///</summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="encoding">����</param>
        ///<returns>true/false</returns>
        public bool Write(string msg, Encoding encoding = null) {
            errorMessage = string.Empty;
            try {
                log.Write(msg, encoding ?? Encoding.UTF8);
                return true;
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return false;
            }
        }
        /// <summary>
        /// д��־
        /// </summary>
        /// <param name="ex">��Ϣ</param>
        /// <param name="encoding">����</param>
        /// <returns>true/false</returns>
        public bool Write(Exception ex, Encoding encoding = null) {
            return Write(ex.ToExceptionDetail(), encoding);
        }
        /// <summary>
        /// д��־
        /// </summary>
        /// <param name="msgFormat">��Ϣ��ʽ</param>
        /// <param name="values">��Ϣ</param>
        /// <returns>true/false</returns>
        public bool Write(string msgFormat, params string[] values) {
            return Write(string.Format(msgFormat, values));
        }

        private static ILog s_log;
        /// <summary>
        /// ʹ���ⲿ���д��־
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        public static void Use(string dllFileName, string className) {
            s_log = (ILog)dllFileName.LoadClass(className);
        }
        /// <summary>
        /// ʹ���ⲿ���д��־
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        public static void Use(string classNameAndAssembly) {
            if (classNameAndAssembly.IsNullEmpty())
                s_log = HttpContext.Current.IsNull() ? (ILog)Singleton<TraceLog>.Instance() : (ILog)Singleton<SimpleLog>.Instance();
            else
                s_log = (ILog)classNameAndAssembly.LoadClass();
        }
        /// <summary>
        /// ʹ���ⲿ���
        /// </summary>
        public static void Use<T>(T t) where T : ILog, new() {
            s_log = Singleton<T>.Instance();
        }
        ///<summary>
        /// д��־
        ///</summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="encoding">����</param>
        ///<returns>true/false</returns>
        public static bool WriteLog(string msg, Encoding encoding = null) {
            if (s_log.IsNull()) {
                string classNameAndAssembly = WebConfig.GetApp("LogProviderName");
                Use(classNameAndAssembly);
            }
            return s_log.Write(msg, encoding ?? Encoding.UTF8);
        }
        /// <summary>
        /// д��־
        /// </summary>
        /// <param name="ex">��Ϣ</param>
        /// <param name="encoding">����</param>
        /// <returns>true/false</returns>
        public static bool WriteLog(Exception ex, Encoding encoding = null) {
            return WriteLog(ex.ToExceptionDetail(), encoding);
        }
        /// <summary>
        /// д��־
        /// </summary>
        /// <param name="msgFormat">��Ϣ��ʽ</param>
        /// <param name="values">��Ϣ</param>
        /// <returns>true/false</returns>
        public static bool WriteLog(string msgFormat, params string[] values) {
            return WriteLog(string.Format(msgFormat, values));
        }
    }
}
