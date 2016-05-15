namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Settingcreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Type = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Display = c.String(),
                        ShortDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);

			Sql(@"
				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('CacheEnabled', 'True', 'Bool', 0, GetDate())

				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('CacheDuration', '1440', 'Integer', 0, GetDate())
			
				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('MaxPasswordAttemptCount', '5', 'Integer', 0, GetDate())

				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('AccountLockOutMinutes', '5', 'Integer', 0, GetDate())
				
				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('AccountRequestExpirationDayCount', '7', 'Integer', 0, GetDate())

				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('InvitationExpirationDayCount', '7', 'Integer', 0, GetDate())

				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('EmailFromName', 'BrightLine iQ', 'String', 0, GetDate())

				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('EmailFromAddress', 'iq-support@brightline.tv', 'String', 0, GetDate())

				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('PasswordExpirationDayCount', '90', 'Integer', 0, GetDate())

				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('PasswordChangeHourWindow', '24', 'Integer', 0, GetDate())

				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('PasswordChangeLimit', '4', 'Integer', 0, GetDate())

				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('PasswordHashHistoryLimit', '12', 'Integer', 0, GetDate())

				insert into Setting([Key], [Value], [Type], IsDeleted, DateCreated)
				Values
				('DownloadApi', '/api/download/', 'String', 0, GetDate())
			");
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Setting");
        }
    }
}
