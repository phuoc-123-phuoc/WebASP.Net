namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableProductDetails : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductDetails", "Name", c => c.String());
            AlterColumn("dbo.ProductDetails", "Value", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductDetails", "Value", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ProductDetails", "Name", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
