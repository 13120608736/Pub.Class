//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

namespace Pub.Class {
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.Odbc;

    /// <summary>
    /// Odbc���ݿ�ʵ��
    /// 
    /// �޸ļ�¼
    ///     2009.11.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public class Odbc : IDbProvider {
        /// <summary>
        /// Odbcʵ��
        /// </summary>
        /// <returns>Odbcʵ��</returns>
        public DbProviderFactory Instance() { return OdbcFactory.Instance; }
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
            if ((cmd as OdbcCommand) != null) OdbcCommandBuilder.DeriveParameters(cmd as OdbcCommand);
        }
        /// <summary>
        /// ����SQL����
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        public DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size) {
            if (Size > 0) return new OdbcParameter(ParamName, (OdbcType)DbType, Size);
            return new OdbcParameter(ParamName, (OdbcType)DbType);
        }
        public DbParameter MakeParam(string ParamName, object value) {
            return new OdbcParameter(ParamName, value);
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
        public string GetParamIdentifier() { return "?"; }
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
            return false;
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
