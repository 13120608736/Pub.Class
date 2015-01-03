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
    /// �û���Ϣʵ��
    /// 
    /// �޸ļ�¼
    ///     2011.11.17 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class UserInfo {
        /// <summary>
        /// �û���Ϣ
        /// </summary>
        public UserInfo() {
            this.Name = null;
            this.Sex = 0;
            this.Email = null;
            this.Token = null;
            this.UserID = null;
            this.Secret = null;
            this.Header = null;
            this.Address = null;
        }
        /// <summary>
        /// UserID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// �û�����
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// �Ա� 1�У�0Ů
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Header
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Secret
        /// </summary>
        public string Secret { get; set; }
    }
    /// <summary>
    /// ������Ϣʵ��
    /// 
    /// �޸ļ�¼
    ///     2011.11.17 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class ConfigInfo { 
        /// <summary>
        /// ��Ȩ��Ϣ
        /// </summary>
        public ConfigInfo() {
            this.AppKey = null;
            this.AppSecret = null;
            this.RedirectUrl = null;
        }
        /// <summary>
        /// appSecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// app key
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// �ص���ַ
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}
