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
    /// ��¼��Ȩ
    /// 
    /// �޸ļ�¼
    ///     2011.11.17 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class OAuth {
        /// <summary>
        /// ȡ��¼URL
        /// </summary>
        /// <param name="authEnum">��Ȩ����</param>
        /// <returns>��¼URL</returns>
        public static string GetAuthUrl(OAuthEnum authEnum) {
            IOAuth auth = null;
            switch (authEnum) {
                case OAuthEnum.msn: auth = new MSNOAuth(); break;
                case OAuthEnum.sina: auth = new SinaOAuth(); break;
                case OAuthEnum.qq: auth = new QQOAuth(); break;
                case OAuthEnum.netease: auth = new NeteaseOAuth(); break;
                case OAuthEnum.sohu: auth = new SohuOAuth(); break;
                case OAuthEnum.kaixin: auth = new KaiXinOAuth(); break;
                case OAuthEnum.renren: auth = new RenRenOAuth(); break;
            }
            return auth.IsNull() ? string.Empty : auth.GetAuthUrl();
        }
        /// <summary>
        /// ȡ��¼URL
        /// </summary>
        /// <param name="authEnum">��Ȩ����</param>
        /// <returns>��¼URL</returns>
        public static string GetAuthUrl(string authEnum) {
            return GetAuthUrl(authEnum.ToEnum<OAuthEnum>());
        }
        /// <summary>
        /// ȡ��¼�˺���Ϣ
        /// </summary>
        /// <param name="authEnum">��Ȩ����</param>
        /// <returns>ȡ��¼�˺���Ϣ</returns>
        public static UserInfo GetUserInfo(OAuthEnum authEnum) {
            IOAuth auth = null;
            switch (authEnum) {
                case OAuthEnum.msn: auth = new MSNOAuth(); break;
                case OAuthEnum.sina: auth = new SinaOAuth(); break;
                case OAuthEnum.qq: auth = new QQOAuth(); break;
                case OAuthEnum.netease: auth = new NeteaseOAuth(); break;
                case OAuthEnum.sohu: auth = new SohuOAuth(); break;
                case OAuthEnum.kaixin: auth = new KaiXinOAuth(); break;
                case OAuthEnum.renren: auth = new RenRenOAuth(); break;
            }
            return auth.IsNull() ? null : auth.GetUserInfo();
        }
        /// <summary>
        /// ȡ��¼�˺���Ϣ
        /// </summary>
        /// <param name="authEnum">��Ȩ����</param>
        /// <returns>ȡ��¼�˺���Ϣ</returns>
        public static UserInfo GetUserInfo(string authEnum) {
            return GetUserInfo(authEnum.ToEnum<OAuthEnum>());
        }
        /// <summary>
        /// ȡ��¼�˺ź�����Ϣ
        /// </summary>
        /// <param name="authEnum">��Ȩ����</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="accessSecret">Access Secret</param>
        /// <returns>ȡ��¼�˺ź�����Ϣ</returns>
        public static IList<UserInfo> GetFriendsInfo(OAuthEnum authEnum, string accessToken, string accessSecret) {
            IOAuth auth = null;
            switch (authEnum) {
                case OAuthEnum.msn: auth = new MSNOAuth(); break;
                case OAuthEnum.sina: auth = new SinaOAuth(); break;
                case OAuthEnum.qq: auth = new QQOAuth(); break;
                case OAuthEnum.netease: auth = new NeteaseOAuth(); break;
                case OAuthEnum.sohu: auth = new SohuOAuth(); break;
                case OAuthEnum.kaixin: auth = new KaiXinOAuth(); break;
                case OAuthEnum.renren: auth = new RenRenOAuth(); break;
            }
            return auth.IsNull() ? null : auth.GetFriendsInfo(accessToken, accessSecret);
        }
        /// <summary>
        /// ȡ��¼�˺ź�����Ϣ
        /// </summary>
        /// <param name="authEnum">��Ȩ����</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="accessSecret">Access Secret</param>
        /// <returns>ȡ��¼�˺ź�����Ϣ</returns>
        public static IList<UserInfo> GetFriendsInfo(string authEnum, string accessToken, string accessSecret) {
            return GetFriendsInfo(authEnum.ToEnum<OAuthEnum>(), accessToken, accessSecret);
        }
        /// <summary>
        /// ͬ����Ϣ
        /// </summary>
        /// <param name="authEnum">��Ȩ����</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="accessSecret">Access Secret</param>
        /// <param name="text">��Ϣ</param>
        public static void SendText(OAuthEnum authEnum, string accessToken, string accessSecret, string text) { 
            IOAuth auth = null;
            switch (authEnum) {
                case OAuthEnum.msn: auth = new MSNOAuth(); break;
                case OAuthEnum.sina: auth = new SinaOAuth(); break;
                case OAuthEnum.qq: auth = new QQOAuth(); break;
                case OAuthEnum.netease: auth = new NeteaseOAuth(); break;
                case OAuthEnum.sohu: auth = new SohuOAuth(); break;
                case OAuthEnum.kaixin: auth = new KaiXinOAuth(); break;
                case OAuthEnum.renren: auth = new RenRenOAuth(); break;
            }
            if (!auth.IsNull()) auth.SendText(accessToken, accessSecret, text);
        }
        /// <summary>
        /// ͬ����Ϣ
        /// </summary>
        /// <param name="authEnum">��Ȩ����</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="accessSecret">Access Secret</param>
        /// <param name="text">��Ϣ</param>
        public static void SendText(string authEnum, string accessToken, string accessSecret, string text) { 
            SendText(authEnum.ToEnum<OAuthEnum>(), accessToken, accessSecret, text);
        }
    }
}
