namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEncryptionFunctionalities : DbMigration
    {
        public override void Up()
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
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StoredPasswords");
        }
    }
}
