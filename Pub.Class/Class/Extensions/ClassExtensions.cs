//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
using System.Xml.Linq;
using System.Web.Script.Serialization;
#endif
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;
using System.Drawing;

namespace Pub.Class {
    /// <summary>
    /// �෽����չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class ClassExtensions {
        /// <summary>
        /// ʵ���� ToJson ɸѡ�����ֶ�
        /// </summary>
        /// <example>
        /// <code>
        /// Msg.WriteEnd(LMS_TaskDetailFactory.Instance().SelectByID(id).ToJson(p => new { p.TaskID, p.TaskName }));
        /// </code>
        /// </example>
        /// <typeparam name="TSource">Դ����</typeparam>
        /// <typeparam name="TResult">�������</typeparam>
        /// <param name="entity">Դ</param>
        /// <param name="selector">ѡ����</param>
        /// <returns>�ַ���</returns>
        public static string ToJson<TSource, TResult>(this TSource entity, Func<TSource, TResult> selector) {
            return selector(entity).ToJson();
            //List<TSource> oblist = new List<TSource>();
            //oblist.Add(entity);
            //string str = oblist.Select(selector).ToJson();
            //return str.Substring(1, str.Length - 2);
        }
        /// <summary>
        /// ʵ���� ɸѡ�����ֶ�
        /// </summary>
        /// <example>
        /// <code>
        /// var t = LMS_TaskDetailFactory.Instance().SelectByID(id).Select(p => new { p.TaskID, p.TaskName }));
        /// </code>
        /// </example>
        /// <typeparam name="TSource">Դ����</typeparam>
        /// <typeparam name="TResult">�������</typeparam>
        /// <param name="entity">Դ</param>
        /// <param name="selector">ѡ����</param>
        /// <returns>ʵ����</returns>
        public static TResult SelectEntity<TSource, TResult>(this TSource entity, Func<TSource, TResult> selector) {
            return selector(entity);
            //List<TSource> oblist = new List<TSource>();
            //oblist.Add(entity);
            //return oblist.Select(selector).ToList()[0];
        }
        /// <summary>
        /// T �Ƿ��� ������ "333".EqualsAny("111", "222", "333")
        /// </summary>
        /// <example>
        /// <code>
        /// "333".EqualsAny("111", "222", "333")
        /// </code>
        /// </example>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="obj">����ֵ</param>
        /// <param name="values">params ����</param>
        /// <returns>true/false</returns>
        public static bool In<T>(this T obj, params T[] values) {
            return (Array.IndexOf(values, obj) != -1);
        }
        /// <summary>
        /// T �Ƿ� ��minValue/maxValue ֮�����minValue/maxValue "222".IsBetween("111", "333")
        /// </summary>
        /// <example>
        /// <code>
        /// "222".IsBetween("111", "333")
        /// </code>
        /// </example>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="value">����ֵ</param>
        /// <param name="minValue">��Сֵ</param>
        /// <param name="maxValue">���ֵ</param>
        /// <returns>true/false</returns>
        public static bool IsBetween<T>(this T value, T minValue, T maxValue) where T : IComparable<T> {
            return IsBetween(value, minValue, maxValue, null);
        }
        /// <summary>
        /// T �Ƿ� ��minValue/maxValue ֮�����minValue/maxValue 
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="value">����ֵ</param>
        /// <param name="minValue">��Сֵ</param>
        /// <param name="maxValue">���ֵ</param>
        /// <param name="comparer"></param>
        /// <returns>true/false</returns>
        public static bool IsBetween<T>(this T value, T minValue, T maxValue, IComparer<T> comparer) where T : IComparable<T> {
            comparer = comparer ?? Comparer<T>.Default;

            var minMaxCompare = comparer.Compare(minValue, maxValue);
            if (minMaxCompare < 0) {
                return ((comparer.Compare(value, minValue) >= 0) && (comparer.Compare(value, maxValue) <= 0));
            } else if (minMaxCompare == 0) {
                return (comparer.Compare(value, minValue) == 0);
            } else {
                return ((comparer.Compare(value, maxValue) >= 0) && (comparer.Compare(value, minValue) <= 0));
            }
        }
        /// <summary>
        /// �����¡
        /// </summary>
        /// <example>
        /// <code>
        /// "test".Clone&lt;string>();
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="o">ֵ</param>
        /// <returns>�¶���</returns>
        public static T Clone<T>(this T o) where T : class {
            Type type = o.GetType().BaseType;

            MethodInfo[] methodInfos = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            MethodInfo cloneMethod = null;
            foreach (var item in methodInfos) {
                if (item.Name == "MemberwiseClone") {
                    cloneMethod = item;
                    break;
                }
            }
            if (cloneMethod.IsNotNull()) return (T)cloneMethod.Invoke(o, null);
            return default(T);
        }
        /// <summary>
        /// �������д����ֽ�
        /// </summary>
        /// <example>
        /// <code>
        /// "test".ToBinary&lt;string>();
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="o">ֵ</param>
        /// <returns>�ֽ�</returns>
        //public static byte[] ToBytes<T>(this T o) {
        //    return SerializeBytes.ToBytes<T>(o);
        //    //var formatter = new BinaryFormatter();
        //    //using (MemoryStream ms = new MemoryStream()) {
        //    //    formatter.Serialize(ms, o);
        //    //    return ms.ToArray();
        //    //}
        //}
        /// <summary>
        /// �������л���ѹ��
        /// </summary>
        /// <typeparam name="T">����/����</typeparam>
        /// <param name="o">�ֽ�</param>
        /// <returns>����</returns>
        public static byte[] ToBinaryDeflateCompress<T>(this T o) where T : class {
            return o.ToBytes<T>().DeflateCompress();
        }
        /// <summary>
        /// �ܵ� T���뺯��ִ�з���R "1".Pipe&lt;string, int>((s) => { return s.ToInt() + 1; })
        /// </summary>
        /// <example>
        /// <code>
        /// "1".Pipe&lt;string, int>((s) => { return s.ToInt() + 1; })
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <typeparam name="R">����ֵ����</typeparam>
        /// <param name="o">��ڲ���ֵ</param>
        /// <param name="action">����</param>
        /// <returns></returns>
        public static R Pipe<T, R>(this T o, Func<T, R> action) {
            //T buffer = o;
            return action(o);
        }
        /// <summary>
        /// �ܵ� T���붯��ִ�� "1".Pipe&lt;string>((s) => { s= s + 1; }) �����T
        /// </summary>
        /// <example>
        /// <code>
        /// "1".Pipe&lt;string>((s) => { s= s + 1; })
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="o">��ڲ���ֵ</param>
        /// <param name="action">����</param>
        /// <returns></returns>
        public static T Pipe<T>(this T o, Action<T> action) {
            //T buffer = o;
            action(o);
            return o;
        }
        /// <summary>
        /// Ĭ��ֵ
        /// </summary>
        /// <example>
        /// <code>
        /// 0.IsDefault&lt;int>();
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="value">����ֵ</param>
        /// <returns>true/false</returns>
        public static bool IsDefault<T>(this T value) { return Equals(value, default(T)); }
        /// <summary>
        /// ������� 1.If(i => i &lt; 0, 0)
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="obj">ֵ</param>
        /// <param name="predicate">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static T If<T>(this T obj, Predicate<T> predicate, T defaultValue = default(T)) {
            if (obj.IsNull()) return defaultValue;
            return predicate(obj) ? obj : defaultValue;
        }
        /// <summary>
        /// ��������� 1.NotIf(i => i &lt; 0, 0)
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="obj">ֵ</param>
        /// <param name="predicate">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static T NotIf<T>(this T obj, Predicate<T> predicate, T defaultValue = default(T)) {
            if (obj.IsNull()) return defaultValue;
            return predicate(obj) ? defaultValue : obj;
        }
        /// <summary>
        /// ������� If 1.If(i => i &lt; 0, i => { })
        /// </summary>
        /// <example>
        /// <code>
        /// If 1.If(i => i &lt; 0, i => { })
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="t">ֵ</param>
        /// <param name="predicate">����</param>
        /// <param name="action">����</param>
        public static void If<T>(this T t, Predicate<T> predicate, Action<T> action) where T : class { if (predicate(t)) action(t); }
        /// <summary>
        /// ������� If int val = 1.If(i => i &lt; 0, i => 0)
        /// </summary>
        /// <example>
        /// <code>
        /// If int val = 1.If(i => i &lt; 0, i => 0)
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="t">ֵ</param>
        /// <param name="predicate">����</param>
        /// <param name="func">����</param>
        /// <returns></returns>
        public static T If<T>(this T t, Predicate<T> predicate, Func<T, T> func) where T : struct { return predicate(t) ? func(t) : t; }
        /// <summary>
        /// �������  1.If(i => i &lt; 0, i => { }, i => { })
        /// </summary>
        /// <example>
        /// <code>
        /// 1.If(i => i &lt; 0, i => { }, i => { })
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="t">ֵ</param>
        /// <param name="predicate">����</param>
        /// <param name="action1">����1</param>
        /// <param name="action2">����2</param>
        public static void If<T>(this T t, Predicate<T> predicate, Action<T> action1, Action<T> action2) where T : class { if (predicate(t)) action1(t); else action2(t); }
        /// <summary>
        /// ������� int val = 1.If(i => i &lt; 0, i => 0, i => i)
        /// </summary>
        /// <example>
        /// <code>
        /// int val = 1.If(i => i &lt; 0, i => 0, i => i)
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="t">ֵ</param>
        /// <param name="predicate">����</param>
        /// <param name="func1">����1</param>
        /// <param name="func2">����2</param>
        /// <returns></returns>
        public static T If<T>(this T t, Predicate<T> predicate, Func<T, T> func1, Func<T, T> func2) where T : struct { return predicate(t) ? func1(t) : func2(t); }
        /// <summary>
        /// Whileѭ������ 1.While(i => i &lt; 0, i => { Msg.WriteEnd(i); })
        /// </summary>
        /// <example>
        /// <code>
        /// 1.While(i => i &lt; 0, i => { Msg.WriteEnd(i); })
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="t">ֵ</param>
        /// <param name="predicate">����</param>
        /// <param name="action">����</param>
        public static void While<T>(this T t, Predicate<T> predicate, Action<T> action) where T : class { while (predicate(t)) action(t); }
