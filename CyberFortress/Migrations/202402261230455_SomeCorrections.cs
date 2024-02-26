namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeCorrections : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SharedFiles", "ReceiverUserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SharedFiles", "ReceiverUserId", c => c.String(nullable: false));
        }
    }
}
