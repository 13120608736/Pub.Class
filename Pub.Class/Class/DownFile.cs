//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;

namespace Pub.Class {
    /// <summary>
    /// �����ļ���ʵ����
    /// 
    /// �޸ļ�¼
    ///     2012.03.01 �汾��1.0 livexy ��������
    ///     
    /// </summary>
    public class DownFileInfo {
        private long fileSize = 0;
        /// <summary>
        /// �ļ���С 
        /// </summary>
        public long FileSize { get { return fileSize; } set { fileSize = value; } }
        private string fileName = string.Empty;
        /// <summary>
        /// �ļ���
        /// </summary>
        public string FileName { get { return fileName; } set { fileName = value; } }
        private long downloadSize = 0;
        /// <summary>
        /// ��ǰ���ش�С(ʵʱ��) 
        /// </summary>
        public long DownloadSize { get { return downloadSize; } set { downloadSize = value; } }
        private bool isComplete = false;
        /// <summary>
        /// �Ƿ���� 
        /// </summary>
        public bool IsComplete { get { return isComplete; } set { isComplete = value; } }
        private string savePath = string.Empty;
        /// <summary>
        /// ����·��
        /// </summary>
        public string SavePath { get { return savePath; } set { savePath = value; } }
        private string fileUrl = string.Empty;
        /// <summary>
        /// �����ļ���ַ
        /// </summary>
        public string FileUrl { get { return fileUrl; } set { fileUrl = value; } }
        private string extName = string.Empty;            //�ļ���չ�� 
        /// <summary>
        /// ����·��
        /// </summary>
        public string ExtName { get { return extName; } set { extName = value; } }
        private int threadNum = 2;
        /// <summary>
        /// �߳�����
        /// </summary>
        public int ThreadNum { get { return threadNum; } set { threadNum = value; } }
    }

    /// <summary>
    /// ���������ļ� hcxiong
    /// 
    /// �޸ļ�¼
    ///     2012.03.01 �汾��1.0 livexy ��������
    /// 
    /// <code>
    /// <example>
    /// new DownFile("http://fetion.chinacache.net/alading/Fetion2011December.exe", Application.StartupPath)
    ///     .FileName("test.exe")
    ///     .Action(p => {
    ///         textBox1.Text = "�ļ���С��{0}/{1}".FormatWith(p.DownloadSize.FormatBytes(), p.FileSize.FormatBytes());
    ///     })
    ///     .Done(p => {
    ///         textBox1.AppendText("�������:" + p.FileName);
    ///     })
    ///     .Error(ex => {
    ///         textBox1.Text = "����ʧ��:" + ex.ToExceptionDetail();
    ///     })
    ///     .Start();
    /// </example>
    /// </code>
    /// </summary>
    public class DownFile {
        private DownFileInfo info = new DownFileInfo();
        private int threadCompleteNum; //�߳�������� 
        private Thread[] thread = null; //�߳����� 
        private List<string> tempFiles = new List<string>();
        private List<List<long>> readFT = new List<List<long>>(); //���ÿ���̶߳�ȡ����ʼ�ͽ���λ�� 
        private readonly object locker = new object();
        private Action<DownFileInfo> action = null;
        private Action<DownFileInfo> done = null;
        private Action<Exception> error = null;

