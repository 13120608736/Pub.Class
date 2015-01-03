//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Pub.Class {
    /// <summary>
    /// Web�������
    /// 
    /// �޸ļ�¼
    ///     2009.11.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class WebCache : ICache2 {
        #region ������

        public WebCache() {
            HttpContext context = HttpContext.Current;
            if (context != null) { _cache = (Cache)context.Cache; } else { _cache = HttpRuntime.Cache; }
        }
        #endregion

        #region ˽�о�̬�ֶ�
        private readonly Cache _cache;
        /// <summary>
        /// ��������
        /// </summary>
        private int Factor = 5;
        #endregion

        #region ��̬����
        public IList<CachedItem> GetList() {
            IList<CachedItem> list = new List<CachedItem>();
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext()) {
                list.Add(new CachedItem(CacheEnum.Key.ToString(), CacheEnum.Value.GetType().ToString()));
            }
            return list;
        }
        /// <summary>
        /// �������û������� 
        /// </summary>
        /// <param name="cacheFactor"></param>
        public void ReSetFactor(int cacheFactor) { Factor = cacheFactor; }
        /// <summary>
        /// ������л�����Ŀ
        /// </summary>
        public void Clear() {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext()) { al.Add(CacheEnum.Key); }

            foreach (string key in al) { _cache.Remove(key); }
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="pattern">���������ƥ��ģʽ</param>
        public void RemoveByPattern(string pattern) {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            while (CacheEnum.MoveNext()) {
                if (regex.IsMatch(CacheEnum.Key.ToString())) _cache.Remove(CacheEnum.Key.ToString());
            }
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="key">�������</param>
        public void Remove(string key) { _cache.Remove(key); }
        /// <summary>
        /// ���ӻ�����Ŀ 
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        public void Insert(string key, object obj) { Insert(key, obj, null, 1); }
        /// <summary>
        /// ���ӻ�����Ŀ
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="seconds">��������</param>
        public void Insert(string key, object obj, int seconds) { Insert(key, obj, null, seconds); }
        /// <summary>
        /// ���ӻ�����Ŀ
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="seconds">��������</param>
        /// <param name="priority">�������ȼ�</param>
        public void Insert(string key, object obj, int seconds, CacheItemPriority priority) { Insert(key, obj, null, seconds, priority); }
        /// <summary>
        /// ���ӻ�����Ŀ
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="dep">����������</param>
        /// <param name="seconds">��������</param>
        public void Insert(string key, object obj, CacheDependency dep, int seconds) { Insert(key, obj, dep, seconds, CacheItemPriority.Normal); }
        /// <summary>
        /// ���ӻ���
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="dep">����������</param>
        /// <param name="seconds">��������</param>
        /// <param name="priority">�������ȼ�</param>
        public void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority) {
            if (obj != null) {
                _cache.Insert(key, obj, dep, DateTime.Now.AddSeconds(Factor * seconds), Cache.NoSlidingExpiration, priority, null);
            }
        }
        /// <summary>
        /// ΢С����
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="secondFactor">����������</param>
        public void MicroInsert(string key, object obj, int secondFactor) {
            if (obj != null) {
                _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(Factor * secondFactor), Cache.NoSlidingExpiration);
            }
        }

        /// <summary>
        /// ���ӻ�����Ŀ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Max(string key, object obj) { Max(key, obj, null); }
        /// <summary>
        /// ��󻺴����
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="dep">����������</param>
        public void Max(string key, object obj, CacheDependency dep) {
            if (obj != null) {
                _cache.Insert(key, obj, dep, DateTime.MaxValue, Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
            }
        }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        public object Get(string key) { return _cache[key]; }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        public T Get<T>(string key) { return (T)_cache[key]; }
        /// <summary>
        /// ���Ƿ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>true/false</returns>
        public bool ContainsKey(string key) { return _cache[key].IsNotNull(); }
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
