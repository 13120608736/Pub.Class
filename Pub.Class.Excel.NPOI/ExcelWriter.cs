//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;

namespace Pub.Class.Excel.NPOI {
    /// <summary>
    /// COM���дExcel11
    /// 
    /// �޸ļ�¼
    ///     2012.03.19 �汾��1.0 livexy ��������
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
            MemoryStream ms = RenderDataSetToExcel(ds) as MemoryStream;
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            data = null;
            ms = null;
            fs = null;
        }
        private Stream RenderDataSetToExcel(DataSet ds) {
            MemoryStream ms = new MemoryStream();
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet;
            HSSFRow headerRow;

            //for (int k = ds.Tables.Count - 1, len = 0; len <= k; k--) {
            //    DataTable dt = ds.Tables[k];
            foreach(DataTable dt in ds.Tables) {
                sheet = (HSSFSheet)workbook.CreateSheet(dt.TableName);
                headerRow = (HSSFRow)sheet.CreateRow(0);

                foreach (DataColumn column in dt.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

                int rowIndex = 1;
                foreach (DataRow row in dt.Rows) {
                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in dt.Columns) {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
        }
        /// <summary>
        /// DataTable����EXCEL�ļ�
        /// </summary>
        /// <param name="dt">DataTable</param>
        public void ToExcel(System.Data.DataTable dt) {
            MemoryStream ms = RenderDataTableToExcel(dt) as MemoryStream;
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            data = null;
            ms = null;
            fs = null;
        }
        private Stream RenderDataTableToExcel(DataTable dt) {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(dt.TableName);
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);

            foreach (DataColumn column in dt.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            int rowIndex = 1;
            foreach (DataRow row in dt.Rows) {
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                foreach (DataColumn column in dt.Columns) {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;
            return ms;
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
            //doc = null;
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
            //cells.Add(row, column, value);
        }
        /// <summary>
        /// �����޸�
        /// </summary>
        public void Save() {
            //doc.Save(fileName, true);
        }
        /// <summary>
        /// ��ָ���Ĺ�����
        /// </summary>
        /// <param name="workSheets">��N��������</param>
        public void OpenWorkSheets(int workSheets) {
            //if (workSheets <= 0) return; //��������ű����1��ʼ
            //cells = doc.Workbook.Worksheets[workSheets].Cells;
        }
    }
}
