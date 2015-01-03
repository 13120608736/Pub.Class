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
    /// ʹ��Top Max ��ҳ  ֧��ACCESS MSSQL2000+ ֻ������������
    /// 
    /// �޸ļ�¼
    ///     2011.11.09 �汾��1.0 livexy ��������
    /// 
    /// <code>
    /// <example>
    /// PagerSql pagerSql = new MaxTopPagerSQL().GetSQL(2, 10, "UC_Member", "MemberID", "MemberID,RealName,Email");
    /// </example>
    /// </code>
    /// </summary>
    public class MaxTopPagerSQL : IPagerSQL {
        /// <summary>
        /// ��ҳSQL���÷��� MaxTop MinTop ֻ������������
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

            //˳��д����
            //SELECT TOP ҳ��С *
            //FROM table1
            //WHERE id >=(
            //SELECT ISNULL(MAX(id),0) 
            //FROM (
            //SELECT TOP ҳ��С*(ҳ��-1)+1 id FROM table1 ORDER BY id
            //) A)
            //ORDER BY id
            //����д����
            //SELECT TOP ҳ��С *
            //FROM table1
            //WHERE id <=(
            //SELECT ISNULL(MIN(id),0) 
            //FROM (
            //SELECT TOP ҳ��С*(ҳ��-1)+1 id FROM table1 ORDER BY id Desc
            //) A)
            //ORDER BY id Desc
            strSql.Clear();
            strSql.Append("select ");
            //if (distinct) strSql.Append("distinct ");
            strSql.AppendFormat("top {0} ", pageSize);
            strSql.AppendFormat("{0} ", fieldList);
            if (pageIndex == 1) {
                if (!tableName.IsNullEmpty()) strSql.AppendFormat("from {0} ", tableName);
                if (!where.IsNullEmpty()) strSql.AppendFormat("where {0} ", where);
                if (!groupBy.IsNullEmpty()) strSql.AppendFormat("group by {0} ", groupBy);
                if (!orderBy.IsNullEmpty()) strSql.AppendFormat("order by {0} ", orderBy);
            } else {
                if (!tableName.IsNullEmpty() && orderBy.EndsWith(" desc", true, null)) strSql.AppendFormat("from {0} where {1} < (select isnull(min({1}),0) from (select top {2} {1} from {3} ", tableName, pk, pageSize * (pageIndex - 1), tableName);
                else if (!tableName.IsNullEmpty()) strSql.AppendFormat("from {0} where {1} > (select isnull(max({1}),0) from (select top {2} {1} from {3} ", tableName, pk, pageSize * (pageIndex - 1), tableName);
                if (!where.IsNullEmpty()) strSql.AppendFormat("where {0} ", where);
                if (!groupBy.IsNullEmpty()) strSql.AppendFormat("group by {0} ", groupBy);
                if (!orderBy.IsNullEmpty()) strSql.AppendFormat("order by {0} ", orderBy);
                strSql.Append(") tempMaxTop ) ");
                if (!orderBy.IsNullEmpty()) strSql.AppendFormat("order by {0} ", orderBy);
            }
            sql.DataSql = strSql.ToString();

            return sql;
        }
    }
}
