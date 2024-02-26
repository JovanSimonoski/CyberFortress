namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSharedFileViewModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SharedFileReceiverViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SharedFileId = c.Int(nullable: false),
                        SharedFileReceiverId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SharedFileReceiverViewModels");
        }
    }
}
