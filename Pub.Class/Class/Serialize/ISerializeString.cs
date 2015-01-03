//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2011 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
using System.Data;

namespace Pub.Class {
    /// <summary>
    /// ���л��ӿ�
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface ISerializeString {
        /// <summary>
        /// ���л�
        /// </summary>
        /// <param name="o">����</param>
        /// <returns>�ַ���</returns>
        string Serialize<T>(T o);
        /// <summary>
        /// data�����л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">����</param>
        /// <returns>����</returns>
        T Deserialize<T>(string data);
        /// <summary>
        /// ���г��ļ�
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="fileName">�ļ���</param>
        void SerializeFile<T>(T o, string fileName);
        /// <summary>
        /// �ļ������л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="fileName">�ļ���</param>
        /// <returns>����</returns>
        T DeserializeFile<T>(string fileName);
        /// <summary>
        /// ���л���DES����
        /// </summary>
        /// <param name="o">����</param>
        /// <param name="key">����KEY</param>
        /// <returns>����</returns>
        string SerializeEncode<T>(T o, string key = "");
        /// <summary>
        /// DES���ܺ����л��ɶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="data">����</param>
        /// <param name="key">����KEY</param>
        /// <returns>����</returns>
        T DecodeDeserialize<T>(string data, string key = "");
    }
}
