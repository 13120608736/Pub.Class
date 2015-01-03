//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;

namespace Pub.Class {
    /// <summary>
    /// OAuthConfig
    /// 
    /// �޸ļ�¼
    ///     2011.11.17 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class OAuthConfig {
        /// <summary>
        /// ȡOAuth.webconfig������Ϣ
        /// </summary>
        /// <param name="authEnum">��Ȩ����</param>
        /// <returns>ȡOAuth.webconfig������Ϣ</returns>
        public static ConfigInfo GetConfigInfo(OAuthEnum authEnum) {
            Xml2 xml = new Xml2("~/oauth.config".GetMapPath());
            string _enum = authEnum.ToString();
            return new ConfigInfo() { 
                AppKey = xml.GetNodeText("/configuration/" + _enum + "/appKey"),
                AppSecret = xml.GetNodeText("/configuration/" + _enum + "/appSecret"),
                RedirectUrl = xml.GetNodeText("/configuration/" + _enum + "/redirectUrl"),
            };
        }
    }

}

