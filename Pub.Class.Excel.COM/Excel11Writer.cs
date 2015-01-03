//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace Pub.Class.Excel.COM {
    /// <summary>
    /// COM���дExcel11
    /// 
    /// �޸ļ�¼
    ///     2011.02.15 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Excel11Writer : IExcelWriter {
        private string fileName = string.Empty;
        private Application xlsApp = new Application();
        private Worksheet xlsSheet = null;
        private Workbook xlsBook = null;
        private Range range = null;
        /// <summary>
        /// ��excel�ļ�
        /// </summary>
        /// <param name="excelPath">excel�ļ�·��</param>
        public void Open(string excelPath) {
            fileName = excelPath;
            xlsApp = new Application();
            if (FileDirectory.FileExists(fileName)) {
                xlsBook = xlsApp.Workbooks.Open(fileName, Type.Missing, false, 5, Type.Missing, Type.Missing, false, Type.Missing, Type.Missing, true, false, 0, true, false, false);
            }
        }
        /// <summary>
        /// DataSet����EXCEL�ļ�
        /// </summary>
        /// <param name="ds">DataSet</param>
        public void ToExcel(DataSet ds) {
            if (xlsBook.IsNotNull()) xlsBook.Close();
            xlsBook = xlsApp.Workbooks.Add(true);
            for (int k = ds.Tables.Count - 1, len = 0; len <= k; k--) toExcel(ds.Tables[k], k);
            (xlsBook.Worksheets.get_Item(ds.Tables.Count + 1) as Worksheet).Delete();
            FileDirectory.FileDelete(fileName);
            xlsBook.SaveAs(fileName, 56, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }
        /// <summary>
        /// DataTable����EXCEL�ļ�
        /// </summary>
        /// <param name="dt">DataTable</param>
        public void ToExcel(System.Data.DataTable dt) {
            if (xlsBook.IsNotNull()) xlsBook.Close();
            xlsBook = xlsApp.Workbooks.Add(true);
            toExcel(dt, 1);
            (xlsBook.Worksheets.get_Item(2) as Worksheet).Delete();
            FileDirectory.FileDelete(fileName);
            xlsBook.SaveAs(fileName, 56, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }
        private void toExcel(System.Data.DataTable dt, int i = 1) {
            xlsSheet = (Worksheet)xlsBook.Worksheets.Add();
            int rowIndex = 1, columnIndex = 0, colstart = 0;
            string tableName = dt.TableName.IsNullEmpty() ? ("Sheet" + i.ToString()) : dt.TableName.Trim('$');
            xlsSheet.Name = tableName;
            foreach (DataColumn col in dt.Columns) {
                columnIndex++;
                Range cel = (Range)xlsSheet.Cells[rowIndex, columnIndex];
                cel.Font.Bold = true;
                xlsSheet.Cells[rowIndex, columnIndex] = col.ColumnName;
            }
            foreach (DataRow row in dt.Rows) {
                rowIndex++;
                foreach (DataColumn col in dt.Columns) {
                    colstart++;
                    Range cel = (Range)xlsSheet.Cells[rowIndex, colstart];
                    xlsSheet.Cells[rowIndex, colstart] = row[col.ColumnName].ToString();
                }
                colstart = 0;
            }
        }
        /// <summary>
        /// ɾ��������
        /// </summary>
        /// <param name="tableName">����</param>
        public void Delete(string tableName) {
            int i = 1, index = 0;
            foreach (Worksheet ws in xlsBook.Worksheets) {
                if (ws.Name.ToLower() == tableName.ToLower()) index = i;
                i++;
            }
            if (index > 0) {
                (xlsBook.Worksheets[index] as Worksheet).Visible = XlSheetVisibility.xlSheetHidden;
                (xlsBook.Worksheets[index] as Worksheet).Delete();
                xlsBook.Save();
                //xlsBook.Application.Save(fileName);
            }
        }
        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose() {
            if (range != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
            range = null;

            if (xlsSheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsSheet);
            xlsSheet = null;

            if (xlsBook != null) {
                xlsBook.Close(false, Type.Missing, Type.Missing); System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsBook);
            }
            xlsBook = null;

            if (xlsApp != null) {
                xlsApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsApp);
            }
            xlsApp = null;
            Safe.KillProcess("EXCEL");
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
            xlsSheet.Cells[row, column] = value;
        }
        /// <summary>
        /// �����޸�
        /// </summary>
        public void Save() {
            xlsBook.Save();
        }
        /// <summary>
        /// ��ָ���Ĺ�����
        /// </summary>
        /// <param name="workSheets">��N��������</param>
        public void OpenWorkSheets(int workSheets) {
            if (workSheets <= 0) return; //��������ű����1��ʼ
            xlsSheet = (Worksheet)xlsBook.Worksheets[workSheets];
        }
    }
}
