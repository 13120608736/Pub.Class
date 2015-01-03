//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ServiceStack.Redis;
using Pub.Class;

namespace Pub.Class {
    /// <summary>
    /// Memcached�������
    /// 
    /// �޸ļ�¼
    ///     2009.11.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class RedisCache : ICache2 {
        #region ˽�о�̬�ֶ�
        private static readonly string[] SingleHost = WebConfig.GetApp("RedisCache.SingleHost").Split(':');
        private static readonly string[] MasterHosts = WebConfig.GetApp("RedisCache.MasterHosts").Split(';');
        private static readonly string[] SlaveHosts = WebConfig.GetApp("RedisCache.SlaveHosts").Split(';');
        private static readonly bool UsePool = WebConfig.GetApp("RedisCache.UsePool").ToBool(false);
        private static IRedisClient client = null;
        private static IRedisClientsManager clients = null;
        /// <summary>
        /// ��������
        /// </summary>
        private int Factor = 5;
        #endregion

        #region ������
        public RedisCache() {
            if (!SingleHost[0].IsNullEmpty()) {
                client = SingleHost.Length == 2 ? new RedisClient(SingleHost[0], SingleHost[0].ToInt(6379)) : new RedisClient(SingleHost[0]);
            } else if (!MasterHosts[0].IsNullEmpty()) {
                clients = UsePool ? CreatePooledRedisClientManager() : CreateBasicRedisClientManager();
            }
        }

        ~RedisCache() {
            if (!client.IsNull()) client.Dispose();
            if (!clients.IsNull()) clients.Dispose();
        }
        private IRedisClientsManager CreatePooledRedisClientManager() { return SlaveHosts[0].IsNullEmpty() ? new PooledRedisClientManager(MasterHosts) : new PooledRedisClientManager(MasterHosts, SlaveHosts); }
        private IRedisClientsManager CreateBasicRedisClientManager() { return SlaveHosts[0].IsNullEmpty() ? new BasicRedisClientManager(MasterHosts) : new BasicRedisClientManager(MasterHosts, SlaveHosts); }
        private IRedisClient GetClient() { return !client.IsNull() ? client : clients.GetClient(); }
        private IRedisClient GetReadOnlyClient() { return !client.IsNull() ? client : clients.GetReadOnlyClient(); }
        #endregion

        #region ��̬����
        public IList<CachedItem> GetList() {
            IList<CachedItem> list = new List<CachedItem>();
            var c = SlaveHosts[0].IsNullEmpty() ? GetClient() : Rand.RndInt(1, 3) == 1 ? GetClient() : GetReadOnlyClient();
            var list2 = c.GetAllKeys();
            foreach (string key in list2) {
                if (!ContainsKey(key)) continue;
                list.Add(new CachedItem(key, Get(key).GetType().ToString()));
            }
            return list;
        }
        /// <summary>
        /// ������л�����Ŀ
        /// </summary>
        public void Clear() {
            var c = GetClient();
            c.FlushAll();
            //using(c.AcquireLock("clear")){ }
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="pattern">���������ƥ��ģʽ</param>
        public void RemoveByPattern(string pattern) {
            var c = SlaveHosts[0].IsNullEmpty() ? GetClient() : Rand.RndInt(1, 3) == 1 ? GetClient() : GetReadOnlyClient();
            var list = c.GetAllKeys();
            foreach (string key in list) {
                if (!ContainsKey(key)) continue;
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
                if (regex.IsMatch(key)) Remove(key);
            }
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="key">�������</param>
        public void Remove(string key) {
            var c = GetClient();
             c.Remove(key);
             //using (c.AcquireLock("remove")) { }
        }
        /// <summary>
        /// ���ӻ�����Ŀ 
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        public void Insert(string key, object obj) {
            if (obj.IsNull()) return;
            var c = GetClient();
            c.Set(key, obj);
            //using (c.AcquireLock("set")) {  }
        }
        /// <summary>
        /// ���ӻ�����Ŀ
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="seconds">��������</param>
        public void Insert(string key, object obj, int seconds) {
            if (obj.IsNull()) return;
            var c = GetClient();
            c.Set(key, obj, DateTime.Now.AddSeconds(Factor * seconds));
            //using (c.AcquireLock("set")) {  }
        }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        public object Get(string key) { 
            var c = SlaveHosts[0].IsNullEmpty() ? GetClient() : Rand.RndInt(1, 3) == 1 ? GetClient() : GetReadOnlyClient();
            return c.ContainsKey(key) ? c.GetValue(key) : null;
            //using (c.AcquireLock("get")) {  }
        }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        public T Get<T>(string key) { 
            var c = SlaveHosts[0].IsNullEmpty() ? GetClient() : Rand.RndInt(1, 3) == 1 ? GetClient() : GetReadOnlyClient();
            return c.ContainsKey(key) ? c.Get<T>(key) : default(T);
            //using (c.AcquireLock("get")) {  }
        }
        /// <summary>
        /// ���Ƿ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>true/false</returns>
        public bool ContainsKey(string key) {
            var c = SlaveHosts[0].IsNullEmpty() ? GetClient() : Rand.RndInt(1, 3) == 1 ? GetClient() : GetReadOnlyClient();
            return c.ContainsKey(key);
        }
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
