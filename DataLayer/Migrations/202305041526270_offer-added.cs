namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offeradded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        OfferID = c.Int(nullable: false, identity: true),
                        OfferTitle = c.String(),
                        OfferCode = c.String(),
                        OfferPercent = c.Int(nullable: false),
                        OfferDiscount = c.Int(nullable: false),
                        IsOptional = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OfferID);
            
            CreateTable(
                "dbo.OfferReceipts",
                c => new
                    {
                        Offer_OfferID = c.Int(nullable: false),
                        Receipt_ReceiptID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Offer_OfferID, t.Receipt_ReceiptID })
                .ForeignKey("dbo.Offers", t => t.Offer_OfferID, cascadeDelete: true)
                .ForeignKey("dbo.Receipts", t => t.Receipt_ReceiptID, cascadeDelete: true)
                .Index(t => t.Offer_OfferID)
                .Index(t => t.Receipt_ReceiptID);
            
            AddColumn("dbo.Receipts", "Confirm", c => c.Boolean(nullable: false));
            AddColumn("dbo.Prices", "PriceValue", c => c.Int(nullable: false));
            DropColumn("dbo.MiniOrders", "IsConfirmed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MiniOrders", "IsConfirmed", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.OfferReceipts", "Receipt_ReceiptID", "dbo.Receipts");
            DropForeignKey("dbo.OfferReceipts", "Offer_OfferID", "dbo.Offers");
            DropIndex("dbo.OfferReceipts", new[] { "Receipt_ReceiptID" });
            DropIndex("dbo.OfferReceipts", new[] { "Offer_OfferID" });
            DropColumn("dbo.Prices", "PriceValue");
            DropColumn("dbo.Receipts", "Confirm");
            DropTable("dbo.OfferReceipts");
            DropTable("dbo.Offers");
        }
    }
}
