namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTableBrands : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45),
                        Logo = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.BrandCategories",
                c => new
                    {
                        Brand_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Brand_Id, t.Category_Id })
                .ForeignKey("dbo.Brands", t => t.Brand_Id, cascadeDelete: true)
                .ForeignKey("dbo.categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Brand_Id)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BrandCategories", "Category_Id", "dbo.categories");
            DropForeignKey("dbo.BrandCategories", "Brand_Id", "dbo.Brands");
            DropIndex("dbo.BrandCategories", new[] { "Category_Id" });
            DropIndex("dbo.BrandCategories", new[] { "Brand_Id" });
            DropIndex("dbo.Brands", new[] { "Name" });
            DropTable("dbo.BrandCategories");
            DropTable("dbo.Brands");
        }
    }
}
