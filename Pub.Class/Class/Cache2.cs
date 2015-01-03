//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
using System.Reflection;

namespace Pub.Class {
    /// <summary>
    /// ��������ʵ����
    /// 
    /// �޸ļ�¼
    ///     2006.05.01 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    [Serializable]
    [EntityInfo("��������ʵ����")]
    public class CachedItem {
        /// <summary>
        /// ���캯��
        /// </summary>
        public CachedItem() { }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="type">����</param>
        public CachedItem(string key, string type) { this.CacheKey = key; this.CacheType = type; }
        private string cacheKey;
        /// <summary>
        /// �����
        /// </summary>
        [EntityInfo("�����")]
        public string CacheKey { get { return this.cacheKey; } set { this.cacheKey = value; } }
        private string cacheType;
        /// <summary>
        /// �������ݵ���������
        /// </summary>
        [EntityInfo("��������")]
        public string CacheType { get { return this.cacheType; } set { this.cacheType = value; } }
        private object cacheData;
        /// <summary>
        /// ��������
        /// </summary>
        [EntityInfo("����")]
        public object CacheData { get { return this.cacheData; } set { this.cacheData = value; } }
        private DateTime startTime;
        /// <summary>
        /// �������ݿ�ʼʱ��
        /// </summary>
        [EntityInfo("��ʼʱ��")]
        public DateTime StartTime { get { return this.startTime; } set { this.startTime = value; } }
        private DateTime endTime;
        /// <summary>
        /// �������ݽ���ʱ��
        /// </summary>
        [EntityInfo("����ʱ��")]
        public DateTime EndTime { get { return this.endTime; } set { this.endTime = value; } }
    }

