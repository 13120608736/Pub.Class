//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Pub.Class;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace Pub.Class {
    /// <summary>
    /// ��Ȩ��¼
    /// 
    /// �޸ļ�¼
    ///     2011.11.17 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface IOAuth {
        /// <summary>
        /// ȡ��¼URL
        /// </summary>
        /// <returns>��¼URL</returns>
        string GetAuthUrl();
        /// <summary>
        /// ȡ��¼�˺���Ϣ
        /// </summary>
        /// <returns>ȡ��¼�˺���Ϣ</returns>
        UserInfo GetUserInfo();
        /// <summary>
        /// ȡ��¼�˺ź�����Ϣ
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="accessSecret">Access Secret</param>
        /// <returns>ȡ��¼�˺ź�����Ϣ</returns>
        IList<UserInfo> GetFriendsInfo(string accessToken, string accessSecret);
        /// <summary>
        /// ͬ����Ϣ
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="accessSecret">Access Secret</param>
        /// <param name="text">��Ϣ</param>
        void SendText(string accessToken, string accessSecret, string text);
    }
}
