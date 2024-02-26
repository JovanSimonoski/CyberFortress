namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSharedFilesModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SharedFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SharedFileName = c.String(nullable: false),
                        SharedFilePath = c.String(nullable: false),
                        SenderUserId = c.String(nullable: false),
                        SenderUserName = c.String(),
                        ReceiverUserId = c.String(nullable: false),
                        ReceiverUserName = c.String(),
                        SharedFileSize = c.Int(nullable: false),
                        SharedFileEncryptionKey = c.Binary(),
                        SharedFileIV = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SharedFiles");
        }
    }
}
