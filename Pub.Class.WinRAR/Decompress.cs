//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Text;
using System.IO;

namespace Pub.Class.WinRAR {
    /// <summary>
    /// ��ѹ���ļ�
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Decompress: IDecompress {
        private string rarSetupPath = "c:\\Program Files\\WinRAR\\rar.exe";

        public Decompress() {
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
        /// ��ѹ���ļ�
        /// </summary>
        /// <param name="zipPath">Դ�ļ�</param>
        /// <param name="directory">Ŀ���ļ�</param>
        /// <param name="password">����</param>
        public void File(string zipPath, string directory, string password = null) {
            password = password.IsNullEmpty() ? "" : " -P" + password;
            string msg = Safe.RunWait(rarSetupPath, System.Diagnostics.ProcessWindowStyle.Hidden, " x -o+ -y \"{0}\" * \"{1}\"{2}".FormatWith(zipPath, directory, password));
        }
    }
}
