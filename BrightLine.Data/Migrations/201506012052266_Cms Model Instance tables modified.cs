namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CmsModelInstancetablesmodified : DbMigration
    {
        public override void Up()
        {
			DropForeignKey("dbo.CmsModelInstance", "CmsModel_Id", "dbo.CmsModel");
			DropForeignKey("dbo.CmsModelInstanceField", "Model_Id", "dbo.CmsModel");
			DropForeignKey("dbo.CmsModelInstanceFieldValue", "Campaign_Id", "dbo.Campaign");
			DropForeignKey("dbo.CmsModelInstanceFieldValue", "CmsModel_Id", "dbo.CmsModel");
			DropForeignKey("dbo.CmsModelInstanceFieldValue", "CmsModelInstanceFieldType_Id", "dbo.CmsModelInstanceFieldType");
			DropIndex("dbo.CmsModelInstance", new[] { "CmsModel_Id" });
			DropIndex("dbo.CmsModelInstanceField", new[] { "Model_Id" });
			DropIndex("dbo.CmsModelInstanceFieldValue", new[] { "Campaign_Id" });
			DropIndex("dbo.CmsModelInstanceFieldValue", new[] { "CmsModel_Id" });
			DropIndex("dbo.CmsModelInstanceFieldValue", new[] { "CmsModelInstanceFieldType_Id" });
			AddColumn("dbo.CmsModelInstance", "Json", c => c.String());
			AddColumn("dbo.CmsModelInstance", "Model_Id", c => c.Int());
			AddColumn("dbo.CmsModelInstanceField", "ModelInstance_Id", c => c.Int());
			AddForeignKey("dbo.CmsModelInstance", "Model_Id", "dbo.CmsModel", "Id");
			AddForeignKey("dbo.CmsModelInstanceField", "ModelInstance_Id", "dbo.CmsModelInstance", "Id");
			CreateIndex("dbo.CmsModelInstance", "Model_Id");
			CreateIndex("dbo.CmsModelInstanceField", "ModelInstance_Id");
			DropColumn("dbo.CmsModelInstance", "CmsModel_Id");
			DropColumn("dbo.CmsModelInstanceField", "Model_Id");
			DropColumn("dbo.CmsModelInstanceFieldValue", "Campaign_Id");
			DropColumn("dbo.CmsModelInstanceFieldValue", "CmsModel_Id");
			DropColumn("dbo.CmsModelInstanceFieldValue", "CmsModelInstanceFieldType_Id");

			DropForeignKey("dbo.CmsModel", "CmsModelBaseType_Id", "dbo.CmsModelBaseType");
			DropForeignKey("dbo.CmsModelInstanceField", "PropertyType_Id", "dbo.CmsModelInstanceFieldType");
			DropIndex("dbo.CmsModel", new[] { "CmsModelBaseType_Id" });
			DropIndex("dbo.CmsModelInstanceField", new[] { "PropertyType_Id" });
			AddColumn("dbo.CmsModelInstanceField", "FieldType_Id", c => c.Int());
			AddForeignKey("dbo.CmsModelInstanceField", "FieldType_Id", "dbo.FieldType", "Id");
			CreateIndex("dbo.CmsModelInstanceField", "FieldType_Id");
			DropColumn("dbo.CmsModel", "CmsModelBaseType_Id");
			DropColumn("dbo.CmsModelInstanceField", "PropertyType_Id");
			DropTable("dbo.CmsModelBaseType");
			DropTable("dbo.CmsModelInstanceFieldType");
        }
        
        public override void Down()
        {
			AddColumn("dbo.CmsModelInstanceFieldValue", "CmsModelInstanceFieldType_Id", c => c.Int());
			AddColumn("dbo.CmsModelInstanceFieldValue", "CmsModel_Id", c => c.Int());
			AddColumn("dbo.CmsModelInstanceFieldValue", "Campaign_Id", c => c.Int());
			AddColumn("dbo.CmsModelInstanceField", "Model_Id", c => c.Int());
			AddColumn("dbo.CmsModelInstance", "CmsModel_Id", c => c.Int());
			DropIndex("dbo.CmsModelInstanceField", new[] { "ModelInstance_Id" });
			DropIndex("dbo.CmsModelInstance", new[] { "Model_Id" });
			DropForeignKey("dbo.CmsModelInstanceField", "ModelInstance_Id", "dbo.CmsModelInstance");
			DropForeignKey("dbo.CmsModelInstance", "Model_Id", "dbo.CmsModel");
			DropColumn("dbo.CmsModelInstanceField", "ModelInstance_Id");
			DropColumn("dbo.CmsModelInstance", "Model_Id");
			DropColumn("dbo.CmsModelInstance", "Json");
			CreateIndex("dbo.CmsModelInstanceFieldValue", "CmsModelInstanceFieldType_Id");
			CreateIndex("dbo.CmsModelInstanceFieldValue", "CmsModel_Id");
			CreateIndex("dbo.CmsModelInstanceFieldValue", "Campaign_Id");
			CreateIndex("dbo.CmsModelInstanceField", "Model_Id");
			CreateIndex("dbo.CmsModelInstance", "CmsModel_Id");
			AddForeignKey("dbo.CmsModelInstanceFieldValue", "CmsModelInstanceFieldType_Id", "dbo.CmsModelInstanceFieldType", "Id");
			AddForeignKey("dbo.CmsModelInstanceFieldValue", "CmsModel_Id", "dbo.CmsModel", "Id");
			AddForeignKey("dbo.CmsModelInstanceFieldValue", "Campaign_Id", "dbo.Campaign", "Id");
			AddForeignKey("dbo.CmsModelInstanceField", "Model_Id", "dbo.CmsModel", "Id");
			AddForeignKey("dbo.CmsModelInstance", "CmsModel_Id", "dbo.CmsModel", "Id");

			CreateTable(
			   "dbo.CmsModelInstanceFieldType",
			   c => new
			   {
				   Id = c.Int(nullable: false, identity: true),
				   Name = c.String(nullable: false),
				   IsDeleted = c.Boolean(nullable: false),
				   DateCreated = c.DateTime(nullable: false),
				   DateUpdated = c.DateTime(),
				   DateDeleted = c.DateTime(),
				   Display = c.String(),
				   ShortDisplay = c.String(),
			   })
			   .PrimaryKey(t => t.Id);

			CreateTable(
				"dbo.CmsModelBaseType",
				c => new
				{
					Id = c.Int(nullable: false, identity: true),
					Name = c.String(nullable: false, maxLength: 255),
					IsDeleted = c.Boolean(nullable: false),
					DateCreated = c.DateTime(nullable: false),
					DateUpdated = c.DateTime(),
					DateDeleted = c.DateTime(),
					Display = c.String(),
					ShortDisplay = c.String(),
				})
				.PrimaryKey(t => t.Id);

			AddColumn("dbo.CmsModelInstanceField", "PropertyType_Id", c => c.Int());
			AddColumn("dbo.CmsModel", "CmsModelBaseType_Id", c => c.Int());
			DropIndex("dbo.CmsModelInstanceField", new[] { "FieldType_Id" });
			DropForeignKey("dbo.CmsModelInstanceField", "FieldType_Id", "dbo.FieldType");
			DropColumn("dbo.CmsModelInstanceField", "FieldType_Id");
			CreateIndex("dbo.CmsModelInstanceField", "PropertyType_Id");
			CreateIndex("dbo.CmsModel", "CmsModelBaseType_Id");
			AddForeignKey("dbo.CmsModelInstanceField", "PropertyType_Id", "dbo.CmsModelInstanceFieldType", "Id");
			AddForeignKey("dbo.CmsModel", "CmsModelBaseType_Id", "dbo.CmsModelBaseType", "Id");
        }
    }
}
