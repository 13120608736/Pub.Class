//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using org.in2bits.MyXls;

namespace Pub.Class.Excel.MyXls {
    /// <summary>
    /// COM�����Excel11
    /// 
    /// �޸ļ�¼
    ///     2012.03.19 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class ExcelReader : IExcelReader {
        private DataSet ds = new DataSet();
        private XlsDocument doc = new XlsDocument();
        /// <summary>
        /// ��excel�ļ�
        /// </summary>
        /// <param name="excelPath">excel�ļ�·��</param>
        public void Open(string excelPath) {

        }
        /// <summary>
        /// excelתDataTable
        /// </summary>
        /// <param name="sheet">������</param>
        /// <param name="endRow">��</param>
        /// <param name="endCol">��</param>
        /// <param name="startRow">��ʼ��</param>
        /// <param name="startCol">��ʼ��</param>
        /// <returns>DataTable</returns>
        public System.Data.DataTable toDataTable(Worksheet sheet, int endRow, int endCol, int startRow = 1, int startCol = 1) {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.TableName = sheet.Name;
            //endRow = endRow + startRow;
            //Range excelRange = null;
            //int cols = 1;
            //for (int col = startCol; col <= endCol; col++) {
            //    excelRange = sheet.Cells[startRow, col] as Range;
            //    string val = excelRange.Text.ToString();
            //    if (string.IsNullOrEmpty(val)) break;
            //    dt.Columns.Add(val);
            //    cols++;
            //}

            //int nulls = 1, rownulls = 1;
            //for (int row = startRow + 1; row <= endRow; row++) {
            //    DataRow dataRow = dt.NewRow();
            //    nulls = 1;
            //    for (int col = startCol; col < cols; col++) {
            //        excelRange = sheet.Cells[row, col] as Range;
            //        string val = excelRange.Text.ToString();
            //        if (string.IsNullOrEmpty(val)) nulls++;
            //        dataRow[col - 1] = val;
            //    }
            //    dt.Rows.Add(dataRow);
            //    if (nulls == cols) rownulls++;
            //    if (rownulls > 10) break;
            //}
            //int count = dt.Rows.Count;
            //if (count > 10 && rownulls > 10) {
            //    int start = count - 10;
            //    for (int i = 0; i < 10; i++) dt.Rows[start].Delete();
            //}
            //if (excelRange != null) {
            //    Marshal.ReleaseComObject(excelRange);
            //    excelRange = null;
            //}
            return dt;
        }
        /// <summary>
        /// excelתDataTable
        /// </summary>
        /// <param name="sheet">������</param>
        /// <param name="endRow">��</param>
        /// <returns>DataTable</returns>
        public System.Data.DataTable toDataTable(Worksheet sheet, int endRow) {
            return toDataTable(sheet, endRow, 50);
        }
        /// <summary>
        /// excelתDataTable
        /// </summary>
        /// <param name="sheet">������</param>
        /// <returns>DataTable</returns>
        public System.Data.DataTable toDataTable(Worksheet sheet) {
            return toDataTable(sheet, 1000, 50);
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
        public System.Data.DataTable ToDataTable(string table) {
            int i = -1, index = 0;
            foreach (System.Data.DataTable dt in ds.Tables) {
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
        public System.Data.DataTable ToDataTable(int i) {
            int count = ds.Tables.Count;
            return i < count ? ds.Tables[i] : null;
        }
        /// <summary>
        /// excelתDataTable ��0��
        /// </summary>
        /// <returns>DataTable</returns>
        public System.Data.DataTable ToDataTable() {
            return ToDataTable(0);
        }
        /// <summary>
        /// ȡ��i���������row,columnλ�õ�����
        /// </summary>
        /// <param name="i">��i��������</param>
        /// <param name="row">��</param>
        /// <param name="column">��</param>
        /// <returns>ֵ</returns>
        public object Cells(string table, int row, int column) {
            System.Data.DataTable dt = ToDataTable(table);
            return dt.IsNull() ? null : dt.Rows[row][column];
        }
        /// <summary>
        /// ȡtable�������row,columnλ�õ�����
        /// </summary>
        /// <param name="table">��������</param>
        /// <param name="row">��</param>
        /// <param name="column">��</param>
        /// <returns>ֵ</returns>
        public object Cells(int i, int row, int column) {
            System.Data.DataTable dt = ToDataTable(i);
            return dt.IsNull() ? null : dt.Rows[row][column];
        }
        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose() {
            doc = null;
            ds.Dispose();
            if (!ds.IsNull()) ds = null;
        }
    }
}
