namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableCustomer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 45),
                        Password = c.String(nullable: false, maxLength: 100),
                        first_name = c.String(nullable: false, maxLength: 45),
                        last_name = c.String(nullable: false, maxLength: 45),
                        phone_number = c.String(nullable: false, maxLength: 15),
                        addressLine1 = c.String(nullable: false, maxLength: 64),
                        addressLine2 = c.String(maxLength: 64),
                        city = c.String(nullable: false, maxLength: 45),
                        state = c.String(nullable: false, maxLength: 45),
                        postal_code = c.String(nullable: false, maxLength: 10),
                        verification_code = c.String(maxLength: 64),
                        Enabled = c.Boolean(nullable: false),
                        created_time = c.DateTime(nullable: false),
                        CountryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .Index(t => t.CountryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "CountryId", "dbo.Countries");
            DropIndex("dbo.Customers", new[] { "CountryId" });
            DropTable("dbo.Customers");
        }
    }
}
