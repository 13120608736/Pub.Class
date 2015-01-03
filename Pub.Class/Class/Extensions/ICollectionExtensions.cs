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
using System.Collections;
using Microsoft.VisualBasic;

namespace Pub.Class {
    /// <summary>
    /// ICollection��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class ICollectionExtensions {
        /// <summary>
        /// IsNullEmpty
        /// </summary>
        /// <param name="self">ICollection��չ</param>
        /// <returns>true/false</returns>
        public static bool IsNullEmpty(this ICollection self) { return self.IsNull() || self.Count == 0; }
        /// <summary>
        /// �����
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="list">IList�б�</param>
        /// <param name="item">ֵ</param>
        /// <returns>IList�б�</returns>
        public static ICollection<T> Add<T>(this ICollection<T> list, T item) {
            list.Add(item);
            return list;
        }
        /// <summary>
        /// ���Ψһ��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="list">IList�б�</param>
        /// <param name="item">ֵ</param>
        /// <returns>IList�б�</returns>
        public static ICollection<T> AddUnique<T>(this ICollection<T> list, T item) {
            lock (((ICollection)list).SyncRoot) { if (!list.Contains(item)) list.Add(item); }
            return list;
        }
        /// <summary>
        /// ���Ψһ�� 
        /// </summary>
        /// <example>
        /// <code>
        /// IList&lt;string> list2 = new List&lt;string>(); 
        /// list2.AddUnique&lt;string>("test1"); 
        /// list.AddUnique&lt;string>(list2);
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="collection">ICollection��չ</param>
        /// <param name="values">ֵ</param>
        /// <returns>true/false</returns>
        public static ICollection<T> AddUnique<T>(this ICollection<T> collection, IEnumerable<T> values) {
            foreach (var value in values) collection.AddUnique<T>(value);
            return collection;
        }
    }
}
