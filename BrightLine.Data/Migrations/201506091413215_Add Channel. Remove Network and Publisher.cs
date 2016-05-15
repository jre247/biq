namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChannelRemoveNetworkandPublisher : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Publisher", "Parent_Id", "dbo.Publisher");
            DropForeignKey("dbo.Publisher", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.Placement", "Network_Id", "dbo.Network");
            DropForeignKey("dbo.Placement", "Publisher_Id", "dbo.Publisher");
            DropForeignKey("dbo.App", "Publisher_Id", "dbo.Publisher");
            DropForeignKey("dbo.PublisherPlatform", "Publisher_Id", "dbo.Publisher");
            DropForeignKey("dbo.PublisherPlatform", "Platform_Id", "dbo.Platform");
            DropIndex("dbo.Publisher", new[] { "Parent_Id" });
            DropIndex("dbo.Publisher", new[] { "Category_Id" });
            DropIndex("dbo.Placement", new[] { "Network_Id" });
            DropIndex("dbo.Placement", new[] { "Publisher_Id" });
            DropIndex("dbo.App", new[] { "Publisher_Id" });
            DropIndex("dbo.PublisherPlatform", new[] { "Publisher_Id" });
            DropIndex("dbo.PublisherPlatform", new[] { "Platform_Id" });
            CreateTable(
                "dbo.Channel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ManifestName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
						Parent_Id = c.Int(),
						Category_Id = c.Int(),
						Publisher_Id = c.Int(),
						Network_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channel", t => t.Parent_Id)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .Index(t => t.Parent_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.ChannelPlatform",
                c => new
                    {
                        Channel_Id = c.Int(nullable: false),
                        Platform_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Channel_Id, t.Platform_Id })
                .ForeignKey("dbo.Channel", t => t.Channel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Platform", t => t.Platform_Id, cascadeDelete: true)
                .Index(t => t.Channel_Id)
                .Index(t => t.Platform_Id);
            
            AddColumn("dbo.Placement", "Channel_Id", c => c.Int());
            AddColumn("dbo.App", "Channel_Id", c => c.Int());
            AddForeignKey("dbo.Placement", "Channel_Id", "dbo.Channel", "Id");
            AddForeignKey("dbo.App", "Channel_Id", "dbo.Channel", "Id");
            CreateIndex("dbo.Placement", "Channel_Id");
            CreateIndex("dbo.App", "Channel_Id");
			// transfer data over into Channel tables
			Sql(@"
insert into [dbo].[Channel] (Name, IsDeleted, DateCreated, Parent_Id, Category_Id, ManifestName, Publisher_Id, Network_Id)
	select Name, IsDeleted, DateCreated, null, Category_Id, null, Id, null
		from [dbo].[Publisher]
		
insert into [dbo].[Channel] (Name, IsDeleted, DateCreated, Parent_Id, Category_Id, ManifestName, Publisher_Id, Network_Id)
	select Name, IsDeleted, DateCreated, null, null, ManifestName, null, Id
		from [dbo].[Network]

;with c as (select Id, Publisher_Id, Parent_Id
	from Channel)
update [dbo].[Channel] set
	Parent_Id = c.Id
	from c
	where Channel.Publisher_Id = c.Parent_Id

insert into [dbo].[ChannelPlatform] (Channel_Id, Platform_Id)
	select distinct c.Id, pp.Platform_Id	
		from [dbo].[Channel] c
		inner join [dbo].PublisherPlatform pp on pp.Publisher_Id = c.Publisher_Id
		
update [dbo].[Placement] set
		Channel_Id = channel.Id
		from [dbo].[Channel] channel
		where Placement.Network_Id = channel.Network_Id
update [dbo].[Placement] set
		Channel_Id = channel.Id
		from [dbo].[Channel] channel
		where Placement.Publisher_Id = channel.Publisher_Id
update [dbo].[App] set
		Channel_Id = channel.Id
		from [dbo].[Channel] channel
		where [App].Publisher_Id = channel.Publisher_Id
");
			DropColumn("dbo.Placement", "Network_Id");
			DropColumn("dbo.Placement", "Publisher_Id");
			//TODO: kill the publisher and network columns, may need to be done manually
			//DropColumn("dbo.Channel", "Publisher_Id");
			//DropColumn("dbo.Channel", "Network_Id");
			DropColumn("dbo.App", "Publisher_Id");
			DropTable("dbo.Publisher");
			DropTable("dbo.Network");
			DropTable("dbo.PublisherPlatform");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PublisherPlatform",
                c => new
                    {
                        Publisher_Id = c.Int(nullable: false),
                        Platform_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Publisher_Id, t.Platform_Id });
            
            CreateTable(
                "dbo.Network",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ManifestName = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Publisher",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Parent_Id = c.Int(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.App", "Publisher_Id", c => c.Int());
            AddColumn("dbo.Placement", "Publisher_Id", c => c.Int());
            AddColumn("dbo.Placement", "Network_Id", c => c.Int());
            DropIndex("dbo.ChannelPlatform", new[] { "Platform_Id" });
            DropIndex("dbo.ChannelPlatform", new[] { "Channel_Id" });
            DropIndex("dbo.App", new[] { "Channel_Id" });
            DropIndex("dbo.Placement", new[] { "Channel_Id" });
            DropIndex("dbo.Channel", new[] { "Category_Id" });
            DropIndex("dbo.Channel", new[] { "Parent_Id" });
            DropForeignKey("dbo.ChannelPlatform", "Platform_Id", "dbo.Platform");
            DropForeignKey("dbo.ChannelPlatform", "Channel_Id", "dbo.Channel");
            DropForeignKey("dbo.App", "Channel_Id", "dbo.Channel");
            DropForeignKey("dbo.Placement", "Channel_Id", "dbo.Channel");
            DropForeignKey("dbo.Channel", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.Channel", "Parent_Id", "dbo.Channel");
            DropColumn("dbo.App", "Channel_Id");
            DropColumn("dbo.Placement", "Channel_Id");
            DropTable("dbo.ChannelPlatform");
            DropTable("dbo.Channel");
            CreateIndex("dbo.PublisherPlatform", "Platform_Id");
            CreateIndex("dbo.PublisherPlatform", "Publisher_Id");
            CreateIndex("dbo.App", "Publisher_Id");
            CreateIndex("dbo.Placement", "Publisher_Id");
            CreateIndex("dbo.Placement", "Network_Id");
            CreateIndex("dbo.Publisher", "Category_Id");
            CreateIndex("dbo.Publisher", "Parent_Id");
            AddForeignKey("dbo.PublisherPlatform", "Platform_Id", "dbo.Platform", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PublisherPlatform", "Publisher_Id", "dbo.Publisher", "Id", cascadeDelete: true);
            AddForeignKey("dbo.App", "Publisher_Id", "dbo.Publisher", "Id");
            AddForeignKey("dbo.Placement", "Publisher_Id", "dbo.Publisher", "Id");
            AddForeignKey("dbo.Placement", "Network_Id", "dbo.Network", "Id");
            AddForeignKey("dbo.Publisher", "Category_Id", "dbo.Category", "Id");
            AddForeignKey("dbo.Publisher", "Parent_Id", "dbo.Publisher", "Id");
        }
    }
}
