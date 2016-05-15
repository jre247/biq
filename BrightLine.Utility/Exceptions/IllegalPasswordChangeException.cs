
namespace BrightLine.Common.Framework.Exceptions
{
	public class IllegalPasswordChangeException  : ViewValidationException
	{
		public IllegalPasswordChangeException(string message)
				: base(message)
			{

			}
	}
}
