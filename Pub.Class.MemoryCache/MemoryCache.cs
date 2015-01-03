//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
using System.IO;

namespace Pub.Class {
    /// <summary>
    /// Memory�������
    /// 
    /// �޸ļ�¼
    ///     2009.11.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class MemoryCache : ICache2 {
        #region ������
        public MemoryCache() { }
        #endregion

        #region ˽�о�̬�ֶ�
#if NET20
        private static readonly ISafeDictionary<string, CachedItem> cacheList = new SafeDictionary<string, CachedItem>();
#else
        private static readonly ISafeDictionary<string, CachedItem> cacheList = new SafeDictionarySlim<string, CachedItem>();
#endif
        /// <summary>
        /// ��������
        /// </summary>
        private int Factor = 5;
        #endregion

        #region ��̬����
        public IList<CachedItem> GetList() {
            IList<CachedItem> list = new List<CachedItem>();
            foreach (string s in cacheList.Keys) { list.Add(cacheList[s]); }
            return list;
        }
        /// <summary>
        /// ������л�����Ŀ
        /// </summary>
        public void Clear() {
            cacheList.Clear();
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="pattern">���������ƥ��ģʽ</param>
        public void RemoveByPattern(string pattern) {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            foreach (string key in cacheList.Keys) {
                if (regex.IsMatch(key)) cacheList.Remove(key);
            }
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="key">�������</param>
        public void Remove(string key) {
            if (cacheList.ContainsKey(key)) cacheList.Remove(key);
        }
        /// <summary>
        /// ���ӻ�����Ŀ 
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        public void Insert(string key, object obj) { Insert(key, obj, 1); }
        /// <summary>
        /// ���ӻ�����Ŀ
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="seconds">��������</param>
        public void Insert(string key, object obj, int seconds) {
            Remove(key);
            CachedItem item = new CachedItem();
            item.StartTime = DateTime.Now;
            item.EndTime = DateTime.Now.AddSeconds(seconds * Factor);
            item.CacheData = obj;
            item.CacheType = obj.GetType().ToString();
            item.CacheKey = key;
            cacheList.Add(key, item);
        }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        public object Get(string key) {
            if (!cacheList.ContainsKey(key)) return null;
            CachedItem item = cacheList[key];
            return DateTime.Now.IsBetween(item.StartTime, item.EndTime) ? item.CacheData : null;
        }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        public T Get<T>(string key) {
            if (!cacheList.ContainsKey(key)) return default(T);
            CachedItem item = cacheList[key];
            return DateTime.Now.IsBetween(item.StartTime, item.EndTime) ? (T)item.CacheData : default(T);
        }
        /// <summary>
        /// ���Ƿ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>true/false</returns>
        public bool ContainsKey(string key) { return cacheList.ContainsKey(key); }
        /// <summary>
        /// ����ѹ��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <param name="obj">ֵ</param>
        /// <param name="seconds">������</param>
        public void Compress<T>(string key, T obj, int seconds) where T : class {
            Insert(key, obj.ToBytes().DeflateCompress(), seconds);
        }
        /// <summary>
        /// ����ѹ��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <param name="obj">ֵ</param>
        public void Compress<T>(string key, T obj) where T : class {
            Compress<T>(key, obj, 1);
        }

        /// <summary>
        /// �����ѹ
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <returns></returns>
        public T Decompress<T>(string key) where T : class {
            return ((byte[])Get(key)).DeflateDecompress().FromBytes<T>();
        }
        #endregion
    }
}
