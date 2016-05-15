namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduniqueconstrainttonamecolumninCmslookuptables : DbMigration
    {
        public override void Up()
        {
			Sql("ALTER TABLE dbo.CmsRefType ADD CONSTRAINT UC_CmsRefType_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.Expose ADD CONSTRAINT UC_Expose_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.FileType ADD CONSTRAINT UC_FileType_Name UNIQUE (Name)");
			Sql("ALTER TABLE dbo.FieldType ADD CONSTRAINT UC_FieldType_Name UNIQUE (Name)");

			//Change ValidationType Name to be smaller size so that we can create unique constraint on it
			Sql("alter table dbo.ValidationType alter column Name nvarchar(255) null");

			Sql("ALTER TABLE dbo.ValidationType ADD CONSTRAINT UC_ValidationType_Name UNIQUE (Name)");
        }
        
        public override void Down()
        {
			Sql("ALTER TABLE dbo.CmsRefType DROP CONSTRAINT UC_CmsRefType_Name");
			Sql("ALTER TABLE dbo.Expose DROP CONSTRAINT UC_Expose_Name");
			Sql("ALTER TABLE dbo.FileType DROP CONSTRAINT UC_FileType_Name");
			Sql("ALTER TABLE dbo.FieldType DROP CONSTRAINT UC_FieldType_Name");
			Sql("ALTER TABLE dbo.ValidationType DROP CONSTRAINT UC_ValidationType_Name");
        }
    }
}