    /// <summary>
    /// �������
    /// 
    /// �޸ļ�¼
    ///     2009.09.04 �汾��1.1 livexy �޸ĶԹ���CACHE��֧��
    ///     2006.05.01 �汾��1.0 livexy ��������
    /// 
    /// <code>
    /// <example>
    /// IList&lt;CachedItem> list = Cache2.GetList();
    /// Cache2.Insert("testCache", 1); 5��
    /// Msg.Write(Cache2.Get("testCache"));
    /// Cache2.Insert("testCache", 1, 2); 10��
    /// Cache2.Remove("testCache");
    /// Cache2.RemoveByPattern("(.+?)Cache");
    /// Cache2.Clear();
    /// Cache2.Compress&lt;int>("testCache", 1); ѹ����CACHE 5��
    /// Msg.Write(Cache2.Decompress&lt;int>("testCache"));
    /// </example>
    /// </code>
    /// </summary>
    public class Cache2 {
        //#region ��ֻ̬������
        /// <summary>
        /// ���û��� �����ΪWebCache/VelocityCache/MemcachedCache/MemcachedCache2
        /// </summary>
        private static readonly string PubClassCache = WebConfig.GetApp("PubClassCache") ?? "WebCache";
        /// <summary>
        /// ��ͬ�Ļ�����ò�ͬ��DLL
        /// </summary>
        private static ICache2 _cache = (ICache2)"Pub.Class.{0},Pub.Class.{0}".FormatWith(PubClassCache).LoadClass();
        //#endregion
        //#region ��̬����
        /// <summary>
        /// ȡ���л������
        /// </summary>
        /// <example>
        /// <code>
        /// IList&lt;CachedItem> list = Cache2.GetList();
        /// </code>
        /// </example>
        /// <returns>ȡ���л������</returns>
        public static IList<CachedItem> GetList() { return _cache.GetList(); }
        /// <summary>
        /// ������л�����Ŀ
        /// </summary>
        /// <example>
        /// <code>
        /// Cache2.Clear();
        /// </code>
        /// </example>
        public static void Clear() { _cache.Clear(); }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <example>
        /// <code>
        /// Cache2.RemoveByPattern("TC_Test_SelectMyTestList_(.+?)");
        /// Cache2.RemoveByPattern("TC_Test_SelectMyTestList_(d+)");
        /// Cache2.RemoveByPattern("TC_Test_SelectMyTestList_[\\s\\S]*");
        /// </code>
        /// </example>
        /// <param name="pattern">���������ƥ��ģʽ</param>
        public static void RemoveByPattern(string pattern) { _cache.RemoveByPattern(pattern); }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <example>
        /// <code>
        /// Cache2.Remove("TC_Test_SelectMyTestList_1");
        /// </code>
        /// </example>
        /// <param name="key">�������</param>
        public static void Remove(string key) { _cache.Remove(key); }
        /// <summary>
        /// ���ӻ�����Ŀ 
        /// </summary>
        /// <example>
        /// <code>
        /// Cache2.Insert("test2", 10);
        /// </code>
        /// </example>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        public static void Insert(string key, object obj) { _cache.Insert(key, obj); }
        /// <summary>
        /// ���ӻ�����Ŀ
        /// </summary>
        /// <example>
        /// <code>
        /// Cache2.Insert("test2", 10, 10);
        /// </code>
        /// </example>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="seconds">��������</param>
        public static void Insert(string key, object obj, int seconds) { _cache.Insert(key, obj, seconds); }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <example>
        /// <code>
        /// Cache2.Get("test2");
        /// AC_Ask ask = (AC_Ask)Cache2.Get("AC_AskCache_SelectByID_1");
        /// </code>
        /// </example>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        public static object Get(string key) { return _cache.Get(key); }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <example>
        /// <code>
        /// Cache2.Get("test2");
        /// AC_Ask ask = (AC_Ask)Cache2.Get("AC_AskCache_SelectByID_1");
        /// </code>
        /// </example>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        public static T Get<T>(string key) { return ContainsKey(key) ? _cache.Get<T>(key) : default(T); }
        /// <summary>
        /// ���Ƿ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>true/false</returns>
        public static bool ContainsKey(string key) { return _cache.ContainsKey(key); }
        /// <summary>
        /// ����ѹ��
        /// </summary>
        /// <example>
        /// <code>
        /// Cache2.Compress&lt;IList&lt;CachedItem>>("test", new List&lt;CachedItem>() { new CachedItem() { CacheKey = "1", CacheType = "string" }, new CachedItem() { CacheKey = "1", CacheType = "string" } });
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <param name="obj">ֵ</param>
        public static void Compress<T>(string key, T obj) where T : class  { _cache.Compress<T>(key, obj); }
        /// <summary>
        /// ����ѹ��
        /// </summary>
        /// <example>
        /// <code>
        /// Cache2.Compress&lt;IList&lt;CachedItem>>("test", new List&lt;CachedItem>() { new CachedItem() { CacheKey = "1", CacheType = "string" }, new CachedItem() { CacheKey = "1", CacheType = "string" } }, 100);
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <param name="obj">ֵ</param>
        /// <param name="seconds">��������</param>
        public static void Compress<T>(string key, T obj, int seconds) where T : class  { _cache.Compress<T>(key, obj, seconds); }
        /// <summary>
        /// �����ѹ
        /// </summary>
        /// <example>
        /// <code>
        /// IList&lt;CachedItem> list = Cache2.Decompress&lt;IList&lt;CachedItem>>("test");
        /// </code>
        /// </example>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <returns>�����ѹ</returns>
        public static T Decompress<T>(string key) where T : class  { return _cache.Decompress<T>(key); }
        //#endregion
        /// <summary>
        /// ����дCACHE
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="key">��</param>
        /// <param name="acquire">����</param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> acquire) {
            return Get(key, 1, acquire);
        }
        /// <summary>
        /// ����дCACHE
        /// </summary>
        /// <example>
        /// <code>
        /// var list = Cache2.Get&lt;IList&lt;CachedItem>>(key, 1440, () => {
        ///     return Cache2.GetList();
        /// })
        /// </code>
        /// </example>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="key">��</param>
        /// <param name="cacheTime">ʱ��</param>
        /// <param name="acquire">����</param>
        /// <returns></returns>
        public static T Get<T>(string key, int cacheTime, Func<T> acquire) {
            if (Cache2.ContainsKey(key)) return Cache2.Get<T>(key);

            T result = acquire();
            _cache.Insert(key, result, cacheTime);
            return result;
        }
        /// <summary>
        /// ɾ��CACHE
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="delCache"></param>
        public static void Remove(string prefix, string[] delCache) {
            if (delCache.IsNull()) return;
            foreach (string s in delCache) {
                if (s.IndexOf("(") == -1 || s.IndexOf("[") == -1)
                    Cache2.Remove(s.IndexOf("Cache_") == -1 ? prefix + s : s);
                else
                    Cache2.RemoveByPattern(s.IndexOf("Cache_") == -1 ? "(" + prefix + s + ")" : s);
            }
        }
        /// <summary>
        /// ʹ���ⲿ���
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        public static void Use(string dllFileName, string className) {
            _cache = (ICache2)dllFileName.LoadClass(className);
        }
        /// <summary>
        /// ʹ���ⲿ���
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        public static void Use(string classNameAndAssembly) {
            if (classNameAndAssembly.IsNullEmpty())
                _cache = (ICache2)"Pub.Class.WebCache,Pub.Class.WebCache".LoadClass();
            else
                _cache = (ICache2)classNameAndAssembly.LoadClass();
        }
        /// <summary>
        /// ʹ���ⲿ���
        /// </summary>
        public static void Use<T>(T t) where T : ICache2, new() {
            _cache = Singleton<T>.Instance();
        }
    }
}
