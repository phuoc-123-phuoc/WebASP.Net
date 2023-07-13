namespace ShopmeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Value = c.String(nullable: false, maxLength: 1024),
                        Category = c.String(nullable: false, maxLength: 45),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Settings");
        }
    }
}
