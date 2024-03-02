namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingPasswordFeature1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EncryptedPasswords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Website = c.String(),
                        Username = c.String(),
                        EncryptedData = c.String(),
                        UserId = c.String(),
                        IV = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EncryptedPasswords");
        }
    }
}
