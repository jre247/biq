namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductLineFeatureTypes : DbMigration
    {
        public override void Up()
        {
		Sql(@"
truncate table [dbo].[ProductLineFeatureType]

insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (2, 10001)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (2, 10004)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (2, 10022)

insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (3, 10001)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (3, 10002)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (3, 10015)

insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (4, 10001)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (4, 10002)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (4, 10015)

insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (5, 10001)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (5, 10020)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (5, 10008)

insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (6, 10004)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (6, 10020)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (6, 10015)

insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (7, 10001)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (7, 10018)
insert into [dbo].[ProductLineFeatureType] (ProductLine_Id, FeatureType_Id) values (7, 10004)
		");
        }
        
        public override void Down()
        {
	        Sql(@"truncate table [dbo].[ProductLineFeatureType]");
        }
    }
}
