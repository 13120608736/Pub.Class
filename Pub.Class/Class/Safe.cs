//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace Pub.Class {
    /// <summary>
    /// ��ȫ������
    /// 
    /// �޸ļ�¼
    ///     2006.05.10 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Safe {
        //#region IsSafeUrl
        /// <summary>
        /// �������ڱ����ύ����
        /// </summary>
        /// <remarks>�����Ƿ��ǰ�ȫURL</remarks>
        /// <param name="doMain">����</param>
        public static bool IsSafeUrl(string doMain) {
            string url = Request2.GetReferrer().ToLower().Trim().Replace("http://", "").Replace("https://", "").Split('/')[0];
            doMain = doMain.ToLower().Trim();
            if (url.IndexOf(doMain) > -1) return true;
            return false;
        }
        //#endregion
        //#region Kill/Run/RestartIISProcess
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="processName">������</param>
        /// <returns></returns>
        public static int IsExistProcess(string processName) {
            int i = 0;
            Process[] Processes = Process.GetProcessesByName(processName);
            foreach (Process CurrentProcess in Processes) i++;
            return i;
        }
        /// <summary>
        /// ���ݽ�������ȡPID
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static int GetPIDByProcessName(string processName) {
            Process[] arrayProcess = Process.GetProcessesByName(processName);

            foreach (Process p in arrayProcess) {
                return p.Id;
            }
            return 0;
        }
#if !MONO40
        /// <summary>
        /// ��ȡ����Ľ��̱�ʶID
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static int GetPIDByProcessWindowTitle(string windowTitle) {
            int rs = 0;
            Process[] arrayProcess = Process.GetProcesses();
            foreach (Process p in arrayProcess) {
                if (p.MainWindowTitle.IndexOf(windowTitle) != -1) {
                    rs = p.Id;
                    break;
                }
            }

            return rs;
        }
        /// <summary>
        /// ���ݴ��������Ҵ��ھ����֧��ģ��ƥ�䣩
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static IntPtr FindWindowByProcessName(string title) {
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps) {
                if (p.MainWindowTitle.IndexOf(title) != -1) {
                    return p.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="processName">������</param>
        /// <returns></returns>
        public static IList<IntPtr> ProcessHandle(string processName) {
            IList<IntPtr> list = new List<IntPtr>();
            Process[] Processes = Process.GetProcessesByName(processName);
            foreach (Process CurrentProcess in Processes) { list.Add(CurrentProcess.MainWindowHandle); }
            return list;
        }
        /// <summary>
        /// ����һ�����̲��ȴ���ִ�����
        /// </summary>
        /// <param name="cmd">����</param>
        /// <param name="arguments">����</param>
        /// <param name="isOutput">�Ƿ񷵻��������</param>
        /// <param name="commands">ִ�ж�������</param>
        /// <returns>������̳����ش�����־</returns>
        public static IntPtr RunWaitHandle(string cmd, string arguments = "", string[] commands = null) {
            cmd = "\"" + cmd + "\"";
            using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
                p.StartInfo.FileName = cmd;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                try {
                    p.Start();
                    if (commands.IsNotNull()) {
                        foreach (string command in commands) p.StandardInput.WriteLine(command);
                    }
                    p.WaitForExit();
                    p.Close();
                    return p.MainWindowHandle;
                } catch {
                    return IntPtr.Zero;
                }
            }
        }
        /// <summary>
        /// ����һ�����̲��ȴ���ִ����� ��ָ��������ʽ
        /// </summary>
        /// <param name="cmd">����</param>
        /// <param name="arguments">����</param>
        /// <param name="winStyle">����״̬ ��� ��С�� ����</param>
        /// <returns>������̳����ش�����־</returns>
        public static IntPtr RunWaitHandle(string cmd, System.Diagnostics.ProcessWindowStyle winStyle, string arguments = "") {
            cmd = "\"" + cmd + "\"";
            using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
                p.StartInfo.FileName = cmd;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = winStyle;
                p.StartInfo.UseShellExecute = true;
                try {
                    p.Start();
                    p.WaitForExit();
                    p.Close();
                    return p.MainWindowHandle;
                } catch {
                    return IntPtr.Zero;
                }
            }
        }
