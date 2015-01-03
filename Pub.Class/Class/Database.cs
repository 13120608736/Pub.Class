//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2006 , LiveXY , Ltd. 
//------------------------------------------------------------

using System;
using System.Data;
using System.Xml;
using System.Data.Common;
using System.Collections;
using System.Configuration;
using System.Reflection;
using System.Web;

namespace Pub.Class {
    /// <summary>
    /// ���ݿ������
    /// 
    /// �޸ļ�¼
    ///     2010.08.01 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    /// <example>
    /// <code>
    /// ���������
    /// Database db = new Database("dbkey");
    /// SqlConnection conn = new SqlConnection(db.ConnectionString);
    /// conn.Open();
    /// using (SqlTransaction trans = conn.BeginTransaction()) {
    ///     try {
    ///         db.ExecuteNonQuery(trans,);
    ///         db.ExecuteNonQuery(trans,);
    ///         trans.Commit();
    ///     } catch (Exception ex) {
    ///         trans.Rollback();
    ///         throw ex;
    ///     }
    /// }
    /// conn.Close();
    /// </code>
    /// </example>
    public partial class Database: IDisposable {
        #region ������
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="pool">��������</param>
        public Database(string pool) { key = pool; }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="dbType">���ݿ����� SqlServer��Access��MySql��OleDb��Odbc��Oracle��SQLite</param>
        /// <param name="connString">�����ַ���</param>
        /// <param name="pool">��������</param>
        public Database(string dbType, string connString, string pool = "") {
            this.connString = connString;
            this.dbType = dbType;
            this.key = pool;
        }
        void IDisposable.Dispose() {
            ResetDbProvider();
        }
        #endregion

        #region ˽�б���
        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        protected string connString = null;
        /// <summary>
        /// ���ӳ�
        /// </summary>
        protected string key = "ConnString";
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        protected string dbType = null;

        /// <summary>
        /// DbProviderFactoryʵ��
        /// </summary>
        private DbProviderFactory factory = null;

        /// <summary>
        /// ���ݽӿ�
        /// </summary>
        private IDbProvider provider = null;
        private int timeout = 30;
        /// <summary>
        /// ��ѯ����ͳ��
        /// </summary>
        private Int64 queryCount = 0;
        /// <summary>
        /// Parameters�����ϣ��
        /// </summary>
        private Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
        private readonly object lockHelper = new object();
        #endregion

