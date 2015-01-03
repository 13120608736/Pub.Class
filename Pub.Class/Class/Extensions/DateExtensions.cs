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
using System.Globalization;
using Microsoft.VisualBasic;
using System.Data.SqlTypes;

namespace Pub.Class {
    /// <summary>
    /// ������չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class DateExtensions {
        private static readonly TimeSpan _OneMinute = new TimeSpan(0, 1, 0);
        private static readonly TimeSpan _TwoMinutes = new TimeSpan(0, 2, 0);
        private static readonly TimeSpan _OneHour = new TimeSpan(1, 0, 0);
        private static readonly TimeSpan _TwoHours = new TimeSpan(2, 0, 0);
        private static readonly TimeSpan _OneDay = new TimeSpan(1, 0, 0, 0);
        private static readonly TimeSpan _TwoDays = new TimeSpan(2, 0, 0, 0);
        private static readonly TimeSpan _OneWeek = new TimeSpan(7, 0, 0, 0);
        private static readonly TimeSpan _TwoWeeks = new TimeSpan(14, 0, 0, 0);
        private static readonly TimeSpan _OneMonth = new TimeSpan(31, 0, 0, 0);
        private static readonly TimeSpan _TwoMonths = new TimeSpan(62, 0, 0, 0);
        private static readonly TimeSpan _OneYear = new TimeSpan(365, 0, 0, 0);
        private static readonly TimeSpan _TwoYears = new TimeSpan(730, 0, 0, 0);
        /// <summary>
        /// ʱ���
        /// </summary>
        /// <param name="startTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <returns>TimeSpanʱ���</returns>
        public static TimeSpan GetTimeSpan(this DateTime startTime, DateTime endTime) {
            return endTime - startTime;
        }
        /// <summary>
        /// ����ת�ַ���yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="time">����</param>
        /// <returns>ת�ַ���yyyy-MM-dd HH:mm:ss</returns>
        public static string ToDateTime(this DateTime time) {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// ����ת�ַ���yyyy-MM-dd
        /// </summary>
        /// <param name="time">����</param>
        /// <returns>ת�ַ���yyyy-MM-dd</returns>
        public static string ToDate(this DateTime time) {
            return time.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// ����ת�ַ���HH:mm:ss
        /// </summary>
        /// <param name="time">����</param>
        /// <returns>ת�ַ���HH:mm:ss</returns>
        public static string ToTime(this DateTime time) {
            return time.ToString("HH:mm:ss");
        }
        /// <summary>
        /// ����ת�ַ���HH:mm
        /// </summary>
        /// <param name="time">����</param>
        /// <returns>ת�ַ���HH:mm</returns>
        public static string ToHHmm(this DateTime time) {
            return time.ToString("HH:mm");
        }
        /// <summary>
        /// ����ת�ַ���yyyy-MM-dd HH:mm:ss.fffffff
        /// </summary>
        /// <param name="time">����</param>
        /// <returns>ת�ַ���yyyy-MM-dd HH:mm:ss.fffffff</returns>
        public static string ToDateTimeF(this DateTime time) {
            return time.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        }
        /// <summary>
        /// ����ת�ַ���yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        /// <param name="time">����</param>
        /// <returns>ת�ַ���yyyy-MM-dd HH:mm:ss.fff</returns>
        public static string ToDateTimeFFF(this DateTime time) {
            return time.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        /// <summary>
        /// ȡ����
        /// </summary>
        /// <param name="dateOfBirth">��������</param>
        /// <returns>��������</returns>
        public static int ToAge(this DateTime dateOfBirth) {
            return ToAge(dateOfBirth, DateTime.Today);
        }
        /// <summary>
        /// ȡ����
        /// </summary>
        /// <param name="dateOfBirth">��������</param>
        /// <param name="referenceDate">�ο�����</param>
        /// <returns>��������</returns>
        public static int ToAge(this DateTime dateOfBirth, DateTime referenceDate) {
            int years = referenceDate.Year - dateOfBirth.Year;
            if (referenceDate.Month < dateOfBirth.Month || (referenceDate.Month == dateOfBirth.Month && referenceDate.Day < dateOfBirth.Day)) --years;
            return years;
        }
        /// <summary>
        /// ���¶�����
        /// </summary>
        /// <param name="date">����</param>
        /// <returns>����</returns>
        public static int GetCountDaysOfMonth(this DateTime date) {
            var nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
        }
        /// <summary>
        /// ���µĵ�һ��
        /// </summary>
        /// <param name="date">����</param>
        /// <returns>���µĵ�һ��DateTime</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date) {
            return new DateTime(date.Year, date.Month, 1);
        }
        /// <summary>
        /// ���µĵ�һ��
        /// </summary>
        /// <param name="date">����</param>
        /// <param name="dayOfWeek">���ڼ�</param>
        /// <returns>���µĵ�һ��DateTime</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date, DayOfWeek dayOfWeek) {
            var dt = date.GetFirstDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek) dt = dt.AddDays(1);
            return dt;
        }
        /// <summary>
        /// ���µ������
        /// </summary>
        /// <param name="date">����</param>
        /// <returns>���µ������DateTime</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date) {
            return new DateTime(date.Year, date.Month, GetCountDaysOfMonth(date));
        }
        /// <summary>
        /// ���µ������
        /// </summary>
        /// <param name="date">����</param>
        /// <param name="dayOfWeek">���ڼ�</param>
        /// <returns>���µ������DateTime</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date, DayOfWeek dayOfWeek) {
            var dt = date.GetLastDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek) dt = dt.AddDays(-1);
            return dt;
        }
        /// <summary>
        /// �����
        /// </summary>
        /// <param name="dt">����</param>
        /// <returns>true/false</returns>
        public static bool IsToday(this DateTime dt) {
            return (dt.Date == DateTime.Today);
        }
        /// <summary>
        /// �����
        /// </summary>
        /// <param name="dto">ʱ���</param>
        /// <returns>true/false</returns>
        public static bool IsToday(this DateTimeOffset dto) {
            return (dto.Date.IsToday());
        }
        /// <summary>
        /// ��ֵʱ��
        /// </summary>
        /// <param name="date">����</param>
        /// <param name="hours">ʱ</param>
        /// <param name="minutes">��</param>
        /// <param name="seconds">��</param>
        /// <returns>DateTime</returns>
        public static DateTime SetTime(this DateTime date, int hours, int minutes, int seconds) {
            return date.SetTime(new TimeSpan(hours, minutes, seconds));
        }
        /// <summary>
        /// ��ֵʱ��
        /// </summary>
        /// <param name="date">����</param>
        /// <param name="time">ʱ��</param>
        /// <returns>DateTime</returns>
        public static DateTime SetTime(this DateTime date, TimeSpan time) {
            return date.Date.Add(time);
        }
