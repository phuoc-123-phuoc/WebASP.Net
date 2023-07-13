namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAllowNullForMainImage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "MainImage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "MainImage", c => c.String(nullable: false));
        }
    }
}
