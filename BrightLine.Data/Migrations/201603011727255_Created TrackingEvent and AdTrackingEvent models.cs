namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedTrackingEventandAdTrackingEventmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdTrackingEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrackingUrl = c.String(nullable: false, maxLength: 1028, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Ad_Id = c.Int(),
                        TrackingEvent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ad", t => t.Ad_Id)
                .ForeignKey("dbo.TrackingEvent", t => t.TrackingEvent_Id)
                .Index(t => t.Ad_Id)
                .Index(t => t.TrackingEvent_Id);
            
            CreateTable(
                "dbo.TrackingEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);

			Sql(@"
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('creativeView', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('start', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('firstQuartile', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('midpoint', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('thirdQuartile', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('complete', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('mute', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('unmute', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('pause', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('rewind', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('resume', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('fullscreen', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('expand', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('collapse', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('acceptInvitation', 0, GetDate())
				insert into TrackingEvent (Name, IsDeleted, DateCreated) values ('close', 0, GetDate())
			");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdTrackingEvent", "TrackingEvent_Id", "dbo.TrackingEvent");
            DropForeignKey("dbo.AdTrackingEvent", "Ad_Id", "dbo.Ad");
            DropIndex("dbo.AdTrackingEvent", new[] { "TrackingEvent_Id" });
            DropIndex("dbo.AdTrackingEvent", new[] { "Ad_Id" });
            DropTable("dbo.TrackingEvent");
            DropTable("dbo.AdTrackingEvent");
        }
    }
}
