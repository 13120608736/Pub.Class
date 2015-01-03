//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2011 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
using System.Data;

namespace Pub.Class {
    /// <summary>
    /// ��ѹ���ļ�
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface IDecompress: IAddIn {
        /// <summary>
        /// ��ѹ���ļ�
        /// </summary>
        /// <param name="zipPath">ZIP�ļ�</param>
        /// <param name="directory">��ѹ��Ŀ¼</param>
        /// <param name="password">����</param>
        void File(string zipPath, string directory, string password = null);
    }

    /// <summary>
    /// ��ѹ���ļ�
    /// 
    /// �޸ļ�¼
    ///     2011.07.11 �汾��1.0 livexy ��������
    /// 
    /// <example>
    /// <code>
    ///         new Decompress("Pub.Class.SharpZip.dll", "Pub.Class.SharpZip.Decompress")
    ///             .File("~/web.config.zip".GetMapPath(), "~/back/".GetMapPath())
    ///             .Dispose();
    /// </code>
    /// </example>
    /// </summary>
    public class Decompress: Disposable {
        private readonly IDecompress decompress = null;
        /// <summary>
        /// ������ ָ��DLL�ļ���ȫ����
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        public Decompress(string dllFileName, string className) {
            if (decompress.IsNull()) {
                decompress = (IDecompress)dllFileName.LoadClass(className);
            }
        }
        /// <summary>
        /// ������ ָ��classNameDllName(DecompressProviderName) Ĭ��Pub.Class.SharpZip.Decompress,Pub.Class.SharpZip
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        public Decompress(string classNameAndAssembly) { 
            if (decompress.IsNull()) {
                decompress = (IDecompress)classNameAndAssembly.IfNullOrEmpty("Pub.Class.SharpZip.Decompress,Pub.Class.SharpZip").LoadClass();
            }
        }
        /// <summary>
        /// ������ ��Web.config�ж�DecompressProviderName Ĭ��Pub.Class.SharpZip.Decompress,Pub.Class.SharpZip
        /// </summary>
        public Decompress() { 
            if (decompress.IsNull()) {
                decompress = (IDecompress)(WebConfig.GetApp("DecompressProviderName") ?? "Pub.Class.SharpZip.Decompress,Pub.Class.SharpZip").LoadClass();
            }
        }
        /// <summary>
        /// ��ѹ���ļ�
        /// </summary>
        /// <param name="zipPath">ZIP�ļ�</param>
        /// <param name="directory">��ѹ��Ŀ¼</param>
        /// <param name="password">����</param>
        public Decompress File(string zipPath, string directory, string password = null) {
            decompress.File(zipPath, directory, password);
            return this;
        }
        /// <summary>
        /// ��ѹ���ļ�
        /// </summary>
        /// <param name="zipPath">ZIP�ļ�</param>
        /// <param name="directory">��ѹ��Ŀ¼</param>
        public Decompress File(string zipPath, string directory) {
            decompress.File(zipPath, directory, null);
            return this;
        }
        /// <summary>
        /// ��using �Զ��ͷ�
        /// </summary>
        protected override void InternalDispose() {
            base.InternalDispose();
        }
    }
}
