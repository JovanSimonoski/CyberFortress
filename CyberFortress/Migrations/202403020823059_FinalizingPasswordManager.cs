namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalizingPasswordManager : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.StoredPasswords");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StoredPasswords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Website = c.String(),
                        Username = c.String(),
                        EncryptedPassword = c.String(),
                        UserId = c.String(),
                        IV = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
