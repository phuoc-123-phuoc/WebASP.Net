namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTableUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false, maxLength: 64),
                        first_name = c.String(nullable: false, maxLength: 45),
                        last_name = c.String(nullable: false, maxLength: 45),
                        Photos = c.String(maxLength: 64),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true);
            
            AddColumn("dbo.Roles", "User_Id", c => c.Int());
            CreateIndex("dbo.Roles", "User_Id");
            AddForeignKey("dbo.Roles", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Roles", "User_Id", "dbo.Users");
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Roles", new[] { "User_Id" });
            DropColumn("dbo.Roles", "User_Id");
            DropTable("dbo.Users");
        }
    }
}
