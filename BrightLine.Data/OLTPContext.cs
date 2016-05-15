using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using BrightLine.Common.Models;

namespace BrightLine.Data
{
	public partial class OLTPContext : DbContext
	{
		public OLTPContext()
			: base(ConfigurationManager.ConnectionStrings["OLTP"].ConnectionString)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<OLTPContext, Migrations.Configuration>());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<Ad>()
				   .HasMany<Feature>(s => s.Features)
				   .WithMany(c => c.Ads)
				   .Map(cs =>
				   {
					   cs.MapLeftKey("Ad_Id");
					   cs.MapRightKey("Feature_Id");
					   cs.ToTable("AdFeature");
				   });


			modelBuilder.Entity<FeatureCategory>()
				   .HasMany<FeatureType>(s => s.FeatureTypes)
				   .WithMany(c => c.FeatureCategories)
				   .Map(cs =>
				   {
					   cs.MapLeftKey("FeatureCategory_Id");
					   cs.MapRightKey("FeatureType_Id");
					   cs.ToTable("FeatureCategoryFeatureType");
				   });

			modelBuilder.Entity<Blueprint>()
				   .HasMany<FeatureCategory>(s => s.FeatureCategories)
				   .WithMany(c => c.Blueprints)
				   .Map(cs =>
				   {
					   cs.MapLeftKey("Blueprint_Id");
					   cs.MapRightKey("FeatureCategory_Id");
					   cs.ToTable("BlueprintFeatureCategory");
				   });

			modelBuilder.Entity<Ad>()
				  .HasOptional(e => e.CompanionAd)
				  .WithMany()
				  .HasForeignKey(m => m.CompanionAd_Id);
				  
			


			base.OnModelCreating(modelBuilder);
		}

		protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
		{
			var entity = entityEntry.Entity;
			if (entity != null)
			{
				var createdProperty = entity.GetType().GetProperty("DateCreated");
				if (createdProperty != null && createdProperty.PropertyType == typeof(DateTime))
				{
					if ((DateTime)createdProperty.GetValue(entity) == DateTime.MinValue)
						createdProperty.SetValue(entity, DateTime.UtcNow);
				}
			}

			return base.ValidateEntity(entityEntry, items);
		}
	}
}
