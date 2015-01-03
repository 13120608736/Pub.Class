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
using System.Threading;

namespace Pub.Class {
    /// <summary>
    /// IList��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class IListExtensions {
        /// <summary>
        /// �Ƿ��
        /// </summary>
        /// <param name="self">List��չ</param>
        /// <returns>true/false</returns>
        public static bool IsNullEmpty(this IList self) { return self.IsNull() || self.Count == 0; }
        /// <summary>
        /// ����
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <typeparam name="K">����ֵ����</typeparam>
        /// <param name="list">IList&lt;T>��չ</param>
        /// <param name="function">Funcί�к���</param>
        /// <returns>IList&lt;K></returns>
        public static IList<K> Map<T, K>(this IList<T> list, Func<T, K> function) {
            var newList = new List<K>(list.Count);
            for (var i = 0; i < list.Count; ++i) newList.Add(function(list[i]));
            return newList;
        }
        /// <summary>
        /// ��IList&lt;T>��ָ���з����IList&lt;IList&lt;T>>
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="list">List��չ</param>
        /// <param name="Count">����</param>
        /// <returns>���б�</returns>
        public static IList<IList<T>> Split<T>(this IList<T> list, int Count) {
            var currentList = new List<T>(Count);
            var returnList = new List<IList<T>>();
            for (int i = 0, j = list.Count; i < j; i++) {
                if (currentList.Count < Count) {
                    currentList.Add(list[i]);
                } else {
                    returnList.Add(currentList);
                    currentList = new List<T>(Count);
                    currentList.Add(list[i]);
                }
            }
            returnList.Add(currentList);
            return returnList;
        }
        /// <summary>
        /// x y�Ե�
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="obj">List��չ</param>
        /// <param name="x">xλ��</param>
        /// <param name="y">yλ��</param>
        public static void Swap<T>(this IList<T> obj, int x, int y) {
            if (x != y) {
                T xValue = obj[x];
                obj[x] = obj[y];
                obj[y] = xValue;
            }
        }
        /// <summary>
        /// BinarySearch ����������б��У����ö��ֲ����ҵ�Ŀ�����б��е�λ�á�
        /// ����պ��и�Ԫ����Ŀ����ȣ��򷵻�true����minIndex�ᱻ�����Ԫ�ص�λ�ã����򣬷���false����minIndex�ᱻ�����Ŀ��С����ӽ�Ŀ���Ԫ�ص�λ��
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="sortedList">List��չ</param>
        /// <param name="target">ֵ</param>
        /// <param name="minIndex">��Сλ��</param>
        /// <returns></returns>
        public static bool BinarySearch<T>(this IList<T> sortedList, T target, out int minIndex) where T : IComparable {
            if (target.CompareTo(sortedList[0]) == 0) {
                minIndex = 0;
                return true;
            }

            if (target.CompareTo(sortedList[0]) < 0) {
                minIndex = -1;
                return false;
            }

            if (target.CompareTo(sortedList[sortedList.Count - 1]) == 0) {
                minIndex = sortedList.Count - 1;
                return true;
            }

            if (target.CompareTo(sortedList[sortedList.Count - 1]) > 0) {
                minIndex = sortedList.Count - 1;
                return false;
            }

            //int targetPosIndex = -1;
            int left = 0;
            int right = sortedList.Count - 1;

            while (right - left > 1) {
                int middle = (left + right) / 2;

                if (target.CompareTo(sortedList[middle]) == 0) {
                    minIndex = middle;
                    return true;
                }

                if (target.CompareTo(sortedList[middle]) < 0) {
                    right = middle;
                } else {
                    left = middle;
                }
            }

            minIndex = left;
            return false;
        }
        /// <summary>
        /// GetIntersection ��Ч��������ListԪ�صĽ�����
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="list1">List��չ</param>
        /// <param name="list2">list2</param>
        /// <returns></returns>
        public static List<T> GetIntersection<T>(this List<T> list1, List<T> list2) where T : IComparable {
            List<T> largList = list1.Count > list2.Count ? list1 : list2;
            List<T> smallList = largList == list1 ? list2 : list1;

            largList.Sort();
            int minIndex = 0;

            List<T> result = new List<T>();
            foreach (T tmp in smallList) {
                if (largList.BinarySearch<T>(tmp, out minIndex)) {
                    result.Add(tmp);
                }
            }

            return result;
        }
        /// <summary>
        /// GetUnion ��Ч��������ListԪ�صĲ�����
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="list1">List��չ</param>
        /// <param name="list2">list2</param>
        /// <returns></returns>
        public static List<T> GetUnion<T>(this IList<T> list1, IList<T> list2) {
            SortedDictionary<T, int> result = new SortedDictionary<T, int>();
            foreach (T tmp in list1) {
                if (!result.ContainsKey(tmp)) result.Add(tmp, 0);
            }

            foreach (T tmp in list2) {
                if (!result.ContainsKey(tmp)) result.Add(tmp, 0);
            }

            return result.Keys.CopyAllToList<T>().ToList();
        }
        /// <summary>
        /// ����̲߳���ִ��
        /// </summary>
        /// <param name="tasks">������</param>
        public static void InParallel(this List<ThreadStart> tasks) {
            InParallel(tasks, int.MaxValue);
        }
        /// <summary>
        /// ����̲߳���ִ��
        /// </summary>
        /// <param name="tasks">������</param>
        /// <param name="maxThreads">�߳���</param>
        public static void InParallel(this List<ThreadStart> tasks, int maxThreads) {
            new ThreadPoolEx().Execute(maxThreads, tasks);
        }
        /// <summary>
        /// �������˳��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> Rand<T>(this IList<T> list) { return list.OrderBy(p => Guid.NewGuid()).ToList(); }
        /// <summary>
        /// LIST����תΪURL����
        /// </summary>
        /// <param name="param">LIST����</param>
        /// <returns>URL����</returns>
        public static string ToUrl(this IList<UrlParameter> param) {
            StringBuilder ParameString = new StringBuilder();
            foreach (UrlParameter par in param) ParameString.AppendFormat("{0}={1}&", par.ParameterName, par.ParameterValue);
            ParameString.RemoveLastChar("&");
            return ParameString.ToString();
        }
        /// <summary>
        /// LIST����תΪURL����
        /// </summary>
        /// <param name="param">LIST����</param>
        /// <returns>URL����</returns>
        public static string ToUrlEncode(this IList<UrlParameter> param) {
            StringBuilder ParameString = new StringBuilder();
            foreach (UrlParameter par in param) ParameString.AppendFormat("{0}={1}&", par.ParameterName, par.ParameterValue.UrlEncode());
            ParameString.RemoveLastChar("&");
            return ParameString.ToString();
        }
        /// <summary>
        /// �����
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="list">IList�б�</param>
        /// <param name="item">ֵ</param>
        /// <returns>IList�б�</returns>
        public static IList<T> Add<T>(this IList<T> list, T item) {
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
        public static IList<T> AddUnique<T>(this IList<T> list, T item) {
            lock (((ICollection)list).SyncRoot) { if (!list.Contains(item)) list.Add(item); }
            return list;
        }
        /// <summary>
        /// ��������˳��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list) {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
