//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
using System.Web.Script.Serialization;
#endif
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace Pub.Class {
    /// <summary>
    /// object��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class ObjectExtensions {
        /// <summary>
        /// ToJson
        /// </summary>
        /// <example>
        /// <code>
        /// Msg.Write("test".ToJson());
        /// </code>
        /// </example>
        /// <param name="obj">object��չ</param>
        /// <returns>json�ַ���</returns>
        public static string ToJson<T>(this T obj) {
            //return ToJson(obj, null);
            return SerializeJson.ToJson<T>(obj);
        }
        /// <summary>
        /// ToJson
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <param name="jsonConverters"></param>
        /// <returns>json�ַ���</returns>
        //public static string ToJson(this object obj, IEnumerable<JavaScriptConverter> jsonConverters) {
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    serializer.MaxJsonLength = int.MaxValue;
        //    if (jsonConverters.IsNotNull()) serializer.RegisterConverters(jsonConverters ?? new JavaScriptConverter[0]);
        //    return serializer.Serialize(obj);
        //}
        /// <summary>
        /// ��ȫSQL
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>��ȫ��SQLֵ</returns>
        public static string SafeSql(this object obj) {
            return obj.IsNull() || obj.IsDBNull() ? string.Empty : obj.ToString().SafeSql();
        }
        /// <summary>
        /// ��ԭSafeSqlת��֮�������
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>��ȫ��SQLֵ</returns>
        public static string ShowSafeSql(this object obj) { 
            return obj.IsNull() || obj.IsDBNull() ? string.Empty : obj.ToString().ShowSafeSql();
        }
        /// <summary>
        /// ��ȫSQL ֻ����'
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>ֻ����'</returns>
        public static string SafeSqlSimple(this object obj) {
            return obj.IsNull() || obj.IsDBNull() ? string.Empty : obj.ToString().SafeSqlSimple();
        }
        /// <summary>
        /// ����ת�� ���ܵ�
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value">object��չ</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object value) { return value.ConvertTo(default(T)); }
        /// <summary>
        /// ����ת�� ���ܵ�
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value">object��չ</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object value, T defaultValue) {
            if (value.IsNotNull()) {
                var targetType = typeof(T);

                var converter = TypeDescriptor.GetConverter(value);
                if (converter.IsNotNull()) {
                    if (converter.CanConvertTo(targetType)) return (T)converter.ConvertTo(value, targetType);
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter.IsNotNull()) {
                    try { if (converter.CanConvertFrom(value.GetType())) return (T)converter.ConvertFrom(value); } catch { return default(T); }
                }
            }
            return defaultValue;
        }
        /// <summary>
        /// ����ת�� ���ܵ�
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value">object��չ</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <param name="ignoreException">����</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object value, T defaultValue, bool ignoreException) {
            if (ignoreException) {
                try {
                    return value.ConvertTo<T>();
                } catch {
                    return defaultValue;
                }
            }
            return value.ConvertTo<T>();
        }
        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static int ToInt(this object strValue, int defValue) { return strValue.IsNull() || strValue.IsDBNull() ? defValue : (int)strValue; }
        /// <summary>
        /// ToTinyInt
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static byte ToTinyInt(this object strValue, byte defValue) { return strValue.IsNull() || strValue.IsDBNull() ? defValue : (byte)strValue; }
        /// <summary>
        /// ToSmallInt
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static short ToSmallInt(this object strValue, short defValue) { return strValue.IsNull() || strValue.IsDBNull() ? defValue : (short)strValue; }
        /// <summary>
        /// ToDecimal
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object strValue, decimal defValue) { return strValue.IsNull() || strValue.IsDBNull() ? defValue : (decimal)strValue; }
        /// <summary>
        /// ToFloat
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static float ToFloat(this object strValue, float defValue) {  return strValue.IsNull() || strValue.IsDBNull() ? defValue : (float)strValue; }
        /// <summary>
        /// ToBigInt
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static Int64 ToBigInt(this object strValue, Int64 defValue) {  return strValue.IsNull() || strValue.IsDBNull() ? defValue : (Int64)strValue; }
        /// <summary>
        /// ToMoney
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static decimal ToMoney(this object strValue, decimal defValue) { return strValue.IsNull() || strValue.IsDBNull() ? defValue : (decimal)strValue; }
        /// <summary>
        /// ToInteger
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static int ToInteger(this object strValue, int defValue) { return strValue.IsNull() || strValue.IsDBNull() ? defValue : (int)strValue; }
        /// <summary>
        /// ToBool
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <param name="defValue">Ĭ��ֵ</param>
        /// <returns>true/false</returns>
        public static bool ToBool(this object strValue, bool defValue) {             return strValue.IsNull() || strValue.IsDBNull() ? defValue : (bool)strValue;        }
        /// <summary>
        /// ToStr()
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static string ToStringEx(this object strValue) { return strValue.IsNull() || strValue.IsDBNull() ? string.Empty : (string)strValue; }
        /// <summary>
        /// ToStr()
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static string ToStr(this object strValue) { return strValue.IsNull() || strValue.IsDBNull() ? string.Empty : strValue.ToString().ToStr(); }
        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static int ToInt(this object strValue) { return strValue.ToInt(0); }
        /// <summary>
        /// ToTinyInt
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static byte ToTinyInt(this object strValue) { return strValue.ToTinyInt(0); }
        /// <summary>
        /// ToSmallInt
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static short ToSmallInt(this object strValue) { return strValue.ToSmallInt(0); }
        /// <summary>
        /// ToDecimal
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object strValue) { return strValue.ToDecimal(0); }
        /// <summary>
        /// ToFloat
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static float ToFloat(this object strValue) { return strValue.ToFloat(0); }
        /// <summary>
        /// ToBigInt
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static Int64 ToBigInt(this object strValue) { return strValue.ToBigInt(0); }
        /// <summary>
        /// ToMoney
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static decimal ToMoney(this object strValue) { return strValue.ToMoney(0); }
        /// <summary>
        /// ToInteger
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static int ToInteger(this object strValue) { return strValue.ToInteger(0); }
        /// <summary>
        /// ToVarChar
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static string ToVarChar(this object strValue) { return strValue.ToStr(); }
        /// <summary>
        /// ToNVarChar
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns></returns>
        public static string ToNVarChar(this object strValue) { return strValue.ToStr(); }
        /// <summary>
        /// ToBool
        /// </summary>
        /// <param name="strValue">object��չ</param>
        /// <returns>true/false</returns>
        public static bool ToBool(this object strValue) { return strValue.ToBool(false); }
        /// <summary>
        /// IsArray �Ƿ�����
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>true/false</returns>
        public static bool IsArray(this object obj) {
            return obj.IsType(typeof(System.Array));
        }
        /// <summary>
        /// IsDBNull
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>true/false</returns>
        public static bool IsDBNull(this object obj) {
            return obj.IsType(typeof(DBNull));
        }
        /// <summary>
        /// IsAllNull
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>true/false</returns>
        public static bool IsAllNull(this object obj) {
            return obj.IsNull() || obj.IsDBNull();
        }
        /// <summary>
        /// IsNotAllNull
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>true/false</returns>
        public static bool IsNotAllNull(this object obj) {
            return !(obj.IsNull() || obj.IsDBNull());
        }
        /// <summary>
        /// IsNotDBNull
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>true/false</returns>
        public static bool IsNotDBNull(this object obj) {
            return !obj.IsDBNull();
        }
        /// <summary>
        /// IsDBNull
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="obj">object��չ</param>
        /// <param name="t">ֵ</param>
        /// <returns>true/false</returns>
        public static T IsDBNull<T>(this object obj, T t) {
            return obj.IsType(typeof(DBNull)) ? t : (T)obj;
        }
        /// <summary>
        /// IsType �����ж�
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <param name="type">����</param>
        /// <returns>true/false</returns>
        public static bool IsType(this object obj, Type type) {
            return obj.GetType().Equals(type);
        }
        /// <summary>
        /// IsNull �����Ƿ�null
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>true/false</returns>
        public static bool IsNull(this object obj) {
            return null == obj;
        }
        /// <summary>
        /// IsNull �����Ƿ�null
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>true/false</returns>
        public static bool IsNullEmpty(this object obj) {
            return null == obj;
        }
        /// <summary>
        /// IsNull �����Ƿ�null
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="obj">object��չ</param>
        /// <param name="t">Դ����ֵ</param>
        /// <returns>true/false</returns>
        public static T IsNull<T>(this object obj, T t) {
            return obj.IsNull() ? t : (T)obj;
        }
        /// <summary>
        /// IsNotNull �����Ƿ��null
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>true/false</returns>
        public static bool IsNotNull(this object obj) {
            return !obj.IsNull();
        }
        /// <summary>
        /// ��Ϊ�յİ�ȫ�Ƚ�
        /// </summary>
        /// <param name="o1">object��չ</param>
        /// <param name="o2">����</param>
        /// <returns>true/false</returns>
        public static bool IsNotNullEquals(this object o1, object o2) {
            return ((o1 == o2) || ((o1.IsNotNull()) && o1.Equals(o2)));
        }
        /// <summary>
        /// ��̬���÷���
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <param name="methodName">������</param>
        /// <param name="parameters">����</param>
        /// <returns>��̬���÷���</returns>
        public static object InvokeMethod(this object obj, string methodName, params object[] parameters) {
            return InvokeMethod<object>(obj, methodName, parameters);
        }
        /// <summary>
        /// ��̬���÷���
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="obj">object��չ</param>
        /// <param name="methodName">������</param>
        /// <returns>��̬���÷���</returns>
        public static T InvokeMethod<T>(this object obj, string methodName) {
            return InvokeMethod<T>(obj, methodName, null);
        }
        /// <summary>
        /// ��̬���÷���
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="obj">object��չ</param>
        /// <param name="methodName">������</param>
        /// <param name="parameters">����</param>
        /// <returns>��̬���÷���</returns>
        public static T InvokeMethod<T>(this object obj, string methodName, params object[] parameters) {
            var type = obj.GetType();
            var method = type.GetMethod(methodName);

            if (method.IsNull()) throw new ArgumentException(string.Format("�Ҳ���������{0}����", methodName), methodName);

            var value = method.Invoke(obj, parameters);
            return (value is T ? (T)value : default(T));
        }
        /// <summary>
        /// ȡ���Ե�ֵ
        /// </summary>
        /// <example>
        /// <code>
        /// CachedItem item = new CachedItem();
        /// item.SetPropertyValue("CacheKey", "test");
        /// Msg.Write(item.GetPropertyValue("CacheKey"));
        /// </code>
        /// </example>
        /// <param name="obj">object��չ</param>
        /// <param name="propertyName">������</param>
        /// <returns>ȡ���Ե�ֵ</returns>
        public static object GetPropertyValue(this object obj, string propertyName) {
            return GetPropertyValue<object>(obj, propertyName, null);
        }
        /// <summary>
        /// ȡ���Ե�ֵ
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="obj">object��չ</param>
        /// <param name="propertyName">������</param>
        /// <returns>ȡ���Ե�ֵ</returns>
        public static T GetPropertyValue<T>(this object obj, string propertyName) {
            return GetPropertyValue<T>(obj, propertyName, default(T));
        }
        /// <summary>
        /// ȡ���Ե�ֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">object��չ</param>
        /// <param name="propertyName">������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>ȡ���Ե�ֵ</returns>
        public static T GetPropertyValue<T>(this object obj, string propertyName, T defaultValue) {
            var type = obj.GetType();
            var property = type.GetPropertyCache(propertyName);

            if (property.IsNull()) throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName), propertyName);

            var value = property.GetValue(obj, null);
            return (value is T ? (T)value : defaultValue);
        }
        /// <summary>
        /// ���ö����ֵ
        /// </summary>
        /// <example>
        /// <code>
        /// CachedItem item = new CachedItem();
        /// item.SetPropertyValue("CacheKey", "test");
        /// Msg.Write(item.GetPropertyValue("CacheKey"));
        /// </code>
        /// </example>
        /// <param name="obj">object��չ</param>
        /// <param name="propertyName">������</param>
        /// <param name="value">ֵ</param>
        public static void SetPropertyValue(this object obj, string propertyName, object value) {
            var type = obj.GetType();
            var property = type.GetPropertyCache(propertyName);

            if (property.IsNull()) throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName), propertyName);

            property.SetValue(obj, value, null);
        }
        /// <summary>
        /// ȡ���Ե�Attributeֵ
        /// </summary>
        /// <example>
        /// <code>
        /// (new CachedItem()).GetAttribute&lt;EntityInfo>().Name
        /// (typeof (CachedItem)).GetAttribute&lt;EntityInfo>().Name
        /// (typeof (CachedItem)).GetProperty("CacheKey").GetAttribute&lt;EntityInfo>().Name
        /// (typeof (CachedItem)).GetMember("CacheKey")[0].GetAttribute&lt;EntityInfo>().Name
        /// </code>
        /// </example>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="obj">object��չ</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this object obj) where T : Attribute {
            return GetAttribute<T>(obj, true);
        }
        /// <summary>
        /// ȡ���Ե�Attributeֵ
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="obj">object��չ</param>
        /// <param name="includeInherited">�̳�</param>
        /// <returns>ȡ���Ե�Attributeֵ</returns>
        public static T GetAttribute<T>(this object obj, bool includeInherited) where T : Attribute {
            if (obj is PropertyInfo) {
                var _attributes = (obj as PropertyInfo).GetCustomAttributes(typeof(T), includeInherited);
                if ((_attributes.IsNotNull()) && (_attributes.Length > 0)) {
                    return (_attributes[0] as T);
                }
                return null;
            } else if (obj is FieldInfo) {
                var _attributes = (obj as FieldInfo).GetCustomAttributes(typeof(T), includeInherited);
                if ((_attributes.IsNotNull()) && (_attributes.Length > 0)) {
                    return (_attributes[0] as T);
                }
                return null;
            } else if (obj is MemberInfo) {
                var _attributes = (obj as MemberInfo).GetCustomAttributes(typeof(T), includeInherited);
                if ((_attributes.IsNotNull()) && (_attributes.Length > 0)) {
                    return (_attributes[0] as T);
                }
                return null;
            } else if (obj is EventInfo) {
                var _attributes = (obj as EventInfo).GetCustomAttributes(typeof(T), includeInherited);
                if ((_attributes.IsNotNull()) && (_attributes.Length > 0)) {
                    return (_attributes[0] as T);
                }
                return null;
            } else if (obj is MethodInfo) {
                var _attributes = (obj as MethodInfo).GetCustomAttributes(typeof(T), includeInherited);
                if ((_attributes.IsNotNull()) && (_attributes.Length > 0)) {
                    return (_attributes[0] as T);
                }
                return null;
            }

            var type = (obj as Type ?? obj.GetType());
            var attributes = type.GetCustomAttributes(typeof(T), includeInherited);
            if ((attributes.IsNotNull()) && (attributes.Length > 0)) {
                return (attributes[0] as T);
            }
            return null;
        }
        /// <summary>
        /// ȡ���Ե�Attributeֵ
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="obj">object��չ</param>
        /// <returns>ȡ���Ե�Attributeֵ</returns>
        public static IEnumerable<T> GetAttributes<T>(this object obj) where T : Attribute {
            return GetAttributes<T>(obj);
        }
        /// <summary>
        /// ȡ���Ե�Attributeֵ
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="obj">object��չ</param>
        /// <param name="includeInherited">�̳�</param>
        /// <returns>ȡ���Ե�Attributeֵ</returns>
        public static IEnumerable<T> GetAttributes<T>(this object obj, bool includeInherited) where T : Attribute {
            if (obj is PropertyInfo) {
                foreach (var attribute in (obj as PropertyInfo).GetCustomAttributes(typeof(T), includeInherited)) {
                    if (attribute is T) yield return (T)attribute;
                }
            } else if (obj is FieldInfo) {
                foreach (var attribute in (obj as FieldInfo).GetCustomAttributes(typeof(T), includeInherited)) {
                    if (attribute is T) yield return (T)attribute;
                }
            } else if (obj is MemberInfo) {
                foreach (var attribute in (obj as MemberInfo).GetCustomAttributes(typeof(T), includeInherited)) {
                    if (attribute is T) yield return (T)attribute;
                }
            } else if (obj is EventInfo) {
                foreach (var attribute in (obj as EventInfo).GetCustomAttributes(typeof(T), includeInherited)) {
                    if (attribute is T) yield return (T)attribute;
                }
            } else if (obj is MethodInfo) {
                foreach (var attribute in (obj as MethodInfo).GetCustomAttributes(typeof(T), includeInherited)) {
                    if (attribute is T) yield return (T)attribute;
                }
            }

            var type = (obj as Type ?? obj.GetType());
            foreach (var attribute in type.GetCustomAttributes(typeof(T), includeInherited)) {
                if (attribute is T) yield return (T)attribute;
            }
        }
        /// <summary>
        /// ת����
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value">object��չ</param>
        /// <returns>ת����</returns>
        public static T ToType<T>(this object value) { return (T)value; }
        /// <summary>
        /// ���л����ֽ�
        /// </summary>
        /// <param name="value">object��չ</param>
        /// <returns>���л����ֽ�</returns>
        public static byte[] ToBytes<T>(this T o, bool compress = false) {
            return compress ? SerializeBytes.ToBytes<T>(o).DeflateCompress() : SerializeBytes.ToBytes<T>(o);
            //BinaryFormatter bf1 = new BinaryFormatter();
            //using (MemoryStream ms = new MemoryStream()) {
            //    bf1.Serialize(ms, value);
            //    return ms.ToArray();
            //}
        }
        /// <summary>
        /// CheckOnNull ����Ƿ�Ϊ�գ����Ϊ����ʾ�쳣
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <param name="parameterName">������</param>
        public static void CheckOnNull(this object obj, string parameterName) {
            if (obj.IsNull()) throw new ArgumentNullException(parameterName);
        }
        /// <summary>
        /// CheckOnNull ����Ƿ�Ϊ�գ����Ϊ����ʾ�쳣
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <param name="parameterName">������</param>
        /// <param name="message">��Ϣ</param>
        public static void CheckOnNull(this object obj, string parameterName, string message) {
            if (obj.IsNull()) throw new ArgumentNullException(parameterName, message);
        }
        /// <summary>
        /// UnsafeCast
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value">object��չ</param>
        /// <returns></returns>
        public static T UnsafeCast<T>(this object value) {
            return value.IsNull() ? default(T) : (T)value;
        }
        /// <summary>
        /// SafeCast
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value">object��չ</param>
        /// <returns></returns>
        public static T SafeCast<T>(this object value) {
            return value is T ? value.UnsafeCast<T>() : default(T);
        }
        /// <summary>
        /// InstanceOf ʵ��
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="value">object��չ</param>
        /// <returns></returns>
        public static bool InstanceOf<T>(this object value) {
            return value is T;
        }
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="o">object��չ</param>
        public static void Write(this object o) { Msg.Write(o); }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="o">object��չ</param>
        public static void WriteEnd(this object o) { Msg.WriteEnd(o); }
        /// <summary>
        /// ���л���MemoryStream
        /// </summary>
        /// <param name="request">object��չ</param>
        /// <returns>���л���MemoryStream</returns>
        public static MemoryStream ToMemoryStream(this object request) {
            BinaryFormatter serializer = new BinaryFormatter();
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            serializer.Serialize(memStream, request);
            return memStream;
        }
        /// <summary>
        /// ���л���XML�ļ�
        /// </summary>
        /// <param name="o">object��չ</param>
        /// <param name="fileName">���л���XML�ļ�</param>
        public static void ToXmlFile(this object o, string fileName) {
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            //if (!FileDirectory.FileExists(fileName)) return;
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write)) serializer.Serialize(stream, o);
        }
        /// <summary>
        /// ���л���XML�ַ���
        /// </summary>
        /// <param name="o">object��չ</param>
        /// <returns>���л���XML�ַ���</returns>
        public static string ToXml<T>(this T o) {
            return SerializeXml.ToXml<T>(o);
            //XmlSerializer serializer = new XmlSerializer(o.GetType());
            //StringBuilder stringBuilder = new StringBuilder();
            //using (TextWriter textWriter = new StringWriter(stringBuilder)) serializer.Serialize(textWriter, o);
            //return stringBuilder.ToString();
        }
        /// <summary>
        /// ����תXML�ַ���
        /// </summary>
        /// <param name="source">object��չ</param>
        /// <param name="encoding">����</param>
        /// <returns>����תXML�ַ���</returns>
        public static string ToXml(this object source, Encoding encoding) {
            using (MemoryStream stream = new MemoryStream()) {
                XmlSerializer serializer = new XmlSerializer(source.GetType());
                serializer.Serialize(stream, source);
                stream.Position = 0;
                return encoding.GetString(stream.ToArray());
            }
        }
        /// <summary>
        /// ����תSOAP�ַ���
        /// </summary>
        /// <param name="obj">object��չ</param>
        /// <returns>����תSOAP�ַ���</returns>
        //public static string ToSoap(this object obj) {
        //    using (MemoryStream Stream = new MemoryStream()) {
        //        SoapFormatter Serializer = new SoapFormatter();
        //        Serializer.Serialize(Stream, obj);
        //        Stream.Flush();
        //        return new UTF8Encoding().GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
        //    }
        //}
        /// <summary>
        /// ȡ��ϸ��Exception������Ϣ
        /// </summary>
        /// <param name="ex">object��չ</param>
        /// <returns>ȡ��ϸ��Exception������Ϣ</returns>
        public static string ToExceptionDetail(this Exception ex) {
            return Safe.Expand(ex);
        }
        /// <summary>
        /// ��ϸ��Exception������Ϣ д���ļ�
        /// </summary>
        /// <param name="ex">object��չ</param>
        /// <param name="fileName">�ļ��� c:\test.log</param>
        public static void ToFile(this Exception ex, string fileName) {
            FileDirectory.FileWrite(fileName, ex.ToExceptionDetail());
        }
        /// <summary>
        /// ����CACHE
        /// </summary>
        /// <param name="value">object��չ</param>
        /// <param name="key">����CACHE</param>
        public static void ToCache(this object value, string key) {
            Cache2.Insert(key, value);
        }
        /// <summary>
        /// ȡCACHE����
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="key">object��չ</param>
        /// <returns>ȡCACHE����</returns>
        public static T FromCache<T>(this string key) {
            return (T)Cache2.Get(key);
        }
        /// <summary>
        /// ������ת�ֽ�����
        /// </summary>
        /// <param name="obj">����</param>
        /// <returns></returns>
        public static byte[] StructToBytes(this object obj) {
            //�õ��ṹ���С
            int size = Marshal.SizeOf(obj);
            //����byte����
            byte[] bytes = new byte[size];
            //����ṹ���С���ڴ�ռ�
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //���ṹ�忼������õ��ڴ�ռ�
            Marshal.StructureToPtr(obj, structPtr, false);
            //���ڴ�ռ俼������
            Marshal.Copy(structPtr, bytes, 0, size);
            //�ͷſռ�
            Marshal.FreeHGlobal(structPtr);
            //��������
            return bytes;
        }
    }
}
