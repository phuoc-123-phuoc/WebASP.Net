namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFieldCreateTime1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "CreatedTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "CreatedTime", c => c.DateTime(nullable: false));
        }
    }
}
