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
    /// дExcel
    /// 
    /// �޸ļ�¼
    ///     2011.07.04 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface IExcelWriter: IAddIn {
        /// <summary>
        /// ��excel�ļ�
        /// </summary>
        /// <param name="excelPath">excel�ļ�·��</param>
        void Open(string excelPath);
        /// <summary>
        /// DataSet����EXCEL�ļ�
        /// </summary>
        /// <param name="ds">DataSet</param>
        void ToExcel(DataSet ds);
        /// <summary>
        /// DataTable����EXCEL�ļ�
        /// </summary>
        /// <param name="dt">DataTable</param>
        void ToExcel(DataTable dt);
        /// <summary>
        /// ɾ��������
        /// </summary>
        /// <param name="tableName">����</param>
        void Delete(string tableName);
        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        void Dispose();
    }

    /// <summary>
    /// дEXCEL
    /// 
    /// �޸ļ�¼
    ///     2011.07.04 �汾��1.0 livexy ��������
    /// 
    /// <example>
    /// <code>
    ///         ExcelWriter excelWriter = new ExcelWriter("Pub.Class.Excel.OleDb.dll", "Pub.Class.Excel.OleDb.ExcelWriter", "~/test2.xls".GetMapPath());
    ///         ExcelWriter excelWriter = new ExcelWriter("Pub.Class.Excel.COM.dll", "Pub.Class.Excel.COM.Excel11Writer", "~/test5.xls".GetMapPath());
    ///         excelWriter.ToExcel(ds);
    ///         excelWriter.Delete("�ж���");
    ///         excelWriter.ToExcel(ds.Tables[1]);
    ///         excelWriter.Dispose();
    /// </code>
    /// </example>
    /// </summary>
    public class ExcelWriter: Disposable {
        private readonly IExcelWriter excelWriter = null;
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        /// <param name="excelPath">excel�ļ�·��</param>
        public ExcelWriter(string dllFileName, string className, string excelPath) {
            if (excelWriter.IsNull()) {
                excelWriter = (IExcelWriter)dllFileName.LoadClass(className);
                excelWriter.Open(excelPath);
            }
        }
        /// <summary>
        /// ������ ָ��classNameDllName(ExcelWriterProviderName) Ĭ��Pub.Class.Excel.OleDb.ExcelWriter,Pub.Class.Excel.OleDb
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        /// <param name="excelPath">excel�ļ�·��</param>
        public ExcelWriter(string classNameAndAssembly, string excelPath) { 
            if (excelWriter.IsNull()) {
                excelWriter = (IExcelWriter)classNameAndAssembly.IfNullOrEmpty("Pub.Class.Excel.OleDb.ExcelWriter,Pub.Class.Excel.OleDb").LoadClass();
                excelWriter.Open(excelPath);
            }
        }
        /// <summary>
        /// ������ ��Web.config�ж�ExcelWriterProviderName Ĭ��Pub.Class.Excel.OleDb.ExcelWriter,Pub.Class.Excel.OleDb
        /// </summary>
        /// <param name="excelPath">excel�ļ�·��</param>
        public ExcelWriter(string excelPath) { 
            if (excelWriter.IsNull()) {
                excelWriter = (IExcelWriter)(WebConfig.GetApp("ExcelWriterProviderName") ?? "Pub.Class.Excel.OleDb.ExcelWriter,Pub.Class.Excel.OleDb").LoadClass();
                excelWriter.Open(excelPath);
            }
        }
        /// <summary>
        /// DataSet����EXCEL�ļ�
        /// </summary>
        /// <param name="ds">DataSet</param>
        public ExcelWriter ToExcel(DataSet ds) {
            excelWriter.ToExcel(ds);
            return this;
        }
        /// <summary>
        /// DataTable����EXCEL�ļ�
        /// </summary>
        /// <param name="dt">DataTable</param>
        public ExcelWriter ToExcel(DataTable dt) {
            excelWriter.ToExcel(dt);
            return this;
        }
        /// <summary>
        /// ɾ��������
        /// </summary>
        /// <param name="tableName">����</param>
        public ExcelWriter Delete(string tableName) {
            excelWriter.Delete(tableName);
            return this;
        }
        /// <summary>
        /// ��using �Զ��ͷ�
        /// </summary>
        protected override void InternalDispose() {
            excelWriter.Dispose();
            base.InternalDispose();
        }
    }
}
