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
    public class SerializeBytes {
        private readonly ISerializeBytes serializeBytes;
        /// <summary>
        /// ������ ָ��DLL�ļ���ȫ����
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        public SerializeBytes(string dllFileName, string className) {
            errorMessage = string.Empty;
            if (serializeBytes.IsNull()) {
                serializeBytes = (ISerializeBytes)dllFileName.LoadClass(className);
            }
        }
        /// <summary>
        /// ������ ָ��classNameDllName(SerializeBytesProviderName) Ĭ��Pub.Class.JavaScriptSerializerString
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        public SerializeBytes(string classNameAndAssembly) {
            errorMessage = string.Empty;
            if (serializeBytes.IsNull()) {
                if (classNameAndAssembly.IsNullEmpty())
                    serializeBytes = Singleton<BinaryFormatterBytes>.Instance();
                else
                    serializeBytes = (ISerializeBytes)classNameAndAssembly.LoadClass();
            }
        }
        /// <summary>
        /// ������ ��Web.config�ж�SerializeBytesProviderName Ĭ��Pub.Class.SimpleSerializeBytes,Pub.Class
        /// </summary>
        public SerializeBytes() {
            errorMessage = string.Empty;
            if (serializeBytes.IsNull()) {
                string classNameAndAssembly = WebConfig.GetApp("SerializeBytesProviderName");
                if (classNameAndAssembly.IsNullEmpty())
                    serializeBytes = Singleton<BinaryFormatterBytes>.Instance();
                else
                    serializeBytes = (ISerializeBytes)classNameAndAssembly.LoadClass();
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
        public byte[] Serialize<T>(T o) {
            errorMessage = string.Empty;
            try {
                return serializeBytes.Serialize(o);
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return null;
            }
        }
        ///<summary>
        /// �����л�
        ///</summary>
        public T Deserialize<T>(byte[] data) {
            errorMessage = string.Empty;
            try {
                return serializeBytes.Deserialize<T>(data);
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
                serializeBytes.SerializeFile(o, fileName);
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
                return serializeBytes.DeserializeFile<T>(fileName);
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return default(T);
            }
        }
        ///<summary>
        /// ���л� DES����
        ///</summary>
        public byte[] SerializeEncode<T>(T o, string key = "") {
            errorMessage = string.Empty;
            try {
                return serializeBytes.SerializeEncode(o, key);
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return null;
            }
        }
        ///<summary>
        /// �����л� DES����
        ///</summary>
        public T DecodeDeserialize<T>(byte[] data, string key = "") {
            errorMessage = string.Empty;
            try {
                return serializeBytes.DecodeDeserialize<T>(data, key);
            } catch (Exception ex) {
                errorMessage = ex.ToExceptionDetail();
                return default(T);
            }
        }

        private static ISerializeBytes s_serializeBytes;
        /// <summary>
        /// ʹ���ⲿ���
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        public static void Use(string dllFileName, string className) {
            s_serializeBytes = (ISerializeBytes)dllFileName.LoadClass(className);
        }
        /// <summary>
        /// ʹ���ⲿ���
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        public static void Use(string classNameAndAssembly) {
            if (classNameAndAssembly.IsNullEmpty())
                s_serializeBytes = Singleton<BinaryFormatterBytes>.Instance();
            else
                s_serializeBytes = (ISerializeBytes)classNameAndAssembly.LoadClass();
        }
        /// <summary>
        /// ʹ���ⲿ���
        /// </summary>
        public static void Use<T>(T t) where T : ISerializeBytes, new() {
            s_serializeBytes = Singleton<T>.Instance();
        }

        ///<summary>
        /// ���л�
        ///</summary>
        public static byte[] ToBytes<T>(T o) {
            try {
                if (s_serializeBytes.IsNull()) {
                    string classNameAndAssembly = WebConfig.GetApp("SerializeBytesProviderName");
                    Use(classNameAndAssembly);
                }
                return s_serializeBytes.Serialize(o);
            } catch (Exception ex) {
                throw ex;
            }
        }
        ///<summary>
        /// �����л�
        ///</summary>
        public static T FromBytes<T>(byte[] data) {
            try {
                if (s_serializeBytes.IsNull()) {
                    string classNameAndAssembly = WebConfig.GetApp("SerializeBytesProviderName");
                    Use(classNameAndAssembly);
                }
                return s_serializeBytes.Deserialize<T>(data);
            } catch (Exception ex) {
                throw ex;
            }
        }

        public static void RegisterTypes(params Type[] types) { 
            try {
                if (s_serializeBytes.IsNull()) {
                    string classNameAndAssembly = WebConfig.GetApp("SerializeBytesProviderName");
                    Use(classNameAndAssembly);
                }
                s_serializeBytes.RegisterTypes(types);
            } catch (Exception ex) {
                throw ex;
            }
        }
    }
}
