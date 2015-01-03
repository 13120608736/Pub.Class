//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Text;
using org.in2bits.MyXls;

namespace Pub.Class.Excel.MyXls {
    /// <summary>
    /// COM���дExcel11
    /// 
    /// �޸ļ�¼
    ///     2012.03.19 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class ExcelWriter : IExcelWriter {
        private string fileName = string.Empty;
        private XlsDocument doc = new XlsDocument();
        private Cells cells;

        /// <summary>
        /// ��excel�ļ�
        /// </summary>
        /// <param name="excelPath">excel�ļ�·��</param>
        public void Open(string excelPath) {
            fileName = excelPath;
            doc.FileName = fileName.GetFileName();
        }
        /// <summary>
        /// DataSet����EXCEL�ļ�
        /// </summary>
        /// <param name="ds">DataSet</param>
        public void ToExcel(DataSet ds) {
            ds.Tables.Do((p, i) => toExcel((System.Data.DataTable)p, i + 1));
            Save();
        }
        /// <summary>
        /// DataTable����EXCEL�ļ�
        /// </summary>
        /// <param name="dt">DataTable</param>
        public void ToExcel(System.Data.DataTable dt) {
            toExcel(dt, 1);
            Save();
        }
        private void toExcel(System.Data.DataTable dt, int i = 1) {
            Worksheet sheet = doc.Workbook.Worksheets.AddNamed(dt.TableName.IfNullOrEmpty("Sheet" + i.ToString()).Trim("$"));
            cells = sheet.Cells;
            int rows = dt.Rows.Count, cols = dt.Columns.Count;
            for (int k = 1; k <= cols; k++) {
                cells.AddValueCell(1, k, dt.Columns[k - 1].Caption).Font.Bold = true;
            }
            for (int j = 2; j <= rows + 1 ; j++) {
                for (int k = 1; k <= cols; k++) {
                    cells.AddValueCell(j, k, dt.Rows[j - 2][k - 1]);
                }
            }
        }
        /// <summary>
        /// ɾ��������
        /// </summary>
        /// <param name="tableName">����</param>
        public void Delete(string tableName) {
            
        }
        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose() {
            System.GC.Collect();
        }
        /// <summary>
        /// д��i���������row,columnλ�õ�����
        /// </summary>
        /// <param name="i">��i��������</param>
        /// <param name="row">��</param>
        /// <param name="column">��</param>
        /// <returns>ֵ</returns>
        public void Cells(int row, int column, object value) {
            cells.AddValueCell(row, column, value);
        }
        /// <summary>
        /// �����޸�
        /// </summary>
        public void Save() {
            doc.Save(fileName.GetParentPath('\\'), true);
        }
        /// <summary>
        /// ��ָ���Ĺ�����
        /// </summary>
        /// <param name="workSheets">��N��������</param>
        public void OpenWorkSheets(int workSheets) {
            if (workSheets <= 0) return; //��������ű����1��ʼ
            cells = doc.Workbook.Worksheets[workSheets].Cells;
        }
    }
}
