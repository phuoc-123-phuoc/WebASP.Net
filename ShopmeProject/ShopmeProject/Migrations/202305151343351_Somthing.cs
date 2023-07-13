namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Somthing : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "orderTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "orderTime", c => c.DateTime());
        }
    }
}
