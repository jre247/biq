using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace BrightLine.Common.Utility.SqlServer
{
	public class DataAccess
	{
		public delegate dynamic Processor(IDataReader reader);
		public delegate void Processor<T>(List<T> resultSet);
		private readonly string _connectionString;
		private readonly List<SqlParameter> _parameters;

		/// <summary>
		/// Creates a new data access object using the connection string that maps to the given key
		/// </summary>
		public DataAccess(string key)
		{
			_connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
			_parameters = new List<SqlParameter>();
		}

		#region Public methods

		public dynamic ExecuteDynamic(string commandText, Processor processor)
		{
			dynamic result;
			using (var connection = new SqlConnection(_connectionString))
			using (var command = CreateCommand(commandText, connection, _parameters))
			using (var reader = command.ExecuteReader())
			{
				result = processor(reader);
			}

			return result;
		}

		public void ExecuteReader<T>(string commandText, Processor<T> processor)
			where T : class, new()
		{
			ExecuteReaders<T, object>(commandText, processor, null);
		}

		public void ExecuteReaders<T1, T2>(string commandText,
				Processor<T1> p1, Processor<T2> p2)
			where T1 : class, new()
			where T2 : class, new()
		{
			ExecuteReaders<T1, T2, object>(commandText, p1, p2, null);
		}

		public void ExecuteReaders<T1, T2, T3>(string commandText,
				Processor<T1> p1, Processor<T2> p2, Processor<T3> p3)
			where T1 : class, new()
			where T2 : class, new()
			where T3 : class, new()
		{
			ExecuteReaders<T1, T2, T3, object>(commandText, p1, p2, p3, null);
		}

		public void ExecuteReaders<T1, T2, T3, T4>(string commandText,
				Processor<T1> p1, Processor<T2> p2, Processor<T3> p3, Processor<T4> p4)
			where T1 : class, new()
			where T2 : class, new()
			where T3 : class, new()
			where T4 : class, new()
		{
			ExecuteReaders<T1, T2, T3, T4, object>(commandText, p1, p2, p3, p4, null);
		}

		public void ExecuteReaders<T1, T2, T3, T4, T5>(string commandText,
			Processor<T1> p1, Processor<T2> p2, Processor<T3> p3, Processor<T4> p4, Processor<T5> p5)
			where T1 : class, new()
			where T2 : class, new()
			where T3 : class, new()
			where T4 : class, new()
			where T5 : class, new()
		{
			ExecuteReaders<T1, T2, T3, T4, T5, object>(commandText, p1, p2, p3, p4, p5, null);
		}

		public void ExecuteReaders<T1, T2, T3, T4, T5, T6>(string commandText,
			Processor<T1> p1, Processor<T2> p2, Processor<T3> p3, Processor<T4> p4, Processor<T5> p5,
			Processor<T6> p6)
			where T1 : class, new()
			where T2 : class, new()
			where T3 : class, new()
			where T4 : class, new()
			where T5 : class, new()
			where T6 : class, new()
		{
			ExecuteReaders<T1, T2, T3, T4, T5, T6, object>(commandText, p1, p2, p3, p4, p5, p6, null);
		}

		public void ExecuteReaders<T1, T2, T3, T4, T5, T6, T7>(string commandText,
			Processor<T1> p1, Processor<T2> p2, Processor<T3> p3, Processor<T4> p4, Processor<T5> p5,
			Processor<T6> p6, Processor<T7> p7)
			where T1 : class, new()
			where T2 : class, new()
			where T3 : class, new()
			where T4 : class, new()
			where T5 : class, new()
			where T6 : class, new()
			where T7 : class, new()
		{
			ExecuteReaders<T1, T2, T3, T4, T5, T6, T7, object>(commandText, p1, p2, p3, p4, p5, p6, p7, null);
		}

		public void ExecuteReaders<T1, T2, T3, T4, T5, T6, T7, T8>(string commandText,
			Processor<T1> p1, Processor<T2> p2, Processor<T3> p3, Processor<T4> p4, Processor<T5> p5,
			Processor<T6> p6, Processor<T7> p7, Processor<T8> p8)
			where T1 : class, new()
			where T2 : class, new()
			where T3 : class, new()
			where T4 : class, new()
			where T5 : class, new()
			where T6 : class, new()
			where T7 : class, new()
			where T8 : class, new()
		{
			ExecuteReaders<T1, T2, T3, T4, T5, T6, T7, T8, object>(commandText, p1, p2, p3, p4, p5, p6, p7, p8, null);
		}

		public void ExecuteReaders<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string commandText,
				Processor<T1> p1, Processor<T2> p2, Processor<T3> p3, Processor<T4> p4, Processor<T5> p5,
				Processor<T6> p6, Processor<T7> p7, Processor<T8> p8, Processor<T9> p9)
			where T1 : class, new()
			where T2 : class, new()
			where T3 : class, new()
			where T4 : class, new()
			where T5 : class, new()
			where T6 : class, new()
			where T7 : class, new()
			where T8 : class, new()
			where T9 : class, new()
		{
			ExecuteReaders<T1, T2, T3, T4, T5, T6, T7, T8, T9, object>(commandText, p1, p2, p3, p4, p5, p6, p7, p8, p9, null);
		}

		public void ExecuteReaders<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string commandText,
			Processor<T1> p1, Processor<T2> p2, Processor<T3> p3, Processor<T4> p4, Processor<T5> p5,
			Processor<T6> p6, Processor<T7> p7, Processor<T8> p8, Processor<T9> p9, Processor<T10> p10)
			where T1 : class, new()
			where T2 : class, new()
			where T3 : class, new()
			where T4 : class, new()
			where T5 : class, new()
			where T6 : class, new()
			where T7 : class, new()
			where T8 : class, new()
			where T9 : class, new()
			where T10 : class, new()
		{
			var resultSet = 1;
			using (var connection = new SqlConnection(_connectionString))
			using (var command = CreateCommand(commandText, connection, _parameters))
			using (var reader = command.ExecuteReader())
			{
				ProcessReader(commandText, reader, p1, advanceResult: false, resultSet: resultSet++);
				ProcessReader(commandText, reader, p2, resultSet: resultSet++);
				ProcessReader(commandText, reader, p3, resultSet: resultSet++);
				ProcessReader(commandText, reader, p4, resultSet: resultSet++);
				ProcessReader(commandText, reader, p5, resultSet: resultSet++);
				ProcessReader(commandText, reader, p6, resultSet: resultSet++);
				ProcessReader(commandText, reader, p7, resultSet: resultSet++);
				ProcessReader(commandText, reader, p8, resultSet: resultSet++);
				ProcessReader(commandText, reader, p9, resultSet: resultSet++);
				ProcessReader(commandText, reader, p10, resultSet: resultSet++);
			}
		}

		/// <summary>
		/// Executes the command and returns the first column of the first row in the result set.
		/// </summary>
		/// <param name="commandText">The command to be executed and resulted.</param>
		/// <param name="parameters">An array of parameters to be added to the command.</param>
		/// <returns>
		///		The first column of the first row in the result set, or null
		///		if the result set is empty. Returns a maximum of 2033 characters.
		///	</returns>
		public T ExecuteScalar<T>(string commandText)
		{
			var scalar = default(T);
			using (var connection = new SqlConnection(_connectionString))
			using (var command = CreateCommand(commandText, connection, _parameters))
			{
				var result = command.ExecuteScalar();
				if (result != DBNull.Value)
					scalar = (T)result;
			}

			return scalar;
		}

		/// <summary>
		/// Executes given query and returns the number of rows affected
		/// </summary>
		/// <param name="commandText">The command to be executed and resulted.</param>
		/// <param name="parameters">An array of parameters to be added to the command.</param>
		/// <returns>The number of rows affected.</returns>
		public int ExecuteNonQuery(string commandText)
		{
			var rows = -1;
			using (var connection = new SqlConnection(_connectionString))
			using (var command = CreateCommand(commandText, connection, _parameters))
			{
				rows = command.ExecuteNonQuery();
			}

			return rows;
		}

		public void AddParameter(string name, object value, DbType dbType = DbType.Object)
		{
			if (string.IsNullOrWhiteSpace(name))
				return;

			var parameter = new SqlParameter
			{
				ParameterName = name,
				DbType = dbType,
				Value = GetTypedValue(dbType, value)
			};

			_parameters.Add(parameter);
		}

		#endregion

		#region Private methods

		private static SqlCommand CreateCommand(string commandText, SqlConnection connection, List<SqlParameter> parameters)
		{
			if (string.IsNullOrWhiteSpace(commandText))
				throw new ArgumentNullException("commandText", "commandText cannot be null or empty");

			var command = new SqlCommand(commandText, connection)
			{
				CommandType = CommandType.StoredProcedure,
				CommandTimeout = 30
			};
			command.Parameters.Clear();
			if (parameters.Any())
				command.Parameters.AddRange(parameters.ToArray());

			if (command.Connection.State == ConnectionState.Closed || command.Connection.State == ConnectionState.Broken)
				command.Connection.Open();

			return command;
		}

		private static object GetTypedValue(DbType dbType, object value)
		{
			var typedValue = value ?? DBNull.Value;
			if (value == null || value == DBNull.Value)
				return typedValue;

			switch (dbType)
			{
				case DbType.Byte:
					byte byteValue;
					byte.TryParse(value.ToString(), out byteValue);
					typedValue = byteValue;
					break;
				case DbType.Boolean:
					short bShortValue;
					if (short.TryParse(value.ToString(), out bShortValue))
						typedValue = (bShortValue != 0);
					else
					{
						bool booleanValue;
						if (bool.TryParse(value.ToString(), out booleanValue))
							typedValue = booleanValue;
					}
					break;
				case DbType.Currency:
					decimal currency;
					if (decimal.TryParse(value.ToString(), NumberStyles.AllowCurrencySymbol, null, out currency))
						typedValue = currency;
					break;
				case DbType.Date:
				case DbType.DateTime:
				case DbType.DateTime2:
				case DbType.Time:
					DateTime dateTime;
					if (DateTime.TryParse(value.ToString(), out dateTime))
						typedValue = dateTime;
					break;
				case DbType.Decimal:
					decimal dec;
					if (decimal.TryParse(value.ToString(), out dec))
						typedValue = dec;
					break;
				case DbType.Double:
				case DbType.VarNumeric:
					double doubleValue;
					if (double.TryParse(value.ToString(), out doubleValue))
						typedValue = doubleValue;
					break;
				case DbType.Guid:
					Guid guidValue;
					if (Guid.TryParse(value.ToString(), out guidValue))
						typedValue = guidValue;
					break;
				case DbType.Int16:
					short shortValue;
					if (short.TryParse(value.ToString(), out shortValue))
						typedValue = shortValue;
					break;
				case DbType.Int32:
					int intValue;
					if (int.TryParse(value.ToString(), out intValue))
						typedValue = intValue;
					break;
				case DbType.Int64:
					long longValue;
					if (long.TryParse(value.ToString(), out longValue))
						typedValue = longValue;
					break;
				case DbType.SByte:
					sbyte sbyteValue;
					if (sbyte.TryParse(value.ToString(), out sbyteValue))
						typedValue = sbyteValue;
					break;
				case DbType.Single:
					float singleValue;
					if (float.TryParse(value.ToString(), out singleValue))
						typedValue = singleValue;
					break;
				case DbType.UInt16:
					ushort ushortValue;
					if (ushort.TryParse(value.ToString(), out ushortValue))
						typedValue = ushortValue;
					break;
				case DbType.UInt32:
					uint uintValue;
					if (uint.TryParse(value.ToString(), out uintValue))
						typedValue = uintValue;
					break;
				case DbType.UInt64:
					ulong ulongValue;
					if (ulong.TryParse(value.ToString(), out ulongValue))
						typedValue = ulongValue;
					break;
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.Binary:
				case DbType.Object:
				case DbType.String:
				case DbType.StringFixedLength:
				case DbType.Xml:
					typedValue = value;
					break;
				default:
					typedValue = value;
					break;
			}

			return typedValue;
		}

		private static void ProcessReader<T>(string commandText, SqlDataReader reader, Processor<T> processor,
			bool advanceResult = true, int resultSet = 1) where T : class, new()
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (advanceResult && !reader.NextResult())
				return;
			if (!reader.HasRows || !reader.Read())
				return;
			if (processor == null)
				return;

			var sw = Stopwatch.StartNew();
			var ordinals = new Dictionary<string, int>(reader.FieldCount);
			for (var index = 0; index < reader.FieldCount; index++)
			{
				var columnName = reader.GetName(index).ToLower();
				if (ordinals.ContainsKey(columnName))
					continue;

				ordinals.Add(columnName, index);
			}

			var list = new List<T>();
			var type = typeof(T);
			var pis = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			var fis = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			do // the Read() call above has already advanced the reader to the first row, don't want to skip it.
			{
				var model = new T();
				foreach (var pi in pis)
				{
					var name = pi.Name.ToLower();
					var ordinal = (ordinals.ContainsKey(name)) ? ordinals[name] : -1;
					if (ordinal == -1 || reader.IsDBNull(ordinal))
						continue;

					var value = reader.GetValue(ordinal);
					ReflectionHelper.TrySetValue(model, pi.Name, value);
				}

				foreach (var fi in fis)
				{
					var name = fi.Name.ToLower();
					if (name[0] == '_') // by convention, fields have _ prefix. Ignore it.
						name = name.Substring(1);

					var ordinal = (ordinals.ContainsKey(name)) ? ordinals[name] : -1;
					if (ordinal == -1 || reader.IsDBNull(ordinal))
						continue;

					var value = reader.GetValue(ordinal);
					ReflectionHelper.TrySetValue(model, fi.Name, value);
				}

				list.Add(model);
			} while (reader.Read());

			processor(list);
		}

		#endregion
	}
}