        #region ����
        /// <summary>
        /// ��ѯ����ͳ��
        /// </summary>
        public Int64 QueryCount { get { return queryCount; } set { queryCount = value; } }
        public int Timeout { get { return timeout; } set { timeout = value; } }

        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        public string ConnString {
            get {
#if !MONO40
                if (string.IsNullOrEmpty(connString)) {
                    if (ConfigurationManager.ConnectionStrings[key].IsNotNull()) {
                        connString = ConfigurationManager.ConnectionStrings[key].ToString();
                        dbType = ConfigurationManager.ConnectionStrings[key].ProviderName;
                    }
                }
#endif
                return connString;
            }
            set { connString = value; }
        }
        /// <summary>
        /// ���ӳ�
        /// </summary>
        public string Pool {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public string DBType {
            get {
#if !MONO40
                if (string.IsNullOrEmpty(dbType)) {
                    if (ConfigurationManager.ConnectionStrings[key].IsNotNull()) dbType = ConfigurationManager.ConnectionStrings[key].ProviderName; else dbType = "SqlServer";
                }
#endif
                return dbType;
            }
            set { dbType = value; }
        }

        /// <summary>
        /// IDbProvider�ӿ�
        /// </summary>
        public IDbProvider Provider {
            get {
                if (provider.IsNull()) {
                    lock (lockHelper) {
                        if (provider.IsNull()) {
                            //System.Web.HttpContext.Current.Response.Write(dbType);
                            //System.Web.HttpContext.Current.Response.End();
                            dbType = DBType;
                            try {
                                provider = (IDbProvider)"Pub.Class.{0},Pub.Class.{0}".FormatWith(dbType).LoadClass();
                                //string _path = (HttpContext.Current.IsNotNull() ? "~/bin/".GetMapPath() : "".GetMapPath()) + "Pub.Class.{0}.dll".FormatWith(dbType);
                                //provider = (IDbProvider)Activator.CreateInstance(Assembly.LoadFrom(_path).GetType("Pub.Class.{0}".FormatWith(dbType), true, true));
                            } catch {
                                throw new Exception(dbType + " - ����web.config��DbType�ڵ����ݿ������Ƿ���ȷ�����磺SqlServer��Access��MySql��OleDb��Odbc");
                            }
                        }
                    }
                }
                return provider;
            }
        }

        /// <summary>
        /// DbFactoryʵ��
        /// </summary>
        public DbProviderFactory Factory {
            get {
                if (factory.IsNull()) factory = Provider.Instance();
                return factory;
            }
        }

        /// <summary>
        /// ˢ�����ݿ��ṩ��
        /// </summary>
        public void ResetDbProvider() {
            connString = null;
            factory = null;
            provider = null;
            dbType = null;
            key = null;
        }
        #endregion

        #region ˽�з���
        /// <summary>
        /// ��DbParameter��������(����ֵ)�����DbCommand����.
        /// ������������κ�һ����������DBNull.Value;
        /// �ò�������ֹĬ��ֵ��ʹ��.
        /// </summary>
        /// <param name="command">������</param>
        /// <param name="commandParameters">DbParameters����</param>
        private void AttachParameters(DbCommand command, DbParameter[] commandParameters) {
            if (command.IsNull()) throw new ArgumentNullException("command");
            if (commandParameters.IsNotNull()) {
                foreach (DbParameter p in commandParameters) {
                    if (p.IsNotNull()) {
                        // ���δ����ֵ���������,���������DBNull.Value.
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && (p.Value.IsNull())) p.Value = DBNull.Value;
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// ��DataRow���͵���ֵ���䵽DbParameter��������.
        /// </summary>
        /// <param name="commandParameters">Ҫ����ֵ��DbParameter��������</param>
        /// <param name="dataRow">��Ҫ������洢���̲�����DataRow</param>
        private void AssignParameterValues(DbParameter[] commandParameters, DataRow dataRow) {
            if ((commandParameters.IsNull()) || (dataRow.IsNull())) return;

            int i = 0;
            // ���ò���ֵ
            foreach (DbParameter commandParameter in commandParameters) {
                // ������������,���������,ֻ�׳�һ���쳣.
                if (commandParameter.ParameterName.IsNull() || commandParameter.ParameterName.Length <= 1) throw new Exception(string.Format("���ṩ����{0}һ����Ч������{1}.", i, commandParameter.ParameterName));
                // ��dataRow�ı��л�ȡΪ�����������������Ƶ��е�����.
                // ������ںͲ���������ͬ����,����ֵ������ǰ���ƵĲ���.
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1) commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }

        /// <summary>
        /// ��һ��������������DbParameter��������.
        /// </summary>
        /// <param name="commandParameters">Ҫ����ֵ��DbParameter��������</param>
        /// <param name="parameterValues">��Ҫ������洢���̲����Ķ�������</param>
        private void AssignParameterValues(DbParameter[] commandParameters, object[] parameterValues) {
            if ((commandParameters.IsNull()) || (parameterValues.IsNull())) return;

            // ȷ����������������������ƥ��,�����ƥ��,�׳�һ���쳣.
            if (commandParameters.Length != parameterValues.Length) throw new ArgumentException("����ֵ�����������ƥ��.");

            // ��������ֵ
            for (int i = 0, j = commandParameters.Length; i < j; i++) {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter) {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value.IsNull()) commandParameters[i].Value = DBNull.Value; else commandParameters[i].Value = paramInstance.Value;
                } else if (parameterValues[i].IsNull()) commandParameters[i].Value = DBNull.Value;
                else commandParameters[i].Value = parameterValues[i];
            }
        }

        /// <summary>
        /// Ԥ�����û��ṩ������,���ݿ�����/����/��������/����
        /// </summary>
        /// <param name="command">Ҫ�����DbCommand</param>
        /// <param name="connection">���ݿ�����</param>
        /// <param name="transaction">һ����Ч�����������nullֵ</param>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢��������SQL�����ı�</param>
        /// <param name="commandParameters">�������������DbParameter��������,���û�в���Ϊ'null'</param>
        /// <param name="mustCloseConnection"><c>true</c> ��������Ǵ򿪵�,��Ϊtrue,���������Ϊfalse.</param>
        private void PrepareCommand(DbCommand command, DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters, out bool mustCloseConnection) {
            if (command.IsNull()) throw new ArgumentNullException("command");
            if (commandText.IsNull() || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open) {
                mustCloseConnection = true;
                connection.Open();
            } else mustCloseConnection = false;

            // ���������һ�����ݿ�����.
            command.Connection = connection;

            // ���������ı�(�洢��������SQL���)
            command.CommandText = commandText;
            command.CommandTimeout = timeout;

            // ��������
            if (transaction.IsNotNull()) {
                if (transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // ������������.
            command.CommandType = commandType;

            // �����������
            if (commandParameters.IsNotNull()) AttachParameters(command, commandParameters);
            return;
        }

        /// <summary>
        /// ̽������ʱ�Ĵ洢����,����DbParameter��������.
        /// ��ʼ������ֵΪ DBNull.Value.
        /// </summary>
        /// <param name="connection">һ����Ч�����ݿ�����</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="includeReturnValueParameter">�Ƿ��������ֵ����</param>
        /// <returns>����DbParameter��������</returns>
        private DbParameter[] DiscoverSpParameterSet(DbConnection connection, string spName, bool includeReturnValueParameter) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            // ����cmdָ���Ĵ洢���̵Ĳ�����Ϣ,����䵽cmd��Parameters��������.
            Provider.DeriveParameters(cmd);
            connection.Close();
            // �������������ֵ����,���������е�ÿһ������ɾ��.
            if (!includeReturnValueParameter) cmd.Parameters.RemoveAt(0);

            // ������������
            DbParameter[] discoveredParameters = new DbParameter[cmd.Parameters.Count];
            // ��cmd��Parameters���������Ƶ�discoveredParameters����.
            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // ��ʼ������ֵΪ DBNull.Value.
            foreach (DbParameter discoveredParameter in discoveredParameters) discoveredParameter.Value = DBNull.Value;
            return discoveredParameters;
        }

        /// <summary>
        /// DbParameter�����������㿽��.
        /// </summary>
        /// <param name="originalParameters">ԭʼ��������</param>
        /// <returns>����һ��ͬ���Ĳ�������</returns>
        private DbParameter[] CloneParameters(DbParameter[] originalParameters) {
            DbParameter[] clonedParameters = new DbParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++) clonedParameters[i] = (DbParameter)((ICloneable)originalParameters[i]).Clone();

            return clonedParameters;
        }
        #endregion ˽�з�������

        #region ExecSql����
        /// <summary>
        /// ִ��ָ�������ַ���,���͵�DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql("SELECT * FROM [tableName]");
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecSql(string commandText) {
            return ExecSql(commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�������ַ���,���͵�DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql("PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecSql(string commandText, params DbParameter[] commandParameters) {
            return ExecSql(commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText, commandParameters);
        }

        /// <summary>
        /// ִ��ָ�������ַ���,���͵�DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql(out id,"SELECT * FROM [tableName]");
        /// </remarks>
        /// <param name="id">��������ID</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecSql(out int id, string commandText) {
            return ExecSql(out id, commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�������ַ���,���͵�DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql(CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecSql(CommandType commandType, string commandText) {
            return ExecSql(commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�������ַ���,���͵�DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql(CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecSql(CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);

            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();

                return ExecSql(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// ִ��ָ�������ַ���,�����ظղ��������ID
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql(out id, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="id">��������ID</param>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecSql(out int id, CommandType commandType, string commandText) {
            return ExecSql(out id, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�������ַ��������ظղ��������ID,���͵�DbCommand.
        /// </summary>
        /// <param name="id">��������ID</param>
        /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>��������Ӱ�������</returns>
        public int ExecSql(out int id, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);

            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();

                return ExecSql(out id, connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ�������� 
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSql(DbConnection connection, CommandType commandType, string commandText) {
            return ExecSql(connection, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql(conn, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">T�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSql(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");

            // ����DbCommand����,������Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ��DbCommand����,�����ؽ��.
            int retval = cmd.ExecuteNonQuery();

            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            if (mustCloseConnection) connection.Close();
            return retval;
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ���������������ID 
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="id">id</param>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSql(out int id, DbConnection connection, CommandType commandType, string commandText) {
            return ExecSql(out id, connection, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql(conn, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="id">id</param>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">T�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSql(out int id, DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (Provider.GetLastIDSQL().Trim() == "") throw new ArgumentNullException("GetLastIDSQL is \"\"");

            // ����DbCommand����,������Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ������
            int retval = cmd.ExecuteNonQuery();
            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Provider.GetLastIDSQL();

            int.TryParse(cmd.ExecuteScalar().ToString(), out id);
            queryCount++;

            if (mustCloseConnection) connection.Close();
            return retval;
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,�����������ֵ�����洢���̲���.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ
        /// ʾ��:  
        ///  int result = ExecSql(conn, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSql(DbConnection connection, string spName, params object[] parameterValues) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �ӻ����м��ش洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // ���洢���̷������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return ExecSql(connection, CommandType.StoredProcedure, spName, commandParameters);
            } else return ExecSql(connection, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,�����������ֵ�����洢���̲���.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ
        /// ʾ��:  
        ///  int result = ExecSql("PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSql(string spName, params object[] parameterValues) {
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �ӻ����м��ش洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(spName);

                // ���洢���̷������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return ExecSql(CommandType.StoredProcedure, spName, commandParameters);
            } else return ExecSql(CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// ִ�д������DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��.:  
        ///  int result = ExecSql(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSql(DbTransaction transaction, CommandType commandType, string commandText) {
            return ExecSql(transaction, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ�д������DbCommand(ָ������).
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSql(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ��DbCommand����,�����ؽ��.
            int retval = cmd.ExecuteNonQuery();

            // ���������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// ִ�д������DbCommand.
        /// </summary>
        /// <remarks>
        /// ʾ��.:  
        ///  int result = ExecSql(out id,trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="id">��������ID</param>
        /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSql(out int id, DbTransaction transaction, CommandType commandType, string commandText) {
            return ExecSql(out id, transaction, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ�д������DbCommand(ָ������).
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int result = ExecSql(out id,trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="id">��������ID</param>
        /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSql(out int id, DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ��
            int retval = cmd.ExecuteNonQuery();
            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Provider.GetLastIDSQL();
            int.TryParse(cmd.ExecuteScalar().ToString(), out id);
            return retval;
        }

        /// <summary>
        /// ִ�д������DbCommand(ָ������ֵ).
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ
        /// ʾ��:  
        ///  int result = ExecSql(conn, trans, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>������Ӱ�������</returns>
        public int ExecSql(DbTransaction transaction, string spName, params object[] parameterValues) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // ���洢���̲�����ֵ
                AssignParameterValues(commandParameters, parameterValues);

                // �������ط���
                return ExecSql(transaction, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                // û�в���ֵ
                return ExecSql(transaction, CommandType.StoredProcedure, spName);
            }
        }
        /// <summary>
        /// ʹ��һ�����ݿ�����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exec"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public T Connection<T>(Func<DbConnection, Database, T> exec, Action<Exception> error = null) {
            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();
                try {
                    T res = exec(connection, this);
                    return res;
                } catch (Exception ex) {
                    if (!error.IsNull()) error(ex);
                }
            }
            return default(T);
        }
        /// <summary>
        /// ִ������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exec"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public T ExecTran<T>(Func<DbTransaction, T> exec, Action<Exception> error = null) {
            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction()) {
                    try {
                        T res = exec(trans);
                        trans.Commit();
                        return res;
                    } catch (Exception ex) {
                        trans.Rollback();
                        if (!error.IsNull()) error(ex);
                    }
                }
            }
            return default(T);
        }
        /// <summary>
        /// ִ������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exec"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public T ExecTran<T>(Func<Database, DbTransaction, T> exec, Action<Exception> error = null) {
            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction()) {
                    try {
                        T res = exec(this, trans);
                        trans.Commit();
                        return res;
                    } catch (Exception ex) {
                        trans.Rollback();
                        if (!error.IsNull()) error(ex);
                    }
                }
            }
            return default(T);
        }
        /// <summary>
        /// ִ������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exec"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public T ExecTran<T>(Func<DbConnection, Database, DbTransaction, T> exec, Action<Exception> error = null) {
            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction()) {
                    try {
                        T res = exec(connection, this, trans);
                        trans.Commit();
                        return res;
                    } catch (Exception ex) {
                        trans.Rollback();
                        if (!error.IsNull()) error(ex);
                    }
                }
            }
            return default(T);
        }
        #endregion ExecSql��������

        #region ExecuteCommandWithSplitter����
        /// <summary>
        /// ���к���GO����Ķ���SQL����
        /// </summary>
        /// <param name="commandText">SQL�����ַ���</param>
        /// <param name="splitter">�ָ��ַ���</param>
        public void ExecuteCommandWithSplitter(string commandText, string splitter) {
            int startPos = 0;

            do {
                int lastPos = commandText.IndexOf(splitter, startPos);
                int len = (lastPos > startPos ? lastPos : commandText.Length) - startPos;
                string query = commandText.Substring(startPos, len);

                if (query.Trim().Length > 0) {
                    try { ExecSql(CommandType.Text, query); } catch { ;}
                }

                if (lastPos == -1)
                    break;
                else
                    startPos = lastPos + splitter.Length;
            } while (startPos < commandText.Length);

        }

        /// <summary>
        /// ���к���GO����Ķ���SQL����
        /// </summary>
        /// <param name="commandText">SQL�����ַ���</param>
        public void ExecuteCommandWithSplitter(string commandText) {
            ExecuteCommandWithSplitter(commandText, "\r\nGO\r\n");
        }
        #endregion ExecuteCommandWithSplitter��������

        #region GetDataSet����
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = GetDataSet("SELECT * FROM [table1]");
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(string commandText) {
            return GetDataSet(commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = GetDataSet("SELECT * FROM [table1]", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(string commandText, params DbParameter[] commandParameters) {
            return GetDataSet(commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText, commandParameters);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = GetDataSet(CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(CommandType commandType, string commandText) {
            return GetDataSet(commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��: 
        ///  DataSet ds = GetDataSet(CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);

            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();

                // ����ָ�����ݿ������ַ������ط���.
                return GetDataSet(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ֱ���ṩ����ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��: 
        ///  DataSet ds = GetDataSet("GetOrders", 24, 36);
        /// </remarks>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(string spName, params object[] parameterValues) {
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �ӻ����м����洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(spName);

                // ���洢���̲�������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return GetDataSet(CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDataSet(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = GetDataSet(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(DbConnection connection, CommandType commandType, string commandText) {
            return GetDataSet(connection, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ���洢���̲���,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = GetDataSet(conn, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");

            // Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // ����DbDataAdapter��DataSet.
            using (DbDataAdapter da = Factory.CreateDataAdapter()) {
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                // ���DataSet.
                da.Fill(ds);
                queryCount++;

                cmd.Parameters.Clear();

                if (mustCloseConnection) connection.Close();
                return ds;
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ������ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��.:  
        ///  DataSet ds = GetDataSet(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(DbConnection connection, string spName, params object[] parameterValues) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �Ȼ����м��ش洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // ���洢���̲�������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return GetDataSet(connection, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDataSet(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ�����������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = GetDataSet(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">����</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(DbTransaction transaction, CommandType commandType, string commandText) {
            return GetDataSet(transaction, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����������,ָ������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataSet ds = GetDataSet(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">����</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // ���� DataAdapter & DataSet
            using (DbDataAdapter da = Factory.CreateDataAdapter()) {
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
        }

        /// <summary>
        /// ִ��ָ�����������,ָ������ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��.:  
        ///  DataSet ds = GetDataSet(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">����</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataSet</returns>
        public DataSet GetDataSet(DbTransaction transaction, string spName, params object[] parameterValues) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �ӻ����м��ش洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // ���洢���̲�������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return GetDataSet(transaction, CommandType.StoredProcedure, spName, commandParameters);
            } else return GetDataSet(transaction, CommandType.StoredProcedure, spName);
        }
        #endregion GetDataSet���ݼ��������

        #region GetDataTable����

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataTable dt = GetDataTable("SELECT * FROM [table1]");
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(string commandText) {
            return GetDataTable(commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataTable dt = GetDataTable("SELECT * FROM [table1]", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(string commandText, params DbParameter[] commandParameters) {
            return GetDataTable(commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText, commandParameters);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataTable dt = GetDataTable(CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(CommandType commandType, string commandText) {
            return GetDataTable(commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��: 
        ///  DataTable dt = GetDataTable(CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);

            // �����������ݿ����Ӷ���,��������ͷŶ���.

            //using (DbConnection connection = (DbConnection)new System.Data.SqlClient.SqlConnection(ConnString))
            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();

                // ����ָ�����ݿ������ַ������ط���.
                return GetDataTable(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ֱ���ṩ����ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��: 
        ///  DataTable dt = GetDataSet("GetOrders", 24, 36);
        /// </remarks>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(string spName, params object[] parameterValues) {
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �ӻ����м����洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(spName);

                // ���洢���̲�������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return GetDataTable(CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDataTable(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataTable dt = GetDataSet(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(DbConnection connection, CommandType commandType, string commandText) {
            return GetDataTable(connection, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ���洢���̲���,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataTable dt = GetDataSet(conn, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");

            // Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // ����DbDataAdapter��DataSet.
            using (DbDataAdapter da = Factory.CreateDataAdapter()) {
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                dt.TableName = "NewTableName";
                // ���DataSet.
                da.Fill(dt);
                queryCount++;

                cmd.Parameters.Clear();

                if (mustCloseConnection) connection.Close();

                return dt;
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ������ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��.:  
        ///  DataTable dt = GetDataTable(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(DbConnection connection, string spName, params object[] parameterValues) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �Ȼ����м��ش洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // ���洢���̲�������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return GetDataTable(connection, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDataTable(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ�����������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataTable dt = GetDataSet(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">����</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(DbTransaction transaction, CommandType commandType, string commandText) {
            return GetDataTable(transaction, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����������,ָ������,����DataSet.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DataTable dt = GetDataSet(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">����</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // ���� DataAdapter & DataSet
            using (DbDataAdapter da = Factory.CreateDataAdapter()) {
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                dt.TableName = "NewTableName";
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;
            }
        }

        /// <summary>
        /// ִ��ָ�����������,ָ������ֵ,����DataSet.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
        /// ʾ��.:  
        ///  DataTable dt = GetDataTable(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">����</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>����һ�������������DataTable</returns>
        public DataTable GetDataTable(DbTransaction transaction, string spName, params object[] parameterValues) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �ӻ����м��ش洢���̲���
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // ���洢���̲�������ֵ
                AssignParameterValues(commandParameters, parameterValues);

                return GetDataTable(transaction, CommandType.StoredProcedure, spName, commandParameters);
            } else return GetDataTable(transaction, CommandType.StoredProcedure, spName);
        }
        #endregion GetDataSet���ݼ��������

        #region GetDbDataReader �����Ķ���
        /// <summary>
        /// ö��,��ʶ���ݿ���������BaseDbHelper�ṩ�����ɵ������ṩ
        /// </summary>
        private enum DbConnectionOwnership {
            /// <summary>��BaseDbHelper�ṩ����</summary>
            Internal,
            /// <summary>�ɵ������ṩ����</summary>
            External
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ���������Ķ���.
        /// </summary>
        /// <remarks>
        /// �����BaseDbHelper������,�����ӹر�DataReaderҲ���ر�.
        /// ����ǵ��ö�������,DataReader�ɵ��ö�����.
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="transaction">һ����Ч������,����Ϊ 'null'</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">DbParameters��������,���û�в�����Ϊ'null'</param>
        /// <param name="connectionOwnership">��ʶ���ݿ����Ӷ������ɵ������ṩ������BaseDbHelper�ṩ</param>
        /// <returns>���ذ����������DbDataReader</returns>
        private DbDataReader GetDbDataReader(DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters, DbConnectionOwnership connectionOwnership) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;
            // ��������
            DbCommand cmd = Factory.CreateCommand();
            try {
                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

                // ���������Ķ���
                DbDataReader dataReader;

                if (connectionOwnership == DbConnectionOwnership.External)
                    dataReader = cmd.ExecuteReader();
                else
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                queryCount++;
                // �������,�Ա��ٴ�ʹ��..
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command
                // then the SqlReader can�t set its values. 
                // When this happen, the parameters can�t be used again in other command.
                bool canClear = true;
                foreach (DbParameter commandParameter in cmd.Parameters) {
                    if (commandParameter.Direction != ParameterDirection.Input) canClear = false;
                }

                if (canClear) cmd.Parameters.Clear();

                return dataReader;
            } catch {
                if (mustCloseConnection) connection.Close();
                throw;
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = GetDbDataReader("GetOrders");
        /// </remarks>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(string commandText) {
            return GetDbDataReader(commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = GetDbDataReader("GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">DbParameter��������(new DbParameter("@prodid", 24))</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(string commandText, params DbParameter[] commandParameters) {
            return GetDbDataReader(commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText, commandParameters);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = GetDbDataReader(CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(CommandType commandType, string commandText) {
            return GetDbDataReader(commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = GetDbDataReader(CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">DbParameter��������(new DbParameter("@prodid", 24))</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            DbConnection connection = null;
            try {
                connection = Factory.CreateConnection();
                connection.ConnectionString = ConnString;
                connection.Open();

                return GetDbDataReader(connection, null, commandType, commandText, commandParameters, DbConnectionOwnership.Internal);
            } catch {
                // If we fail to return the SqlDatReader, we need to close the connection ourselves
                if (connection.IsNotNull()) connection.Close();
                throw;
            }

        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ����������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// ʾ��:  
        ///  DbDataReader dr = GetDbDataReader("GetOrders", 24, 36);
        /// </remarks>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(string spName, params object[] parameterValues) {
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                DbParameter[] commandParameters = GetSpParameterSet(spName);

                AssignParameterValues(commandParameters, parameterValues);

                return GetDbDataReader(CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDbDataReader(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ���������Ķ���.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = GetDbDataReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(DbConnection connection, CommandType commandType, string commandText) {
            return GetDbDataReader(connection, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ����Ӷ���������Ķ���,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = GetDbDataReader(conn, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandParameters">DbParameter��������</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            return GetDbDataReader(connection, (DbTransaction)null, commandType, commandText, commandParameters, DbConnectionOwnership.External);
        }

        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ����Ӷ���������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// ʾ��:  
        ///  DbDataReader dr = GetDbDataReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">T�洢������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(DbConnection connection, string spName, params object[] parameterValues) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return GetDbDataReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDbDataReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbDataReader dr = GetDbDataReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(DbTransaction transaction, CommandType commandType, string commandText) {
            return GetDbDataReader(transaction, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///   DbDataReader dr = GetDbDataReader(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������DbParameter��������</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            return GetDbDataReader(transaction.Connection, transaction, commandType, commandText, commandParameters, DbConnectionOwnership.External);
        }

        /// <summary>
        /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  DbDataReader dr = GetDbDataReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReader(DbTransaction transaction, string spName, params object[] parameterValues) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return GetDbDataReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                // û�в���ֵ
                return GetDbDataReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion GetDbDataReader�����Ķ���

        #region GetScalar ���ؽ�����еĵ�һ�е�һ��

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar("GetOrderCount");
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(string commandText) {
            return GetScalar(commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar(CommandType.StoredProcedure, "GetOrderCount", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������DbParameter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(string commandText, params DbParameter[] commandParameters) {
            return GetScalar(commandText.IndexOf(" ") > 0 ? CommandType.Text : CommandType.StoredProcedure, commandText, commandParameters);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar(CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(CommandType commandType, string commandText) {
            // ִ�в���Ϊ�յķ���
            return GetScalar(commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar(CommandType.StoredProcedure, "GetOrderCount", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������DbParameter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();

                // ����ָ�����ݿ������ַ������ط���.
                return GetScalar(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar("GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="spName">�洢��������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(string spName, params object[] parameterValues) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);

            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();

                if (connection.IsNull()) throw new ArgumentNullException("connection");
                if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

                // ����в���ֵ
                if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                    // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                    DbParameter[] commandParameters = GetSpParameterSet(spName);

                    // ���洢���̲�����ֵ
                    AssignParameterValues(commandParameters, parameterValues);

                    // �������ط���
                    return GetScalar(connection, spName, commandParameters);
                } else {
                    // û�в���ֵ
                    return GetScalar(CommandType.StoredProcedure, spName);
                }
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(DbConnection connection, CommandType commandType, string commandText) {
            // ִ�в���Ϊ�յķ���
            return GetScalar(connection, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������DbParameter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");

            // ����DbCommand����,������Ԥ����
            DbCommand cmd = Factory.CreateCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (DbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ��DbCommand����,�����ؽ��.
            object retval = cmd.ExecuteScalar();

            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();

            if (mustCloseConnection) connection.Close();

            return retval;
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar(conn, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(DbConnection connection, string spName, params object[] parameterValues) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // ���洢���̲�����ֵ
                AssignParameterValues(commandParameters, parameterValues);

                // �������ط���
                return GetScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                // û�в���ֵ
                return GetScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(DbTransaction transaction, CommandType commandType, string commandText) {
            // ִ�в���Ϊ�յķ���
            return GetScalar(transaction, commandType, commandText, (DbParameter[])null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,ָ������,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������DbParameter��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // ����DbCommand����,������Ԥ����
            DbCommand cmd = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ��DbCommand����,�����ؽ��.
            object retval = cmd.ExecuteScalar();
            queryCount++;
            // �������,�Ա��ٴ�ʹ��.
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  int orderCount = (int)GetScalar(trans, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalar(DbTransaction transaction, string spName, params object[] parameterValues) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // PPull the parameters for this stored procedure from the parameter cache ()
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // ���洢���̲�����ֵ
                AssignParameterValues(commandParameters, parameterValues);

                // �������ط���
                return GetScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                // û�в���ֵ
                return GetScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion GetScalar

        #region FillDataSet ������ݼ�
        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ�������ݼ�.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataSet(CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)</param>
        public void FillDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (dataSet.IsNull()) throw new ArgumentNullException("dataSet");

            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();

                // ����ָ�����ݿ������ַ������ط���.
                FillDataSet(connection, commandType, commandText, dataSet, tableNames);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ�������ݼ�.ָ���������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataSet(CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="commandParameters">����������DbParameter��������</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        public void FillDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (dataSet.IsNull()) throw new ArgumentNullException("dataSet");
            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();

                // ����ָ�����ݿ������ַ������ط���.
                FillDataSet(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ�������ݼ�,ָ���洢���̲���ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  FillDataSet(CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, 24);
        /// </remarks>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>    
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        public void FillDataSet(string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (dataSet.IsNull()) throw new ArgumentNullException("dataSet");
            // �����������ݿ����Ӷ���,��������ͷŶ���.
            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                connection.Open();

                // ����ָ�����ݿ������ַ������ط���.
                FillDataSet(connection, spName, dataSet, tableNames, parameterValues);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ�������ݼ�.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataSet(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>    
        public void FillDataSet(DbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames) {
            FillDataSet(connection, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ�������ݼ�,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataSet(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param name="commandParameters">����������DbParameter��������</param>
        public void FillDataSet(DbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters) {
            FillDataSet(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ�������ݼ�,ָ���洢���̲���ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  FillDataSet(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        public void FillDataSet(DbConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (dataSet.IsNull()) throw new ArgumentNullException("dataSet");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // ���洢���̲�����ֵ
                AssignParameterValues(commandParameters, parameterValues);

                // �������ط���
                FillDataSet(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            } else {
                // û�в���ֵ
                FillDataSet(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,ӳ�����ݱ�������ݼ�.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataSet(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        public void FillDataSet(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames) {
            FillDataSet(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,ӳ�����ݱ�������ݼ�,ָ������.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataSet(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param name="commandParameters">����������DbParameter��������</param>
        public void FillDataSet(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters) {
            FillDataSet(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,ӳ�����ݱ�������ݼ�,ָ���洢���̲���ֵ.
        /// </summary>
        /// <remarks>
        /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
        /// 
        /// ʾ��:  
        ///  FillDataSet(trans, "GetOrders", ds, new string[]{"orders"}, 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param name="parameterValues">������洢������������Ķ�������</param>
        public void FillDataSet(DbTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (dataSet.IsNull()) throw new ArgumentNullException("dataSet");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ����в���ֵ
            if ((parameterValues.IsNotNull()) && (parameterValues.Length > 0)) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // ���洢���̲�����ֵ
                AssignParameterValues(commandParameters, parameterValues);

                // �������ط���
                FillDataSet(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            } else {
                // û�в���ֵ
                FillDataSet(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        /// <summary>
        /// [˽�з���][�ڲ�����]ִ��ָ�����ݿ����Ӷ���/���������,ӳ�����ݱ�������ݼ�,DataSet/TableNames/DbParameters.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  FillDataSet(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new DbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="transaction">һ����Ч����������</param>
        /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
        /// <param name="commandText">�洢�������ƻ�SQL���</param>
        /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
        /// <param name="tableNames">��ӳ������ݱ�����
        /// �û�����ı��� (������ʵ�ʵı���.)
        /// </param>
        /// <param name="commandParameters">����������DbParameter��������</param>
        private void FillDataSet(DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params DbParameter[] commandParameters) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (dataSet.IsNull()) throw new ArgumentNullException("dataSet");

            // ����DbCommand����,������Ԥ����
            DbCommand command = Factory.CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ������
            using (DbDataAdapter dataAdapter = Factory.CreateDataAdapter()) {
                dataAdapter.SelectCommand = command;
                // ׷�ӱ�ӳ��
                if (tableNames.IsNotNull() && tableNames.Length > 0) {
                    string tableName = "Table";
                    for (int index = 0; index < tableNames.Length; index++) {
                        if (tableNames[index].IsNull() || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName += (index + 1).ToString();
                    }
                }

                // ������ݼ�ʹ��Ĭ�ϱ�����
                dataAdapter.Fill(dataSet);

                // �������,�Ա��ٴ�ʹ��.
                command.Parameters.Clear();
            }

            if (mustCloseConnection) connection.Close();
        }
        #endregion

        #region UpdateDataSet �������ݼ�
        /// <summary>
        /// ִ�����ݼ����µ����ݿ�,ָ��inserted, updated, or deleted����.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  UpdateDataSet(insertCommand, deleteCommand, updateCommand, dataSet, "Order");
        /// </remarks>
        /// <param name="insertCommand">[׷�Ӽ�¼]һ����Ч��SQL����洢����</param>
        /// <param name="deleteCommand">[ɾ����¼]һ����Ч��SQL����洢����</param>
        /// <param name="updateCommand">[���¼�¼]һ����Ч��SQL����洢����</param>
        /// <param name="dataSet">Ҫ���µ����ݿ��DataSet</param>
        /// <param name="tableName">Ҫ���µ����ݿ��DataTable</param>
        public void UpdateDataSet(DbCommand insertCommand, DbCommand deleteCommand, DbCommand updateCommand, DataSet dataSet, string tableName) {
            if (insertCommand.IsNull()) throw new ArgumentNullException("insertCommand");
            if (deleteCommand.IsNull()) throw new ArgumentNullException("deleteCommand");
            if (updateCommand.IsNull()) throw new ArgumentNullException("updateCommand");
            if (tableName.IsNull() || tableName.Length == 0) throw new ArgumentNullException("tableName");

            // ����DbDataAdapter,��������ɺ��ͷ�.
            using (DbDataAdapter dataAdapter = Factory.CreateDataAdapter()) {
                // ������������������
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                // �������ݼ��ı䵽���ݿ�
                dataAdapter.Update(dataSet, tableName);

                // �ύ���иı䵽���ݼ�.
                dataSet.AcceptChanges();
            }
        }
        #endregion

        #region CreateCommand ����һ��DbCommand����
        /// <summary>
        /// ����DbCommand����,ָ�����ݿ����Ӷ���,�洢�������Ͳ���.
        /// </summary>
        /// <remarks>
        /// ʾ��:  
        ///  DbCommand command = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName");
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="sourceColumns">Դ�������������</param>
        /// <returns>����DbCommand����</returns>
        public DbCommand CreateCommand(DbConnection connection, string spName, params string[] sourceColumns) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ��������
            DbCommand cmd = Factory.CreateCommand();
            cmd.CommandText = spName;
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;

            // ����в���ֵ
            if ((sourceColumns.IsNotNull()) && (sourceColumns.Length > 0)) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // ��Դ����е�ӳ�䵽DataSet������.
                for (int index = 0; index < sourceColumns.Length; index++)
                    commandParameters[index].SourceColumn = sourceColumns[index];

                // Attach the discovered parameters to the DbCommand object
                AttachParameters(cmd, commandParameters);
            }

            return cmd;
        }
        #endregion

        #region ExecSqlTypedParams ���ͻ�����(DataRow)
        /// <summary>
        /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,������Ӱ�������.
        /// </summary>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSqlTypedParams(String spName, DataRow dataRow) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return ExecSql(CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return ExecSql(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,������Ӱ�������.
        /// </summary>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSqlTypedParams(DbConnection connection, String spName, DataRow dataRow) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return ExecSql(connection, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return ExecSql(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,������Ӱ�������.
        /// </summary>
        /// <param name="transaction">һ����Ч���������� object</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����Ӱ�������</returns>
        public int ExecSqlTypedParams(DbTransaction transaction, String spName, DataRow dataRow) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // Sf the row has values, the store procedure parameters must be initialized
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return ExecSql(transaction, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return ExecSql(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region GetDataSetTypedParams ���ͻ�����(DataRow)
        /// <summary>
        /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
        /// </summary>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����һ�������������DataSet.</returns>
        public DataSet GetDataSetTypedParams(String spName, DataRow dataRow) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            //���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetDataSet(CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDataSet(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
        /// </summary>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����һ�������������DataSet.</returns>
        public DataSet GetDataSetTypedParams(DbConnection connection, String spName, DataRow dataRow) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetDataSet(connection, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDataSet(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
        /// </summary>
        /// <param name="transaction">һ����Ч���������� object</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����һ�������������DataSet.</returns>
        public DataSet GetDataSetTypedParams(DbTransaction transaction, String spName, DataRow dataRow) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetDataSet(transaction, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDataSet(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region GetDataTableTypedParams ���ͻ�����(DataRow)
        /// <summary>
        /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
        /// </summary>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����һ�������������DataTable.</returns>
        public DataTable GetDataTableTypedParams(String spName, DataRow dataRow) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            //���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetDataTable(CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDataTable(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
        /// </summary>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����һ�������������DataTable.</returns>
        public DataTable GetDataTableTypedParams(DbConnection connection, String spName, DataRow dataRow) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetDataTable(connection, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDataTable(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
        /// </summary>
        /// <param name="transaction">һ����Ч���������� object</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>����һ�������������DataTable.</returns>
        public DataTable GetDataTableTypedParams(DbTransaction transaction, String spName, DataRow dataRow) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetDataTable(transaction, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDataTable(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region GetDbDataReaderTypedParams ���ͻ�����(DataRow)
        /// <summary>
        /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataReader.
        /// </summary>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReaderTypedParams(String spName, DataRow dataRow) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetDbDataReader(ConnString, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDbDataReader(ConnString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataReader.
        /// </summary>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReaderTypedParams(DbConnection connection, String spName, DataRow dataRow) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetDbDataReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDbDataReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataReader.
        /// </summary>
        /// <param name="transaction">һ����Ч���������� object</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ذ����������DbDataReader</returns>
        public DbDataReader GetDbDataReaderTypedParams(DbTransaction transaction, String spName, DataRow dataRow) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetDbDataReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetDbDataReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region GetScalarTypedParams ���ͻ�����(DataRow)
        /// <summary>
        /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalarTypedParams(String spName, DataRow dataRow) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetScalar(CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetScalar(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalarTypedParams(DbConnection connection, String spName, DataRow dataRow) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(connection, spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,���ؽ�����еĵ�һ�е�һ��.
        /// </summary>
        /// <param name="transaction">һ����Ч���������� object</param>
        /// <param name="spName">�洢��������</param>
        /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public object GetScalarTypedParams(DbTransaction transaction, String spName, DataRow dataRow) {
            if (transaction.IsNull()) throw new ArgumentNullException("transaction");
            if (transaction.IsNotNull() && transaction.Connection.IsNull()) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            // ���row��ֵ,�洢���̱����ʼ��.
            if (dataRow.IsNotNull() && dataRow.ItemArray.Length > 0) {
                // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
                DbParameter[] commandParameters = GetSpParameterSet(transaction.Connection, spName);

                // �������ֵ
                AssignParameterValues(commandParameters, dataRow);

                return GetScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            } else {
                return GetScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region ���淽��
        /// <summary>
        /// ׷�Ӳ������鵽����.
        /// </summary>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <param name="commandParameters">Ҫ����Ĳ�������</param>
        public void CacheParameterSet(string commandText, params DbParameter[] commandParameters) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (commandText.IsNull() || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = ConnString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /// <summary>
        /// �ӻ����л�ȡ��������.
        /// </summary>
        /// <param name="commandText">�洢��������SQL���</param>
        /// <returns>��������</returns>
        public DbParameter[] GetCachedParameterSet(string commandText) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (commandText.IsNull() || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = ConnString + ":" + commandText;

            DbParameter[] cachedParameters = paramCache[hashKey] as DbParameter[];
            if (cachedParameters.IsNull()) return null; else return CloneParameters(cachedParameters);
        }
        #endregion ���淽������

        #region ����ָ���Ĵ洢���̵Ĳ�����
        /// <summary>
        /// ����ָ���Ĵ洢���̵Ĳ�����
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param name="spName">�洢������</param>
        /// <returns>����DbParameter��������</returns>
        public DbParameter[] GetSpParameterSet(string spName) {
            return GetSpParameterSet(spName, false);
        }

        /// <summary>
        /// ����ָ���Ĵ洢���̵Ĳ�����
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param name="spName">�洢������</param>
        /// <param name="includeReturnValueParameter">�Ƿ��������ֵ����</param>
        /// <returns>����DbParameter��������</returns>
        public DbParameter[] GetSpParameterSet(string spName, bool includeReturnValueParameter) {
            if (ConnString.IsNull() || ConnString.Length == 0) throw new ArgumentNullException(key);
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            using (DbConnection connection = Factory.CreateConnection()) {
                connection.ConnectionString = ConnString;
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// [�ڲ�]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���).
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ������ַ�</param>
        /// <param name="spName">�洢������</param>
        /// <returns>����DbParameter��������</returns>
        internal DbParameter[] GetSpParameterSet(DbConnection connection, string spName) {
            return GetSpParameterSet(connection, spName, false);
        }

        /// <summary>
        /// [�ڲ�]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���)
        /// </summary>
        /// <remarks>
        /// �����������ѯ���ݿ�,������Ϣ�洢������.
        /// </remarks>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="includeReturnValueParameter">
        /// �Ƿ��������ֵ����
        /// </param>
        /// <returns>����DbParameter��������</returns>
        internal DbParameter[] GetSpParameterSet(DbConnection connection, string spName, bool includeReturnValueParameter) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            using (DbConnection clonedConnection = (DbConnection)((ICloneable)connection).Clone()) {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// [˽��]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���)
        /// </summary>
        /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="includeReturnValueParameter">�Ƿ��������ֵ����</param>
        /// <returns>����DbParameter��������</returns>
        private DbParameter[] GetSpParameterSetInternal(DbConnection connection, string spName, bool includeReturnValueParameter) {
            if (connection.IsNull()) throw new ArgumentNullException("connection");
            if (spName.IsNull() || spName.Length == 0) throw new ArgumentNullException("spName");

            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            DbParameter[] cachedParameters;

            cachedParameters = paramCache[hashKey] as DbParameter[];
            if (cachedParameters.IsNull()) {
                DbParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        }
        #endregion ��������������

        #region ���ɲ���
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="ParamName">������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <param name="Value">����ֵ</param>
        /// <returns></returns>
        public DbParameter MakeInParam(string ParamName, DbType DbType, int Size, object Value) {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="ParamName">������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <returns></returns>
        public DbParameter MakeOutParam(string ParamName, DbType DbType, int Size) {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="ParamName">������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <param name="Direction">����/���</param>
        /// <param name="Value">ֵ</param>
        /// <returns></returns>
        public DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size, ParameterDirection Direction, object Value) {
            DbParameter param;
            param = Provider.MakeParam(ParamName, DbType, Size);
            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value.IsNull())) param.Value = Value;
            return param;
        }
        public DbParameter MakeParam(string ParamName, object Value) {
            return Provider.MakeParam(ParamName, Value);
        }
        #endregion ���ɲ�������

        #region ִ��GetScalar,��������ַ������������
        /// <summary>
        /// ȡ��һ�е�һ������
        /// </summary>
        /// <param name="commandType">commandType</param>
        /// <param name="commandText">SQL</param>
        /// <returns></returns>
        public string GetScalarToStr(CommandType commandType, string commandText) {
            object ec = GetScalar(commandType, commandText);
            if (ec.IsNull()) return "";
            return ec.ToString();
        }
        /// <summary>
        /// ȡ��һ�е�һ������
        /// </summary>
        /// <param name="commandType">commandType</param>
        /// <param name="commandText">SQL</param>
        /// <param name="commandParameters">����</param>
        /// <returns></returns>
        public string GetScalarToStr(CommandType commandType, string commandText, params DbParameter[] commandParameters) {
            object ec = GetScalar(commandType, commandText, commandParameters);
            if (ec.IsNull()) return "";
            return ec.ToString();
        }
        #endregion

        /// <summary>
        /// ��ʼ�ַ�
        /// </summary>
        public string GetIdentifierStart() { return Provider.GetIdentifierStart(); }
        /// <summary>
        /// �����ַ�
        /// </summary>
        public string GetIdentifierEnd() { return Provider.GetIdentifierEnd(); }
        /// <summary>
        /// ����ǰ������
        /// </summary>
        public string GetParamIdentifier() { return Provider.GetParamIdentifier(); }
        /// <summary>
        /// SqlServer�����ݸ���
        /// </summary>
        /// <param name="dt">����Դ dt.TableNameһ��Ҫ�����ݿ������Ӧ</param>
        /// <param name="dbkey">���ݿ�</param>
        /// <param name="sqlOptions">ѡ�� Ĭ��Default</param>
        /// <param name="isTran">�Ƿ�ʹ������ Ĭ��false</param>
        /// <param name="timeout">��ʱʱ��7200 2Сʱ</param>
        /// <param name="batchSize">ÿһ�����е�����</param>
        /// <param name="error">������</param>
        /// <returns>true/false</returns>
        public bool DataBulkCopy(DataTable dt, BulkCopyOptions sqlOptions = BulkCopyOptions.Default, bool isTran = false, int timeout = 7200, int batchSize = 10000, Action<Exception> error = null) {
            return Provider.DataBulkCopy(dt, key, sqlOptions, isTran, timeout, batchSize, error);
        }
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
        public bool DataBulkCopy(DataTable dt, Action<Exception> error = null, BulkCopyOptions options = BulkCopyOptions.Default, bool isTran = false, int timeout = 7200, int batchSize = 10000) {
            return Provider.DataBulkCopy(dt, key, options, isTran, timeout, batchSize, error);
        }
        /// <summary>
        /// SqlServer�����ݸ���
        /// </summary>
        /// <param name="dt">����Դ dt.TableNameһ��Ҫ�����ݿ������Ӧ</param>
        /// <param name="dbkey">���ݿ�</param>
        /// <param name="options">ѡ�� Ĭ��Default</param>
        /// <param name="timeout">��ʱʱ��7200 2Сʱ</param>
        /// <param name="batchSize">ÿһ�����е�����</param>
        /// <param name="error">������</param>
        /// <param name="isTran">ʹ������</param>
        /// <returns>true/false</returns>
        public bool DataBulkCopy(DataTable dt, bool isTran = true, BulkCopyOptions options = BulkCopyOptions.Default, int timeout = 7200, int batchSize = 10000, Action<Exception> error = null) {
            return Provider.DataBulkCopy(dt, key, options, true, timeout, batchSize, error);
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
            return Provider.DataBulkCopy(conn, tran, dt, options, timeout, batchSize, error);
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
        public bool DataBulkCopy(IDbConnection conn, IDbTransaction tran, DataTable dt, Action<Exception> error = null, BulkCopyOptions options = BulkCopyOptions.Default, int timeout = 7200, int batchSize = 10000) { 
            return Provider.DataBulkCopy(conn, tran, dt, options, timeout, batchSize, error);
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
        public bool DataBulkCopy(IDataReader dr, string tableName, BulkCopyOptions options = BulkCopyOptions.Default, bool isTran = false, int timeout = 7200, int batchSize = 10000, Action<Exception> error = null) {
            return Provider.DataBulkCopy(dr, tableName, key, options, isTran, timeout, batchSize, error);
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
        public bool DataBulkCopy(IDataReader dr, string tableName, Action<Exception> error = null, BulkCopyOptions options = BulkCopyOptions.Default, bool isTran = false, int timeout = 7200, int batchSize = 10000) {
            return Provider.DataBulkCopy(dr, tableName, key, options, isTran, timeout, batchSize, error);
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
        public bool DataBulkCopy(IDataReader dr, string tableName, bool isTran = true, BulkCopyOptions options = BulkCopyOptions.Default, int timeout = 7200, int batchSize = 10000, Action<Exception> error = null) {
            return Provider.DataBulkCopy(dr, tableName, key, options, isTran, timeout, batchSize, error);
        }
    }
}
