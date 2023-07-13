namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColForCus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "authentication_type", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "authentication_type");
        }
    }
}
