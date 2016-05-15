using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility
{
	public class BoolMessage
	{
		public readonly bool Success;
		public readonly string Message;


		/// <summary>
		/// Initialize fields.
		/// </summary>
		/// <param name="success"></param>
		/// <param name="message"></param>
		public BoolMessage(bool success, string message)
		{
			Success = success;
			Message = message;
		}
	}



	public class BoolMessageItem<T>
	{
		public readonly bool Success;
		public readonly string Message;
		public readonly T Item;
		public readonly List<string> Errors;


		/// <summary>
		/// Initialize fields.
		/// </summary>
		/// <param name="success"></param>
		/// <param name="message"></param>
		public BoolMessageItem(bool success, string message, T item)
		{
			Success = success;
			Message = message;
			Item = item;
		}


		/// <summary>
		/// Initialize fields.
		/// </summary>
		/// <param name="success"></param>
		/// <param name="message"></param>
		public BoolMessageItem(bool success, string message, T item, List<string> errors)
		{
			Success = success;
			Message = message;
			Item = item;
			Errors = errors;
		}
	}

	public class BoolMessageItem
	{
		public readonly bool Success;
		public readonly string Message;
		public readonly List<string> Errors;
		public int EntityId { get; set; }

		/// <summary>
		/// Initialize fields.
		/// </summary>
		/// <param name="success"></param>
		/// <param name="message"></param>
		public BoolMessageItem(bool success, string message)
		{
			Success = success;
			Message = message;
		}

		public static BoolMessageItem GetSuccessMessage()
		{
			return new BoolMessageItem(true, null);
		}

		/// <summary>
		/// Initialize fields.
		/// </summary>
		/// <param name="success"></param>
		/// <param name="message"></param>
		public BoolMessageItem(bool success, string message, List<string> errors)
		{
			Success = success;
			Message = message;
			Errors = errors;
		}

		public static JObject ToJObject(BoolMessageItem modelBoolMessage)
		{
			if (modelBoolMessage == null)
				return null;

			var json = JObject.FromObject(modelBoolMessage);
			return json;
		}
	}
}
