namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        first_name = c.String(nullable: false, maxLength: 45),
                        last_name = c.String(nullable: false, maxLength: 45),
                        addressLine1 = c.String(nullable: false, maxLength: 64),
                        addressLine2 = c.String(maxLength: 64),
                        phone_number = c.String(nullable: false, maxLength: 15),
                        city = c.String(nullable: false, maxLength: 45),
                        state = c.String(nullable: false, maxLength: 45),
                        country = c.String(nullable: false, maxLength: 45),
                        postal_code = c.String(nullable: false, maxLength: 10),
                        shippingCost = c.Single(nullable: false),
                        productCost = c.Single(nullable: false),
                        subtotal = c.Single(nullable: false),
                        tax = c.Single(nullable: false),
                        total = c.Single(nullable: false),
                        CustomerId = c.Int(),
                        status = c.String(nullable: false, maxLength: 45),
                        paymentMethod = c.String(nullable: false, maxLength: 45),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(),
                        quantity = c.Int(nullable: false),
                        productCost = c.Single(nullable: false),
                        shippingCost = c.Single(nullable: false),
                        unitPrice = c.Single(nullable: false),
                        subtotal = c.Single(nullable: false),
                        OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
        }
    }
}
