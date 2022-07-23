namespace Contaminants_Monitoring.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCenterToDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Center", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Center");
        }
    }
}
