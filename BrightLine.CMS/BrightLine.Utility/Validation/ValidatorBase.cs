using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.Validation
{
    /// <summary>
    /// Used to COLLECT errors during validation. This class should be used when its more convienent to collect 
    /// as many errors in 1 pass as possible. Rather than failing at the first one.
    /// e.g. Use case such as with providing a list of all the errors to end-user so person can fix them 
    /// all at once.
    /// </summary>
	public abstract class ValidatorBase
	{
		protected List<string> _errors = new List<string>();


		/// <summary>
		/// Validate.
		/// </summary>
		/// <returns></returns>
		public virtual bool Validate()
		{
			return false;
		}


		/// <summary>
		/// Whether or not there are any errors
		/// </summary>
		/// <returns></returns>
		public bool HasErrors()
		{
			return _errors != null && _errors.Count > 0;
		}


		/// <summary>
		/// Get all errors.
		/// </summary>
		/// <returns></returns>
		public List<string> GetErrors()
		{
			return _errors;
		}


		/// <summary>
		/// Clear all the errors.
		/// </summary>
		public void ClearErrors()
		{
			if(_errors != null)
				_errors.Clear();
		}

		
		/// <summary>
		/// Collects the errors into the erorr list.
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="message"></param>
		public void CollectError(string prefix, string message)
		{
			var msg = string.IsNullOrEmpty(prefix) ? "" : prefix + " ";
			msg += message;
			_errors.Add(msg);
		}


        /// <summary>
        /// Collects the errors into the erorr list.
        /// </summary>
        /// <param name="success"></param>
        /// <param name="prefix"></param>
        /// <param name="message"></param>
        public void CollectError(bool isError, string prefix, string message)
        {
            if (!isError)
                return;

            var msg = string.IsNullOrEmpty(prefix) ? "" : prefix + " ";
            msg += message;
            _errors.Add(msg);
        }


		/// <summary>
		/// Collects errors from other validator.
		/// </summary>
		/// <param name="validator"></param>
		public void CollectErrors(ValidatorBase validator)
		{
			// Now collect the errors.
			if(!validator.HasErrors())
				return;
			_errors.AddRange(validator.GetErrors());
		}


		protected virtual void Init()
		{
			
		}


		protected BoolMessageItem<T> BuildValidationResult<T>(T item)
		{
		    return BuildValidationResult<T>(item, Environment.NewLine, false);
		}


        protected BoolMessageItem<T> BuildValidationResult<T>(T item, string lineSeparator, bool includeErrorList)
        {
            var message = "";
            foreach (var msg in _errors)
                message += msg + Environment.NewLine;

            var success = _errors.Count == 0;

            if(!includeErrorList)
                return new BoolMessageItem<T>(success, message, item);

            return new BoolMessageItem<T>(success, message, item, _errors);
        }


        protected void Ensure(bool isSuccess, string errorMessage)
        {
            if (isSuccess)
                return;
            throw new ArgumentException(errorMessage);
        }


        protected void ThrowIf(bool error, string errorMessage)
        {
            if (!error)
                return;
            throw new ArgumentException(errorMessage);
        }
	}
}
