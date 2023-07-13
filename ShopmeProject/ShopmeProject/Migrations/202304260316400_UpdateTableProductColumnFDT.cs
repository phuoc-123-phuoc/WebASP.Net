namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableProductColumnFDT : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "FullDescription", c => c.String(nullable: false, maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "FullDescription", c => c.String(nullable: false));
        }
    }
}
