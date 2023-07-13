namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColunmIscheck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "isChecked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "isChecked");
        }
    }
}
