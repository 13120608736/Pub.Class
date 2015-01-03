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
    /// double��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class DoubleExtensions {
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this double number, int percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this double number, float percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this double number, double percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this double number, long percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this double position, int total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this double position, float total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this double position, double total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this double position, long total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
        /// <summary>
        /// ����decimalPointsλС��
        /// </summary>
        /// <param name="val">ֵ</param>
        /// <param name="decimalPoints">С��λ��</param>
        /// <returns>����decimalPointsλС��</returns>
        public static double Round(this double val, int decimalPoints) {
            return Math.Round(val, decimalPoints);
        }
        /// <summary>
        /// ����2λС��
        /// </summary>
        /// <param name="val">ֵ</param>
        /// <returns>����2λС��</returns>
        public static double Round2(this double val) {
            return Math.Round(val, 2);
        }
        /// <summary>
        /// ����2λС��
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns>����2λС��</returns>
        public static long Round(this double value) {
            if (value >= 0) return (long)Math.Floor(value);
            return (long)Math.Ceiling(value);
        }
        /// <summary>
        /// ��� ��λ,�ָ�
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns>��� ��λ,�ָ�</returns>
        public static string ToCurrency(this double value) {
            return value.ToString("N");
        }
        /// <summary>
        /// �����ת��д
        /// </summary>
        /// <param name="value">�����</param>
        /// <returns>��д�����</returns>
        public static string ToRMB(this double value) { return ((decimal)value).ToRMB(); }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static double IfEqual(this double obj, double value, double defaultValue) {
            return obj == value ? defaultValue : obj;
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static double IfNotEqual(this double obj, double value, double defaultValue) {
            return obj != value ? defaultValue : obj;
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static double IfMoreThan(this double obj, double value, double defaultValue) {
            return obj > value ? defaultValue : obj;
        }
        /// <summary>
        /// ���С��
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static double IfLessThan(this double obj, double value, double defaultValue) {
            return obj < value ? defaultValue : obj;
        }
        /// <summary>
        /// ������ڵ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static double IfMoreThanOrEqual(this double obj, double value, double defaultValue) {
            return obj >= value ? defaultValue : obj;
        }
        /// <summary>
        /// ���С�ڵ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static double IfLessThanOrEqual(this double obj, double value, double defaultValue) {
            return obj <= value ? defaultValue : obj;
        }
    }
}
