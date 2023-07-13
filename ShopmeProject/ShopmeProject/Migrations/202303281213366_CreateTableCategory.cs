namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        Alias = c.String(nullable: false, maxLength: 64),
                        Image = c.String(nullable: false, maxLength: 128),
                        Enabled = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.categories", t => t.ParentId)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Alias, unique: true)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.categories", "ParentId", "dbo.categories");
            DropIndex("dbo.categories", new[] { "ParentId" });
            DropIndex("dbo.categories", new[] { "Alias" });
            DropIndex("dbo.categories", new[] { "Name" });
            DropTable("dbo.categories");
        }
    }
}
