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
    /// ������Ƶ����
    /// 
    /// �޸ļ�¼
    ///     2011.10.18 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class VideoShare {
        /// <summary>
        /// ȡָ��URL����Ƶ����
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns>������Ƶʵ��</returns>
        public static VideoInfo GetVideoInfo(string url) {
            if (!url.IsUrl()) return null;
            string host = url.Split('/')[2];
            int len = url.Length, len2 = host.Length + 10;
            if (len2 > len) return null;

            IVideoShare share = null;
            if (host.ToLower().IndexOf("tudou.com") != -1) share = new TodouShare();
            else if (host.ToLower().IndexOf("youku.com") != -1) share = new YoukuShare();
            else if (host.ToLower().IndexOf("ku6.com") != -1) share = new Ku6Share();
            else if (host.ToLower().IndexOf("pptv.com") != -1) share = new PPTVShare();
            else if (host.ToLower().IndexOf("56.com") != -1) share = new V56Share();
            else if (host.ToLower().IndexOf("163.com") != -1) share = new O163Share();
            else if (host.ToLower().IndexOf("v.qq.com") != -1) share = new VQQShare();

            return share.IsNull() ? null : share.GetVideoInfo(url);
        }
    }
}
