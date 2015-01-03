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
    /// ��Excel
    /// 
    /// �޸ļ�¼
    ///     2011.07.04 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface IExcelReader: IAddIn {
        /// <summary>
        /// ��excel�ļ�
        /// </summary>
        /// <param name="excelPath">excel�ļ�·��</param>
        void Open(string excelPath);
        /// <summary>
        /// excelתDataSet
        /// </summary>
        /// <returns>DataSet</returns>
        DataSet ToDataSet();
        /// <summary>
        /// excelתDataTable
        /// </summary>
        /// <param name="table">DataTable����</param>
        /// <returns>DataTable</returns>
        DataTable ToDataTable(string table);
        /// <summary>
        /// excelתDataTable
        /// </summary>
        /// <param name="i">����</param>
        /// <returns>DataTable</returns>
        DataTable ToDataTable(int i);
        /// <summary>
        /// excelתDataTable ��0��
        /// </summary>
        /// <returns>DataTable</returns>
        DataTable ToDataTable();
        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        void Dispose();
    }

    /// <summary>
    /// ��EXCEL
    /// 
    /// �޸ļ�¼
    ///     2011.07.04 �汾��1.0 livexy ��������
    /// 
    /// <example>
    /// <code>
    ///         ExcelReader excelReader = new ExcelReader("Pub.Class.Excel.OleDb.dll", "Pub.Class.Excel.OleDb.ExcelReader", "~/test.xls".GetMapPath());
    ///         ExcelReader excelReader = new ExcelReader("Pub.Class.Excel.COM.dll", "Pub.Class.Excel.COM.Excel11Reader", "~/test.xls".GetMapPath());
    ///         DataSet ds = excelReader.ToDataSet();
    ///         foreach (DataTable dt in ds.Tables) { 
    ///             Msg.Write(dt.TableName + "��" + dt.ToJson() + "<br /><br />");
    ///         }
    ///         Msg.Write(excelReader.ToDataTable("��ѡ��").ToJson());
    ///         excelReader.Dispose();
    /// </code>
    /// </example>
    /// </summary>
    public class ExcelReader: Disposable {
        private readonly IExcelReader excelReader = null;
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="dllFileName">dll�ļ���</param>
        /// <param name="className">�����ռ�.����</param>
        /// <param name="excelPath">excel�ļ�·��</param>
        public ExcelReader(string dllFileName, string className, string excelPath) {
            if (excelReader.IsNull()) {
                excelReader = (IExcelReader)dllFileName.LoadClass(className);
                excelReader.Open(excelPath);
            }
        }
        /// <summary>
        /// ������ ָ��classNameDllName(ExcelReaderProviderName) Ĭ��Pub.Class.Excel.OleDb.ExcelReader,Pub.Class.Excel.OleDb
        /// </summary>
        /// <param name="classNameAndAssembly">�����ռ�.����,��������</param>
        /// <param name="excelPath">excel�ļ�·��</param>
        public ExcelReader(string classNameAndAssembly, string excelPath) { 
            if (excelReader.IsNull()) {
                excelReader = (IExcelReader)classNameAndAssembly.IfNullOrEmpty("Pub.Class.Excel.OleDb.ExcelReader,Pub.Class.Excel.OleDb").LoadClass();
                excelReader.Open(excelPath);
            }
        }
        /// <summary>
        /// ������ ��Web.config�ж�ExcelReaderProviderName Ĭ��Pub.Class.Excel.OleDb.ExcelReader,Pub.Class.Excel.OleDb
        /// </summary>
        /// <param name="excelPath">excel�ļ�·��</param>
        public ExcelReader(string excelPath) { 
            if (excelReader.IsNull()) {
                excelReader = (IExcelReader)(WebConfig.GetApp("ExcelReaderProviderName") ?? "Pub.Class.Excel.OleDb.ExcelReader,Pub.Class.Excel.OleDb").LoadClass();
                excelReader.Open(excelPath);
            }
        }
        /// <summary>
        /// excelתDataSet
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet ToDataSet() {
            return excelReader.ToDataSet();
        }
        /// <summary>
        /// excelתDataTable
        /// </summary>
        /// <param name="table">DataTable����</param>
        /// <returns>DataTable</returns>
        public DataTable ToDataTable(string table) {
            return excelReader.ToDataTable(table);
        }
        /// <summary>
        /// excelתDataTable
        /// </summary>
        /// <param name="i">����</param>
        /// <returns>DataTable</returns>
        public DataTable ToDataTable(int i) {
            return excelReader.ToDataTable(i);
        }
        /// <summary>
        /// excelתDataTable ��0��
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ToDataTable() {
            return excelReader.ToDataTable(0);
        }
        /// <summary>
        /// ��using �Զ��ͷ�
        /// </summary>
        protected override void InternalDispose() {
            excelReader.Dispose();
            base.InternalDispose();
        }
    }
}
