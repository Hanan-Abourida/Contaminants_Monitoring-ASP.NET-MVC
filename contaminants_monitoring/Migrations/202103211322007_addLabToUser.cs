namespace Contaminants_Monitoring.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLabToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LabId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LabId");
        }
    }
}
