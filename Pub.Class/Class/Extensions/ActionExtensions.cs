//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
#if NET20
using Pub.Class.Linq;
#else
using System.Linq;
using System.Web.Script.Serialization;
#endif
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Pub.Class {
    /// <summary>
    /// ������չ
    /// 
    /// �޸ļ�¼
    ///     2009.06.25 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class ActionExtensions {
#if !MONO40
        /// <summary>
        /// �޲�������ִ��ʱ�� ������ϸ��Ϣ
        /// </summary>
        /// <example>
        /// <code>
        /// Msg.WriteEnd(ActionExtensions.Time(() => { Msg.Write(1); }, "����", 1000));
        /// </code>
        /// </example>
        /// <param name="action">����</param>
        /// <param name="name">��������</param>
        /// <param name="iteration">ִ�д���</param>
        /// <returns>����ִ��ʱ�����(ms)</returns>
        public static string Time(this Action action, string name = "", int iteration = 1) {
            if (name.IsNullEmpty()) {
                var watch = Stopwatch.StartNew();
                long cycleCount = WinApi.GetCycleCount();
                for (int i = 0; i < iteration; i++) action();
                long cpuCycles = WinApi.GetCycleCount() - cycleCount;
                watch.Stop();
                return watch.Elapsed.ToString();
            } else {
                StringBuilder sb = new StringBuilder();

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                int[] gcCounts = new int[GC.MaxGeneration + 1];
                for (int i = 0; i <= GC.MaxGeneration; i++) gcCounts[i] = GC.CollectionCount(i);

                var watch = Stopwatch.StartNew();
                long cycleCount = WinApi.GetCycleCount();
                for (int i = 0; i < iteration; i++) action();
                long cpuCycles = WinApi.GetCycleCount() - cycleCount;
                watch.Stop();

                sb.AppendFormat("{0} ѭ��{1}�β��Խ����<br />", name, iteration);
                sb.AppendFormat("ʹ��ʱ�䣺{0}<br />", watch.Elapsed.ToString());
                sb.AppendFormat("CPU���ڣ�{0}<br />", cpuCycles.ToString("N0"));

                for (int i = 0; i <= GC.MaxGeneration; i++) sb.AppendFormat("Gen����{0}��{1}<br />", i, GC.CollectionCount(i) - gcCounts[i]);
                sb.Append("<br />");
                return sb.ToString();
            }
        }
#endif
        /// <summary>
        /// ���Է���
        /// </summary>
        /// <param name="action">����</param>
        /// <param name="numRetries">���Դ���</param>
        /// <param name="retryTimeout">��ʱ�೤ʱ������ԣ���λ����</param>
        /// <param name="throwIfFail">�����������Բ�������Ȼ�����쳣ʱ�Ƿ��쳣�׳�</param>
        /// <param name="onFailureAction">����ʧ��ִ�еķ���</param>
        /// <returns></returns>
        public static void Retry(this Action action, int numRetries, int retryTimeout, bool throwIfFail, Action<Exception> onFailureAction) {
            if (action.IsNull()) throw new ArgumentNullException("action");
            numRetries--;
            do {
                bool istrue = false;
                try {
                    action();
                    istrue = true;
                } catch (Exception ex) {
                    istrue = false;
                    if (onFailureAction.IsNotNull()) onFailureAction(ex);
                    if (numRetries <= 0 && throwIfFail) throw ex;
                }
                if (retryTimeout > 0 && !istrue) Thread.Sleep(retryTimeout);
            } while (numRetries-- > 0);
        }
    }
}
