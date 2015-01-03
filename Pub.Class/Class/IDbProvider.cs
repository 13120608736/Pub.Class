//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Data.Common;

namespace Pub.Class {
    /// <summary>
    /// ���ݿ�����ӿ�
    /// 
    /// �޸ļ�¼
    ///     2011.12.18 �汾��1.1 livexy ���GetIdentifierStart/GetIdentifierEnd/GetParamIdentifier
    ///     2006.05.04 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public interface IDbProvider: IAddIn {
        /// <summary>
        /// ����DbProviderFactoryʵ��
        /// </summary>
        /// <returns></returns>
        DbProviderFactory Instance();
        /// <summary>
        /// ���ظղ����¼������IDֵ, �粻֧����Ϊ""
        /// </summary>
        /// <returns></returns>
        string GetLastIDSQL();
        /// <summary>
        /// �Ƿ�֧��ȫ������
        /// </summary>
        /// <returns></returns>
        bool IsFullTextSearchEnabled();
        /// <summary>
        /// �Ƿ�֧��ѹ�����ݿ�
        /// </summary>
        /// <returns></returns>
        bool IsCompactDatabase();
        /// <summary>
        /// �Ƿ�֧�ֱ������ݿ�
        /// </summary>
        /// <returns></returns>
        bool IsBackupDatabase();
        /// <summary>
        /// �Ƿ�֧�����ݿ��Ż�
        /// </summary>
        /// <returns></returns>
        bool IsDbOptimize();
        /// <summary>
        /// �Ƿ�֧�����ݿ�����
        /// </summary>
        /// <returns></returns>
        bool IsShrinkData();
        /// <summary>
        /// �Ƿ�֧�ִ洢����
        /// </summary>
        /// <returns></returns>
        bool IsStoreProc();
        /// <summary>
        /// ����SQL������Ϣ�����
        /// </summary>
        /// <param name="cmd"></param>
        void DeriveParameters(IDbCommand cmd);
        /// <summary>
        /// ����SQL����
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size);
        DbParameter MakeParam(string ParamName, object value);
        /// <summary>
        /// ��ʼ�ַ�
        /// </summary>
        string GetIdentifierStart();
        /// <summary>
        /// �����ַ�
        /// </summary>
        string GetIdentifierEnd();
        /// <summary>
        /// ����ǰ������
        /// </summary>
        string GetParamIdentifier();
        /// <summary>
        /// �����ݸ���
        /// </summary>
        /// <param name="dt">����Դ dt.TableNameһ��Ҫ�����ݿ������Ӧ</param>
        /// <param name="dbkey">���ݿ�</param>
        /// <param name="options">ѡ�� Ĭ��Default</param>
        /// <param name="isTran">�Ƿ�ʹ������ Ĭ��false</param>
        /// <param name="timeout">��ʱʱ��7200 2Сʱ</param>
        /// <param name="batchSize">ÿһ�����е�����</param>
        /// <param name="error">������</param>
        /// <returns>true/false</returns>
        bool DataBulkCopy(DataTable dt, string dbkey = "", BulkCopyOptions options = BulkCopyOptions.Default, bool isTran = false, int timeout = 7200, int batchSize = 10000, Action<Exception> error = null);
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
        bool DataBulkCopy(IDbConnection conn, IDbTransaction tran, DataTable dt, BulkCopyOptions options = BulkCopyOptions.Default, int timeout = 7200, int batchSize = 10000, Action<Exception> error = null);
        /// <summary>
        /// �����ݸ���
        /// </summary>
        /// <param name="dr">����Դ</param>
        /// <param name="tableName">һ��Ҫ�����ݿ������Ӧ</param>
        /// <param name="dbkey">���ݿ�</param>
        /// <param name="options">ѡ�� Ĭ��Default</param>
        /// <param name="isTran">�Ƿ�ʹ������ Ĭ��false</param>
        /// <param name="timeout">��ʱʱ��7200 2Сʱ</param>
        /// <param name="batchSize">ÿһ�����е�����</param>
        /// <param name="error">������</param>
        /// <returns>true/false</returns>
        bool DataBulkCopy(IDataReader dr, string tableName, string dbkey = "", BulkCopyOptions options = BulkCopyOptions.Default, bool isTran = false, int timeout = 7200, int batchSize = 10000, Action<Exception> error = null);
    }
}
