namespace CyberFortress.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSoftwareDownload1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Softwares",
                c => new
                    {
                        SoftwareId = c.Int(nullable: false, identity: true),
                        SoftwareName = c.String(),
                        SoftwareDescription = c.String(),
                        SoftwareImage = c.String(),
                    })
                .PrimaryKey(t => t.SoftwareId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Softwares");
        }
    }
}
