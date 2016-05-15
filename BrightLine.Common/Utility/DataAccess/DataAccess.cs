using BrightLine.Utility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace BrightLine.Common.Utility.DataAccess
{
	public class DataAccess : IDisposable
	{
		private const string DefaultStoredProcedureName = "VOID";
		private readonly Database _database;
		private readonly DbCommand _command;

		/// <summary>
		/// Creates a new data access object using the connection string that maps to the given key
		/// </summary>
		public DataAccess(string key)
		{
			DatabaseFactory.ClearDatabaseProviderFactory();
			DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());

			_database = new SqlDatabase(GetConnectionString(key));

			_command = _database.GetStoredProcCommand(DefaultStoredProcedureName);

		}

		/// <summary>
		/// Adds a paramter to the current data access object to be used when an execute method is called
		/// </summary>
		public void AddParameter(string name, DbType dbType, object value)
		{
			_database.AddInParameter(_command, name, dbType, value ?? DBNull.Value);
		}

		/// <summary>
		/// Returns an IDataReader for a given query.  The database connection closes when the reader is closed.
		/// </summary>
		public IDataReader ExecuteReader(string commandText, CommandType commandType = CommandType.StoredProcedure, int commandTimeout = 30)
		{
			try
			{
				_command.CommandText = commandText;
				_command.CommandType = commandType;
				_command.CommandTimeout = commandTimeout;

				return new DataReader(_database.ExecuteReader(_command));
			}
			catch (DataException ex)
			{
				Log.Error(ex);
				throw;
			}
		}

		/// <summary>
		/// Executes given query and returns the number of rows affected
		/// </summary>
		public int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.StoredProcedure, int commandTimeout = 30)
		{
			try
			{
				_command.CommandText = commandText;
				_command.CommandType = commandType;
				_command.CommandTimeout = commandTimeout;


				return _database.ExecuteNonQuery(_command);
			}
			catch (DataException ex)
			{
				Log.Error(ex);
				throw;
			}
		}

		/// <summary>
		/// Executes given query and returns a scalar value
		/// </summary>
		public T ExecuteScalar<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, int commandTimeout = 30)
		{
			try
			{
				_command.CommandText = commandText;
				_command.CommandType = commandType;
				_command.CommandTimeout = commandTimeout;


				return (T)ToSystemValue(_database.ExecuteScalar(_command));
			}
			catch (DataException ex)
			{
				Log.Error(ex);
				throw;
			}
		}

		internal static object ToSystemValue(object dbValue)
		{
			return dbValue == DBNull.Value ? null : dbValue;
		}

		private string GetConnectionString(string key)
		{
			return ConfigurationManager.ConnectionStrings[key].ConnectionString;
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (_command != null)
				_command.Dispose();
		}

		#endregion
	}
}