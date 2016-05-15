using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace BrightLine.Common.Utility
{
	public class DatabaseHelper
	{
		private readonly string _connectionString;


		/// <summary>
		/// Instantiates an instance with the provided connection string key
		/// </summary>
		/// <param name="key">The database connection string.</param>
		public DatabaseHelper(string key)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be empty or null.");

			var css = ConfigurationManager.ConnectionStrings[key];
			if (css == null)
				throw new ArgumentOutOfRangeException("key", key + " does not exist.");

			_connectionString = css.ConnectionString;
		}


		/// <summary>
		/// executes
		/// </summary>
		/// <param name="commandText">Sql text or StoredProcedure Name. </param>
		/// <param name="dbParameters">List of parameters</param>
		/// <example>
		/// IDatabase db = new Database("connectionString value");
		/// DataTable tbl = db.ExecuteDataTable("select * from users", CommandType.Text, null);
		/// </example>
		/// <returns></returns>
		public DataTable ExecuteDataTableText(string commandText, params DbParameter[] dbParameters)
		{
			return ExecuteDataTable(commandText, CommandType.Text, dbParameters);
		}


		/// <summary>
		/// executes
		/// </summary>
		/// <param name="commandText">Sql text or StoredProcedure Name. </param>
		/// <param name="commandType"><see cref="System.Data.CommandType"/></param>
		/// <param name="dbParameters">List of parameters</param>
		/// <example>
		/// IDatabase db = new Database("connectionString value");
		/// DataTable tbl = db.ExecuteDataTable("select * from users", CommandType.Text, null);
		/// </example>
		/// <returns></returns>
		public DataTable ExecuteDataTable(string commandText, CommandType commandType, params DbParameter[] dbParameters)
		{
			return Execute<DataTable>(commandText, commandType, dbParameters, false,
									  (command) =>
									  {
										  var reader = command.ExecuteReader();
										  var table = new DataTable();
										  table.Load(reader);
										  return table;
									  });
		}


		/// <summary>
		/// A template method to execute any command action.
		/// This is made virtual so that it can be extended to easily include Performance profiling.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="commandText">Sql text or StoredProcedure Name. </param>
		/// <param name="commandType"><see cref="System.Data.CommandType"/></param>
		/// <param name="dbParameters">List of parameters</param>
		/// <param name="useTransaction"></param>
		/// <param name="executor"></param>
		/// <example>
		/// IDatabase db = new Database("connectionString value");
		/// int result = db.Execute(int)("Users_GrantAccessToAllUsers", CommandType.StoredProcedure, 
		///                               null, delegate(DbCommand cmd) { cmd.ExecuteNonQuery(); } );
		/// </example>
		/// <returns></returns>
		private TResult Execute<TResult>(string commandText, CommandType commandType, DbParameter[] dbParameters, bool useTransaction, Func<DbCommand, TResult> executor)
		{
			TResult result;
			var connection = GetConnection();
			try
			{
				using (connection)
				{
					var command = connection.CreateCommand();
					var transaction = useTransaction ? connection.BeginTransaction() : null;
					command.Connection = connection;
					command.CommandType = commandType;
					command.CommandText = commandText;
					command.Transaction = transaction;
					if (dbParameters != null && dbParameters.Length > 0)
						command.Parameters.AddRange(dbParameters);

					result = executor(command);
					if (useTransaction)
						transaction.Commit();
				}
			}
			finally
			{
				connection.Close();
			}

			return result;
		}


		/// <summary>
		/// Executes sql against the database.
		/// </summary>
		/// <param name="sql">The sql to execute.</param>
		public void ExecuteSql(string sql)
		{
			var connection = GetConnection();
			try
			{
				using (connection)
				{
					var command = connection.CreateCommand();
					command.Connection = connection;
					command.CommandType = CommandType.Text;
					command.CommandText = sql;
					command.ExecuteNonQuery();
				}
			}
			finally
			{
				connection.Close();
			}
		}


		/// <summary>
		/// Gets a new open sql connection from the connection string.
		/// </summary>
		/// <returns>An open sql connection.</returns>
		private SqlConnection GetConnection()
		{
			var connection = new SqlConnection(_connectionString);
			connection.Open();
			return connection;
		}
	}
}