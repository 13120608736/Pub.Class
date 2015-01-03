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
using System.Text.RegularExpressions;

namespace Pub.Class {
    /// <summary>
    /// Decimal��չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class DecimalExtensions {
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">ֵ</param>
        /// <param name="percent">�ٷ�֮</param>
        /// <returns>�ٷ���</returns>
        public static decimal PercentageOf(this decimal number, int percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">ֵ</param>
        /// <param name="percent">�ٷ�֮</param>
        /// <returns>�ٷ���</returns>
        public static decimal PercentageOf(this decimal number, decimal percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ���
        /// </summary>
        /// <param name="number">ֵ</param>
        /// <param name="percent">�ٷ�֮</param>
        /// <returns>�ٷ���</returns>
        public static decimal PercentageOf(this decimal number, long percent) {
            return (decimal)(number * percent / 100);
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position">λ��</param>
        /// <param name="total">����</param>
        /// <returns>�ٷ�֮</returns>
        public static decimal PercentOf(this decimal position, int total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)position / (decimal)total * 100;
            return result;
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this decimal position, decimal total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)position / (decimal)total * 100;
            return result;
        }
        /// <summary>
        /// �ٷ�֮
        /// </summary>
        /// <param name="position"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static decimal PercentOf(this decimal position, long total) {
            decimal result = 0;
            if (position > 0 && total > 0) result = (decimal)position / (decimal)total * 100;
            return result;
        }
        /// <summary>
        /// ����decimalPointsλС��
        /// </summary>
        /// <param name="val">ֵ</param>
        /// <param name="decimalPoints">С��λ��</param>
        /// <returns>����decimalPointsλС��</returns>
        public static decimal Round(this decimal val, int decimalPoints) {
            return Math.Round(val, decimalPoints);
        }
        /// <summary>
        /// ����2λС��
        /// </summary>
        /// <param name="val">ֵ</param>
        /// <returns>����2λС��</returns>
        public static decimal Round2(this decimal val) {
            return Math.Round(val, 2);
        }
        /// <summary>
        /// ��� ��λ,�ָ�
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns>��� ��λ,�ָ�</returns>
        public static string ToCurrency(this decimal value) {
            return value.ToString("N");
        }
        /// <summary>
        /// �����ת��д
        /// </summary>
        /// <param name="num">�����</param>
        /// <returns>��д�����</returns>
        public static string ToRMB2(this decimal num) {
            string str = "��Ҽ��������½��ƾ�";
            string str2 = "��Ǫ��ʰ��Ǫ��ʰ��Ǫ��ʰԪ�Ƿ�";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            string str7 = "";
            int num4 = 0;
            num = Math.Round(Math.Abs(num), 2);
            str4 = ((long)(num * 100M)).ToString();
            int length = str4.Length;
            if (length > 15) {
                return "���";
            }
            str2 = str2.Substring(15 - length);
            for (int i = 0; i < length; i++) {
                str3 = str4.Substring(i, 1);
                int startIndex = Convert.ToInt32(str3);
                if (((i != (length - 3)) && (i != (length - 7))) && ((i != (length - 11)) && (i != (length - 15)))) {
                    if (str3 == "0") {
                        str6 = "";
                        str7 = "";
                        num4++;
                    } else if ((str3 != "0") && (num4 != 0)) {
                        str6 = "��" + str.Substring(startIndex, 1);
                        str7 = str2.Substring(i, 1);
                        num4 = 0;
                    } else {
                        str6 = str.Substring(startIndex, 1);
                        str7 = str2.Substring(i, 1);
                        num4 = 0;
                    }
                } else if ((str3 != "0") && (num4 != 0)) {
                    str6 = "��" + str.Substring(startIndex, 1);
                    str7 = str2.Substring(i, 1);
                    num4 = 0;
                } else if ((str3 != "0") && (num4 == 0)) {
                    str6 = str.Substring(startIndex, 1);
                    str7 = str2.Substring(i, 1);
                    num4 = 0;
                } else if ((str3 == "0") && (num4 >= 3)) {
                    str6 = "";
                    str7 = "";
                    num4++;
                } else if (length >= 11) {
                    str6 = "";
                    num4++;
                } else {
                    str6 = "";
                    str7 = str2.Substring(i, 1);
                    num4++;
                }
                if ((i == (length - 11)) || (i == (length - 3))) {
                    str7 = str2.Substring(i, 1);
                }
                str5 = str5 + str6 + str7;
                if ((i == (length - 1)) && (str3 == "0")) {
                    str5 = str5 + '��';
                }
            }
            if (num == 0M) {
                str5 = "��Ԫ��";
            }
            return str5;
        }
        /// <summary>
        /// ������ת����д�����
        /// </summary>
        /// <param name="input">�����</param>
        /// <returns>��д�����</returns>
        public static string ToRMB(this decimal input) {
            string s = input.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            string result = Regex.Replace(d, ".", delegate(Match m) { return "��Ԫ����Ҽ��������½��ƾ��տտտտտտշֽ�ʰ��Ǫ�f�|�׾������"[m.Value[0] - '-'].ToString(); });
            return result;
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static decimal IfEqual(this decimal obj, decimal value, decimal defaultValue) {
            return obj == value ? defaultValue : obj;
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static decimal IfNotEqual(this decimal obj, decimal value, decimal defaultValue) {
            return obj != value ? defaultValue : obj;
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static decimal IfMoreThan(this decimal obj, decimal value, decimal defaultValue) {
            return obj > value ? defaultValue : obj;
        }
        /// <summary>
        /// ���С��
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static decimal IfLessThan(this decimal obj, decimal value, decimal defaultValue) {
            return obj < value ? defaultValue : obj;
        }
        /// <summary>
        /// ������ڵ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static decimal IfMoreThanOrEqual(this decimal obj, decimal value, decimal defaultValue) {
            return obj >= value ? defaultValue : obj;
        }
        /// <summary>
        /// ���С�ڵ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <param name="value">Ŀ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static decimal IfLessThanOrEqual(this decimal obj, decimal value, decimal defaultValue) {
            return obj <= value ? defaultValue : obj;
        }
    }
}
