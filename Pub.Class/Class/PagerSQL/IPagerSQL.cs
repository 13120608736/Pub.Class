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
    /// ��ҳSQL�ӿ�
    /// 
    /// �޸ļ�¼
    ///     2011.11.09 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface IPagerSQL {
        /// <summary>
        /// ��ҳSQL���÷���
        /// </summary>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">ÿҳ��ʾ����</param>
        /// <param name="tableName">������</param>
        /// <param name="pk">����</param>
        /// <param name="fieldList">�ֶ��б�</param>
        /// <param name="where">where���� and or ��ʼ</param>
        /// <param name="groupBy">��������</param>
        /// <param name="orderBy">��������</param>
        /// <returns>��ҳSQL</returns>
        PagerSql GetSQL(int pageIndex, int pageSize, string tableName, string pk = "*", string fieldList = "*", string where = "", string groupBy = "", string orderBy = "");
    }

    /// <summary>
    /// ��ҳSQLʵ����
    /// </summary>
    [Serializable]
    [EntityInfo("��ҳSQLʵ����")]
    public class PagerSql {
        /// <summary>
        /// ͳ�Ƽ�¼��SQL
        /// </summary>
        [EntityInfo("ͳ�Ƽ�¼��SQL")]
        public string CountSql { set; get; }
        /// <summary>
        /// ȡ����SQL
        /// </summary>
        [EntityInfo("ȡ����SQL")]
        public string DataSql { set; get; }
		/// <summary>
		/// SQL����ת��ʵ������
		/// </summary>
		/// <returns>ʵ������</returns>
		/// <param name="totalRecords">�ܼ�¼��</param>
		/// <param name="dbkey">Dbkey.</param>
		/// <typeparam name="T">ʵ����</typeparam>
		public IList<T> ToList<T>(out long totalRecords, string dbkey = "") where T : class, new() {
			IList<T> list = new List<T>(); totalRecords = 0;
			IDataReader dr = Data.Pool(dbkey).GetDbDataReader(DataSql + ";" + CountSql);
			if (dr.IsNull()) return list;
			list = dr.ToList<T>(false);
			bool result = dr.NextResult();
			if (result) { dr.Read(); totalRecords = dr[0].ToString().ToBigInt(); }
			dr.Close (); dr.Dispose(); dr = null;
			return list;
		}
    }
}
