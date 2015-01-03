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
    public class SerializeXml {
        private readonly ISerializeString serializeString;
        /// <summary>
        /// ������ ָ��DLL�ļ���ȫ����
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        public SerializeXml(string dllFileName, string className) {
            errorMessage = string.Empty;
            if (serializeString.IsNull()) {
                serializeString = (ISerializeString)dllFileName.LoadClass(className);
            }
        }
        /// <summary>
        /// ������ ָ��classNameDllName(SerializeXmlProviderName) Ĭ��Pub.Class.JavaScriptSerializerString
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        public SerializeXml(string classNameAndAssembly) {
            errorMessage = string.Empty;
            if (serializeString.IsNull()) {
                if (classNameAndAssembly.IsNullEmpty())
                    serializeString = Singleton<XmlSerializerString>.Instance();
                else
                    serializeString = (ISerializeString)classNameAndAssembly.LoadClass();
            }
        }
        /// <summary>
        /// ������ ��Web.config�ж�SerializeXmlProviderName Ĭ��Pub.Class.SimpleSerializeString,Pub.Class
        /// </summary>
        public SerializeXml() {
            errorMessage = string.Empty;
            if (serializeString.IsNull()) {
                string classNameAndAssembly = WebConfig.GetApp("SerializeXmlProviderName");
                if (classNameAndAssembly.IsNullEmpty())
                    serializeString = Singleton<XmlSerializerString>.Instance();
                else
                    serializeString = (ISerializeString)classNameAndAssembly.LoadClass();
            }
        }
        private string errorMessage = string.Empty;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string ErrorMessage { get { return errorMessage; } }
        ///<summary>
        /// ���л�
        ///</summary>
        public string Serialize<T>(T o) {
            errorMessage = string.Empty;
            try {
                return serializeString.Serialize(o);
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return null;
            }
        }
        ///<summary>
        /// �����л�
        ///</summary>
        public T Deserialize<T>(string data) {
            errorMessage = string.Empty;
            try {
                return serializeString.Deserialize<T>(data);
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return default(T);
            }
        }
        ///<summary>
        /// ���л��ļ�
        ///</summary>
        public void SerializeFile<T>(T o, string fileName) {
            errorMessage = string.Empty;
            try {
                serializeString.SerializeFile(o, fileName);
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
            }
        }
        ///<summary>
        /// �����л��ļ�
        ///</summary>
        public T DeserializeFile<T>(string fileName) {
            errorMessage = string.Empty;
            try {
                return serializeString.DeserializeFile<T>(fileName);
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return default(T);
            }
        }
        ///<summary>
        /// ���л� DES����
        ///</summary>
        public string SerializeEncode<T>(T o, string key = "") {
            errorMessage = string.Empty;
            try {
                return serializeString.SerializeEncode(o, key);
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return null;
            }
        }
        ///<summary>
        /// �����л� DES����
        ///</summary>
        public T DecodeDeserialize<T>(string data, string key = "") {
            errorMessage = string.Empty;
            try {
                return serializeString.DecodeDeserialize<T>(data, key);
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return default(T);
            }
        }

        private static ISerializeString s_serializeString;
        /// <summary>
        /// ʹ���ⲿ���
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        public static void Use(string dllFileName, string className) {
            s_serializeString = (ISerializeString)dllFileName.LoadClass(className);
        }
        /// <summary>
        /// ʹ���ⲿ���
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        public static void Use(string classNameAndAssembly) {
            if (classNameAndAssembly.IsNullEmpty())
                s_serializeString = Singleton<XmlSerializerString>.Instance();
            else
                s_serializeString = (ISerializeString)classNameAndAssembly.LoadClass();
        }
        /// <summary>
        /// ʹ���ⲿ���
        /// </summary>
        public static void Use<T>(T t) where T : ISerializeString, new() {
            s_serializeString = Singleton<T>.Instance();
        }

        ///<summary>
        /// ���л�
        ///</summary>
        public static string ToXml<T>(T o) {
            try {
                if (s_serializeString.IsNull()) {
                    string classNameAndAssembly = WebConfig.GetApp("SerializeXmlProviderName");
                    Use(classNameAndAssembly);
                }
                return s_serializeString.Serialize(o);
            } catch (Exception ex) {
                throw ex;
            }
        }
        ///<summary>
        /// �����л�
        ///</summary>
        public static T FromXml<T>(string data) {
            try {
                if (s_serializeString.IsNull()) {
                    string classNameAndAssembly = WebConfig.GetApp("SerializeXmlProviderName");
                    Use(classNameAndAssembly);
                }
                return s_serializeString.Deserialize<T>(data);
            } catch (Exception ex) {
                throw ex;
            }
        }
    }
}
