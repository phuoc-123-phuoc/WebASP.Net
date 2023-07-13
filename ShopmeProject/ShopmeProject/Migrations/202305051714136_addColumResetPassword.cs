namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumResetPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "reset_password_token", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "reset_password_token");
        }
    }
}
