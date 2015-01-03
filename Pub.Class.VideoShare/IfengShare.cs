//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Pub.Class;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
#endif
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace Pub.Class {
    /// <summary>
    /// �����Ƶ
    /// 
    /// �޸ļ�¼
    ///     2011.10.18 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class IfengShare : IVideoShare {
        /// <summary>
        /// ȡָ��URL����Ƶ����
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns>������Ƶʵ��</returns>
        public VideoInfo GetVideoInfo(string url) {
            #region ��������
            //http://v.ifeng.com/include/exterior.swf?guid=d6dcd88a-78b5-4dca-9dbf-4e5504a1ee90&AutoPlay=false
            //http://v.ifeng.com/news/world/201110/d6dcd88a-78b5-4dca-9dbf-4e5504a1ee90.shtml
            //var videoinfo={"id":"d6dcd88a-78b5-4dca-9dbf-4e5504a1ee90","wapstatus":"0","name":"���������ջ����ع� ��ץͷ������ǿվ��","duration":"128","url":"http://v.ifeng.com/news/world/201110/d6dcd88a-78b5-4dca-9dbf-4e5504a1ee90.shtml","img":"http://img.ifeng.com/itvimg/2011/10/21/d0c833eb-5c5a-49d2-9e9d-1e83ae3b4551140.jpg"};
            #endregion
            if (url.IndexOf(".swf?") != -1) return null;

            string[] list = url.Split('/');
            string code = list[list.Length - 1];
            code = code.Left(code.Length - 6);
            string data = (Net2.GetRemoteHtmlCode4(url, Encoding.UTF8) ?? "").ReplaceRN();

            string title = (data.GetMatchingValues("\"name\":\"(.+?)\"", "\"name\":\"", "\"").FirstOrDefault() ?? "").Trim();
            string img = data.GetMatchingValues("\"img\":\"(.+?)\"", "\"img\":\"", "\"").FirstOrDefault() ?? "";
            string flv = "http://v.ifeng.com/include/exterior.swf?guid={0}&AutoPlay=false".FormatWith(code);

            return new VideoInfo() { PicUrl = img, Title = title, Url = flv };
        }
    }
}
