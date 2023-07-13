namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableOrderTrack : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.order_track",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Notes = c.String(maxLength: 256),
                        UpdatedTime = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 45),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.order_track", "OrderId", "dbo.Orders");
            DropIndex("dbo.order_track", new[] { "OrderId" });
            DropTable("dbo.order_track");
        }
    }
}
