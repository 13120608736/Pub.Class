//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Pub.Class {
    /// <summary>
    /// Singleton ���͵�ʵ����
    /// 
    /// �޸ļ�¼
    ///     2008.06.12 �汾��1.0 livexy ��������
    /// 
    /// <example>
    /// <code>
    /// public class UC_Member : Singleton&lt;UC_Member> { }
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Singleton<T> where T : new() {
        private static T instance = new T();
        private static readonly object lockHelper = new object();
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        public static T Instance() {
            if (instance.IsNull()) {
                lock (lockHelper) {
                    if (instance.IsNull()) instance = new T();
                }
            }

            return instance;
        }
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="value"></param>
        public void Instance(T value) {
            instance = value;
        }
    }

    /// <summary>
    /// SingletonEx ������͵�ʵ����
    /// 
    /// �޸ļ�¼
    ///     2008.06.12 �汾��1.0 livexy ��������
    /// 
    /// <example>
    /// <code>
    /// SingletonEx&lt;ICompress>.Instance("Pub.Class.SharpZip.dll", "Pub.Class.SharpZip.Compress").File("~/web.config".GetMapPath(), "~/web.config.zip".GetMapPath());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">����ӿ�</typeparam>
    public sealed class SingletonAddIn<T> where T : IAddIn {
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <example>
        /// <code>
        /// SingletonAddIn&lt;ICompress>.Instance("Pub.Class.SharpZip.dll", "Pub.Class.SharpZip.Compress")
        ///     .File("~/web.config".GetMapPath(), "~/web.config.zip".GetMapPath());
        /// </code>
        /// </example>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">����</param>
        public static T Instance(string dllFileName, string className) {
            return (T)dllFileName.LoadClass(className);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <example>
        /// <code>
        /// SingletonAddIn&lt;ICompress>.InstanceDynamic("Pub.Class.SharpZip.dll", "Pub.Class.SharpZip.Compress")
        ///     .File("~/web.config".GetMapPath(), "~/web.config.zip".GetMapPath());
        /// </code>
        /// </example>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">����</param>
        public static T InstanceDynamic(string dllFileName, string className) {
            return (T)dllFileName.LoadDynamicClass(className);
        }
        /// <summary>
        /// ��ȡʵ�� ���ܱȽϺ�
        /// </summary>
        /// <example>
        /// <code>
        /// SingletonAddIn&lt;ICompress>.Instance("Pub.Class.SharpZip.Compress,Pub.Class.SharpZip")
        ///     .File("~/web.config".GetMapPath(), "~/web.config.zip".GetMapPath());
        /// </code>
        /// </example>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        public static T Instance(string classNameAndAssembly) {
            return (T)classNameAndAssembly.LoadClass();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <returns></returns>
        public static T Instance<T>() where T : IAddIn, new() {
            return Singleton<T>.Instance();
        }
    }
}