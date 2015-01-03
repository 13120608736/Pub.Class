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
    /// open.163.com��Ƶ����
    /// 
    /// �޸ļ�¼
    ///     2011.10.18 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class O163Share : IVideoShare {
        /// <summary>
        /// ȡָ��URL����Ƶ����
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns>������Ƶʵ��</returns>
        public VideoInfo GetVideoInfo(string url) {
            #region ��������
            //http://v.163.com/movie/2011/8/R/S/M7BN25K5V_M7BPHVERS.html
            //http://swf.ws.126.net/movieplayer/-0-2_M7BN25K5V_M7BPHVERS-vimg1_ws_126_net//image/snapshot_movie/2011/8/H/F/M7BPHVDHF-.swf
            //http://swf.ws.126.net/movieplayer/-0-2_M7BN25K5V_M7BPHVERS-.swf
            #endregion
            if (url.EndsWith(".swf")) return null;

            string[] list = url.Split('/');
            string code = list[list.Length - 1];
            code = code.Left(code.Length - 5);
            string data = (Net2.GetRemoteHtmlCode4(url) ?? "").ReplaceRN();
            string on = data.GetMatchingValues("<li class=\"on\">                 	   <a class=\"picLink\"(.+?)<div class=\"nowplay\">���ڲ���...</div>                 	</li>").FirstOrDefault() ?? "";
            //Msg.WriteEnd(on);
            string title = (data.GetMatchingValues("<title>(.+?)</title>", "<title>", "</title>").FirstOrDefault() ?? "").Trim();
            string img = on.GetMatchingValues("<img height=\"90\" width=\"120\" src=\"(.+?)\" />", "<img height=\"90\" width=\"120\" src=\"", "\" />").FirstOrDefault() ?? "";
            string flv = "http://swf.ws.126.net/movieplayer/-0-2_{0}-.swf".FormatWith(code);
            if (title.IndexOf("_����") != -1) title = title.Left(title.IndexOf("_����")).Trim().TrimEnd('_').Trim();

            return new VideoInfo() { PicUrl = img, Title = title, Url = flv };
        }
    }
}
