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
    /// ������Ƶ�ӿ�
    /// 
    /// �޸ļ�¼
    ///     2011.10.18 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface IVideoShare {
        /// <summary>
        /// ȡָ��URL����Ƶ����
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns>������Ƶʵ��</returns>
        VideoInfo GetVideoInfo(string url);
    }
}
