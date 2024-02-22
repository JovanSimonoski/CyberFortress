namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedSomeThings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoredFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoredFileName = c.String(nullable: false),
                        StoredFilePath = c.String(nullable: false),
                        UserId = c.String(nullable: false),
                        StoredFileSize = c.Int(nullable: false),
                        StoredFileEncryptionKey = c.Binary(),
                        StoredFileIV = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StoredFiles");
        }
    }
}
