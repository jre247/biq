namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CmsModelSettingValidationadded : DbMigration
    {
        public override void Up()
        {
			//CreateTable(
			//	"dbo.CmsModelDefinition",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//			Blueprint_Id = c.Int(),
			//		})
			//	.PrimaryKey(t => t.Id)
			//	.ForeignKey("dbo.Blueprint", t => t.Blueprint_Id)
			//	.Index(t => t.Blueprint_Id);
            
			//CreateTable(
			//	"dbo.CmsField",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(),
			//			Description = c.String(),
			//			List = c.Boolean(nullable: false),
			//			DefaultValueString = c.String(),
			//			DefaultValueNumber = c.Int(),
			//			DefaultValueFloat = c.Single(),
			//			DefaultValueBool = c.Boolean(),
			//			DefaultValueDateTime = c.DateTime(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//			Type_Id = c.Int(),
			//			Expose_Id = c.Int(),
			//			CmsRef_Id = c.Int(),
			//			CmsModelDefinition_Id = c.Int(),
			//			CmsSettingDefinition_Id = c.Int(),
			//		})
			//	.PrimaryKey(t => t.Id)
			//	.ForeignKey("dbo.FieldType", t => t.Type_Id)
			//	.ForeignKey("dbo.Expose", t => t.Expose_Id)
			//	.ForeignKey("dbo.CmsRef", t => t.CmsRef_Id)
			//	.ForeignKey("dbo.CmsModelDefinition", t => t.CmsModelDefinition_Id)
			//	.ForeignKey("dbo.CmsSettingDefinition", t => t.CmsSettingDefinition_Id)
			//	.Index(t => t.Type_Id)
			//	.Index(t => t.Expose_Id)
			//	.Index(t => t.CmsRef_Id)
			//	.Index(t => t.CmsModelDefinition_Id)
			//	.Index(t => t.CmsSettingDefinition_Id);
            
			//CreateTable(
			//	"dbo.FieldType",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//		})
			//	.PrimaryKey(t => t.Id);
            
			//CreateTable(
			//	"dbo.Validation",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Value = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//			ValidationType_Id = c.Int(),
			//			FileType_Id = c.Int(),
			//			CmsField_Id = c.Int(),
			//		})
			//	.PrimaryKey(t => t.Id)
			//	.ForeignKey("dbo.ValidationType", t => t.ValidationType_Id)
			//	.ForeignKey("dbo.FileType", t => t.FileType_Id)
			//	.ForeignKey("dbo.CmsField", t => t.CmsField_Id)
			//	.Index(t => t.ValidationType_Id)
			//	.Index(t => t.FileType_Id)
			//	.Index(t => t.CmsField_Id);
            
			//CreateTable(
			//	"dbo.ValidationType",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(),
			//			SystemType = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//		})
			//	.PrimaryKey(t => t.Id);
            
			//CreateTable(
			//	"dbo.FileType",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//		})
			//	.PrimaryKey(t => t.Id);
            
			//CreateTable(
			//	"dbo.Expose",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//		})
			//	.PrimaryKey(t => t.Id);
            
			//CreateTable(
			//	"dbo.CmsRef",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//			CmsRefType_Id = c.Int(),
			//			CmsModelDefinition_Id = c.Int(),
			//			PageDefinition_Id = c.Int(),
			//		})
			//	.PrimaryKey(t => t.Id)
			//	.ForeignKey("dbo.CmsRefType", t => t.CmsRefType_Id)
			//	.ForeignKey("dbo.CmsModelDefinition", t => t.CmsModelDefinition_Id)
			//	.ForeignKey("dbo.PageDefinition", t => t.PageDefinition_Id)
			//	.Index(t => t.CmsRefType_Id)
			//	.Index(t => t.CmsModelDefinition_Id)
			//	.Index(t => t.PageDefinition_Id);
            
			//CreateTable(
			//	"dbo.CmsRefType",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//		})
			//	.PrimaryKey(t => t.Id);
            
			//CreateTable(
			//	"dbo.CmsModel",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//			CmsModelBaseType_Id = c.Int(),
			//			CmsModelDefinition_Id = c.Int(),
			//			Feature_Id = c.Int(),
			//		})
			//	.PrimaryKey(t => t.Id)
			//	.ForeignKey("dbo.CmsModelBaseType", t => t.CmsModelBaseType_Id)
			//	.ForeignKey("dbo.CmsModelDefinition", t => t.CmsModelDefinition_Id)
			//	.ForeignKey("dbo.Feature", t => t.Feature_Id)
			//	.Index(t => t.CmsModelBaseType_Id)
			//	.Index(t => t.CmsModelDefinition_Id)
			//	.Index(t => t.Feature_Id);
            
			//CreateTable(
			//	"dbo.CmsModelBaseType",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(nullable: false, maxLength: 255),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//		})
			//	.PrimaryKey(t => t.Id);
            
			//CreateTable(
			//	"dbo.CmsModelInstance",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//			CmsModel_Id = c.Int(),
			//		})
			//	.PrimaryKey(t => t.Id)
			//	.ForeignKey("dbo.CmsModel", t => t.CmsModel_Id)
			//	.Index(t => t.CmsModel_Id);
            
			//CreateTable(
			//	"dbo.CmsModelInstanceField",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(nullable: false),
			//			IsRequired = c.Boolean(nullable: false),
			//			Metatype = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//			Model_Id = c.Int(),
			//			PropertyType_Id = c.Int(),
			//		})
			//	.PrimaryKey(t => t.Id)
			//	.ForeignKey("dbo.CmsModel", t => t.Model_Id)
			//	.ForeignKey("dbo.CmsModelInstanceFieldType", t => t.PropertyType_Id)
			//	.Index(t => t.Model_Id)
			//	.Index(t => t.PropertyType_Id);
            
			//CreateTable(
			//	"dbo.CmsModelInstanceFieldType",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(nullable: false),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//		})
			//	.PrimaryKey(t => t.Id);
            
			//CreateTable(
			//	"dbo.CmsModelInstanceFieldValue",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			NumberValue = c.Double(),
			//			StringValue = c.String(),
			//			BoolValue = c.Boolean(),
			//			DateValue = c.DateTime(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//			Campaign_Id = c.Int(),
			//			CmsModel_Id = c.Int(),
			//			CmsModelInstanceField_Id = c.Int(),
			//			CmsModelInstanceFieldType_Id = c.Int(),
			//		})
			//	.PrimaryKey(t => t.Id)
			//	.ForeignKey("dbo.Campaign", t => t.Campaign_Id)
			//	.ForeignKey("dbo.CmsModel", t => t.CmsModel_Id)
			//	.ForeignKey("dbo.CmsModelInstanceField", t => t.CmsModelInstanceField_Id)
			//	.ForeignKey("dbo.CmsModelInstanceFieldType", t => t.CmsModelInstanceFieldType_Id)
			//	.Index(t => t.Campaign_Id)
			//	.Index(t => t.CmsModel_Id)
			//	.Index(t => t.CmsModelInstanceField_Id)
			//	.Index(t => t.CmsModelInstanceFieldType_Id);
            
			//CreateTable(
			//	"dbo.CmsSetting",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//			Feature_Id = c.Int(),
			//			CmsSettingDefinition_Id = c.Int(),
			//		})
			//	.PrimaryKey(t => t.Id)
			//	.ForeignKey("dbo.Feature", t => t.Feature_Id)
			//	.ForeignKey("dbo.CmsSettingDefinition", t => t.CmsSettingDefinition_Id)
			//	.Index(t => t.Feature_Id)
			//	.Index(t => t.CmsSettingDefinition_Id);
            
			//CreateTable(
			//	"dbo.CmsSettingInstance",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.Int(nullable: false),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//		})
			//	.PrimaryKey(t => t.Id)
			//	.ForeignKey("dbo.CmsSetting", t => t.Id)
			//	.Index(t => t.Id);
            
			//CreateTable(
			//	"dbo.CmsSettingDefinition",
			//	c => new
			//		{
			//			Id = c.Int(nullable: false, identity: true),
			//			Name = c.String(),
			//			IsDeleted = c.Boolean(nullable: false),
			//			DateCreated = c.DateTime(nullable: false),
			//			DateUpdated = c.DateTime(),
			//			DateDeleted = c.DateTime(),
			//			Display = c.String(),
			//			ShortDisplay = c.String(),
			//		})
			//	.PrimaryKey(t => t.Id);

			

            
        }
        
        public override void Down()
        {
			//DropIndex("dbo.CmsSettingInstance", new[] { "Id" });
			//DropIndex("dbo.CmsSetting", new[] { "CmsSettingDefinition_Id" });
			//DropIndex("dbo.CmsSetting", new[] { "Feature_Id" });
			//DropIndex("dbo.CmsModelInstanceFieldValue", new[] { "CmsModelInstanceFieldType_Id" });
			//DropIndex("dbo.CmsModelInstanceFieldValue", new[] { "CmsModelInstanceField_Id" });
			//DropIndex("dbo.CmsModelInstanceFieldValue", new[] { "CmsModel_Id" });
			//DropIndex("dbo.CmsModelInstanceFieldValue", new[] { "Campaign_Id" });
			//DropIndex("dbo.CmsModelInstanceField", new[] { "PropertyType_Id" });
			//DropIndex("dbo.CmsModelInstanceField", new[] { "Model_Id" });
			//DropIndex("dbo.CmsModelInstance", new[] { "CmsModel_Id" });
			//DropIndex("dbo.CmsModel", new[] { "Feature_Id" });
			//DropIndex("dbo.CmsModel", new[] { "CmsModelDefinition_Id" });
			//DropIndex("dbo.CmsModel", new[] { "CmsModelBaseType_Id" });
			//DropIndex("dbo.CmsRef", new[] { "PageDefinition_Id" });
			//DropIndex("dbo.CmsRef", new[] { "CmsModelDefinition_Id" });
			//DropIndex("dbo.CmsRef", new[] { "CmsRefType_Id" });
			//DropIndex("dbo.Validation", new[] { "CmsField_Id" });
			//DropIndex("dbo.Validation", new[] { "FileType_Id" });
			//DropIndex("dbo.Validation", new[] { "ValidationType_Id" });
			//DropIndex("dbo.CmsField", new[] { "CmsSettingDefinition_Id" });
			//DropIndex("dbo.CmsField", new[] { "CmsModelDefinition_Id" });
			//DropIndex("dbo.CmsField", new[] { "CmsRef_Id" });
			//DropIndex("dbo.CmsField", new[] { "Expose_Id" });
			//DropIndex("dbo.CmsField", new[] { "Type_Id" });
			//DropIndex("dbo.CmsModelDefinition", new[] { "Blueprint_Id" });
			//DropForeignKey("dbo.CmsSettingInstance", "Id", "dbo.CmsSetting");
			//DropForeignKey("dbo.CmsSetting", "CmsSettingDefinition_Id", "dbo.CmsSettingDefinition");
			//DropForeignKey("dbo.CmsSetting", "Feature_Id", "dbo.Feature");
			//DropForeignKey("dbo.CmsModelInstanceFieldValue", "CmsModelInstanceFieldType_Id", "dbo.CmsModelInstanceFieldType");
			//DropForeignKey("dbo.CmsModelInstanceFieldValue", "CmsModelInstanceField_Id", "dbo.CmsModelInstanceField");
			//DropForeignKey("dbo.CmsModelInstanceFieldValue", "CmsModel_Id", "dbo.CmsModel");
			//DropForeignKey("dbo.CmsModelInstanceFieldValue", "Campaign_Id", "dbo.Campaign");
			//DropForeignKey("dbo.CmsModelInstanceField", "PropertyType_Id", "dbo.CmsModelInstanceFieldType");
			//DropForeignKey("dbo.CmsModelInstanceField", "Model_Id", "dbo.CmsModel");
			//DropForeignKey("dbo.CmsModelInstance", "CmsModel_Id", "dbo.CmsModel");
			//DropForeignKey("dbo.CmsModel", "Feature_Id", "dbo.Feature");
			//DropForeignKey("dbo.CmsModel", "CmsModelDefinition_Id", "dbo.CmsModelDefinition");
			//DropForeignKey("dbo.CmsModel", "CmsModelBaseType_Id", "dbo.CmsModelBaseType");
			//DropForeignKey("dbo.CmsRef", "PageDefinition_Id", "dbo.PageDefinition");
			//DropForeignKey("dbo.CmsRef", "CmsModelDefinition_Id", "dbo.CmsModelDefinition");
			//DropForeignKey("dbo.CmsRef", "CmsRefType_Id", "dbo.CmsRefType");
			//DropForeignKey("dbo.Validation", "CmsField_Id", "dbo.CmsField");
			//DropForeignKey("dbo.Validation", "FileType_Id", "dbo.FileType");
			//DropForeignKey("dbo.Validation", "ValidationType_Id", "dbo.ValidationType");
			//DropForeignKey("dbo.CmsField", "CmsSettingDefinition_Id", "dbo.CmsSettingDefinition");
			//DropForeignKey("dbo.CmsField", "CmsModelDefinition_Id", "dbo.CmsModelDefinition");
			//DropForeignKey("dbo.CmsField", "CmsRef_Id", "dbo.CmsRef");
			//DropForeignKey("dbo.CmsField", "Expose_Id", "dbo.Expose");
			//DropForeignKey("dbo.CmsField", "Type_Id", "dbo.FieldType");
			//DropForeignKey("dbo.CmsModelDefinition", "Blueprint_Id", "dbo.Blueprint");
			//DropTable("dbo.CmsSettingDefinition");
			//DropTable("dbo.CmsSettingInstance");
			//DropTable("dbo.CmsSetting");
			//DropTable("dbo.CmsModelInstanceFieldValue");
			//DropTable("dbo.CmsModelInstanceFieldType");
			//DropTable("dbo.CmsModelInstanceField");
			//DropTable("dbo.CmsModelInstance");
			//DropTable("dbo.CmsModelBaseType");
			//DropTable("dbo.CmsModel");
			//DropTable("dbo.CmsRefType");
			//DropTable("dbo.CmsRef");
			//DropTable("dbo.Expose");
			//DropTable("dbo.FileType");
			//DropTable("dbo.ValidationType");
			//DropTable("dbo.Validation");
			//DropTable("dbo.FieldType");
			//DropTable("dbo.CmsField");
			//DropTable("dbo.CmsModelDefinition");
        }
    }
}
