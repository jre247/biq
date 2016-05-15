namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeValidationTypeminandmaxtominDateTimeandmaxDateTime : DbMigration
    {
        public override void Up()
        {
			Sql(@"
				update ValidationType
				set name = 'minDatetime'
				where id = 12


				update ValidationType
				set name = 'maxDatetime'
				where id = 13
			");
        }
        
        public override void Down()
        {
			Sql(@"
				update ValidationType
				set name = 'min'
				where id = 12


				update ValidationType
				set name = 'max'
				where id = 13
			");
        }
    }
}
