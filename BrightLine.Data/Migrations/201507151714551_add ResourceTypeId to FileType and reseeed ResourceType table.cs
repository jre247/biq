namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addResourceTypeIdtoFileTypeandreseeedResourceTypetable : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.Resource", "Extension_Id", c => c.Int());
			AddColumn("dbo.FileType", "ResourceType_Id", c => c.Int());
			CreateIndex("dbo.Resource", "Extension_Id");
			CreateIndex("dbo.FileType", "ResourceType_Id");
			AddForeignKey("dbo.FileType", "ResourceType_Id", "dbo.ResourceType", "Id");
			AddForeignKey("dbo.Resource", "Extension_Id", "dbo.FileType", "Id");
			DropColumn("dbo.Resource", "Extension");

//			Sql(@"
//				--remove everything from FileType
//				delete from filetype
//
//				--remove everything in ResourceType, since it's legacy data
//				delete from resourcetype
//
//				-- Reseed ResourceType back to 0
//				DBCC CHECKIDENT ('resourcetype', RESEED, 0)
//
//				-- Reseed FileType back to 0
//				DBCC CHECKIDENT ('filetype', RESEED, 0)
//
//				--insert Image and Video in ResourceType
//				insert into resourcetype
//				(name, isdeleted, datecreated, display)
//				values
//				('Image', 0, GetDate(), 'Image')
//				insert into resourcetype
//				(name, isdeleted, datecreated, display)
//				values
//				('Video', 0, GetDate(), 'Video')
//
//				--insert 'png' into FileType
//				insert into filetype
//				(name, isdeleted, datecreated, ResourceType_Id)
//				values
//				('jpeg', 0, GetDate(), 1)
//
//				--insert 'png' into FileType
//				insert into filetype
//				(name, isdeleted, datecreated, ResourceType_Id)
//				values
//				('png', 0, GetDate(), 1)
//
//				--insert 'mp4' into FileType
//				insert into filetype
//				(name, isdeleted, datecreated, ResourceType_Id)
//				values
//				('mp4', 0, GetDate(), 2)
//
//				--update FileType 'jpeg' to key to resource type 'Image'
//				update FileType
//				set ResourceType_Id = 1
//				where id = 1
//
//
//				
//				update filetype
//				set ResourceType_Id = 1
//				where id = 2
//			");

			Sql(@"

				
				update filetype
				set ResourceType_Id = 1
				where id = 2
			");
        }
        
        public override void Down()
        {
			AddColumn("dbo.Resource", "Extension", c => c.String());
			DropForeignKey("dbo.Resource", "Extension_Id", "dbo.FileType");
			DropForeignKey("dbo.FileType", "ResourceType_Id", "dbo.ResourceType");
			DropIndex("dbo.FileType", new[] { "ResourceType_Id" });
			DropIndex("dbo.Resource", new[] { "Extension_Id" });
			DropColumn("dbo.FileType", "ResourceType_Id");
			DropColumn("dbo.Resource", "Extension_Id");
        }
    }
}
