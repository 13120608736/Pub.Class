//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace Pub.Class.IonicZip {
    /// <summary>
    /// ��ѹ���ļ�
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Decompress : IDecompress {
        /// <summary>
        /// ��ѹ���ļ�
        /// </summary>
        /// <param name="zipPath">Դ�ļ�</param>
        /// <param name="directory">Ŀ���ļ�</param>
        /// <param name="password">����</param>
        public void File(string zipPath, string directory, string password = null) {
            using (ZipFile zip = ZipFile.Read(zipPath)) {
                if (!password.IsNullEmpty()) zip.Password = password;
                foreach (ZipEntry entry in zip) entry.Extract(directory, ExtractExistingFileAction.OverwriteSilently);
            }
        }
    }
}
