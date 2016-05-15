namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddisplayFieldNameanddisplayFieldTypetoCmsModelDefinition : DbMigration
    {
        public override void Up()
        {

			AddColumn("dbo.CmsModelDefinition", "DisplayFieldName", c => c.String());
			AddColumn("dbo.CmsModelDefinition", "DisplayFieldType_Id", c => c.Int());
			CreateIndex("dbo.CmsModelDefinition", "DisplayFieldType_Id");
			AddForeignKey("dbo.CmsModelDefinition", "DisplayFieldType_Id", "dbo.FieldType", "Id");
          
        }
        
        public override void Down()
        {

			DropForeignKey("dbo.CmsModelDefinition", "DisplayFieldType_Id", "dbo.FieldType");
			DropIndex("dbo.CmsModelDefinition", new[] { "DisplayFieldType_Id" });
			DropColumn("dbo.CmsModelDefinition", "DisplayFieldType_Id");
			DropColumn("dbo.CmsModelDefinition", "DisplayFieldName");

        }
    }
}
