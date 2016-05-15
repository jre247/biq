using BrightLine.Common.Models;
using BrightLine.Common.Utility.Authentication;

namespace BrightLine.Data.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<BrightLine.Data.OLTPContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(BrightLine.Data.OLTPContext context)
		{
		}
	}
}
