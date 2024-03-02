namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingPasswordFeature2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EncryptedPasswords", "IV");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EncryptedPasswords", "IV", c => c.Binary());
        }
    }
}
