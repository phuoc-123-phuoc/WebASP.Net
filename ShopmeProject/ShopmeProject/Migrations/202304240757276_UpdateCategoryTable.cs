namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.categories", "all_parent_ids", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.categories", "all_parent_ids");
        }
    }
}