#if !(NET20 || MONO40)
        /// <summary>
        /// ToDateTimeOffset ����תʱ���
        /// </summary>
        /// <param name="localDateTime">ʱ��</param>
        /// <returns>DateTimeOffset</returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime) {
            return localDateTime.ToDateTimeOffset(null);
        }
        /// <summary>
        /// ToDateTimeOffset ����תʱ���
        /// </summary>
        /// <param name="localDateTime">ʱ��</param>
        /// <param name="localTimeZone">localTimeZone</param>
        /// <returns>DateTimeOffset</returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime, TimeZoneInfo localTimeZone) {
            localTimeZone = (localTimeZone ?? TimeZoneInfo.Local);
            if (localDateTime.Kind != DateTimeKind.Unspecified) localDateTime = new DateTime(localDateTime.Ticks, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, localTimeZone);
        }
#endif
        /// <summary>
        /// ���ܵĵ�һ��
        /// </summary>
        /// <param name="date">����</param>
        /// <returns>���ܵĵ�һ��DateTime</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date) {
            return date.GetFirstDayOfWeek(null);
        }
        /// <summary>
        /// ���ܵĵ�һ��
        /// </summary>
        /// <param name="date">����</param>
        /// <param name="cultureInfo">��������</param>
        /// <returns>���ܵĵ�һ��DateTime</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date, CultureInfo cultureInfo) {
            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);
            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            while (date.DayOfWeek != firstDayOfWeek) date = date.AddDays(-1);
            return date;
        }
        /// <summary>
        /// ���ܵ����һ��
        /// </summary>
        /// <param name="date">����</param>
        /// <returns>���ܵ����һ��DateTime</returns>
        public static DateTime GetLastDayOfWeek(this DateTime date) {
            return date.GetLastDayOfWeek(null);
        }
        /// <summary>
        /// ���ܵ����һ��
        /// </summary>
        /// <param name="date">����</param>
        /// <param name="cultureInfo">��������</param>
        /// <returns>���ܵ����һ��DateTime</returns>
        public static DateTime GetLastDayOfWeek(this DateTime date, CultureInfo cultureInfo) {
            return date.GetFirstDayOfWeek(cultureInfo).AddDays(6);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="date">ʱ��</param>
        /// <param name="weekday">���ڼ�</param>
        /// <returns>����DateTime</returns>
        public static DateTime GetWeekday(this DateTime date, DayOfWeek weekday) {
            return date.GetWeekday(weekday, null);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="date">ʱ��</param>
        /// <param name="weekday">���ڼ�</param>
        /// <param name="cultureInfo">��������</param>
        /// <returns>����DateTime</returns>
        public static DateTime GetWeekday(this DateTime date, DayOfWeek weekday, CultureInfo cultureInfo) {
            var firstDayOfWeek = date.GetFirstDayOfWeek(cultureInfo);
            return firstDayOfWeek.GetNextWeekday(weekday);
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="date">ʱ��</param>
        /// <param name="weekday">���ڼ�</param>
        /// <returns>������DateTime</returns>
        public static DateTime GetNextWeekday(this DateTime date, DayOfWeek weekday) {
            while (date.DayOfWeek != weekday) date = date.AddDays(1);
            return date;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="date">ʱ��</param>
        /// <param name="weekday">���ڼ�</param>
        /// <returns>������DateTime</returns>
        public static DateTime GetPreviousWeekday(this DateTime date, DayOfWeek weekday) {
            while (date.DayOfWeek != weekday) date = date.AddDays(-1);
            return date;
        }
#if !(NET20 || MONO40)
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <param name="date">ʱ���</param>
        /// <param name="hours">ʱ</param>
        /// <param name="minutes">��</param>
        /// <param name="seconds">��</param>
        /// <returns>DateTimeOffset</returns>
        public static DateTimeOffset SetTime(this DateTimeOffset date, int hours, int minutes, int seconds) {
            return date.SetTime(new TimeSpan(hours, minutes, seconds));
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <param name="date">ʱ���</param>
        /// <param name="time">ʱ��</param>
        /// <returns>DateTimeOffset</returns>
        public static DateTimeOffset SetTime(this DateTimeOffset date, TimeSpan time) {
            return date.SetTime(time, null);
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <param name="date">ʱ���</param>
        /// <param name="time">ʱ��</param>
        /// <param name="localTimeZone">TimeZoneInfo</param>
        /// <returns>DateTimeOffset</returns>
        public static DateTimeOffset SetTime(this DateTimeOffset date, TimeSpan time, TimeZoneInfo localTimeZone) {
            var localDate = date.ToLocalDateTime(localTimeZone);
            localDate.SetTime(time);
            return localDate.ToDateTimeOffset(localTimeZone);
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <param name="dateTimeUtc">ʱ���</param>
        /// <returns>DateTime</returns>
        public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeUtc) {
            return dateTimeUtc.ToLocalDateTime(null);
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <param name="dateTimeUtc">ʱ���</param>
        /// <param name="localTimeZone">TimeZoneInfo</param>
        /// <returns>DateTime</returns>
        public static DateTime ToLocalDateTime(this DateTimeOffset dateTimeUtc, TimeZoneInfo localTimeZone) {
            localTimeZone = (localTimeZone ?? TimeZoneInfo.Local);
            return TimeZoneInfo.ConvertTime(dateTimeUtc, localTimeZone).DateTime;
        }
        /// <summary>
        /// ������ʱ�������ڵ�ʱ���ý��죬���죬ǰ���ʾ�����ʱ�䣬������
        /// </summary>
        /// <param name="time">���Ƚϵ�ʱ��</param>
        /// <param name="currentDateTime">Ŀ��ʱ��</param>
        /// <returns>������ʱ�������ڵ�ʱ���ý��죬���죬ǰ���ʾ�����ʱ�䣬������</returns>
        public static string ToAgo(this DateTime time, DateTime currentDateTime) {
            string result = "";
            if (currentDateTime.Year == time.Year && currentDateTime.Month == time.Month) { //���date�͵�ǰʱ����ݻ����·ݲ�һ�£���ֱ�ӷ���"yyyy-MM-dd HH:mm"��ʽ����
                if (DateDiff(DateInterval.Hour, time, currentDateTime) <= 10) { //���date�͵�ǰʱ������10Сʱ��(������3Сʱ)
                    if (DateDiff(DateInterval.Hour, time, currentDateTime) > 0) return DateDiff(DateInterval.Hour, time, currentDateTime) + "Сʱǰ";
                    if (DateDiff(DateInterval.Minute, time, currentDateTime) > 0) return DateDiff(DateInterval.Minute, time, currentDateTime) + "����ǰ";
                    if (DateDiff(DateInterval.Second, time, currentDateTime) >= 0) return DateDiff(DateInterval.Second, time, currentDateTime) + "��ǰ";
                    else return "�ղ�";//Ϊ�˽��postdatetimeʱ�侫�Ȳ������·���ʱ������ļ���
                } else {
                    switch (currentDateTime.Day - time.Day) {
                        case 0: result = "���� " + time.ToString("HH") + ":" + time.ToString("mm"); break;
                        case 1: result = "���� " + time.ToString("HH") + ":" + time.ToString("mm"); break;
                        case 2: result = "ǰ�� " + time.ToString("HH") + ":" + time.ToString("mm"); break;
                        default: result = time.ToString("yyyy-MM-dd HH:mm"); break;
                    }
                }
            } else result = time.ToString("yyyy-MM-dd HH:mm");
            return result;
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="value">��ʼʱ��</param>
        /// <param name="date">����ʱ��</param>
        /// <param name="interval">��ʽ�� DateInterval.Year DateInterval.Month DateInterval.Day DateInterval.Hour DateInterval.Minute DateInterval.Second DateInterval.WeekOfYear DateInterval.Quarter</param>
        /// <returns></returns>
        public static long DateDiff(this DateTime value, DateTime date, DateInterval interval) {
            if (interval == DateInterval.Year) return date.Year - value.Year;
            if (interval == DateInterval.Month) return (date.Month - value.Month) + (12 * (date.Year - value.Year));
            TimeSpan ts = date - value;
            if (interval == DateInterval.Day || interval == DateInterval.DayOfYear) return ts.TotalDays.Round();
            if (interval == DateInterval.Hour) return ts.TotalHours.Round();
            if (interval == DateInterval.Minute) return ts.TotalMinutes.Round();
            if (interval == DateInterval.Second) return ts.TotalSeconds.Round();
            if (interval == DateInterval.Weekday) return (ts.TotalDays / 7.0).Round();
            if (interval == DateInterval.WeekOfYear) {
                while (date.DayOfWeek != DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek) date = date.AddDays(-1);
                while (value.DayOfWeek != DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek) value = value.AddDays(-1);
                ts = date - value;
                return (ts.TotalDays / 7.0).Round();
            }
            if (interval == DateInterval.Quarter) {
                double valueQuarter = GetQuarter(value.Month);
                double dateQuarter = GetQuarter(date.Month);
                double valueDiff = dateQuarter - valueQuarter;
                double dateDiff = 4 * (date.Year - value.Year);
                return (valueDiff + dateDiff).Round();
            }
            return 0;
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="interval">��ʽ�� DateInterval.Year DateInterval.Month DateInterval.Day DateInterval.Hour DateInterval.Minute DateInterval.Second DateInterval.WeekOfYear DateInterval.Quarter</param>
        /// <param name="date1">��ʼʱ��</param>
        /// <param name="date2">����ʱ��</param>
        /// <returns></returns>
        private static long DateDiff(this DateInterval interval, DateTime date1, DateTime date2) {
            if (interval == DateInterval.Year) return date2.Year - date1.Year;
            if (interval == DateInterval.Month) return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
            TimeSpan ts = date2 - date1;
            if (interval == DateInterval.Day || interval == DateInterval.DayOfYear) return ts.TotalDays.Round();
            if (interval == DateInterval.Hour) return ts.TotalHours.Round();
            if (interval == DateInterval.Minute) return ts.TotalMinutes.Round();
            if (interval == DateInterval.Second) return ts.TotalSeconds.Round();
            if (interval == DateInterval.Weekday) return (ts.TotalDays / 7.0).Round();
            if (interval == DateInterval.WeekOfYear) {
                while (date2.DayOfWeek != DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek) date2 = date2.AddDays(-1);
                while (date1.DayOfWeek != DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek) date1 = date1.AddDays(-1);
                ts = date2 - date1;
                return (ts.TotalDays / 7.0).Round();
            }
            if (interval == DateInterval.Quarter) {
                double date1Quarter = GetQuarter(date1.Month);
                double date2Quarter = GetQuarter(date2.Month);
                double date1Diff = date2Quarter - date1Quarter;
                double date2Diff = 4 * (date2.Year - date1.Year);
                return (date1Diff + date2Diff).Round();
            }
            return 0;
        }
#endif
        /// <summary>
        /// ***ǰ ��1����ǰ 1Сʱǰ
        /// </summary>
        /// <param name="date">ʱ��</param>
        /// <returns>***ǰ ��1����ǰ 1Сʱǰ</returns>
        public static string ToAgo(this DateTime date) {
            TimeSpan timeSpan = date.GetTimeSpan(DateTime.Now);
            if (timeSpan < TimeSpan.Zero) return "δ��";
            if (timeSpan < _OneMinute) return "����";
            if (timeSpan < _TwoMinutes) return "1 ����ǰ";
            if (timeSpan < _OneHour) return String.Format("{0} ����ǰ", timeSpan.Minutes);
            if (timeSpan < _TwoHours) return "1 Сʱǰ";
            if (timeSpan < _OneDay) return String.Format("{0} Сʱǰ", timeSpan.Hours);
            if (timeSpan < _TwoDays) return "����";
            if (timeSpan < _OneWeek) return String.Format("{0} ��ǰ", timeSpan.Days);
            if (timeSpan < _TwoWeeks) return "1 ��ǰ";
            if (timeSpan < _OneMonth) return String.Format("{0} ��ǰ", timeSpan.Days / 7);
            if (timeSpan < _TwoMonths) return "1 ��ǰ";
            if (timeSpan < _OneYear) return String.Format("{0} ��ǰ", timeSpan.Days / 31);
            if (timeSpan < _TwoYears) return "1 ��ǰ";
            return String.Format("{0} ��ǰ", timeSpan.Days / 365);
        }
        /// <summary>
        /// һ�������
        /// </summary>
        /// <param name="datetime">����</param>
        /// <returns>һ�������</returns>
        public static int WeekOfYear(this DateTime datetime) {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            System.Globalization.CalendarWeekRule weekrule = dateinf.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// һ�������
        /// </summary>
        /// <param name="datetime">����</param>
        /// <param name="weekrule">��һ�ܵĹ���</param>
        /// <returns>һ�������</returns>
        public static int WeekOfYear(this DateTime datetime, System.Globalization.CalendarWeekRule weekrule) {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            DayOfWeek firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// һ�������
        /// </summary>
        /// <param name="datetime">����</param>
        /// <param name="firstDayOfWeek">���ڼ�</param>
        /// <returns>һ�������</returns>
        public static int WeekOfYear(this DateTime datetime, DayOfWeek firstDayOfWeek) {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            System.Globalization.CalendarWeekRule weekrule = dateinf.CalendarWeekRule;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// һ�������
        /// </summary>
        /// <param name="datetime">����</param>
        /// <param name="weekrule">��һ�ܵĹ���</param>
        /// <param name="firstDayOfWeek">���ڼ�</param>
        /// <returns>һ�������</returns>
        public static int WeekOfYear(this DateTime datetime, System.Globalization.CalendarWeekRule weekrule, DayOfWeek firstDayOfWeek) {
            System.Globalization.CultureInfo ciCurr = System.Globalization.CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        /// <summary>
        /// �ڼ�����
        /// </summary>
        /// <param name="month">����</param>
        /// <returns></returns>
        public static int GetQuarter(this int month) {
            if (month <= 3) return 1;
            if (month <= 6) return 2;
            if (month <= 9) return 3;
            return 4;
        }
        /// <summary>
        /// ���շ�
        /// </summary>
        /// <param name="date">����</param>
        /// <returns>true/false</returns>
        public static bool IsWeekday(this DateTime date) {
            return !date.IsWeekend();
        }
        /// <summary>
        /// ��ĩ��
        /// </summary>
        /// <param name="value">����</param>
        /// <returns>true/false</returns>
        public static bool IsWeekend(this DateTime value) {
            return value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday;
        }
        /// <summary>
        /// �����
        /// </summary>
        /// <param name="value">����</param>
        /// <returns>true/false</returns>
        public static bool IsLeapYear(this DateTime value) {
            return System.DateTime.IsLeapYear(value.Year);
        }
        /// <summary>
        /// һ��Ŀ�ʼʱ�� ��2011-1-1 0:0:0
        /// </summary>
        /// <param name="date">����</param>
        /// <returns>һ��Ŀ�ʼʱ�� ��2011-1-1 0:0:0</returns>
        public static DateTime DayBegin(this DateTime date) {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }
        /// <summary>
        /// һ��Ľ���ʱ�� ��2011-1-1 23:59:59
        /// </summary>
        /// <param name="date">����</param>
        /// <returns>һ��Ľ���ʱ�� ��2011-1-1 23:59:59</returns>
        public static DateTime DayEnd(this DateTime date) {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }
        /// <summary>
        /// SQL����
        /// </summary>
        /// <param name="obj">ʱ��</param>
        /// <returns>SQL����</returns>
        public static DateTime ToSqlDate(this object obj) {
            DateTime dt = Convert.ToDateTime(obj);
            DateTime dtMin = SqlDateTime.MinValue.Value;
            if (dt < dtMin) return SqlDateTime.MinValue.Value;
            DateTime dtMax = SqlDateTime.MaxValue.Value;
            if (dt > dtMax) return SqlDateTime.MaxValue.Value;
            return dt;
        }
        /// <summary>
        /// IsOnTime ʱ��val��requiredTime֮��Ĳ�ֵ�Ƿ���maxToleranceInSecs��Χ֮�ڡ�
        /// </summary>
        /// <param name="requiredTime">��ʼʱ��</param>
        /// <param name="val">����ʱ��</param>
        /// <param name="maxToleranceInSecs">��Χ</param>
        /// <returns>IsOnTime ʱ��val��requiredTime֮��Ĳ�ֵ�Ƿ���maxToleranceInSecs��Χ֮�ڡ� true/false</returns>
        public static bool IsOnTime(this DateTime requiredTime, DateTime val, int maxToleranceInSecs) {
            TimeSpan span = val - requiredTime;
            double spanMilliseconds = span.TotalMilliseconds < 0 ? (-span.TotalMilliseconds) : span.TotalMilliseconds;
            return (spanMilliseconds <= (maxToleranceInSecs * 1000));
        }
        /// <summary>
        /// IsOnTime ����ѭ�����ã�ʱ��val��startTime֮��Ĳ�ֵ(>0)��cycleSpanInSecs�������Ľ���Ƿ���maxToleranceInSecs��Χ֮�ڡ�
        /// </summary>
        /// <param name="startTime">��ʼʱ��</param>
        /// <param name="val">����ʱ��</param>
        /// <param name="cycleSpanInSecs">��cycleSpanInSecs������</param>
        /// <param name="maxToleranceInSecs">��Χ֮��</param>
        /// <returns>IsOnTime ����ѭ�����ã�ʱ��val��startTime֮��Ĳ�ֵ(>0)��cycleSpanInSecs�������Ľ���Ƿ���maxToleranceInSecs��Χ֮�ڡ� true/false</returns>
        public static bool IsOnTime(this DateTime startTime, DateTime val, int cycleSpanInSecs, int maxToleranceInSecs) {
            TimeSpan span = val - startTime;
            double spanMilliseconds = span.TotalMilliseconds;
            double residual = spanMilliseconds % (cycleSpanInSecs * 1000);
            return (residual <= (maxToleranceInSecs * 1000));
        }
        /// <summary>
        /// RFC822
        /// </summary>
        /// <param name="date">ʱ��</param>
        /// <returns>RFC822ʱ���ַ���</returns>
        public static string ToRFC822Time(this DateTime date) {
            int offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            string timeZone = "+" + offset.ToString().PadLeft(2, '0');
            if (offset < 0) {
                int i = offset * -1;
                timeZone = "-" + i.ToString().PadLeft(2, '0');
            }
            return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'), System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }
        /// <summary>
        /// ����ת��д
        /// </summary>
        /// <param name="time">��ǰʱ��</param>
        /// <returns>����ת��д</returns>
        public static string ToUpper(this DateTime time) {
            string number = "��һ�����������߰˾�";
            System.Text.StringBuilder date = new System.Text.StringBuilder();
            string[] infos = new string[] { time.Year.ToString(), time.Month.ToString("00"), time.Day.ToString("00") };
            int value;
            for (int i = 0; i < infos[0].Length; i++) {
                value = int.Parse(infos[0].Substring(i, 1));
                date.Append(number.Substring(value, 1));
            }
            date.Append("��");

            for (int i = 0; i < infos[1].Length; i++) {
                value = int.Parse(infos[1].Substring(i, 1));
                if (i == 0) {
                    if (value > 0) date.Append("ʮ");
                } else {
                    if (value > 0) date.Append(number.Substring(value, 1));
                }
            }
            date.Append("��");
            for (int i = 0; i < infos[2].Length; i++) {
                value = int.Parse(infos[2].Substring(i, 1));
                if (i == 0) {
                    if (value > 0) {
                        if (value > 1) date.Append(number.Substring(value, 1));
                        date.Append("ʮ");
                    }
                } else {
                    if (value > 0) date.Append(number.Substring(value, 1));
                }
            }
            date.Append("��");
            return date.ToString();
        }
        /// <summary>
        /// ToJavascriptDate
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static double ToJavascriptDate(this DateTime dt) {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = dt.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }
        /// <summary>
        /// �¸���
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime NextMonth(this DateTime dt) {
            return dt.AddMonths(1);
        }
        /// <summary>
        /// �ϸ���
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime PrevMonth(this DateTime dt) {
            return dt.AddMonths(-1);
        }
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static int ToUnixTimestamp(DateTime dateTime) {
            TimeSpan diff = dateTime.ToUniversalTime() - _epoch;
            return Convert.ToInt32(Math.Floor(diff.TotalSeconds));
        }
        public static DateTime FromUnixTimestamp(int timestamp) {
            return (_epoch + TimeSpan.FromSeconds(timestamp)).ToLocalTime();
        }
    }
}
