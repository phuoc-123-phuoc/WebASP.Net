namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotThingD : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BrandCategories", newName: "CategoryBrands");
            DropPrimaryKey("dbo.CategoryBrands");
            AddPrimaryKey("dbo.CategoryBrands", new[] { "Category_Id", "Brand_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CategoryBrands");
            AddPrimaryKey("dbo.CategoryBrands", new[] { "Brand_Id", "Category_Id" });
            RenameTable(name: "dbo.CategoryBrands", newName: "BrandCategories");
        }
    }
}
