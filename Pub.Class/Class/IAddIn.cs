//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;

namespace Pub.Class {
    /// <summary>
    /// ����ӿ�
    /// 
    /// �޸ļ�¼
    ///     2011.07.01 �汾��1.0 livexy �����˽ӿ�
    /// 
    /// </summary>
    public interface IAddIn {

    }
    /// <summary>
    /// ����ӿ�
    /// 
    /// �޸ļ�¼
    ///     2012.01.10 �汾��1.0 livexy �����˽ӿ�
    /// 
    /// </summary>
    public interface IPlugin : IAddIn {
        /// <summary>
        /// ִ�����
        /// </summary>
        /// <param name="args">����</param>
        void Main(params string[] args);
    }
}
