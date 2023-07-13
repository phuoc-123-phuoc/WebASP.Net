namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcolumn4TableOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "deliverDays", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "orderTime", c => c.DateTime());
            AddColumn("dbo.Orders", "deliverDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "deliverDate");
            DropColumn("dbo.Orders", "orderTime");
            DropColumn("dbo.Orders", "deliverDays");
        }
    }
}
