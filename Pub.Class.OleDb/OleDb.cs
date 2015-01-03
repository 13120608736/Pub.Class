//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

namespace Pub.Class {
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;
    using Pub.Class;

    /// <summary>
    /// OleDb���ݿ�ʵ��
    /// 
    /// �޸ļ�¼
    ///     2009.11.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class OleDb : IDbProvider {
        /// <summary>
        /// OleDbʵ��
        /// </summary>
        /// <returns>OleDbʵ��</returns>
        public DbProviderFactory Instance() { return OleDbFactory.Instance; }
        /// <summary>
        /// ���ظղ����¼������IDֵ
        /// </summary>
        /// <returns>SQL</returns>
        public string GetLastIDSQL() { return "SELECT @@IDENTITY"; }
        /// <summary>
        /// �Ƿ�֧��ȫ������
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsFullTextSearchEnabled() { return false; }
        /// <summary>
        /// �Ƿ�֧��ѹ�����ݿ�
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsCompactDatabase() { return false; }
        /// <summary>
        /// �Ƿ�֧�ֱ������ݿ�
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsBackupDatabase() { return false; }
        /// <summary>
        /// �Ƿ�֧�����ݿ��Ż�
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsDbOptimize() { return false; }
        /// <summary>
        /// �Ƿ�֧�����ݿ�����
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsShrinkData() { return false; }
        /// <summary>
        /// �Ƿ�֧�ִ洢����
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsStoreProc() { return false; }
        /// <summary>
        /// ����SQL������Ϣ�����
        /// </summary>
        /// <param name="cmd"></param>
        public void DeriveParameters(IDbCommand cmd) {
            if ((cmd as OleDbCommand) != null) OleDbCommandBuilder.DeriveParameters(cmd as OleDbCommand);
        }
        /// <summary>
        /// ����SQL����
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        public DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size) {
            if (Size > 0) return new OleDbParameter(ParamName, (OleDbType)DbType, Size);
            return new OleDbParameter(ParamName, (OleDbType)DbType);
        }
        public DbParameter MakeParam(string ParamName, object value) {
            return new OleDbParameter(ParamName, value);
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
        /// Oracle�����ݸ���
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
            if (Data.Pool(dbkey).DBType != "OleDb") return false;
            using (OleDbConnection connection = new OleDbConnection(Data.Pool(dbkey).ConnString)) {
                connection.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter("select * from " + dt.TableName + "  where 1=0", connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                int rowcount = dt.Rows.Count;
                for (int n = 0; n < rowcount; n++) {
                    dt.Rows[n].SetAdded();
                }
                adapter.UpdateBatchSize = batchSize;
                try {
                    adapter.Update(dt);
                } catch(Exception ex) {
                    if (error.IsNotNull()) error(ex);
                    return false;
                }
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
            return false;
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
            return false;
        }
    }
}
