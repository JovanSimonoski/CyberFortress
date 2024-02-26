namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeSomeChanges : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SharedFileReceiverViewModels", "SharedFileReceiverId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SharedFileReceiverViewModels", "SharedFileReceiverId", c => c.String());
        }
    }
}
