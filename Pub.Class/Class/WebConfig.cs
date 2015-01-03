//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.Configuration;

namespace Pub.Class {
    /// <summary>
    /// Web.Config������
    /// 
    /// �޸ļ�¼
    ///     2006.05.15 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class WebConfig {
        /// <summary>
        /// Web.Config ������DoMain����
        /// </summary>
        public static readonly string DoMain = WebConfig.GetApp("DoMain") ?? string.Empty;
        //#region GetApp
        /// <summary>
        /// ȡappSettings�������
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>����ֵ</returns>
        public static string GetApp(string key) {
            if (ConfigurationManager.AppSettings[key].IsNotNull()) return ConfigurationManager.AppSettings[key].ToString();
            return null;
        }
        /// <summary>
        /// ȡappSettings�������
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">Ĭ��ֵ</param>
        /// <returns>����ֵ</returns>
        public static T GetApp<T>(string key, T value) {
            if (ConfigurationManager.AppSettings[key].IsNotNull()) return ConfigurationManager.AppSettings[key].ToString().ConvertTo<T>();
            return default(T);
        }
        /// <summary>
        /// ȡappSettings�������
        /// </summary>
        /// <param name="i">����</param>
        /// <returns>ȡappSettings�������</returns>
        public static string GetApp(int i) {
            if (ConfigurationManager.AppSettings[i].IsNotNull()) return ConfigurationManager.AppSettings[i].ToString();
            return null;
        }
        /// <summary>
        /// ȡappSettings�������
        /// </summary>
        /// <returns>ȡappSettings�������</returns>
        public static NameValueCollection GetApp() {
            return ConfigurationManager.AppSettings;
        }
        /// <summary>
        /// �޸�appSettings������� ��������� ���
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">ȡappSettings�������</param>
        public static void SetApp(string key, string value) {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection section = (AppSettingsSection)config.GetSection("appSettings");
            if (section.Settings[key].IsNull()) {
                section.Settings.Add(key, value);
            } else {
                section.Settings[key].Value = value;
            }
            config.Save();
        }
        /// <summary>
        /// ɾ��һ��appSettings�ڵ�
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static void RemoveApp(string key) {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection section = config.AppSettings;
            if (section.Settings[key].IsNotNull()) section.Settings.Remove(key);
            config.Save();
        }
        //#endregion
        //#region GetConn
#if !MONO40
        /// <summary>
        /// ȡconnectionStrings�������
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>ȡconnectionStrings�������</returns>
        public static string GetConn(string key) {
            if (ConfigurationManager.ConnectionStrings[key].IsNotNull()) return ConfigurationManager.ConnectionStrings[key].ToString();
            return null;
        }
        /// <summary>
        /// ȡconnectionStrings�������
        /// </summary>
        /// <param name="i">����</param>
        /// <returns>ȡconnectionStrings�������</returns>
        public static string GetConn(int i) {
            if (ConfigurationManager.ConnectionStrings[i].IsNotNull()) return ConfigurationManager.ConnectionStrings[i].ToString();
            return null;
        }
        /// <summary>
        /// ȡconnectionStrings�������
        /// </summary>
        /// <returns>ȡconnectionStrings�������</returns>
        public static ConnectionStringSettingsCollection GetConn() {
            return ConfigurationManager.ConnectionStrings;
        }
        /// <summary>
        /// ����/��дһ�����ݿ����Ӵ�
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="connString">�����ַ���</param>
        /// <param name="providerName">���ݿ�����</param>
        /// <returns></returns>
        public static void SetConn(string key, string connString, string providerName) {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            ConnectionStringsSection section = config.ConnectionStrings;
            if (section.ConnectionStrings[key].IsNull()) {
                section.ConnectionStrings.Add(new ConnectionStringSettings(key, connString, providerName));
            } else {
                section.ConnectionStrings[key].ConnectionString = connString;
            }
            config.Save();
        }
#endif
        //#endregion
        //#region GetSection
        /// <summary>
        /// GetSection&lt;appSettings>("configuration/appSettings")
        /// </summary>
        /// <typeparam name="TReturn">��������</typeparam>
        /// <param name="sectionName">�ڵ�</param>
        /// <returns></returns>
        public static TReturn GetSection<TReturn>(string sectionName) {
            return (TReturn)ConfigurationManager.GetSection(sectionName);
        }
        //#endregion
    }
}
