//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2011 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
using System.Data;

namespace Pub.Class {
    /// <summary>
    /// WebService �������� Enum
    /// 
    /// �޸ļ�¼
    ///     2011.11.09 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public enum WebServiceEnum {
        /// <summary>
        /// get
        /// </summary>
        get,
        /// <summary>
        /// post
        /// </summary>
        post,
        /// <summary>
        /// soap
        /// </summary>
        soap,
        /// <summary>
        /// dynamic
        /// </summary>
        dynamic
    }
}
