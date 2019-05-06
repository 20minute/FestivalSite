namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Festivaliers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        Prenom = c.String(),
                        Naissance = c.DateTime(nullable: false),
                        Email = c.String(),
                        Mdp = c.String(),
                        Genre = c.String(),
                        Telephone = c.String(),
                        CodePostal = c.String(),
                        Ville = c.String(),
                        Rue = c.String(),
                        Pays = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Festivaliers");
        }
    }
}