#if !NET20
        /// <summary>
        /// T ���л��� XDocument
        /// </summary>
        /// <example>
        /// <code>
        /// "test".ToXDocument&lt;string>();
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="obj">ֵ</param>
        /// <returns>XDocument</returns>
        public static XDocument ToXDocument<T>(this T obj) {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            XDocument xdoc = new XDocument();
            using (XmlWriter w = xdoc.CreateWriter()) xs.Serialize(w, obj);
            return xdoc;
        }
#endif
        /// <summary>
        /// Color ת16�����ַ���
        /// </summary>
        /// <param name="c">Color</param>
        /// <returns>�ַ���</returns>
        public static string ToHex(this Color c) {
            string f = String.Format("{0:X2}", Convert.ToInt32(c.R));
            f += String.Format("{0:X2}", Convert.ToInt32(c.G));
            f += String.Format("{0:X2}", Convert.ToInt32(c.B));
            return f;
        }
        /// <summary>
        /// ����ѯ  ������Ů�Լ������var p1 = people.GetDescendants(p => p.IsMale ? p.Children : Enumerable.Empty&lt;People>(), p =>p.IsMale);
        /// </summary>
        /// <example>
        /// <code>
        /// public abstract class People{
        ///     public bool IsMale { get; private set; }
        ///     public abstract IEnumerable&lt;People> Children { get; }
        /// }
        /// ������Ů�Լ������var p1 = people.GetDescendants(p => p.IsMale ? p.Children : Enumerable.Empty&lt;People>(), p =>p.IsMale);
        /// ������Ů�Լ������var p1 = people.GetDescendants(p => p.IsMale ? p.Children : Enumerable.Empty&lt;People>(), p =>p.IsMale);
        /// ����Ů�Ե�������������: var p2 = people.GetDescendants(p => p.IsMale ? p.Children : Enumerable.Empty&lt;People>(), null);
        /// ��ȡ��������: var descendants = people.GetDescendants(p => p.Children, null);
        /// ��ȡ����������� var males = people.GetDescendants(p => p.Children, p => p.IsMale);
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="root">ֵ</param>
        /// <param name="childSelector">����</param>
        /// <param name="filter">����</param>
        /// <returns></returns>
        public static IEnumerable<T> GetDescendants<T>(this T root, Func<T, IEnumerable<T>> childSelector, Predicate<T> filter) {
            foreach (T t in childSelector(root)) {
                if (filter.IsNull() || filter(t)) yield return t;
                foreach (T child in GetDescendants((T)t, childSelector, filter)) yield return child;
            }
        }
        /// <summary>
        /// ����ѯ 
        /// </summary>
        /// <example>
        /// <code>
        /// ��ȡTreeView�������ԡ��ơ���β�������: var treeViewNode = treeView1.GetDescendants(treeView => treeView.Nodes.Cast&lt;TreeNode>(),treeNode => treeNode.Nodes.Cast&lt;TreeNode>(),treeNode => treeNode.Text.EndsWith("��"));
        /// </code>
        /// </example>
        /// <typeparam name="TRoot">������</typeparam>
        /// <typeparam name="T">����</typeparam>
        /// <param name="root">��ֵ</param>
        /// <param name="rootChildSelector">����</param>
        /// <param name="childSelector">��ѡ��</param>
        /// <param name="filter">����</param>
        /// <returns></returns>
        public static IEnumerable<T> GetDescendants<TRoot, T>(this TRoot root, Func<TRoot, IEnumerable<T>> rootChildSelector, Func<T, IEnumerable<T>> childSelector, Predicate<T> filter) {
            foreach (T t in rootChildSelector(root)) {
                if (filter.IsNull() || filter(t)) yield return t;
                foreach (T child in GetDescendants(t, childSelector, filter)) yield return child;
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="c">IEnumerable</param>
        /// <param name="t">ֵ</param>
        /// <returns></returns>
        public static bool In<T>(this T t, IEnumerable<T> c) {
            return c.Any(i => i.Equals(t));
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <typeparam name="TSource">Դ����</typeparam>
        /// <param name="first">ֵ</param>
        /// <param name="right">IEnumerator</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Concat<TSource>(this TSource first, IEnumerator<TSource> right) {
            yield return first;
            while (right.MoveNext()) yield return right.Current;
        }
        /// <summary>
        /// �������Ϊnull����ú���ί�в����غ���ί�еķ���ֵ�����򷵻ض�����
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="func">����Ϊnullʱ���ڵ��õĺ���ί��</param>
        /// <returns>�������Ϊnull�򷵻ض��������򷵻�<paramref name="func"/>����ί�еķ���ֵ</returns>
        /// <example>
        /// <code>
        /// string v = null;
        /// string d = v.IfNull&lt;string&gt;(()=>"v is null");  //d = "v is null";
        /// string t = d.IfNull(() => "d is null");              //t = "v is null";
        /// </code>
        /// </example>
        public static T IfNull<T>(this T obj, Func<T> func) where T : class {
            if (obj.IsNull() || obj.IsDBNull()) {
                return func.IsNull() ? default(T) : func();
            } else {
                return obj;
            }
        }
        /// <summary>
        /// �����
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>object</returns>
        public static object IfNull(this object obj, object defaultValue) {
            return obj.IsNull() || obj.IsDBNull() ? defaultValue : obj;
        }
        /// <summary>
        /// �����
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="obj">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>T</returns>
        public static T IfNull<T>(this T obj, T defaultValue) {
            return obj.IsNull() || obj.IsDBNull() ? defaultValue : obj;
        }
        /// <summary>
        /// ��ʾ����������������
        /// </summary>
        /// <example>
        /// <code>
        /// var root = new User {
        ///     UserID = 1000,
        ///     Name = "�ܻ���",
        ///     Child = new User {
        ///         UserID = 1000,
        ///         Name = "�ܻ���1",
        ///         Child = new User {
        ///             UserID = 1000,
        ///             Name = "�ܻ���11",
        /// 
        ///         }
        ///     }
        /// };
        /// Msg.WriteEnd(root.Dump("root").Replace(" ","nbsp;").ReplaceRNToBR());
        /// </code>
        /// </example>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="value">����</param>
        /// <param name="name">����</param>
        /// <returns></returns>
        //public static string Dump<T>(this T value, string name) {
        //    using (var writer = new StringWriter(System.Globalization.CultureInfo.InvariantCulture)) {
        //        Dump(value, name, writer);
        //        return writer.ToString();
        //    }
        //}
        /// <summary>
        /// ��ʾ����������������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="value">����</param>
        /// <param name="name">����</param>
        /// <param name="writer">writer</param>
        /// <returns></returns>
        //public static T Dump<T>(this T value, string name, TextWriter writer) {
        //    if (name.IsNullEmpty()) throw new ArgumentNullException("name");
        //    if (writer.IsNull()) throw new ArgumentNullException("writer");
        //    Dumper.Dump(value, name, writer);
        //    return value;
        //}
        /// <summary>
        /// ��ʾ����������������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="value">����</param>
        /// <param name="name">����</param>
        /// <returns></returns>
        //public static T DebugDump<T>(this T value, string name) {
        //    if (name.IsNullEmpty()) throw new ArgumentNullException("name");
        //    using (var writer = new DebugWriter()) {
        //        return Dump(value, name, writer);
        //    }
        //}
        ///// <summary>
        ///// T ���л��� XML�ַ���
        ///// </summary>
        ///// <example>
        ///// <code>
        ///// "test".ToXml&lt;string>();
        ///// </code>
        ///// </example>
        ///// <typeparam name="T">����</typeparam>
        ///// <param name="obj">ֵ</param>
        ///// <returns>XML�ַ���</returns>
        //public static string ToXml<T>(this T obj) {
        //    string s = null;
        //    using (MemoryStream ms = new MemoryStream(1000)) {
        //        XmlSerializer serializer = new XmlSerializer(typeof(T));
        //        XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
        //        xmlnsEmpty.Add("", "");
        //        serializer.Serialize(ms, obj, xmlnsEmpty);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        using (StreamReader reader = new StreamReader(ms)) {
        //            s = reader.ReadToEnd();
        //        }
        //        ms.Close();
        //        serializer = null;
        //    }
        //    return s;
        //}
    }
}
