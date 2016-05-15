using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.Validation
{
	public abstract class ModelValidator : ValidatorBase
	{
		/// <summary>
		/// Collect model error.
		/// </summary>
		/// <param name="modelName"></param>
		/// <param name="row"></param>
		/// <param name="columnName"></param>
		/// <param name="message"></param>
		protected void CollectModelRecordError(string modelName, int row, string columnName, string message)
		{
			CollectError("Model : " + modelName, ", Row : " + row + ", Column : " + columnName + " : " + message);
		}


		/// <summary>
		/// Collect model error.
		/// </summary>
		/// <param name="modelName"></param>
		/// <param name="row"></param>
		/// <param name="columnName"></param>
		/// <param name="message"></param>
		protected void CollectModelRecordError(string modelName, int row, object key, string columnName, string message)
		{
			var keyName = key == null ? "" : key.ToString();
			CollectError("Model : " + modelName, ", Row : " + row + ", Key : " + keyName +", Column : " + columnName + " : " + message);
		}


	    /// <summary>
	    /// Collect model error.
	    /// </summary>
	    /// <param name="success"></param>
	    /// <param name="modelName"></param>
	    /// <param name="row"></param>
	    /// <param name="columnName"></param>
	    /// <param name="message"></param>
	    protected void CollectModelRecordError(bool success, string modelName, int row, object key, string columnName, string message)
	    {
	        if (success)
	            return;
            var keyName = key == null ? "" : key.ToString();
            CollectError("Model : " + modelName, ", Row : " + row + ", Key : " + keyName + ", Column : " + columnName + " : " + message);
        }


		/// <summary>
		/// Collect model error.
		/// </summary>
		/// <param name="modelName"></param>
		/// <param name="row"></param>
        /// <param name="column"></param>
		/// <param name="message"></param>
		protected void CollectModelRecordError(string modelName, int row, int column, string message)
		{
			CollectError("Model : " + modelName, ", Row : " + row + ", Column : " + column + " : " + message);
		}
	}
}
