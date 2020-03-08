namespace personal_site_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentsToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(nullable: false),
                        EntryId = c.Int(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entries", t => t.EntryId, cascadeDelete: true)
                .Index(t => t.EntryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "EntryId", "dbo.Entries");
            DropIndex("dbo.Comments", new[] { "EntryId" });
            DropTable("dbo.Comments");
        }
    }
}
