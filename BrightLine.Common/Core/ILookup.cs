using System.Runtime.Serialization;

namespace BrightLine.Core
{
	public interface ILookup
	{
		[DataMember]
		int Id { get; set; }

		[DataMember]
		string Name { get; set; }
	}
}
