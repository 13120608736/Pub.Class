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
using System.Net;

namespace Pub.Class {
    /// <summary>
    /// long��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class LongExtensions {
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">long��չ</param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this long number, int percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">long��չ</param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this long number, float percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">long��չ</param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this long number, double percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">long��չ</param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this long number, decimal percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">long��չ</param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this long number, long percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">long��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this long position, int total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)position / (decimal)total * 100;
            return result;
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">long��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this long position, float total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">long��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this long position, double total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">long��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this long position, decimal total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)position / (decimal)total * 100;
            return result;
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">long��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this long position, long total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)position / (decimal)total * 100;
            return result;
        }
        /// <summary>
        /// �ļ���С��ʽ�� ��СKB
        /// </summary>
        /// <param name="bytes">long��չ</param>
        /// <returns></returns>
        public static string FormatKB(this long bytes) {
            if ((double)bytes <= 999) return bytes.ToString() + " bytes";
            return ThreeNonZeroDigits((double)bytes / 1024) + " KB";
        }
        /// <summary>
        /// �ļ���С��ʽ�� ��С�ֽ�
        /// </summary>
        /// <param name="bytes">long��չ</param>
        /// <returns>�ļ���С��ʽ�� ��С�ֽ� </returns>
        public static string FormatBytes(this long bytes) {
            const double ONE_KB = 1024;
            const double ONE_MB = ONE_KB * 1024;
            const double ONE_GB = ONE_MB * 1024;
            const double ONE_TB = ONE_GB * 1024;
            const double ONE_PB = ONE_TB * 1024;
            const double ONE_EB = ONE_PB * 1024;
            const double ONE_ZB = ONE_EB * 1024;
            const double ONE_YB = ONE_ZB * 1024;

            if ((double)bytes <= 999) return bytes.ToString() + " bytes";
            else if ((double)bytes <= ONE_KB * 999) return ThreeNonZeroDigits((double)bytes / ONE_KB) + " KB";
            else if ((double)bytes <= ONE_MB * 999) return ThreeNonZeroDigits((double)bytes / ONE_MB) + " MB";
            else if ((double)bytes <= ONE_GB * 999) return ThreeNonZeroDigits((double)bytes / ONE_GB) + " GB";
            else if ((double)bytes <= ONE_TB * 999) return ThreeNonZeroDigits((double)bytes / ONE_TB) + " TB";
            else if ((double)bytes <= ONE_PB * 999) return ThreeNonZeroDigits((double)bytes / ONE_PB) + " PB";
            else if ((double)bytes <= ONE_EB * 999) return ThreeNonZeroDigits((double)bytes / ONE_EB) + " EB";
            else if ((double)bytes <= ONE_ZB * 999) return ThreeNonZeroDigits((double)bytes / ONE_ZB) + " ZB";
            else return ThreeNonZeroDigits((double)bytes / ONE_YB) + " YB";
        }
        /// <summary>
        /// ����2λС��
        /// </summary>
        /// <param name="value">long��չ</param>
        /// <returns>����2λС��</returns>
        public static string ThreeNonZeroDigits(this double value) {
            if (value >= 100) return ((int)value).ToString();
            else if (value >= 10) return value.ToString("0.0");
            else return value.ToString("0.00");
        }
        /// <summary>
        /// ��� ��λ,�ָ�
        /// </summary>
        /// <param name="value">long��չ</param>
        /// <returns>��� ��λ,�ָ�</returns>
        public static string ToCurrency(this long value) {
            return value.ToString("N");
        }
        /// <summary>
        /// �����ת��д
        /// </summary>
        /// <param name="value">long��չ</param>
        /// <returns>�����ת��д</returns>
        public static string ToRMB(this long value) { return ((decimal)value).ToRMB(); }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static long IfEqual(this long obj, long value, long defaultValue) {
            return obj == value ? defaultValue : obj;
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static long IfNotEqual(this long obj, long value, long defaultValue) {
            return obj != value ? defaultValue : obj;
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static long IfMoreThan(this long obj, long value, long defaultValue) {
            return obj > value ? defaultValue : obj;
        }
        /// <summary>
        /// ���С��
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static long IfLessThan(this long obj, long value, long defaultValue) {
            return obj < value ? defaultValue : obj;
        }
        /// <summary>
        /// ������ڵ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static long IfMoreThanOrEqual(this long obj, long value, long defaultValue) {
            return obj >= value ? defaultValue : obj;
        }
        /// <summary>
        /// ���С�ڵ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static long IfLessThanOrEqual(this long obj, long value, long defaultValue) {
            return obj <= value ? defaultValue : obj;
        }
        /// <summary>
        /// תʱ��
        /// </summary>
        /// <param name="seconds">int��չ</param>
        /// <returns>תʱ�� 1:12:12</returns>
        public static string ToTime(this long seconds) {
            var hour = seconds / 3600;
            var minute = (seconds - hour * 3600) / 60;
            seconds = seconds % 60;
            return (hour < 10 ? "0" + hour.ToString() : hour.ToString()) + ":" + (minute < 10 ? "0" + minute.ToString() : minute.ToString()) + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());
        }
        public static string ToIP(this long ip) {
            StringBuilder sb = new StringBuilder();
            var ips = IPAddress.Parse(ip.ToString()).ToString().Split('.');
            for (int i = ips.Length - 1; i >= 0; i--) sb.Append(ips[i]).Append(".");
            return sb.ToString().Trim('.');
        }
    }
}
