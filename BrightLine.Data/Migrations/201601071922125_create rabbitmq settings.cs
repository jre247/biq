namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createrabbitmqsettings : DbMigration
    {
        public override void Up()
        {
			Sql(@"
				insert into Setting ([Key], Value, Type, IsDeleted, DateCreated)
				values
				('RabbitMQUsername', 'writer', 'String', 0, GETDATE())

				insert into Setting ([Key], Value, Type, IsDeleted, DateCreated)
				values
				('RabbitMQQueue', 'generator', 'String', 0, GETDATE())

				insert into Setting ([Key], Value, Type, IsDeleted, DateCreated)
				values
				('RabbitMQExchange', 'publishing', 'String', 0, GETDATE())

				insert into Setting ([Key], Value, Type, IsDeleted, DateCreated)
				values
				('RabbitMQHost', '104.130.192.200', 'String', 0, GETDATE())


				insert into Setting ([Key], Value, Type, IsDeleted, DateCreated)
				values
				('RabbitMQRoutingKey', 'campaigns', 'String', 0, GETDATE())
			");
        }
        
        public override void Down()
        {
			Sql(@"
				delete from Setting
				where [Key] in ('RabbitMQUsername', 'RabbitMQQueue', 'RabbitMQExchange', 'RabbitMQHost', 'RabbitMQRoutingKey')
			");
        }
    }
}
