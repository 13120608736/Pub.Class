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

namespace Pub.Class {
    /// <summary>
    /// IDataReader��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class IDataReaderExtensions {
        /// <summary>
        /// ��� IDataReader To ʵ���б� IL
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToList&lt;UC_Member>(true);
        /// </code>
        /// </example>
        /// <typeparam name="TResult">ʵ��</typeparam>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="isClose">�Ƿ�ر�DataReader</param>
        /// <returns>ʵ���б�</returns>
        public static List<TResult> ToList<TResult>(this IDataReader dr, bool isClose) where TResult : class, new() {
            List<TResult> list = new List<TResult>();
            if (dr.IsNull()) return list;
            IDataReaderEntityBuilder<TResult> eblist = IDataReaderEntityBuilder<TResult>.CreateBuilder(dr);
            while (dr.Read()) list.Add(eblist.Build(dr));
            if (isClose) { dr.Close(); dr.Dispose(); dr = null; }
            return list;
        }
        /// <summary>
        /// ʵ��ת�� IL
        /// </summary>
        /// <typeparam name="TResult">ʵ��</typeparam>
        /// <param name="dr">IDataReader��չ</param>
        /// <returns>ʵ��</returns>
        public static TResult ToEntity<TResult>(this IDataReader dr) where TResult : class, new() {
            return dr.ToList<TResult>().FirstOrDefault();
        }
        /// <summary>
        /// ʵ��ת�� IL
        /// </summary>
        /// <typeparam name="TResult">ʵ��</typeparam>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="isClose">�Ƿ�ر�DataReader</param>
        /// <returns>ʵ��</returns>
        public static TResult ToEntity<TResult>(this IDataReader dr, bool isClose) where TResult : class, new() {
            return dr.ToList<TResult>(isClose).FirstOrDefault();
        }
        /// <summary>
        /// ��� IDataReader To ʵ���б� IL
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToList&lt;UC_Member>();
        /// </code>
        /// </example>
        /// <typeparam name="TResult">ʵ��</typeparam>
        /// <param name="dr">IDataReader��չ</param>
        /// <returns>ʵ���б�</returns>
        public static List<TResult> ToList<TResult>(this IDataReader dr) where TResult : class, new() {
            return dr.ToList<TResult>(true);
        }
        /// <summary>
        /// IDataReader To ʵ���б� ����CACHE 
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToList2&lt;UC_Member>(true);
        /// </code>
        /// </example>
        /// <typeparam name="TResult">ʵ��</typeparam>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="isClose">�Ƿ�ر�DataReader</param>
        /// <returns>ʵ���б�</returns>
        public static List<TResult> ToList2<TResult>(this IDataReader dr, bool isClose) where TResult : class, new() {
            //�������صļ���   
            List<TResult> oblist = new List<TResult>();
            if (dr.IsNull()) return oblist;

            List<string> drColumns = new List<string>();
            int len = dr.FieldCount;
            for (int j = 0; j < len; j++) drColumns.Add(dr.GetName(j).Trim());

            //����һ�����Ե��б�   
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //��ȡTResult������ʵ��  ��������   
            Type t = typeof(TResult);
            //���TResult �����е�Public ���� ���ҳ�TResult���Ժ�DataTable����������ͬ������(PropertyInfo) �����뵽�����б�    
            Array.ForEach<PropertyInfo>(t.GetPropertiesCache(), p => { if (drColumns.IndexOf(p.Name) != -1) prlist.Add(p); });

            while (dr.Read()) {
                //����TResult��ʵ��   
                TResult ob = new TResult();
                //�ҵ���Ӧ������  ����ֵ   
                prlist.ForEach(p => { if (dr[p.Name] != DBNull.Value) p.SetValue(ob, dr[p.Name], null); });
                //���뵽���صļ�����.   
                oblist.Add(ob);
            }
            if (isClose) { dr.Close(); dr.Dispose(); dr = null; }
            return oblist;
        }
        /// <summary>
        /// IDataReader To ʵ���б� ����CACHE 
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToList2&lt;UC_Member>();
        /// </code>
        /// </example>
        /// <typeparam name="TResult">ʵ��</typeparam>
        /// <param name="dr">IDataReader��չ</param>
        /// <returns></returns>
        public static List<TResult> ToList2<TResult>(this IDataReader dr) where TResult : class, new() {
            return dr.ToList2<TResult>(true);
        }
        /// <summary>
        /// IDataReader To ʵ���б� ���� 
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToList3&lt;UC_Member>(true);
        /// </code>
        /// </example>
        /// <typeparam name="TResult">ʵ��</typeparam>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="isClose">�Ƿ�ر�DataReader</param>
        /// <returns>ʵ���б�</returns>
        public static List<TResult> ToList3<TResult>(this IDataReader dr, bool isClose) where TResult : class, new() {
            List<TResult> list = new List<TResult>();
            if (dr.IsNull()) return list;
            int len = dr.FieldCount;

            while (dr.Read()) {
                TResult info = new TResult();
                for (int j = 0; j < len; j++) {
                    if (dr[j].IsNull() || string.IsNullOrEmpty(dr[j].ToString())) continue;
                    info.GetType().GetPropertyCache(dr.GetName(j).Trim()).SetValue(info, dr[j], null);
                }
                list.Add(info);
            }
            if (isClose) { dr.Close(); dr.Dispose(); dr = null; }
            return list;
        }
        /// <summary>
        /// IDataReader To ʵ���б� ���� 
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToList3&lt;UC_Member>();
        /// </code>
        /// </example>
        /// <typeparam name="TResult">ʵ��</typeparam>
        /// <param name="dr">IDataReader��չ</param>
        /// <returns>ʵ���б�</returns>
        public static List<TResult> ToList3<TResult>(this IDataReader dr) where TResult : class, new() {
            return dr.ToList3<TResult>(true);
        }
        public static IList<T> ToList4<T>(this IDataReader dr, bool isClose) where T : class,new() {
            List<T> list = new List<T>();
            if (dr == null) return list;
            PropertyInfo[] propertys = typeof(T).GetPropertiesCache(); //typeof(T).GetProperties();
            while (dr.Read()) {
                T _t = (T)Activator.CreateInstance(typeof(T));
                //�ҵ���Ӧ������  ����ֵ   
                propertys.ToList().ForEach(p => { if (dr[p.Name] != DBNull.Value) p.SetValue(_t, dr[p.Name], null); });
                //���뵽���صļ�����.   
                list.Add(_t);
            }
            if (isClose) { dr.Close(); dr.Dispose(); dr = null; }
            return list;
        }
        public static IList<T> ToList4<T>(this IDataReader dr) where T : class,new() {
            return dr.ToList4<T>(true);
        }
        /// <summary>
        /// IDataReader תJSON �ٶ����
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToDataReaderJson(true);
        /// </code>
        /// </example>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="isClose">�Ƿ�ر�DataReader</param>
        /// <returns>תJSON</returns>
        public static string ToDataReaderJson(this IDataReader dr, bool isClose) {
            if (dr.IsNull()) return "[]";
            StringBuilder jsonHtml = new StringBuilder();
            jsonHtml.Append("[");

            JavaScriptSerializerString serializer = new JavaScriptSerializerString();
            while (dr.Read()) {
                jsonHtml.Append("{");
                for (int i = 0, len = dr.FieldCount; i < len; i++) jsonHtml.AppendFormat("\"{0}\":{1},", dr.GetName(i), serializer.Serialize(dr[i]));
                jsonHtml.Remove(jsonHtml.Length - 1, 1);
                jsonHtml.Append("},");
            }
            if (isClose) { dr.Close(); dr.Dispose(); dr = null; }

            if (jsonHtml.Length > 1) jsonHtml.Remove(jsonHtml.Length - 1, 1);
            jsonHtml.Append("]");
            return jsonHtml.ToString();
        }
        /// <summary>
        /// IDataReader תJSON �ٶ����
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToJson(true);
        /// </code>
        /// </example>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="isClose">�Ƿ�ر�DataReader</param>
        /// <returns>תJSON</returns>
        public static string ToJson(this IDataReader dr, bool isClose) {
            return dr.ToDataReaderJson(isClose);
        }
        /// <summary>
        /// IDataReader תJSON �ٶ����
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToDataReaderJson();
        /// </code>
        /// </example>
        /// <param name="dr">IDataReader��չ</param>
        /// <returns>תJSON</returns>
        public static string ToDataReaderJson(this IDataReader dr) {
            return ToDataReaderJson(dr, true);
        }
        /// <summary>
        /// IDataReader תJSON �ٶ����
        /// </summary>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="fields">�ֶ�</param>
        /// <returns>תJSON</returns>
        public static string ToJson(this IDataReader dr, params string[] fields) {
            return dr.ToJson(fields, null);
        }
        /// <summary>
        /// IDataReader תJSON �ٶ����
        /// </summary>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="fields">�ֶ�</param>
        /// <param name="alias">����</param>
        /// <returns>תJSON</returns>
        public static string ToJson(this IDataReader dr, string[] fields, string[] alias) {
            if (dr.IsNull()) return "[]";
            if (alias.IsNull() || fields.Length != alias.Length) alias = fields;
            if (fields.IsNull() || fields.Length == 0) return dr.ToJson();

            JavaScriptSerializerString serializer = new JavaScriptSerializerString();
            StringBuilder jsonHtml = new StringBuilder();
            jsonHtml.Append("[");

            while (dr.Read()) {
                jsonHtml.Append("{");
                for (int i = 0, len = fields.Length; i < len; i++) jsonHtml.AppendFormat("\"{0}\":{1},", alias[i], serializer.Serialize(dr[fields[i]]));
                jsonHtml.Remove(jsonHtml.Length - 1, 1);
                jsonHtml.Append("},");
            }
            dr.Close(); dr.Dispose(); dr = null;

            if (jsonHtml.Length > 1) jsonHtml.Remove(jsonHtml.Length - 1, 1);
            jsonHtml.Append("]");
            return jsonHtml.ToString();
        }
        /// <summary>
        /// IDataReader תJSON �ٶ����
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToJson();
        /// </code>
        /// </example>
        /// <param name="dr">IDataReader��չ</param>
        /// <returns>תJSON</returns>
        public static string ToJson(this IDataReader dr) {
            return dr.ToDataReaderJson(true);
        }
        /// <summary>
        /// ȡfield��ֵ
        /// </summary>
        /// <typeparam name="T">ֵ����</typeparam>
        /// <param name="reader">IDataReader��չ</param>
        /// <param name="field">�ֶ�</param>
        /// <returns>ֵ</returns>
        public static T Get<T>(this IDataReader reader, string field) {
            return reader.Get<T>(field, default(T));
        }
        /// <summary>
        /// ȡfield��ֵ
        /// </summary>
        /// <typeparam name="T">ֵ����</typeparam>
        /// <param name="reader">IDataReader��չ</param>
        /// <param name="field">�ֶ�</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>ֵ</returns>
        public static T Get<T>(this IDataReader reader, string field, T defaultValue) {
            var value = reader[field];
            if (value == DBNull.Value) return defaultValue;
            return value.ConvertTo<T>(defaultValue);
        }
        /// <summary>
        /// תDataTable
        /// </summary>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="isClose">�Ƿ�ر�DataReader</param>
        /// <returns>תDataTable</returns>
        public static DataTable ToDataTable(this IDataReader dr, bool isClose) {
            DataTable dt = new DataTable();
            if (dr.IsNull()) return null;
            var fieldCount = dr.FieldCount;
            for (int i = 0; i < fieldCount; i++)
                dt.Columns.Add(dr.GetName(i).Replace("_", ""), dr.GetFieldType(i));

            while (dr.Read()) {
                var row = new object[fieldCount];
                dr.GetValues(row);
                dt.Rows.Add(row);
            }
            if (isClose) { dr.Close(); dr.Dispose(); dr = null; }
            return dt;
        }
        /// <summary>
        /// תDataTable
        /// </summary>
        /// <param name="dr">IDataReader��չ</param>
        /// <returns>תDataTable</returns>
        public static DataTable ToDataTable(this IDataReader dr) { return dr.ToDataTable(true); }
        /// <summary>
        /// ͨ��IDataReader���CSV��ʽ����
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToCSV(true);
        /// </code>
        /// </example>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="c">char</param>
        /// <param name="isClose">�Ƿ�ر�DataReader</param>
        /// <returns>CSV�ַ���</returns>
        public static string ToCSV(this IDataReader dr, char c = ',', bool isClose = true) {
            if (dr.IsNull()) return "";
            StringBuilder jsonHtml = new StringBuilder();

            for (int i = 0, len = dr.FieldCount; i < len; i++) jsonHtml.AppendFormat("{0}{1}", dr.GetName(i), c);
            jsonHtml.Remove(jsonHtml.Length - 1, 1);
            jsonHtml.Append("\n");

            while (dr.Read()) {
                for (int i = 0, len = dr.FieldCount; i < len; i++) jsonHtml.AppendFormat("{0}{1}", dr[i].ToString(), c);
                jsonHtml.Remove(jsonHtml.Length - 1, 1);
                jsonHtml.Append("\n");
            }
            if (isClose) { dr.Close(); dr.Dispose(); dr = null; }

            return jsonHtml.ToString();
        }
        /// <summary>
        /// ͨ��IDataReader���CSV��ʽ����
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToCSV();
        /// </code>
        /// </example>
        /// <param name="dr">IDataReader��չ</param>
        /// <returns>CSV�ַ���</returns>
        public static string ToCSV(this IDataReader dr) { return dr.ToCSV(',', true); }
        /// <summary>
        /// ͨ��IDataReader���CSV��ʽ����
        /// </summary>
        /// <example>
        /// <code>
        /// Data.GetDbDataReader("Select MemberID,RealName from UC_Member").ToCSV('\t');
        /// </code>
        /// </example>
        /// <param name="dr">IDataReader��չ</param>
        /// <param name="c">char</param>
        /// <returns>CSV�ַ���</returns>
        public static string ToCSV(this IDataReader dr, char c) { return dr.ToCSV(c, true); }
    }
}
