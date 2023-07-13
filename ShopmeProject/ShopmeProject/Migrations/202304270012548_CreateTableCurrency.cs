namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableCurrency : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 64),
                        Symbol = c.String(nullable: false, maxLength: 3),
                        Code = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Currencies");
        }
    }
}
