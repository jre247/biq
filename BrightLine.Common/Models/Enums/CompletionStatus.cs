using System.ComponentModel;

namespace BrightLine.Common.Models.Enums
{
	public enum CompletionStatus
	{
		[Description("Not started")]
		NotStarted = 0,
		[Description("In progress")]
		InProgress = 1,
		Open = 2,
		Complete = 3,
		Overdue = 4,
		Deactivated = 5,
		Incomplete = 6,
		Error = 7,
		Created = 8
	}
}