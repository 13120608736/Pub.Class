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
using System.Threading;

namespace Pub.Class {
    /// <summary>
    /// int��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class IntExtensions {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="iInt">int��չ</param>
        /// <param name="val">����</param>
        /// <returns>������</returns>
        public static bool IsMod(this int iInt, int val) {
            if ((iInt % val) == 0) return true;
            return false;
        }
        /// <summary>
        /// ������, ������
        /// </summary>
        /// <param name="value">int��չ</param>
        /// <returns>������, ������</returns>
        public static bool IsOdd(this int value) {
            return ((value & 1) == 1);
        }
        /// <summary>
        /// ��2������ ż��
        /// </summary>
        /// <param name="iInt">int��չ</param>
        /// <returns>��2������ ż��</returns>
        public static bool IsEven(this int iInt) {
            return iInt.IsMod(2);
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ToBeChecked">int��չ</param>
        /// <returns>������</returns>
        public static bool IsPrime<T>(this int ToBeChecked) {
            System.Collections.BitArray numbers = new System.Collections.BitArray(ToBeChecked + 1, true);
            for (Int32 i = 2; i < ToBeChecked + 1; i++)
                if (numbers[i]) {
                    for (Int32 j = i * 2; j < ToBeChecked + 1; j += i) numbers[j] = false;
                    if (numbers[i]) {
                        if (ToBeChecked == i) return true;
                    }
                }
            return false;
        }
        /// <summary>
        /// ������ǰ
        /// </summary>
        /// <param name="years">int��չ</param>
        /// <returns>������ǰ</returns>
        public static DateTime YearsAgo(this int years) {
            return DateTime.Now.AddYears(-years);
        }
        /// <summary>
        /// ������ǰ
        /// </summary>
        /// <param name="months">int��չ</param>
        /// <returns>������ǰ</returns>
        public static DateTime MonthsAgo(this int months) {
            return DateTime.Now.AddMonths(-months);
        }
        /// <summary>
        /// ������ǰ
        /// </summary>
        /// <param name="days">int��չ</param>
        /// <returns>������ǰ</returns>
        public static DateTime DaysAgo(this int days) {
            return DateTime.Now.AddDays(-days);
        }
        /// <summary>
        /// ����Сʱǰ
        /// </summary>
        /// <param name="hours">int��չ</param>
        /// <returns>����Сʱǰ</returns>
        public static DateTime HoursAgo(this int hours) {
            return DateTime.Now.AddHours(-hours);
        }
        /// <summary>
        /// ���ٷ���ǰ
        /// </summary>
        /// <param name="minutes">int��չ</param>
        /// <returns>���ٷ���ǰ</returns>
        public static DateTime MinutesAgo(this int minutes) {
            return DateTime.Now.AddMinutes(-minutes);
        }
        /// <summary>
        /// ������ǰ
        /// </summary>
        /// <param name="seconds">int��չ</param>
        /// <returns>������ǰ</returns>
        public static DateTime SecondsAgo(this int seconds) {
            return DateTime.Now.AddSeconds(-seconds);
        }
        /// <summary>
        /// ����0 TRUE
        /// </summary>
        /// <param name="val">int��չ</param>
        /// <returns>����0 TRUE</returns>
        public static bool ToBool(this int val) {
            return val > 0 ? true : false;
        }
        /// <summary>
        /// True
        /// </summary>
        /// <param name="iBool">int��չ</param>
        /// <returns></returns>
        public static bool True(this int iBool) { return ToBool(iBool) == true; }
        /// <summary>
        /// False
        /// </summary>
        /// <param name="iBool">int��չ</param>
        /// <returns></returns>
        public static bool False(this int iBool) { return ToBool(iBool) == false; }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">int��չ</param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this int number, int percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">int��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this int position, int total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)position / (decimal)total * 100;
            return result;
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">int��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this int? position, int total) {
            if (position.IsNull()) return 0;
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">int��չ</param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this int number, float percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">int��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this int position, float total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">int��չ</param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this int number, double percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">int��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this int position, double total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">int��չ</param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this int number, decimal percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">int��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this int position, decimal total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)position / (decimal)total * 100;
            return result;
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">int��չ</param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static decimal PercentageOf(this int number, long percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">int��չ</param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this int position, long total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)position / (decimal)total * 100;
            return result;
        }
        /// <summary>
        /// ��ʱ 1000Ϊһ��
        /// </summary>
        /// <param name="ms">int��չ</param>
        /// <returns>��ʱ 1000Ϊһ��</returns>
        public static int Sleep(this int ms) { Thread.Sleep(ms); return ms; }
        /// <summary>
        /// ���ɸ�����
        /// </summary>
        /// <param name="len">int��չ</param>
        /// <returns>���ɸ�����</returns>
        public static string Jammer(this int len) {
            string randStr = string.Empty;
            for (int i = 0; i < len; i++) {
                randStr += ((char)(Rand.RndInt(0, 33))).ToString() + ((char)(Rand.RndInt(63, 126))).ToString();
            }

            return randStr;
        }
        /// <summary>
        /// IsBetween
        /// </summary>
        /// <param name="i">int��չ</param>
        /// <param name="lowerBound">��С</param>
        /// <param name="upperBound">���</param>
        /// <returns></returns>
        public static bool IsBetween(this int i, int lowerBound, int upperBound) {
            return i.IsBetween(lowerBound, upperBound, false);
        }
        /// <summary>
        /// IsBetween
        /// </summary>
        /// <param name="i">int��չ</param>
        /// <param name="lowerBound">��С</param>
        /// <param name="upperBound">���</param>
        /// <param name="includeBounds">�Ƿ������С���ֵ</param>
        /// <returns></returns>
        public static bool IsBetween(this int i, int lowerBound, int upperBound, bool includeBounds) {
            if (includeBounds)
                return i >= lowerBound && i <= upperBound;
            else
                return i > lowerBound && i < upperBound;
        }
        /// <summary>
        /// ʱ���
        /// </summary>
        /// <param name="integer">int��չ</param>
        /// <returns>ʱ���</returns>
        public static TimeSpan ToTimeSpan(this int integer) {
            int hours = integer / 100;
            int minutes = integer - hours * 100;
            return new TimeSpan(hours, minutes, 0);
        }
        /// <summary>
        /// �ļ���С��ʽ�� ��СKB
        /// </summary>
        /// <param name="bytes">int��չ</param>
        /// <returns>�ļ���С��ʽ�� ��СKB</returns>
        public static string FormatKB(this int bytes) { return ((long)bytes).FormatKB(); }
        /// <summary>
        /// �ļ���С��ʽ��
        /// </summary>
        /// <param name="bytes">int��չ</param>
        /// <returns>�ļ���С��ʽ�� ��СKB</returns>
        public static string FormatBytes(this int bytes) { return ((long)bytes).FormatBytes(); }
        /// <summary>
        /// ���� start/end����
        /// </summary>
        /// <param name="start">int��չ</param>
        /// <param name="end">end</param>
        /// <returns>���� start/end����</returns>
        public static IEnumerable<int> Range(this int start, int end) {
            for (int i = start; i <= end; i++)
                yield return i;
        }
        /// <summary>
        /// 8����
        /// </summary>
        /// <param name="n">int��չ</param>
        /// <returns>8����</returns>
        public static string ToOctal(this int n) {
            return string.Format("{0:X8}", n);
        }
        /// <summary>
        /// 8����
        /// </summary>
        /// <param name="n">int��չ</param>
        /// <returns>8����</returns>
        public static string ToOctal(this long n) {
            return string.Format("{0:X8}", n);
        }
        /// <summary>
        /// תʱ��
        /// </summary>
        /// <param name="seconds">int��չ</param>
        /// <returns>תʱ�� 1:12:12</returns>
        public static string ToTime(this int seconds) {
            return ((long)seconds).ToTime();
        }
        /// <summary>
        /// תEnum
        /// </summary>
        /// <typeparam name="T">Դ����</typeparam>
        /// <param name="number">int��չ</param>
        /// <returns></returns>
        public static T ToEnum<T>(this int number) {
            return (T)Enum.ToObject(typeof(T), number);
        }
        /// <summary>
        /// ��� ��λ,�ָ�
        /// </summary>
        /// <param name="value">int��չ</param>
        /// <returns>��� ��λ,�ָ�</returns>
        public static string ToCurrency(this int value) {
            return value.ToString("N");
        }
        /// <summary>
        /// �����ת��д
        /// </summary>
        /// <param name="value">int��չ</param>
        /// <returns>�����ת��д</returns>
        public static string ToRMB(this int value) { return ((decimal)value).ToRMB(); }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static int IfEqual(this int obj, int value, int defaultValue) {
            return obj == value ? defaultValue : obj;
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static int IfNotEqual(this int obj, int value, int defaultValue) {
            return obj != value ? defaultValue : obj;
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static int IfMoreThan(this int obj, int value, int defaultValue) {
            return obj > value ? defaultValue : obj;
        }
        /// <summary>
        /// ���С��
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static int IfLessThan(this int obj, int value, int defaultValue) {
            return obj < value ? defaultValue : obj;
        }
        /// <summary>
        /// ������ڵ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static int IfMoreThanOrEqual(this int obj, int value, int defaultValue) {
            return obj >= value ? defaultValue : obj;
        }
        /// <summary>
        /// ���С�ڵ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static int IfLessThanOrEqual(this int obj, int value, int defaultValue) {
            return obj <= value ? defaultValue : obj;
        }
    }
}
