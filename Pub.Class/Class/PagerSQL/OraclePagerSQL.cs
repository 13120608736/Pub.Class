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
using System.Text;

namespace Pub.Class {
    /// <summary>
    /// Oracle��ҳ  ֧��oracle
    /// 
    /// �޸ļ�¼
    ///     2011.11.09 �汾��1.0 livexy ��������
    /// 
    /// <code>
    /// <example>
    /// PagerSql pagerSql = new OraclePagerSQL().GetSQL(2, 10, "UC_Member", "MemberID", "MemberID,RealName,Email");
    /// </example>
    /// </code>
    /// </summary>
    public class OraclePagerSQL : IPagerSQL {
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
        public PagerSql GetSQL(int pageIndex, int pageSize, string tableName, string pk = "*", string fieldList = "*", string where = "", string groupBy = "", string orderBy = "") {
            PagerSql sql = new PagerSql();
            StringBuilder strSql = new StringBuilder();

            strSql.Append("select ");
            strSql.AppendFormat("count({0}) as total ", pk);
            if (!tableName.IsNullEmpty()) strSql.AppendFormat("from {0} ", tableName);
            if (!where.IsNullEmpty()) strSql.AppendFormat("where {0} ", where);
            if (!groupBy.IsNullEmpty()) strSql.AppendFormat("group by {0} ", groupBy);
            sql.CountSql = strSql.ToString();
            //SELECT * FROM (
            //SELECT MY_TABLE.*,ROWNUM AS MY_ROWNUM FROM (
            // ������дʵ�ʵ���Ҫ��ѯ��SQL���**/
            //) AS MY_TABLE WHERE ROWNUM <=200/**������һҳ�е����һ����¼**/
            //) WHERE MY_ROWNUM>=10 /**������һҳ�еĵ�һ����¼**/

            //select * from t_xiaoxi where rowid in(select rid from (select rownum rn,rid from(select rowid rid,cid from t_xiaoxi  order by cid desc) where rownum<10000) where rn>9980) order by cid desc;
            //select * from (select t.*,row_number() over(order by cid desc) rk from t_xiaoxi t) where rk<10000 and rk>9980;
            //select * from(select t.*,rownum rn from(select * from t_xiaoxi order by cid desc) t where rownum<10000) where rn>9980;
            strSql.Clear();
            strSql.Append("select ");
            strSql.AppendFormat("{0} ", fieldList);

            if (!tableName.IsNullEmpty()) strSql.AppendFormat("from {0} (select {1},rownum as my_rownum from (", tableName, fieldList);
            strSql.Append("select ");
            //if (distinct) strSql.Append("distinct ");
            strSql.AppendFormat("{0} ", fieldList);
            if (!tableName.IsNullEmpty()) strSql.AppendFormat("from {0} ", tableName);
            if (!where.IsNullEmpty()) strSql.AppendFormat("where {0} ", where);
            if (!groupBy.IsNullEmpty()) strSql.AppendFormat("group by {0} ", groupBy);
            if (!orderBy.IsNullEmpty()) strSql.AppendFormat("order by {0} ", orderBy);
            strSql.AppendFormat(") as my_table where rownum<{0}) where my_rownum>{1}", pageSize * (pageIndex - 1) + pageSize, pageSize * (pageIndex - 1));
            if (!orderBy.IsNullEmpty()) strSql.AppendFormat("order by {0} ", orderBy);
            sql.DataSql = strSql.ToString();

            return sql;
        }
    }
}
