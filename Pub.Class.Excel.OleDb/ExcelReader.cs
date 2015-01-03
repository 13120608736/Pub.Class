//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

namespace Pub.Class.Excel.OleDb {
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;
    /// <summary>
    /// OleDb��Excel11
    /// 
    /// �޸ļ�¼
    ///     2011.02.15 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class ExcelReader : IExcelReader {
        private DataSet ds = new DataSet();
        /// <summary>
        /// ��excel�ļ�
        /// </summary>
        /// <param name="excelPath">excel�ļ�·��</param>
        public void Open(string excelPath) {
            string connStr = "provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelPath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";

            OleDbConnection conn = new OleDbConnection(connStr); 
            conn.Open();
            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            conn.Close(); conn.Dispose(); conn = null;

            Data.ResetDbProvider();
            Data.DBType = "OleDb";
            Data.ConnString = connStr;

            foreach (DataRow row in dt.Rows) {
                string name = row["TABLE_NAME"].ToString();
                dt = Data.GetDataTable("select * from [{0}]".FormatWith(name));
                name = name.Trim('\'').Trim('$');
                if (ds.Tables.IndexOf(name) == -1) {
                    ds.Tables.Add(dt);
                    dt.TableName = name;
                }
            }
        }
        /// <summary>
        /// excelתDataSet
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet ToDataSet() {
            return ds;
        }
        /// <summary>
        /// excelתDataTable
        /// </summary>
        /// <param name="table">DataTable����</param>
        /// <returns>DataTable</returns>
        public DataTable ToDataTable(string table) {
            int i = -1, index = 0;
            foreach (DataTable dt in ds.Tables) {
                if (dt.TableName.TrimEnd('$').ToLower().Equals(table.TrimEnd('$').ToLower())) { i = index; break; }
                index++;
            }
            return i == -1 ? null : ToDataTable(i);
        }
        /// <summary>
        /// excelתDataTable
        /// </summary>
        /// <param name="i">����</param>
        /// <returns>DataTable</returns>
        public DataTable ToDataTable(int i) {
            int count = ds.Tables.Count;
            return i < count ? ds.Tables[i] : null;
        }
        /// <summary>
        /// excelתDataTable ��0��
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable ToDataTable() {
            return ToDataTable(0);
        }
        ///// <summary>
        ///// ȡtable�������row,columnλ�õ�����
        ///// </summary>
        ///// <param name="table">��������</param>
        ///// <param name="row">��</param>
        ///// <param name="column">��</param>
        ///// <returns>ֵ</returns>
        //public object Cells(string table, int row, int column) {
        //    DataTable dt = ToDataTable(table);
        //    return dt.IsNull() ? null : dt.Rows[row][column];
        //}
        ///// <summary>
        ///// ȡ��i���������row,columnλ�õ�����
        ///// </summary>
        ///// <param name="i">��i��������</param>
        ///// <param name="row">��</param>
        ///// <param name="column">��</param>
        ///// <returns>ֵ</returns>
        //public object Cells(int i, int row, int column) {
        //    DataTable dt = ToDataTable(i);
        //    return dt.IsNull() ? null : dt.Rows[row][column];
        //}
        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose() {
            ds.Dispose();
        }
    }
}
