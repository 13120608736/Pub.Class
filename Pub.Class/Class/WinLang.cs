//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Pub.Class {
    /// <summary>
    /// ��������
    /// 
    /// �޸ļ�¼
    ///     2006.05.17 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class WinLang {
        //#region WinLang
        /// <summary>
        /// �����ļ�Ŀ¼
        /// </summary>
        public static string LangPath = string.Empty;
        /// <summary>
        /// ��ʼ�������ļ�Ŀ¼
        /// </summary>
        /// <param name="langPath">�����ļ�Ŀ¼</param>
        public static void SetPath(string langPath) {
            LangPath = langPath.TrimEnd('\\') + "\\";
        }
        /// <summary>
        /// �������������ļ��б� 0Ϊ��ǰʹ�õ�����
        /// </summary>
        /// <returns>�������������ļ��б� 0Ϊ��ǰʹ�õ�����</returns>
        public static string[] GetLangList() {
            DirectoryInfo Folder = new DirectoryInfo(LangPath);
            FileInfo[] subFiles = Folder.GetFiles();
            string fileList = string.Empty;

            string LangConfigFile = LangPath + "langConfig.inf";
            IniFile ini = new IniFile(LangConfigFile);
            string DefaultLang = ini.ReadValue("Language", "DefaultLang");
            string firstLang = string.Empty;

            for (int j = 0; j < subFiles.Length; j++) {
                if (subFiles[j].Extension.ToLower().Equals(".ini")) {
                    firstLang = subFiles[0].Name.Substring(0, subFiles[0].Name.Length - 4);
                    fileList += subFiles[j].Name.Substring(0, subFiles[j].Name.Length - 4) + "|";
                }
            }

            if (DefaultLang.Equals("Cookies")) {
                DefaultLang = Cookie2.Get("Lang", "Default").Trim().Base64Decode();
                if (!DefaultLang.Equals(string.Empty)) fileList = DefaultLang + "|" + fileList; else fileList = firstLang + "|" + fileList;
            } else fileList = DefaultLang + "|" + fileList;
            return fileList.TrimEnd('|').Split('|');
        }
        /// <summary>
        /// �޸�ʹ�õ�����
        /// </summary>
        /// <param name="langName">����</param>
        public static void SetLang(string langName) {
            string LangConfigFile = LangPath + "langConfig.inf";
            IniFile ini = new IniFile(LangConfigFile);
            ini.WriteValue("Language", "DefaultLang", langName);
        }
        /// <summary>
        /// ��������ѡ�������б�
        /// </summary>
        /// <returns>��������ѡ�������б�</returns>
        public static string cboLangList() {
            string LangConfigFile = LangPath + "langConfig.inf";
            IniFile ini = new IniFile(LangConfigFile);
            string DefaultLang = ini.ReadValue("Language", "DefaultLang");
            string CookiesDefaultLang = ini.ReadValue("Language", "CookiesDefaultLang");

            string _value = string.Empty;
            if (DefaultLang.Equals("Cookies")) {
                DefaultLang = Cookie2.Get("Lang", "Default").Trim().Base64Decode();
                if (DefaultLang.Trim().Equals(string.Empty)) DefaultLang = CookiesDefaultLang;
                _value = "<select name=\"cboLangList\" onchange=\"window.location='changeLang.aspx?lang=' + this.value\">";
                string[] LangArr = GetLangList();
                for (int i = 1; i < LangArr.Length; i++) {
                    _value += "<option value=\"" + LangArr[i] + "\" " + Selected(DefaultLang, LangArr[i]) + ">" + LangArr[i] + "</option>";
                }
                _value += "</select>";
            }
            return _value;
        }
        /// <summary>
        /// ����selected
        /// </summary>
        /// <param name="str1">ֵ1</param>
        /// <param name="str2">ֵ2</param>
        /// <returns>����selected</returns>
        public static string Selected(string str1, string str2) {
            if (str1 == str2) return "selected"; else return string.Empty;
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="section">section</param>
        /// <param name="key">KEY</param>
        /// <returns>������Ϣ</returns>
        public static string Msg(string section, string key) {
            string LangConfigFile = LangPath + "langConfig.inf";
            IniFile ini = new IniFile(LangConfigFile);
            string DefaultLang = ini.ReadValue("Language", "DefaultLang");
            string CookiesDefaultLang = ini.ReadValue("Language", "CookiesDefaultLang");

            if (DefaultLang.Equals("Cookies")) DefaultLang = Cookie2.Get("Lang", "Default").Trim().Base64Decode();
            if (DefaultLang.Trim().Equals(string.Empty)) {
                LangConfigFile = LangPath + CookiesDefaultLang + ".ini";
            } else LangConfigFile = LangPath + DefaultLang + ".ini";
            //Pub.Class.Msg.Write(LangConfigFile);
            ini = new IniFile(LangConfigFile);
            string msgValue = ini.ReadValue("system", "msgExit");
            return msgValue;
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="frmName">WINFORM</param>
        /// <param name="section">section</param>
        /// <param name="key">key</param>
        /// <returns>������Ϣ</returns>
        public static string Msg(string frmName, string section, string key) {
            if (frmName.Equals(string.Empty)) return string.Empty;
            string LangConfigFile = LangPath + "langConfig.inf";
            IniFile ini = new IniFile(LangConfigFile);
            string DefaultLang = ini.ReadValue("Language", "DefaultLang");
            string CookiesDefaultLang = ini.ReadValue("Language", "CookiesDefaultLang");

            if (DefaultLang.Equals("Cookies")) DefaultLang = Cookie2.Get("Lang", "Default").Trim().Base64Decode();
            if (DefaultLang.Trim().Equals(string.Empty)) {
                LangConfigFile = LangPath + CookiesDefaultLang + "\\" + frmName + ".ini";
            } else LangConfigFile = LangPath + DefaultLang + "\\" + frmName + ".ini";
            //Pub.Class.Msg.Write(LangConfigFile);
            ini = new IniFile(LangConfigFile);
            string msgValue = ini.ReadValue("system", "msgExit");
            return msgValue;
        }
        //#endregion
    }
}
