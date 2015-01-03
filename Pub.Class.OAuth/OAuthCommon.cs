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
using System.Security.Cryptography;

namespace Pub.Class {
    /// <summary>
    /// OAuth��������
    /// 
    /// �޸ļ�¼
    ///     2011.11.17 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class OAuthCommon {
        /// <summary>
        /// ʱ���
        /// </summary>
        /// <returns>ʱ���</returns>
        public static string GetTimestamp() {
            return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
        }
        /// <summary>
        /// 32λGUID
        /// </summary>
        /// <returns>32λGUID</returns>
        public static string GetGUID32() {
            return Guid.NewGuid().ToString().Substring(0, 32);
        }
        /// <summary>
        /// LIST����תΪURL����
        /// </summary>
        /// <param name="param">LIST����</param>
        /// <returns>URL����</returns>
        public static string GetUrlParameter(List<UrlParameter> param) {
            StringBuilder ParameString = new StringBuilder();
            foreach (UrlParameter par in param) ParameString.AppendFormat("{0}={1}&", par.ParameterName, par.ParameterValue);
            ParameString.RemoveLastChar("&");
            return ParameString.ToString();
        }
        /// <summary>
        /// HMACSHA1����
        /// </summary>
        /// <param name="appSecret">appSecret</param>
        /// <param name="accessSecret">accessSecret</param>
        /// <param name="data">����</param>
        /// <returns>����</returns>
        public static string GetHMACSHA1(string appSecret, string accessSecret, string data) {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", appSecret, accessSecret));
            HashAlgorithm hash = hmacsha1;
            byte[] dataBuffer = Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hash.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }
        /// <summary>
        /// LIST����תΪURL���� ��&����
        /// </summary>
        /// <param name="param">LIST����</param>
        /// <returns>URL����</returns>
        public static string GetUrlParameter2(List<UrlParameter> paras) {
            StringBuilder sbList = new StringBuilder();
            foreach (UrlParameter para in paras) sbList.AppendFormat("{0}={1}", para.ParameterName, para.ParameterValue);
            return sbList.ToString();
        }
        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MD5(string text) {
            MD5 md5Hash = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder sbList = new StringBuilder();
            foreach (byte d in data) sbList.Append(d.ToString("x2"));
            return sbList.ToString();
        }
    }
}