#endif
        /// <summary>
        /// ɱ������
        /// </summary>
        /// <param name="processName">������</param>
        public static void KillProcess(string processName) {
            if (string.IsNullOrEmpty(processName)) throw new ArgumentNullException("ProcessName");
            KillProcessAsync(processName, 0);
        }
        /// <summary>
        /// ɱ������
        /// </summary>
        /// <param name="processName">������</param>
        /// <param name="TimeToKill">��ʱ</param>
        public static void KillProcess(string processName, int TimeToKill) {
            if (string.IsNullOrEmpty(processName))
                throw new ArgumentNullException("ProcessName");
            ThreadPool.QueueUserWorkItem(delegate { KillProcessAsync(processName, TimeToKill); });
        }
        /// <summary>
        /// ɱ������
        /// </summary>
        /// <param name="processName">������</param>
        /// <param name="TimeToKill">��ʱ</param>
        public static void KillProcessAsync(string processName, int TimeToKill) {
            if (TimeToKill > 0) Thread.Sleep(TimeToKill);
            Process[] Processes = Process.GetProcessesByName(processName);
            foreach (Process CurrentProcess in Processes) {
                CurrentProcess.Kill();
            }
        }
        /// <summary>
        /// ����һ�����̲��ȴ���ִ�����
        /// </summary>
        /// <param name="cmd">����</param>
        /// <param name="arguments">����</param>
        /// <param name="isOutput">�Ƿ񷵻��������</param>
        /// <param name="commands">ִ�ж�������</param>
        /// <returns>������̳����ش�����־</returns>
        public static string RunWait(string cmd, string arguments = "", bool isOutput = true, string[] commands = null) {
            cmd = "\"" + cmd + "\"";
            using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
                p.StartInfo.FileName = cmd;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = isOutput;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                try {
                    p.Start();
                    if (commands.IsNotNull()) {
                        foreach (string command in commands) p.StandardInput.WriteLine(command);
                    }
                    StringBuilder error = new StringBuilder();
                    error.AppendLine(p.StandardError.ReadToEnd().Trim());
                    if (isOutput) error.Append(" ").AppendLine(p.StandardOutput.ReadToEnd().Trim());
                    p.WaitForExit();
                    p.Close();
                    return error.ToString().Trim();
                } catch (Exception ex) {
                    return ex.ToExceptionDetail();
                }
            }
        }
        public static void RunWait(string cmd, string arguments, string[] commands, Action<string> msg = null) {
            cmd = "\"" + cmd + "\"";
            using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
                p.StartInfo.FileName = cmd;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                try {
                    p.Start();
                    if (commands.IsNotNull()) {
                        foreach (string command in commands) p.StandardInput.WriteLine(command);
                    }
                    while (p.WaitForExit(0) == false) {
                        if (msg != null) msg.BeginInvoke(p.StandardOutput.ReadLine(), null, null);
                    }
                    p.WaitForExit();
                    p.Close();
                } catch (Exception ex) {
                    if (msg != null) msg.BeginInvoke(ex.Message, null, null);
                }
            }
        }
        public static void RunWait(string cmd, string arguments, Action<string> msg) {
            RunWait(cmd, arguments, null, msg);
        }
        public static void RunWait(string cmd, string[] commands, Action<string> msg) {
            RunWait(cmd, "", commands, msg);
        }
        public static void RunWait(string cmd, Action<string> msg) {
            RunWait(cmd, "", null, msg);
        }
        /// <summary>
        /// ����һ�����̲��ȴ���ִ����� ��ָ��������ʽ
        /// </summary>
        /// <param name="cmd">����</param>
        /// <param name="arguments">����</param>
        /// <param name="winStyle">����״̬ ��� ��С�� ����</param>
        /// <returns>������̳����ش�����־</returns>
        public static string RunWait(string cmd, System.Diagnostics.ProcessWindowStyle winStyle, string arguments = "") {
            cmd = "\"" + cmd + "\"";
            using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
                p.StartInfo.FileName = cmd;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = winStyle;
                p.StartInfo.UseShellExecute = true;
                try {
                    p.Start();
                    p.WaitForExit();
                    p.Close();
                    return string.Empty;
                } catch (Exception ex) {
                    return ex.ToExceptionDetail();
                }
            }
        }
        /// <summary>
        /// �첽ִ�н���
        /// </summary>
        /// <param name="cmd">����</param>
        /// <param name="arguments">����</param>
        /// <returns>������̳����ش�����־</returns>
        public static string RunAsync(string cmd, string arguments = "") {
            cmd = "\"" + cmd + "\"";
            using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
                p.StartInfo.FileName = cmd;
                if (!arguments.IsNullEmpty()) p.StartInfo.Arguments = arguments;
                p.StartInfo.UseShellExecute = false;
                try {
                    p.Start();
                    p.Close();
                    return string.Empty;
                } catch (Exception ex) {
                    return ex.ToExceptionDetail();
                }
            }
        }
        /// <summary>
        /// �첽ִ�н���
        /// </summary>
        /// <param name="cmd">����</param>
        /// <param name="arguments">����</param>
        /// <param name="winStyle">����״̬ ��� ��С�� ����</param>
        /// <returns>������̳����ش�����־</returns>
        public static string RunAsync(string cmd, System.Diagnostics.ProcessWindowStyle winStyle, string arguments = "") {
            cmd = "\"" + cmd + "\"";
            using (System.Diagnostics.Process p = new System.Diagnostics.Process()) {
                p.StartInfo.FileName = cmd;
                if (!arguments.IsNullEmpty()) p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = winStyle;
                p.StartInfo.UseShellExecute = true;
                try {
                    p.Start();
                    p.Close();
                    return string.Empty;
                } catch (Exception ex) {
                    return ex.ToExceptionDetail();
                }
            }
        }
        /// <summary>
        /// hack tip:ͨ������web.config�ļ���ʽ������IIS���̳أ�ע��iis��web԰���������1,��Ϊ�����������û��ſɵ��ø÷�����
        /// </summary>
        public static void RestartIISProcess() {
            try {
                System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
                xmldoc.Load("~/web.config".GetMapPath());
                System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter("~/web.config".GetMapPath(), null);
                writer.Formatting = System.Xml.Formatting.Indented;
                xmldoc.WriteTo(writer);
                writer.Flush();
                writer.Close();
            } catch { ; }
        }
        //#endregion
        //#region COOKIES��ˢ��ҳ�����
        /// <summary>
        /// ���ô�ҳ���ʱ��
        /// </summary>
        public static void SetDateTime() {
            Cookie2.Set("__sysTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        /// <summary>
        /// �ж��Ƿ���ָ�����������ύ���ݣ����ﵽ�ж��Ƿ�ˢ��ҳ���Ŀ��
        /// </summary>
        /// <param name="seconds">��������</param>
        /// <returns>��/��</returns>
        public static bool IsRefresh(int seconds) {
            string _sysTime = Cookie2.Get("__sysTime");
            if (_sysTime.Trim() == "") return true;
            if (!_sysTime.IsDateTime()) return true;
            DateTime _startTime = DateTime.Parse(_sysTime);
            DateTime _endTime = DateTime.Now;
            TimeSpan _value = _startTime.GetTimeSpan(_endTime);
            if (_value.Seconds >= seconds) return false;
            else {
                Js.Alert("������ˢ�£�������ύ���ݣ���" + seconds.ToString() + "����ύ���ݡ�");
                return true;
            }
        }
        //#endregion
        //#region ��ȫ�ύ/��̬����
        /// <summary>
        /// ��ȫ�ύ���������ύ
        /// </summary>
        /// <param name="doMain">����</param>
        public static void SafeGetPost(string doMain) {
            if (string.IsNullOrEmpty(doMain)) return;
            bool isTrue = false;
            string[] doMainArr = doMain.Split('|');
            for (int i = 0; i <= doMainArr.Length - 1; i++) if (Safe.IsSafeUrl(doMainArr[i])) isTrue = true;
            if (!isTrue) { Msg.Write("�������ڱ����ύ���ݡ�"); Msg.End(); }
        }
        /// <summary>
        /// ����DLL�ķ��� ��֧�����ط��� �ٶ���
        /// </summary>
        /// <example>
        /// <code>
        /// Safe.DllInvoke("../bin/Pub.Class.dll".GetMapPath(), "Pub.Class", "Session2", "Set", new object[] { "test", "3" });
        /// Msg.Write(Safe.DllInvoke("../bin/Pub.Class.dll".GetMapPath(), "Pub.Class", "Session2", "Get", new object[] { "test" }));
        /// </code>
        /// </example>
        /// <param name="DllFileName">dllȫ·��</param>
        /// <param name="NameSpace">�����ռ�</param>
        /// <param name="ClassName">����</param>
        /// <param name="MethodName">������</param>
        /// <param name="ObjArrayParams">����</param>
        /// <returns>����ֵ</returns>
        public static object DllInvoke(string DllFileName, string NameSpace, string ClassName, string MethodName, object[] ObjArrayParams) {
            Assembly DllAssembly = Assembly.LoadFrom(DllFileName);
            Type[] DllTypes = DllAssembly.GetTypes();
            foreach (Type DllType in DllTypes) {
                if (DllType.Namespace == NameSpace && DllType.Name == ClassName) {
                    MethodInfo MyMethod = DllType.GetMethod(MethodName);
                    if (MyMethod.IsNotNull()) {
                        object mObject = Activator.CreateInstance(DllType);
                        return MyMethod.Invoke(mObject, ObjArrayParams);
                    }
                }
            }
            return (object)0;
        }
        //#endregion
        //#region ������Ϣ
        /// <summary>
        /// ��ʾ��ϸ�ĳ�����Ϣ
        /// </summary>
        /// <param name="ex">Exception ex</param>
        /// <returns></returns>
        public static string Expand(Exception ex) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("*******************************************************************************************************");
            if (!HttpContext.Current.IsNull()) {
                sb.AppendLine(string.Format("* DateTime :   {0}	IP��{1}	MemberID��{2}	OS��{3}	Brower��{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Request2.GetIP(), "", Request2.GetOS(), Request2.GetBrowser()));
                sb.AppendLine("* Url      :   " + Request2.GetUrl());
                sb.AppendLine("* Request  :   " + Request2.GetRequest());
            }
            _expandException(ex, 1, sb);
            sb.AppendLine("*******************************************************************************************************");
            sb.AppendLine("");
            return sb.ToString();
        }
        /// <summary>
        /// ��ʾ��ϸ�ĳ�����Ϣ
        /// </summary>
        /// <param name="ex">Exception ex</param>
        /// <param name="offSet"></param>
        /// <param name="sb"></param>
        private static void _expandException(Exception ex, int offSet, StringBuilder sb) {
            if (ex.IsNull()) return;
            Type t = ex.GetType();
            string paddingString = "";
            if (offSet > 1) paddingString = new String(' ', offSet * 4);
            sb.AppendFormat("{0}Exception:   {1}{2}", paddingString, t.Name, Environment.NewLine);
            sb.AppendFormat("{0}Message:     {1}{2}", paddingString, ex.Message, Environment.NewLine);
            sb.AppendFormat("{0}Source:      {1}{2}", paddingString, ex.Source, Environment.NewLine);
            if (ex.StackTrace.IsNotNull()) sb.AppendFormat("{0}Stack Trace: {1}{2}", paddingString, ex.StackTrace.Trim(), Environment.NewLine);
            if (ex.TargetSite.IsNotNull()) sb.AppendFormat("{0}Method:      {1}{2}", paddingString, ex.TargetSite.Name, Environment.NewLine);
            sb.AppendFormat("{0}Native:      {1}{2}", paddingString, ex.ToString(), Environment.NewLine);
            sb.AppendFormat("{0}Data:        {1}{2}", paddingString, expandData(ex.Data, offSet), Environment.NewLine);

            //Exception baseException = ex.GetBaseException();
            //if (baseException.IsNotNull()) sb.AppendFormat("{0}Base:        {1}{2}", paddingString, ex.GetBaseException(), Environment.NewLine);

            _expandException(ex.InnerException, offSet + 1, sb);
        }
        /// <summary>
        /// ��ʾ��ϸ�ĳ�����Ϣ
        /// </summary>
        /// <param name="iDictionary">IDictionary</param>
        /// <param name="offSet">offSet</param>
        /// <returns></returns>
        private static string expandData(System.Collections.IDictionary iDictionary, int offSet) {
            StringBuilder sb = new StringBuilder();
            offSet += 4;
            string paddingString = "";
            if (offSet > 1) paddingString = new string(' ', offSet);

            sb.AppendFormat("{0}Total Data Entries: {1}{2}", paddingString, iDictionary.Count, Environment.NewLine);
            int counter = 1;
            paddingString = new string(' ', paddingString.Length + 4);
            foreach (DictionaryEntry de in iDictionary) {
                sb.AppendFormat("{0}{1}:[{2} {3}]  ", paddingString, counter++, de.Key.GetType().FullName, de.Key.ToString());
                if (de.Value.IsNotNull()) sb.AppendFormat("{0}", de.Value.ToString()); else sb.AppendFormat("{0}", "(null)");
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
        //#endregion
        /// <summary>
        /// string ת �ṹ�� char[]
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static void ToStructChars(ref char[] chars, string str, int len) {
            chars = new char[len];
            Array.Copy(str.PadRight(len, '\0').ToCharArray(), chars, len);
        }
        /// <summary>
        /// char[] ת �ṹ�� char[]
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static void ToStructChars(ref char[] chars, char[] str, int len) {
            chars = new char[len];
            Array.Copy(new string(str).PadRight(len, '\0').ToCharArray(), chars, len);
        }

    }
}
