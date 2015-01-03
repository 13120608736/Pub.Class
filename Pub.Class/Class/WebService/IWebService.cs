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
    /// WebService ���ýӿ�
    /// 
    /// �޸ļ�¼
    ///     2011.11.09 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface IWebService {
        /// <summary>
        /// WebService���÷���
        /// </summary>
        /// <param name="url">WebService �ӿڵ�ַ</param>
        /// <param name="className">����</param>
        /// <param name="methodName">������</param>
        /// <param name="parms">����</param>
        /// <returns>�����ַ���</returns>
        string Call(string url, string className, string methodName, Hashtable parms);
        /// <summary>
        /// WebService���÷���
        /// </summary>
        /// <param name="url">WebService �ӿڵ�ַ</param>
        /// <param name="className">����</param>
        /// <param name="methodName">������</param>
        /// <param name="parms">����</param>
        /// <returns>�����ַ���</returns>
        string Call(string url, string className, string methodName, IList<UrlParameter> parms);
    }
}
