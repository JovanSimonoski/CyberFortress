namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSharedFileViewModelFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SharedFileReceiverViewModels", "SharedFileReceiverUsername", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SharedFileReceiverViewModels", "SharedFileReceiverUsername");
        }
    }
}
