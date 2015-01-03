//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace Pub.Class.WinRAR {
    /// <summary>
    /// ѹ���ļ�
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Compress : ICompress {
        private string rarSetupPath = "c:\\Program Files\\WinRAR\\rar.exe";

        public Compress() {
            if (Registry2.Exists("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths", "WinRAR.exe")) {
                string regPath = Registry2.Read("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\WinRAR.exe", "").ToStr();
                if (FileDirectory.FileExists(regPath)) rarSetupPath = regPath;
            }
            if (!FileDirectory.FileExists(rarSetupPath)) {
                rarSetupPath = "c:\\Program Files (x86)\\WinRAR\\rar.exe";
                if (!FileDirectory.FileExists(rarSetupPath)) {
                    rarSetupPath = "rar.exe".GetBinFileFullPath();
                    if (!FileDirectory.FileExists(rarSetupPath)) {
                        throw new ArgumentNullException("δ�ҵ�WinRAR��װ����");
                    }
                }
            }
        }
        /// <summary>
        /// ѹ���ļ�
        /// </summary>
        /// <param name="source">Դ�ļ�</param>
        /// <param name="desc">Ŀ��ZIP�ļ�·��</param>
        /// <param name="password">����</param>
        public void File(string source, string descZip, string password = null) {
            password = password.IsNullEmpty() ? "" : " -P" + password;
            string msg = Safe.RunWait(rarSetupPath, System.Diagnostics.ProcessWindowStyle.Hidden, " a -ep \"{0}\" \"{1}\"{2}".FormatWith(descZip, source, password));
        }
        /// <summary>
        /// ������ļ�ѹ����һ���ļ�
        /// </summary>
        /// <param name="source">����ļ�������ȫ·��������e:\tmp\tmp1\DD.cs</param>
        /// <param name="descZip">Ŀ��ZIP�ļ�·��</param>
        /// <param name="password">����</param>
        public void File(string[] source, string descZip, string password = null) {
            password = password.IsNullEmpty() ? "" : " -P" + password;
            if (source.Length == 0) return;
            StringBuilder sbFile = new StringBuilder();
            foreach (string info in source) sbFile.AppendFormat(" \"{0}\"", info);
            string msg = Safe.RunWait(rarSetupPath, System.Diagnostics.ProcessWindowStyle.Hidden, " a -ep \"{0}\" {1}{2}".FormatWith(descZip, sbFile.ToString(), password));
        }
        /// <summary>
        /// ѹ��Ŀ¼
        /// </summary>
        /// <param name="source">Ҫѹ����Ŀ¼</param>
        /// <param name="descZip">ѹ������ļ���</param>
        /// <param name="password">����</param>
        public void Directory(string source, string descZip, string password = null) {
            password = password.IsNullEmpty() ? "" : " -P" + password;
            string msg = Safe.RunWait(rarSetupPath, System.Diagnostics.ProcessWindowStyle.Hidden, " a -r -ep1 \"{0}\" \"{1}\\*.*\"{2}".FormatWith(descZip, source.Trim('\\'), password));
        }
    }
}
