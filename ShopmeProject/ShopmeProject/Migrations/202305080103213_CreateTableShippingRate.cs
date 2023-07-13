namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableShippingRate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShippingRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        rate = c.Single(nullable: false),
                        days = c.Int(nullable: false),
                        codSupported = c.Boolean(nullable: false),
                        CountryId = c.Int(),
                        state = c.String(nullable: false, maxLength: 45),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .Index(t => t.CountryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShippingRates", "CountryId", "dbo.Countries");
            DropIndex("dbo.ShippingRates", new[] { "CountryId" });
            DropTable("dbo.ShippingRates");
        }
    }
}
