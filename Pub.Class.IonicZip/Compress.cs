//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Ionic.Zip;
using Ionic.Zlib;

namespace Pub.Class.IonicZip {
    /// <summary>
    /// ѹ���ļ�
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Compress : ICompress {
        /// <summary>
        /// ѹ���ļ�
        /// </summary>
        /// <param name="source">Դ�ļ�</param>
        /// <param name="desc">Ŀ��ZIP�ļ�·��</param>
        /// <param name="password">����</param>
        public void File(string source, string descZip, string password = null) {
            FileInfo objFile = new FileInfo(source);
            if (!objFile.Exists) return;
            string filename = objFile.Name;

            using (ZipFile zip = new ZipFile()) {
                if (!password.IsNullEmpty()) zip.Password = password;
                zip.AddFile(source, "");
                zip.Save(descZip);
            }
        }
        /// <summary>
        /// ������ļ�ѹ����һ���ļ�
        /// </summary>
        /// <param name="source">����ļ�������ȫ·��������e:\tmp\tmp1\DD.cs</param>
        /// <param name="descZip">Ŀ��zip�ļ�·��</param>
        /// <param name="password">����</param>
        public void File(string[] source, string descZip, string password = null) {
            using (ZipFile zip = new ZipFile()) {
                if (!password.IsNullEmpty()) zip.Password = password;
                foreach(string file in source) zip.AddFile(file, "");
                zip.Save(descZip);
            }
        }
        /// <summary>
        /// ѹ��Ŀ¼
        /// </summary>
        /// <param name="source">Ҫѹ����Ŀ¼</param>
        /// <param name="descZip">ѹ������ļ���</param>
        /// <param name="password">����</param>
        public void Directory(string source, string descZip, string password = null) {
            source = source.Trim('\\') + "\\";
            using (ZipFile zip = new ZipFile()) {
                if (!password.IsNullEmpty()) zip.Password = password;
                zip.AddDirectory(source, "");
                zip.Save(descZip);
            }
        }
    }
}
