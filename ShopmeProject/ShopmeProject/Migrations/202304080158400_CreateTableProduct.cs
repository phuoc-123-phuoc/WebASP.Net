namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Alias = c.String(nullable: false, maxLength: 255),
                        ShortDescription = c.String(nullable: false, maxLength: 512),
                        FullDescription = c.String(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(),
                        Enabled = c.Boolean(nullable: false),
                        InStock = c.Boolean(nullable: false),
                        Cost = c.Single(nullable: false),
                        Price = c.Single(nullable: false),
                        DiscountPercent = c.Single(nullable: false),
                        Length = c.Single(nullable: false),
                        Width = c.Single(nullable: false),
                        Height = c.Single(nullable: false),
                        Weight = c.Single(nullable: false),
                        ReviewCount = c.Int(nullable: false),
                        AverageRating = c.Single(nullable: false),
                        MainImage = c.String(nullable: false),
                        CategoryId = c.Int(),
                        BrandId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId)
                .ForeignKey("dbo.categories", t => t.CategoryId)
                .Index(t => t.CategoryId)
                .Index(t => t.BrandId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "CategoryId", "dbo.categories");
            DropForeignKey("dbo.Products", "BrandId", "dbo.Brands");
            DropIndex("dbo.Products", new[] { "BrandId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropTable("dbo.Products");
        }
    }
}
