namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNightwatchTransactionmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NightwatchTransaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        TransactionId = c.Guid(nullable: false),
                        Success = c.Boolean(nullable: false),
                        DateCompleted = c.DateTime(),
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
            DropTable("dbo.NightwatchTransaction");
        }
    }
}
