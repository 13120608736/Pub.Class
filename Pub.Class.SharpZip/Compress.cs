//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using System.Collections;
using System.Collections.Generic;

namespace Pub.Class.SharpZip {
    /// <summary>
    /// ѹ���ļ�
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Compress: ICompress {
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

            FileStream fs = System.IO.File.OpenRead(source);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            FileStream ZipFile = System.IO.File.Create(descZip);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            if (!password.IsNullEmpty()) ZipStream.Password = password;
            ZipEntry ZipEntry = new ZipEntry(filename);
            ZipStream.PutNextEntry(ZipEntry);
            ZipStream.SetLevel(6);
            ZipStream.Write(buffer, 0, buffer.Length);
            ZipStream.Finish();
            ZipStream.Close();
        }
        /// <summary>
        /// ������ļ�ѹ����һ���ļ�
        /// </summary>
        /// <param name="source">����ļ�������ȫ·��������e:\tmp\tmp1\DD.cs</param>
        /// <param name="descZip">Ŀ��ZIP�ļ�·��</param>
        /// <param name="password">����</param>
        public void File(string[] source, string descZip, string password = null) {
            ZipOutputStream outStream = new ZipOutputStream(System.IO.File.Create(descZip));
            if (!password.IsNullEmpty()) outStream.Password = password;
            Crc32 crc = new Crc32();
            foreach (string info in source) {
                File(info, crc, outStream);
            }
            outStream.Finish();
            outStream.Close();
        }
        /// <summary>
        /// ���ܣ�ѹ��һ���ļ�,������·����Ϣ
        /// </summary>
        /// <param name="strSrcFile">��ѹ���ļ�</param>
        /// <param name="crc">CRCУ��</param>
        /// <param name="outStream"></param>
        private void File(string source, Crc32 crc, ZipOutputStream outStream) {
            string strFileName = Path.GetFileName(source); // �ļ���������·����Ϣ
            #region ��ȡ�ļ���Ϣ
            FileStream fs = System.IO.File.OpenRead(source);
            long iLength = fs.Length;// �ļ�����
            byte[] buffer = new byte[iLength];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            #endregion
            ZipEntry entry = new ZipEntry(strFileName);
            entry.CompressionMethod = CompressionMethod.Deflated; // deflate
            entry.DateTime = DateTime.Now;
            entry.Size = iLength;
            #region CRCУ��
            crc.Reset();
            crc.Update(buffer);
            entry.Crc = crc.Value;
            #endregion
            outStream.PutNextEntry(entry);
            outStream.Write(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// ѹ��Ŀ¼
        /// </summary>
        /// <param name="source">Ҫѹ����Ŀ¼</param>
        /// <param name="descZip">ѹ������ļ���</param>
        /// <param name="password">����</param>
        public void Directory(string source, string descZip, string password = null) {
            source = source.Trim('\\') + "\\";
            IList<string> filenames = new List<string>();
            FileDirectory.FileList(source, ref filenames, source);
            Crc32 crc = new Crc32();
            ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(descZip));
            if (!password.IsNullEmpty()) s.Password = password;
            s.SetLevel(6);
            foreach (string file in filenames) {
                FileStream fs = System.IO.File.OpenRead(source + file);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                ZipEntry entry = new ZipEntry(file);
                entry.DateTime = DateTime.Now;
                entry.Size = fs.Length;
                fs.Close();
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                s.PutNextEntry(entry);
                s.Write(buffer, 0, buffer.Length);
            }
            s.Finish();
            s.Close();
        }
    }
}
