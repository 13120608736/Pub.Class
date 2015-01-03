//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;
using Memcached.ClientLibrary;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Pub.Class {
    /// <summary>
    /// Memcached�������
    /// 
    /// �޸ļ�¼
    ///     2009.11.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class MemcachedCache : ICache2 {
        #region ˽�о�̬�ֶ�
        public static readonly ArrayList memcachedServer = new ArrayList();
        private static SockIOPool pool = null;
        private static MemcachedClient mc = null;
        /// <summary>
        /// ��������
        /// </summary>
        private int Factor = 5;
        #endregion

        #region ������
        public MemcachedCache() {
            foreach (string info in (WebConfig.GetApp("MemcachedCache.MemcachedServer") ?? string.Empty).Split(';')) if (!string.IsNullOrEmpty(info.Trim())) memcachedServer.Add(info);

            if (pool.IsNull() && memcachedServer.Count > 0) {
                pool = SockIOPool.GetInstance();
                pool.SetServers(memcachedServer);

                pool.InitConnections = Convert.ToInt32(WebConfig.GetApp("MemcachedCache.InitConnections") ?? "3");
                pool.MinConnections = Convert.ToInt32(WebConfig.GetApp("MemcachedCache.MinConnections") ?? "3");
                pool.MaxConnections = Convert.ToInt32(WebConfig.GetApp("MemcachedCache.MaxConnections") ?? "5");
                pool.SocketConnectTimeout = Convert.ToInt32(WebConfig.GetApp("MemcachedCache.SocketConnectTimeout") ?? "1000");
                pool.SocketTimeout = Convert.ToInt32(WebConfig.GetApp("MemcachedCache.SocketTimeout") ?? "3000");
                pool.MaintenanceSleep = Convert.ToInt32(WebConfig.GetApp("MemcachedCache.MaintenanceSleep") ?? "30");
                pool.Failover = (WebConfig.GetApp("MemcachedCache.Failover") ?? "") == "true" ? true : false;
                pool.Nagle = (WebConfig.GetApp("MemcachedCache.Nagle") ?? "") == "true" ? true : false;
                //pool.HashingAlgorithm = HashingAlgorithm.NewCompatibleHash;
                pool.Initialize();

                mc = new MemcachedClient();
                mc.PoolName = WebConfig.GetApp("MemcachedCache.PoolName") ?? "default";
                mc.EnableCompression = (WebConfig.GetApp("MemcachedCache.EnableCompression") ?? "") == "true" ? true : false;
            }
        }

        ~MemcachedCache() {
            if (memcachedServer.Count > 0 && !pool.IsNull()) {
                pool.Shutdown();
            }
        }
        #endregion

        #region ��̬����
        public IList<CachedItem> GetList() {
            IList<CachedItem> list = new List<CachedItem>();
            ArrayList cacheItem = GetStats(memcachedServer, Stats.Items, "");
            foreach (string item in cacheItem) {
                if (item.IndexOf(":number:") < 0) continue;
                ArrayList cachearr = GetStats(memcachedServer, Stats.CachedDump, item.Split(':')[1] + " 0");
                foreach (string cache in cachearr) {
                    string cacheName = cache.Split(':')[0];
                    if (!mc.KeyExists(cacheName)) continue;
                    list.Add(new CachedItem(cacheName, Get(cacheName).GetType().ToString()));
                }
            }
            return list;
        }
        public ArrayList GetStats() {
            ArrayList arrayList = new ArrayList();
            foreach (string server in memcachedServer) {
                arrayList.Add(server);
            }
            return GetStats(arrayList, Stats.Default, null);
        }
        public ArrayList GetStats(ArrayList serverArrayList, Stats statsCommand, string param) {
            ArrayList statsArray = new ArrayList();
            param = string.IsNullOrEmpty(param) ? "" : param.Trim().ToLower();

            string commandstr = "stats";
            //ת��stats�������
            switch (statsCommand) {
                case Stats.Reset: { commandstr = "stats reset"; break; }
                case Stats.Malloc: { commandstr = "stats malloc"; break; }
                case Stats.Maps: { commandstr = "stats maps"; break; }
                case Stats.Sizes: { commandstr = "stats sizes"; break; }
                case Stats.Slabs: { commandstr = "stats slabs"; break; }
                case Stats.Items: { commandstr = "stats items"; break; }
                case Stats.CachedDump: {
                        string[] statsparams = param.Split(" ");
                        if (statsparams.Length == 2)
                            if (statsparams.IsNumberArray())
                                commandstr = "stats cachedump  " + param;

                        break;
                    }
                case Stats.Detail: {
                        if (string.Equals(param, "on") || string.Equals(param, "off") || string.Equals(param, "dump"))
                            commandstr = "stats detail " + param.Trim();

                        break;
                    }
                default: { commandstr = "stats"; break; }
            }
            //���ط���ֵ
            Hashtable stats = mc.Stats(serverArrayList, commandstr);
            foreach (string key in stats.Keys) {
                statsArray.Add(key);
                Hashtable values = (Hashtable)stats[key];
                foreach (string key2 in values.Keys) {
                    statsArray.Add(key2 + ":" + values[key2]);
                }
            }
            return statsArray;
        }

        /// <summary>
        /// Stats�����в���
        /// </summary>
        public enum Stats {
            /// <summary>
            /// stats : ��ʾ��������Ϣ, ͳ�����ݵ�
            /// </summary>
            Default = 0,
            /// <summary>
            /// stats reset : ���ͳ������
            /// </summary>
            Reset = 1,
            /// <summary>
            /// stats malloc : ��ʾ�ڴ��������
            /// </summary>
            Malloc = 2,
            /// <summary>
            /// stats maps : ��ʾ"/proc/self/maps"����
            /// </summary>
            Maps = 3,
            /// <summary>
            /// stats sizes
            /// </summary>
            Sizes = 4,
            /// <summary>
            /// stats slabs : ��ʾ����slab����Ϣ,����chunk�Ĵ�С,��Ŀ,ʹ�������
            /// </summary>
            Slabs = 5,
            /// <summary>
            /// stats items : ��ʾ����slab��item����Ŀ������item������(���һ�η��ʾ������ڵ�����)
            /// </summary>
            Items = 6,
            /// <summary>
            /// stats cachedump slab_id limit_num : ��ʾĳ��slab�е�ǰ limit_num �� key �б�
            /// </summary>
            CachedDump = 7,
            /// <summary>
            /// stats detail [on|off|dump] : ���û�����ʾ��ϸ������¼   on:����ϸ������¼  off:�ر���ϸ������¼ dump: ��ʾ��ϸ������¼(ÿһ����ֵget,set,hit,del�Ĵ���)
            /// </summary>
            Detail = 8
        }
        /// <summary>
        /// ������л�����Ŀ
        /// </summary>
        public void Clear() {
            mc.FlushAll();
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="pattern">���������ƥ��ģʽ</param>
        public void RemoveByPattern(string pattern) {
            ArrayList cacheItem = GetStats(memcachedServer, Stats.Items, "");
            foreach (string item in cacheItem) {
                if (item.IndexOf(":number:") < 0) continue;
                ArrayList cachearr = GetStats(memcachedServer, Stats.CachedDump, item.Split(':')[1] + " 0");
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
                foreach (string cache in cachearr) {
                    string cacheName = cache.Split(':')[0];
                    if (!mc.KeyExists(cacheName)) continue;
                    if (regex.IsMatch(cacheName)) Remove(cacheName);
                }
            }
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="key">�������</param>
        public void Remove(string key) { mc.Delete(key); }
        /// <summary>
        /// ���ӻ�����Ŀ 
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        public void Insert(string key, object obj) { if (obj != null) mc.Set(key, obj); }
        public void Add(string key, object obj) { if (obj != null && !mc.KeyExists(key)) mc.Add(key, obj); }
        /// <summary>
        /// ���ӻ�����Ŀ
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="seconds">��������</param>
        public void Insert(string key, object obj, int seconds) {
            if (obj != null) mc.Set(key, obj, DateTime.Now.AddSeconds(Factor * seconds));
        }
        public void Add(string key, object obj, int seconds) {
            if (obj != null && !mc.KeyExists(key)) mc.Add(key, obj, DateTime.Now.AddSeconds(Factor * seconds)); //TimeSpan.FromSeconds();
        }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        public object Get(string key) { return mc.KeyExists(key) ? mc.Get(key) : null; }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        public T Get<T>(string key) { return mc.KeyExists(key) ? (T)mc.Get(key) : default(T); }
        /// <summary>
        /// ���Ƿ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>true/false</returns>
        public bool ContainsKey(string key) { return mc.KeyExists(key); }
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
