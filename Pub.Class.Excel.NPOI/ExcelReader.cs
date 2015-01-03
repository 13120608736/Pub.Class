//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using NPOI.HSSF.UserModel;
using System.IO;

namespace Pub.Class.Excel.NPOI {
    /// <summary>
    /// COM�����Excel11
    /// 
    /// �޸ļ�¼
    ///     2012.03.19 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class ExcelReader : IExcelReader {
        private DataSet ds = new DataSet();
        /// <summary>
        /// ��excel�ļ�
        /// </summary>
        /// <param name="excelPath">excel�ļ�·��</param>
        public void Open(string excelPath) {
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(excelPath, FileMode.Open, FileAccess.Read)) {
                hssfworkbook = new HSSFWorkbook(file);
            }

            for (int k = 0; k < hssfworkbook.NumberOfSheets; k++) {
                DataTable dt = new DataTable();
                HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(k);

                if (ds.Tables.IndexOf(sheet.SheetName) == -1) {
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                    HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    for (int j = 0; j < cellCount; j++) {
                        HSSFCell cell = (HSSFCell)headerRow.GetCell(j);
                        dt.Columns.Add(cell.ToString());
                    }

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) {
                        HSSFRow row = (HSSFRow)sheet.GetRow(i);
                        DataRow dataRow = dt.NewRow();

                        for (int j = row.FirstCellNum; j < cellCount; j++) {
                            if (row.GetCell(j) != null)
                                dataRow[j] = row.GetCell(j).ToString();
                        }

                        dt.Rows.Add(dataRow);
                    }
                    dt.TableName = sheet.SheetName;
                    ds.Tables.Add(dt);
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
        public System.Data.DataTable ToDataTable() {
            return ds.Tables[0];
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
        /// �ͷ���Դ
        /// </summary>
        public void Dispose() {
            ds.Dispose();
        }
    }
}
