using BrightLine.Common.Utility;
using BrightLine.Core;
using System.Runtime.Serialization;

namespace BrightLine.Common.Core
{
	[DataContract]
	public class EntityLookup : ILookup
	{
		public readonly static ILookup Select = new EntityLookup(int.MinValue, "Select...");
		public readonly static ILookup NotApplicable = new EntityLookup(-1, "N/A");

		public EntityLookup() :
			this(0, null)
		{ }

		public EntityLookup(int id, string name)
		{
			Id = id;
			Name = name;
		}

		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }

		public override string ToString()
		{
			return Name;
		}

		public static ILookup ToLookup<TEntity>(TEntity entity, string property = null) where TEntity : class, IEntity
		{
			if (entity == null)
				return null;

			var name = (ReflectionHelper.TryGetValue(entity, property ?? "Name") ?? "").ToString();
			var lookup = new EntityLookup(entity.Id, name);
			return lookup;
		}
	}
}