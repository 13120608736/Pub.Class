//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Pub.Class {
    /// <summary>
    /// Singleton2 ���Ͷ�����ʵ������ʱʵ����
    /// 
    /// �޸ļ�¼
    ///     2008.06.12 �汾��1.0 livexy ��������
    /// 
    /// <example>
    /// <code>
    /// public class UC_Member : Singleton2&lt;UC_Member> { }
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton2<T> where T : new() {
        private Singleton2() { }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        public static T Instance() {
            return Singleton2Creator.instance;
        }
        class Singleton2Creator {
            internal static readonly T instance = new T();
        }
    }
}