        /// <summary> 
        /// ���캯�� 
        /// </summary> 
        /// <param name="threahNum">�߳�����</param> 
        /// <param name="fileUrl">�ļ�Url·��</param> 
        /// <param name="savePath">���ر���·��</param> 
        public DownFile(string fileUrl, string savePath, int threahNum) {
            info.ThreadNum = threahNum;
            info.FileUrl = fileUrl;
            info.SavePath = savePath;
            info.FileName = System.IO.Path.GetFileName(fileUrl);
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="fileUrl">�ļ�Url·��</param>
        /// <param name="savePath">���ر���·��</param>
        public DownFile(string fileUrl, string savePath) {
            info.FileUrl = fileUrl;
            info.SavePath = savePath;
            info.FileName = System.IO.Path.GetFileName(fileUrl);
        }
        /// <summary>
        /// ���ع�����ִ��
        /// </summary>
        /// <param name="action">����</param>
        /// <returns></returns>
        public DownFile Action(Action<DownFileInfo> action) {
            this.action = action;
            return this;
        }
        /// <summary>
        /// ������ɺ�ִ��
        /// </summary>
        /// <param name="done">����</param>
        /// <returns></returns>
        public DownFile Done(Action<DownFileInfo> done) {
            this.done = done;
            return this;
        }
        /// <summary>
        /// ���س����ִ��
        /// </summary>
        /// <param name="error">����</param>
        /// <returns></returns>
        public DownFile Error(Action<Exception> error) {
            this.error = error;
            return this;
        }
        /// <summary>
        /// �޸��ļ���
        /// </summary>
        /// <param name="fileName">���ļ���</param>
        /// <returns></returns>
        public DownFile FileName(string fileName) {
            info.FileName = fileName;
            return this;
        }
        /// <summary>
        /// ����
        /// </summary>
        public void Start() {
            if (null == thread) {
                new Thread(() => {
                    int singelNum = 0;
                    int remainder = 0;
                    try {
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(info.FileUrl);
                        request.MaximumAutomaticRedirections = 4;
#if !MONO40
                        request.MaximumResponseHeadersLength = 4;
#endif
                        request.Method = "HEAD";
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        info.ExtName = System.IO.Path.GetExtension(response.ResponseUri.ToString());
                        info.FileSize = response.ContentLength;
                        singelNum = (int)(info.FileSize / info.ThreadNum);      //ƽ������ 
                        remainder = (int)(info.FileSize % info.ThreadNum);      //��ȡʣ��� 
                        request.Abort();
                        response.Close();
                    } catch (Exception ex) {
                        if (error != null) error.BeginInvoke(ex, null, null);
                        return;
                    }

                    thread = new Thread[info.ThreadNum];
                    for (int i = 0; i < info.ThreadNum; i++) {
                        List<long> range = new List<long>();
                        range.Add(i * singelNum);
                        if (remainder != 0 && (info.ThreadNum - 1) == i)    //ʣ��Ľ������һ���߳� 
                            range.Add(i * singelNum + singelNum + remainder - 1);
                        else
                            range.Add(i * singelNum + singelNum - 1);
                        readFT.Add(range);
                        thread[i] = new Thread(new ThreadStart(Download));
                        thread[i].Name = i.ToString();
                        thread[i].IsBackground = true;
                        thread[i].Start();
                    }
                }).Start();
            } else {
                foreach (var info in thread) { if (info.ThreadState == ThreadState.SuspendRequested || info.ThreadState == ThreadState.Suspended) info.Resume(); }
            }
        }
        /// <summary>
        /// �߳�����
        /// </summary>
        private void Download() {
            Stream httpFileStream = null, localFileStram = null;
            HttpWebResponse httpResponse = null;
            try {
                string tmpFileBlock = String.Format(@"{0}\{1}{2}.dat", info.SavePath, info.FileName, Thread.CurrentThread.Name);
                bool isExist = System.IO.File.Exists(tmpFileBlock);
                tempFiles.Add(tmpFileBlock);
                HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(info.FileUrl);
                long form = readFT[Convert.ToInt32(Thread.CurrentThread.Name)][0];
                long to = readFT[Convert.ToInt32(Thread.CurrentThread.Name)][1];
                if (isExist) {
                    long size = new FileInfo(tmpFileBlock).Length;
                    lock (locker) info.DownloadSize += size;
                    form += size;
                }
                if (form > to) form = to;
                httpRequest.AddRange((int)form, (int)to);
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                httpFileStream = httpResponse.GetResponseStream();

                localFileStram = new FileStream(tmpFileBlock, isExist ? FileMode.Append : FileMode.Create);
                byte[] by = new byte[1024];
                int getByteSize = httpFileStream.Read(by, 0, 1024); //Read���������ض���by�����е����ֽ��� 
                while (getByteSize > 0) {
                    lock (locker) info.DownloadSize += getByteSize;
                    localFileStram.Write(by, 0, getByteSize);
                    getByteSize = httpFileStream.Read(by, 0, 1024);
                    if (action != null) action.BeginInvoke(info, null, null);
                }
                Interlocked.Increment(ref threadCompleteNum);
            } catch (Exception ex) {
                if (error != null) error.BeginInvoke(ex, null, null);
            } finally {
                if (httpFileStream != null) httpFileStream.Close();
                if (localFileStram != null) localFileStram.Close();
                if (httpResponse != null) httpResponse.Close();
            }
            if (threadCompleteNum == info.ThreadNum) {
                Complete();
                info.IsComplete = true;
                if (done != null) done.BeginInvoke(info, null, null);
            }
        }
        /// <summary> 
        /// ������ɺ�ϲ��ļ��� 
        /// </summary> 
        private void Complete() {
            Stream mergeFile = null;
            BinaryWriter AddWriter = null;
            try {
                mergeFile = new FileStream(String.Format(@"{0}\{1}", info.SavePath, info.FileName), FileMode.Create);
                AddWriter = new BinaryWriter(mergeFile);
                foreach (string file in tempFiles) {
                    using (FileStream fs = new FileStream(file, FileMode.Open)) {
                        BinaryReader TempReader = new BinaryReader(fs);
                        AddWriter.Write(TempReader.ReadBytes((int)fs.Length));
                        TempReader.Close();
                    }
                    File.Delete(file);
                }
            } catch (Exception ex) {
                if (error != null) error.BeginInvoke(ex, null, null);
            } finally {
                AddWriter.Close();
                mergeFile.Close();
            }
        }
        /// <summary>
        /// ��ִֹ��
        /// </summary>
        public void Stop() {
            if (thread == null) return;
            foreach (var info in thread) { if (info.ThreadState == ThreadState.Running) info.Abort(); }
            thread = null;
        }
        /// <summary>
        /// ��ִͣ��
        /// </summary>
        public void Pause() {
            if (thread == null) return;
            foreach (var info in thread) { if (info.ThreadState == ThreadState.Running) info.Suspend(); }
        }
    }
}
