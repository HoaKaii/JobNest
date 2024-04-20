namespace JobsFinder_Main.IdentityMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Status", c => c.Boolean());
        }
    }
}
