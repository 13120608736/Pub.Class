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
    /// PagerSQL �������� Enum
    /// 
    /// �޸ļ�¼
    ///     2011.11.09 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public enum PagerSQLEnum {
        /// <summary>
        /// not_in
        /// </summary>
        not_in,
        /// <summary>
        /// top_max
        /// </summary>
        max_top,
        /// <summary>
        /// top_top
        /// </summary>
        top_top,
        /// <summary>
        /// row_number
        /// </summary>
        row_number,
        /// <summary>
        /// mysql
        /// </summary>
        mysql,
        /// <summary>
        /// oracle
        /// </summary>
        oracle,
        /// <summary>
        /// sqlite
        /// </summary>
        sqlite
    }
}
