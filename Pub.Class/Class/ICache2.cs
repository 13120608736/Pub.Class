//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;

namespace Pub.Class {
    /// <summary>
    /// ����ӿ�
    /// 
    /// �޸ļ�¼
    ///     2006.05.01 �汾��1.0 livexy �����˽ӿ�
    /// 
    /// </summary>
    public interface ICache2: IAddIn {
        /// <summary>
        /// ���ػ�������б�
        /// </summary>
        /// <returns></returns>
        IList<CachedItem> GetList();
        /// <summary>
        /// ������л�����Ŀ
        /// </summary>
        void Clear();
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="pattern">���������ƥ��ģʽ</param>
        void RemoveByPattern(string pattern);
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="key">�������</param>
        void Remove(string key);
        /// <summary>
        /// ���ӻ�����Ŀ 
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        void Insert(string key, object obj);
        /// <summary>
        /// ���ӻ�����Ŀ
        /// </summary>
        /// <param name="key">�������</param>
        /// <param name="obj">�������</param>
        /// <param name="seconds">��������</param>
        void Insert(string key, object obj, int seconds);
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        object Get(string key);
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="key">�������</param>
        /// <returns>���ػ������</returns>
        T Get<T>(string key);
        /// <summary>
        /// ���Ƿ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>true/false</returns>
        bool ContainsKey(string key);
        /// <summary>
        /// ����ѹ��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <param name="obj">ֵ</param>
        void Compress<T>(string key, T obj) where T : class ;
        /// <summary>
        /// ����ѹ��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <param name="obj">ֵ</param>
        /// <param name="seconds">��������</param>
        void Compress<T>(string key, T obj, int seconds) where T : class ;
        /// <summary>
        /// �����ѹ
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <returns></returns>
        T Decompress<T>(string key) where T : class ;
    }
}
