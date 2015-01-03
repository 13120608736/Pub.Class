//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Web;
using System.Text;

namespace Pub.Class {
    /// <summary>
    /// �ϴ��ļ���
    /// 
    /// �޸ļ�¼
    ///     2006.05.13 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Upload {
        //#region ������� enum
        /// <summary>
        /// �������
        /// </summary>
        public enum RandFileType {
            /// <summary>
            /// �����
            /// </summary>
            None = 0,
            /// <summary>
            /// ���������
            /// </summary>
            FileName_DateTime,
            /// <summary>
            /// ���������
            /// </summary>
            FileName_RandNumber,
            /// <summary>
            /// �������
            /// </summary>
            DateTime
        }
        //#endregion
        //#region ˽�г�Ա
        private int _State = 0;
        private int _fileSize = 0;//KB
        private string _fileName = "";
        private string _allowedExtensions = ".gif|.png|.jpg|.bmp";//".xls";
        private string _filePath = "~/";
        private double _maxFileSize = 4000;
        private bool _isCreateFolderForNotExits = false;
        private int _RandNumbers = 5;
        private RandFileType _RandFileType = RandFileType.None;
        //#endregion
        //#region ����
        /// <summary>
        /// �����ϴ�����չ��
        /// </summary>
        public string AllowedExtensions { set { _allowedExtensions = value; } }
        /// <summary>
        /// �ļ����浽��
        /// </summary>
        public string FilePath { set { _filePath = value; } }
        /// <summary>
        /// �����ϴ��ļ���С
        /// </summary>
        public int MaxFileSize { set { _maxFileSize = value; } }
        /// <summary>
        /// �����
        /// </summary>
        public int RandNumbers { set { _RandNumbers = value; } }
        /// <summary>
        /// �Ƿ��Զ��½������ڵ��ļ�Ŀ¼
        /// </summary>
        public bool isCreateFolderForNotExits { set { _isCreateFolderForNotExits = value; } }
        /// <summary>
        /// �ļ��������
        /// </summary>
        public RandFileType RndFileType { set { _RandFileType = value; } }
        /// <summary>
        /// �����ļ���С
        /// </summary>
        public int FileSize { get { return _fileSize; } }
        /// <summary>
        /// �����ļ���
        /// </summary>
        public string FileName { get { return _fileName; } }
        /// <summary>
        /// ���ز���״̬
        /// </summary>
        public int State { get { return _State; } }
        //#endregion
        //#region ������
        /// <summary>
        /// ������
        /// </summary>
        public Upload() { }
        //#endregion
        //#region �ϴ��ļ�
        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        /// <param name="fileUpload">fileUpload���</param>
        public void DoUpLoad(System.Web.UI.WebControls.FileUpload fileUpload) {
            string __filePath = HttpContext.Current.Server.MapPath(_filePath);
            if (!System.IO.Directory.Exists(__filePath)) {
                if (_isCreateFolderForNotExits) {
                    FileDirectory.DirectoryVirtualCreate(_filePath);
                } else { _State = 3; return; }
            }
            if (fileUpload.PostedFile.ContentLength / 1024 > _maxFileSize) { _State = 4; return; }
            string randStr = "";
            switch (_RandFileType) {
                case RandFileType.None: randStr = ""; break;
                case RandFileType.FileName_DateTime: randStr = "_" + Rand.RndDateStr(); break;
                case RandFileType.FileName_RandNumber: randStr = "_" + Rand.RndCode(_RandNumbers); break;
                case RandFileType.DateTime: randStr = Rand.RndDateStr(); break;
            }
            bool isTrue = false;
            if (fileUpload.HasFile) {
                string _fileExt = System.IO.Path.GetExtension(fileUpload.FileName).ToLower();
                string[] _allowedExt = _allowedExtensions.Split(new string[] { "|" }, StringSplitOptions.None);
                for (int i = 0; i < _allowedExt.Length; i++) {
                    if (_fileExt == _allowedExt[i]) { isTrue = true; }
                }
                if (isTrue) {
                    try {
                        string fNameNoExt = System.IO.Path.GetFileNameWithoutExtension(fileUpload.FileName);
                        if (_RandFileType == RandFileType.DateTime) fNameNoExt = "";
                        fileUpload.SaveAs(__filePath + fNameNoExt + randStr + System.IO.Path.GetExtension(fileUpload.FileName));
                        _State = 0;
                        _fileSize = fileUpload.PostedFile.ContentLength / 1024;
                        _fileName = _filePath + fNameNoExt + randStr + System.IO.Path.GetExtension(fileUpload.FileName);
                    } catch { _State = 2; }
                } else { _State = 1; }
            } else { _State = 2; }
        }
        //#endregion
    }
}
