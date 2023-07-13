namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableCountryState : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45),
                        Code = c.String(nullable: false, maxLength: 45),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45),
                        CountryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .Index(t => t.Name, unique: true)
                .Index(t => t.CountryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropIndex("dbo.States", new[] { "CountryId" });
            DropIndex("dbo.States", new[] { "Name" });
            DropIndex("dbo.Countries", new[] { "Code" });
            DropIndex("dbo.Countries", new[] { "Name" });
            DropTable("dbo.States");
            DropTable("dbo.Countries");
        }
    }
}
