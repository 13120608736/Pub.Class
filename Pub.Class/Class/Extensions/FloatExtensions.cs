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
    /// Float��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class FloatExtensions {
        /// <summary>
        /// �ٷ��� 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="percentOf"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this float value, int percentOf) {
            return (decimal)(value / percentOf * 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="value"></param>
        /// <param name="percentOf"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this float value, float percentOf) {
            return (decimal)(value / percentOf * 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="value"></param>
        /// <param name="percentOf"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this float value, double percentOf) {
            return (decimal)(value / percentOf * 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="value"></param>
        /// <param name="percentOf"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this float value, long percentOf) {
            return (decimal)(value / percentOf * 100);
        }
        /// <summary>
        /// ��� ��λ,�ָ�
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns>��� ��λ,�ָ�</returns>
        public static string ToCurrency(this int value) {
            return value.ToString("N");
        }
        /// <summary>
        /// ����decimalPointsλС��
        /// </summary>
        /// <param name="val">ֵ</param>
        /// <param name="decimalPoints">С��λ��</param>
        /// <returns>����decimalPointsλС��</returns>
        public static float Round(this float val, int decimalPoints) {
            return (float)Math.Round((double)val, decimalPoints);
        }
        /// <summary>
        /// ����2λС��
        /// </summary>
        /// <param name="val">ֵ</param>
        /// <returns>����2λС��</returns>
        public static float Round2(this float val) {
            return (float)Math.Round((double)val, 2);
        }
    }
}
