namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createcontractratetyperatecardandupdatechannel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contract",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsCurrent = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Channel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channel", t => t.Channel_Id)
                .Index(t => t.Channel_Id);
            
            CreateTable(
                "dbo.RateCard",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MinImpressionCount = c.Int(),
                        MaxImpressionCount = c.Int(),
                        Rate = c.Single(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                        Contract_Id = c.Int(),
                        RateType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contract", t => t.Contract_Id)
                .ForeignKey("dbo.RateType", t => t.RateType_Id)
                .Index(t => t.Contract_Id)
                .Index(t => t.RateType_Id);
            
            CreateTable(
                "dbo.RateType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RateCard", "RateType_Id", "dbo.RateType");
            DropForeignKey("dbo.RateCard", "Contract_Id", "dbo.Contract");
            DropForeignKey("dbo.Contract", "Channel_Id", "dbo.Channel");
            DropIndex("dbo.RateCard", new[] { "RateType_Id" });
            DropIndex("dbo.RateCard", new[] { "Contract_Id" });
            DropIndex("dbo.Contract", new[] { "Channel_Id" });
            DropTable("dbo.RateType");
            DropTable("dbo.RateCard");
            DropTable("dbo.Contract");
        }
    }
}
