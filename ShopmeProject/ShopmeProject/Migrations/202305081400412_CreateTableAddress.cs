namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        first_name = c.String(nullable: false, maxLength: 45),
                        last_name = c.String(nullable: false, maxLength: 45),
                        phone_number = c.String(nullable: false, maxLength: 15),
                        addressLine1 = c.String(nullable: false, maxLength: 64),
                        addressLine2 = c.String(maxLength: 64),
                        city = c.String(nullable: false, maxLength: 45),
                        state = c.String(nullable: false, maxLength: 45),
                        postal_code = c.String(nullable: false, maxLength: 10),
                        CountryId = c.Int(),
                        CustomerId = c.Int(),
                        default_address = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CountryId)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Addresses", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Addresses", "CountryId", "dbo.Countries");
            DropIndex("dbo.Addresses", new[] { "CustomerId" });
            DropIndex("dbo.Addresses", new[] { "CountryId" });
            DropTable("dbo.Addresses");
        }
    }
}
