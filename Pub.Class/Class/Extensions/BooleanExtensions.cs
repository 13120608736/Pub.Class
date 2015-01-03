//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
using System.Web.Script.Serialization;
#endif
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography;

namespace Pub.Class {
    /// <summary>
    /// ������չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class BooleanExtensions {
        /// <summary>
        /// ����Ϊtrueִ��actionTrue��falseִ��actionFalse
        /// </summary>
        /// <example>
        /// <code>
        /// false.Do(() => { Msg.Write("1"); }, () => { Msg.Write("2"); });
        /// </code>
        /// </example>
        /// <param name="iBool">����</param>
        /// <param name="actionTrue">trueʱִ�ж���</param>
        /// <param name="actionFalse">falseʱִ�ж���</param>
        public static void Do(this bool iBool, Action actionTrue, Action actionFalse) {
            if (iBool) actionTrue(); else actionFalse();
        }
        /// <summary>
        /// ����Ϊtrueִ��actionTrue
        /// </summary>
        /// <param name="iBool">����</param>
        /// <param name="actionTrue">trueʱִ�ж���</param>
        public static void Do(this bool iBool, Action actionTrue) {
            if (iBool) actionTrue();
        }
        /// <summary>
        /// True
        /// </summary>
        /// <param name="iBool">����</param>
        /// <returns>true/false</returns>
        public static bool True(this bool iBool) { return iBool == true; }
        /// <summary>
        /// False
        /// </summary>
        /// <param name="iBool">����</param>
        /// <returns>true/false</returns>
        public static bool False(this bool iBool) { return iBool == false; }
        /// <summary>
        /// Not
        /// </summary>
        /// <param name="iBool">����</param>
        /// <returns>true/false</returns>
        public static bool Not(this bool iBool) { return !iBool; }
        /// <summary>
        /// if
        /// </summary>
        /// <param name="iff">����</param>
        /// <param name="exec">ִ��</param>
        /// <returns></returns>
        public static bool If(this bool iff, Func<bool> exec) { 
            if (!iff) return false;
            return exec();
        }
        /// <summary>
        /// else
        /// </summary>
        /// <param name="iff">����</param>
        /// <param name="exec">ִ��</param>
        /// <returns></returns>
        public static bool Else(this bool iff, Func<bool> exec) {
            if (iff) return true;
            return exec();
        }
        /// <summary>
        /// else
        /// </summary>
        /// <param name="iff">����</param>
        /// <param name="exec">ִ��</param>
        /// <returns></returns>
        public static void Else(this bool iff, Action exec) {
            if (!iff) exec();
        }
    }
}
