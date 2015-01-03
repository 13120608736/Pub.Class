//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;

namespace Pub.Class {
    /// <summary>
    /// �����������
    /// 
    /// �޸ļ�¼
    ///     2006.05.09 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Rand {
        //#region RndInt
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="num1">��ʼ</param>
        /// <param name="num2">����</param>
        /// <returns>�Ӷ��ٵ�����֮������� ������ʼ����������</returns>
        public static int RndInt(int num1, int num2) {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return rnd.Next(num1, num2);
        }
        /// <summary>
        /// ��������� �б�
        /// </summary>
        /// <param name="num1">��ʼ</param>
        /// <param name="num2">����</param>
        /// <param name="len">��</param>
        /// <returns>��������� �б�</returns>
        public static IList<int> RndInt(int num1, int num2, int len) {
            IList<int> list = new List<int>();
            for (int i = 0; i < len; i++) list.Add(RndInt(num1, num2));
            return list;
        }
        /// <summary>
        /// ��������� �б�
        /// </summary>
        /// <param name="len">��</param>
        /// <returns>��������� �б�</returns>
        public static IList<int> RndInt(int len) {
            IList<int> list = RndInt(0, int.MaxValue, len);
            return list;
        }
        //#endregion
        //#region RndNum
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="len">���ɳ���</param>
        /// <returns>����ָ�����ȵ����������</returns>
        public static string RndNum(int len) {
            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < len; i++) {
                num.Append(arrChar[rnd.Next(0, 9)].ToString());
            }
            return num.ToString();
        }
        /// <summary>
        /// ����������б�
        /// </summary>
        /// <param name="len">��</param>
        /// <param name="count">��</param>
        /// <returns>����������б�</returns>
        public static IList<string> RndNum(int len, int count) {
            IList<string> list = new List<string>();
            for (int i = 0; i < count; i++) list.Add(RndNum(len));
            return list;
        }
        //#endregion
        //#region RndDateStr
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns>�������������</returns>
        public static string RndDateStr() {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + Rand.RndInt(1000, 9999).ToString();
        }
        /// <summary>
        /// ������������б�
        /// </summary>
        /// <param name="len">��</param>
        /// <returns>������������б�</returns>
        public static IList<string> RndDateStr(int len) {
            IList<string> list = new List<string>();
            for (int i = 0; i < len; i++) list.Add(RndDateStr());
            return list;
        }
        //#endregion
        //#region RndCode
        /// <summary>
        /// ���ֺ���ĸ�����
        /// </summary>
        /// <param name="len">���ɳ���</param>
        /// <returns>����ָ�����ȵ����ֺ���ĸ�������</returns>
        public static string RndCode(int len) {
            char[] arrChar = new char[]{
               'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
               '0','1','2','3','4','5','6','7','8','9',
               'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'};
            System.Text.StringBuilder num = new System.Text.StringBuilder();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < len; i++) {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        /// <summary>
        /// ���ֺ���ĸ���������
        /// </summary>
        /// <param name="len">��</param>
        /// <param name="count">��</param>
        /// <returns>���ֺ���ĸ���������</returns>
        public static IList<string> RndCodeList(int len, int count) {
            IList<string> list = new List<string>();
            for (int i = 0; i < count; i++) list.Add(RndCode(len));
            return list;
        }
        //#endregion
        //#region RndLetter
        /// <summary>
        /// ��ĸ�����
        /// </summary>
        /// <param name="len">���ɳ���</param>
        /// <returns>����ָ�����ȵ���ĸ�����</returns>
        public static string RndLetter(int len) {
            char[] arrChar = new char[]{
                'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
                '_',
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'};
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < len; i++) {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        /// <summary>
        /// ��ĸ������б�
        /// </summary>
        /// <param name="len">���ɳ���</param>
        /// <param name="count">��</param>
        /// <returns>��ĸ������б�</returns>
        public static IList<string> RndLetterList(int len, int count) {
            IList<string> list = new List<string>();
            for (int i = 0; i < count; i++) list.Add(RndLetter(len));
            return list;
        }
        //#endregion
        //#region RndGuid
        /// <summary>
        /// ����GUID
        /// </summary>
        /// <returns>����GUID</returns>
        public static string RndGuid() {
            return Guid.NewGuid().ToString();
        }
        /// <summary>
        /// ����GUID �б�
        /// </summary>
        /// <param name="len">��</param>
        /// <returns>����GUID �б�</returns>
        public static IList<string> RndGuid(int len) {
            IList<string> list = new List<string>();
            for (int i = 0; i < len; i++) list.Add(RndGuid());
            return list;
        }
        //#endregion
        //#region RndColor
        /// <summary>
        /// �����ɫ
        /// </summary>
        /// <param name="iUseAlpha">ʹ��͸��</param>
        /// <returns>�����ɫ</returns>
        public static Color RndColor(bool iUseAlpha) {
            int vAlpha = 255;
            if (!iUseAlpha) vAlpha = RndInt(0, 255);
            int vRed = RndInt(0, 255);
            int vBlue = RndInt(0, 255);
            int vGreen = RndInt(0, 255);
            Color vColor = Color.FromArgb(vAlpha, vRed, vGreen, vBlue);
            return vColor;
        }
        /// <summary>
        /// �����ɫ�ַ���
        /// </summary>
        /// <returns>�����ɫ�ַ���</returns>
        public static string RandColor() {
            string vRed = Convert.ToString(RndInt(0, 255), 16); vRed = vRed.Length == 1 ? "0" + vRed : vRed;
            string vBlue = Convert.ToString(RndInt(0, 255), 16); vBlue = vBlue.Length == 1 ? "0" + vBlue : vBlue;
            string vGreen = Convert.ToString(RndInt(0, 255), 16); vGreen = vGreen.Length == 1 ? "0" + vGreen : vGreen;
            return vRed + vBlue + vGreen;
        }
        /// <summary>
        /// �����ɫ�ַ����б�
        /// </summary>
        /// <param name="len">��</param>
        /// <returns>�����ɫ�ַ����б�</returns>
        public static IList<string> RandColor(int len) {
            IList<string> list = new List<string>();
            for (int i = 0; i < len; i++) list.Add(RandColor());
            return list;
        }
        //#endregion
    }
}
