namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColunmIscheck1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Roles", "isChecked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Roles", "isChecked", c => c.Boolean(nullable: false));
        }
    }
}
