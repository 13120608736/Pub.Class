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
    /// ������Ƶʵ��
    /// 
    /// �޸ļ�¼
    ///     2011.10.18 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class VideoInfo {
        /// <summary>
        /// ��Ƶַַ
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// ��ƵͼƬַַ
        /// </summary>
        public string PicUrl { get; set; }
    }
}
