namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIVtoStoredPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoredPasswords", "IV", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoredPasswords", "IV");
        }
    }
}
