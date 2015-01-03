//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Pub.Class {
    /// <summary>
    /// Servers��
    /// 
    /// �޸ļ�¼
    ///     2006.05.10 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Server2 {
        //#region GetMapPath
        /// <summary>
        /// ��õ�ǰ����·��
        /// </summary>
        /// <param name="strPath">ָ����·��</param>
        /// <returns>����·��</returns>
        public static string GetMapPath(string strPath) {
            if (HttpContext.Current.IsNotNull())
                return HttpContext.Current.Server.MapPath(strPath);
            else {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith(".\\")) strPath = strPath.Substring(2);
                strPath = strPath.TrimStart('~').TrimStart('\\');
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        //#endregion
    }
}
