//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

namespace Pub.Class.Excel.OleDb {
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;
    using System.Text;
    /// <summary>
    /// OleDbдExcel11
    /// 
    /// �޸ļ�¼
    ///     2011.02.15 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class ExcelWriter : IExcelWriter {
        private string fileName = string.Empty;
        /// <summary>
        /// ��excel�ļ�
        /// </summary>
        /// <param name="excelPath">excel�ļ�·��</param>
        public void Open(string excelPath) {
            fileName = excelPath;
        }
        /// <summary>
        /// DataSet����EXCEL�ļ�
        /// </summary>
        /// <param name="ds">DataSet</param>
        public void ToExcel(DataSet ds) {
            ds.ToExcel(fileName);
        }
        /// <summary>
        /// DataTable����EXCEL�ļ�
        /// </summary>
        /// <param name="dt">DataTable</param>
        public void ToExcel(DataTable dt) {
            dt.ToExcel(fileName);
        }
        /// <summary>
        /// ɾ��������
        /// </summary>
        /// <param name="tableName">����</param>
        public void Delete(string tableName) {
            ExcelReader reader = new ExcelReader();
            reader.Open(fileName);
            DataSet ds = reader.ToDataSet();
            reader.Dispose();
            DataSet ds2 = new DataSet();
            ds.Tables.Do((p, i) => {
                string table = ((DataTable)p).TableName.Trim('$').ToLower();
                if (!tableName.Trim('$').ToLower().Equals(table)) {
                    ds2.Tables.Add((DataTable)p);
                }
            });
            ToExcel(ds2);
        }
        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose() {
            Safe.KillProcess("EXCEL");
        }
    }
}
