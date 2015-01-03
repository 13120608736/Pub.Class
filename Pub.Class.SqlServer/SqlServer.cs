//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Pub.Class {
    /// <summary>
    /// SqlServer���ݿ�ʵ��
    /// 
    /// �޸ļ�¼
    ///     2009.11.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class SqlServer : IDbProvider {
        /// <summary>
        /// SqlClientʵ��
        /// </summary>
        /// <returns>SqlClientʵ��</returns>
        public DbProviderFactory Instance() { return SqlClientFactory.Instance; }
        /// <summary>
        /// ���ظղ����¼������IDֵ
        /// </summary>
        /// <returns>SQL</returns>
        public string GetLastIDSQL() { return "SELECT SCOPE_IDENTITY()"; }
        /// <summary>
        /// �Ƿ�֧��ȫ������
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsFullTextSearchEnabled() { return true; }
        /// <summary>
        /// �Ƿ�֧��ѹ�����ݿ�
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsCompactDatabase() { return true; }
        /// <summary>
        /// �Ƿ�֧�ֱ������ݿ�
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsBackupDatabase() { return true; }
        /// <summary>
        /// �Ƿ�֧�����ݿ��Ż�
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsDbOptimize() { return false; }
        /// <summary>
        /// �Ƿ�֧�����ݿ�����
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsShrinkData() { return true; }
        /// <summary>
        /// �Ƿ�֧�ִ洢����
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsStoreProc() { return true; }
        /// <summary>
        /// ����SQL������Ϣ�����
        /// </summary>
        /// <param name="cmd"></param>
        public void DeriveParameters(IDbCommand cmd) {
            if ((cmd as SqlCommand) != null) SqlCommandBuilder.DeriveParameters(cmd as SqlCommand);
        }
        /// <summary>
        /// ����SQL����
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        public DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size) {
            if (Size > 0) return new SqlParameter(ParamName, (SqlDbType)DbType, Size);
            return new SqlParameter(ParamName, (SqlDbType)DbType);
        }
        public DbParameter MakeParam(string ParamName, object value) {
            return new SqlParameter(ParamName, value);
        }
        /// <summary>
        /// ��ʼ�ַ�
        /// </summary>
        public string GetIdentifierStart() { return "["; }
        /// <summary>
        /// �����ַ�
        /// </summary>
        public string GetIdentifierEnd() { return "]"; }
        /// <summary>
        /// ����ǰ������
        /// </summary>
        public string GetParamIdentifier() { return "@"; }
        /// <summary>
        /// SqlServer�����ݸ���
        /// </summary>
        /// <param name="dt">����Դ dt.TableNameһ��Ҫ�����ݿ������Ӧ</param>
        /// <param name="dbkey">���ݿ�</param>
        /// <param name="options">ѡ�� Ĭ��Default</param>
        /// <param name="isTran">�Ƿ�ʹ������ Ĭ��false</param>
        /// <param name="timeout">��ʱʱ��7200 2Сʱ</param>
        /// <param name="batchSize">ÿһ�����е�����</param>
        /// <param name="error">������</param>
        /// <returns>true/false</returns>
        public bool DataBulkCopy(DataTable dt, string dbkey = "", BulkCopyOptions options = BulkCopyOptions.Default, bool isTran = false, int timeout = 7200, int batchSize = 10000, Action<Exception> error = null) {
            if (Data.Pool(dbkey).DBType != "SqlServer") return false;
            SqlTransaction tran = null;
            using(SqlConnection conn = new SqlConnection(Data.Pool(dbkey).ConnString)) {
                conn.Open();
                if (isTran) tran = conn.BeginTransaction();
                using (System.Data.SqlClient.SqlBulkCopy bc = new System.Data.SqlClient.SqlBulkCopy(conn, ((int)options).ToEnum<SqlBulkCopyOptions>(), tran)) {
                    bc.BulkCopyTimeout = timeout;
                    bc.BatchSize = batchSize;
                    bc.DestinationTableName = dt.TableName;
                    try {
                        bc.WriteToServer(dt);
                        if (isTran) tran.Commit();
                    } catch(Exception ex) {
                        if (isTran) tran.Rollback();
                        if (error.IsNotNull()) error(ex);
                        return false;
                    }
                }
                conn.Close();
            }
            return true;
        }
        /// <summary>
        /// �����ݸ���
        /// </summary>
        /// <param name="conn">����Դ</param>
        /// <param name="tran">����</param>
        /// <param name="dt">����Դ dt.TableNameһ��Ҫ�����ݿ������Ӧ</param>
        /// <param name="options">ѡ�� Ĭ��Default</param>
        /// <param name="timeout">��ʱʱ��7200 2Сʱ</param>
        /// <param name="batchSize">ÿһ�����е�����</param>
        /// <param name="error">������</param>
        /// <returns></returns>
        public bool DataBulkCopy(IDbConnection conn, IDbTransaction tran, DataTable dt, BulkCopyOptions options = BulkCopyOptions.Default, int timeout = 7200, int batchSize = 10000, Action<Exception> error = null) {
            using (System.Data.SqlClient.SqlBulkCopy bc = new System.Data.SqlClient.SqlBulkCopy((SqlConnection)conn, ((int)options).ToEnum<SqlBulkCopyOptions>(), (SqlTransaction)tran)) {
                bc.BulkCopyTimeout = timeout;
                bc.BatchSize = batchSize;
                bc.DestinationTableName = dt.TableName;
                try {
                    bc.WriteToServer(dt);
                } catch(Exception ex) {
                    if (error.IsNotNull()) error(ex);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// SqlServer�����ݸ���
        /// </summary>
        /// <param name="dr">����Դ</param>
        /// <param name="tableName">��Ӧ�ı���</param>
        /// <param name="dbkey">���ݿ�</param>
        /// <param name="options">ѡ�� Ĭ��Default</param>
        /// <param name="isTran">�Ƿ�ʹ������ Ĭ��false</param>
        /// <param name="timeout">��ʱʱ��7200 2Сʱ</param>
        /// <param name="batchSize">ÿһ�����е�����</param>
        /// <param name="error">������</param>
        /// <returns>true/false</returns>
        public bool DataBulkCopy(IDataReader dr, string tableName, string dbkey = "", BulkCopyOptions options = BulkCopyOptions.Default, bool isTran = false, int timeout = 7200, int batchSize = 10000, Action<Exception> error = null) {
            if (Data.Pool(dbkey).DBType != "SqlServer") return false;
            SqlTransaction tran = null;
            using(SqlConnection conn = new SqlConnection(Data.Pool(dbkey).ConnString)) {
                if (isTran) tran = conn.BeginTransaction();
                using (System.Data.SqlClient.SqlBulkCopy bc = new System.Data.SqlClient.SqlBulkCopy(conn, ((int)options).ToEnum<SqlBulkCopyOptions>(), tran)) {
                    bc.BulkCopyTimeout = timeout;
                    bc.BatchSize = batchSize;
                    bc.DestinationTableName = tableName;
                    try {
                        bc.WriteToServer(dr);
                        if (isTran) tran.Commit();
                    } catch(Exception ex) {
                        if (isTran) tran.Rollback();
                        if (error.IsNotNull()) error(ex);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
