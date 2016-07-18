using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BikeDistributor.Test
{
    [TestClass]
    public class OrderTest
    {
        private readonly static Bike Defy = new Bike { Brand = "Giant", Model = "Defy 1", Price = 1000 };
        private readonly static Bike Elite = new Bike { Brand = "Specialized", Model = "Venge Elite", Price = 2000 };
        private readonly static Bike DuraAce = new Bike { Brand = "Specialized", Model = "S-Works Venge Dura-Ace", Price = 5000 };
        private readonly static Bike XCut = new Bike { Brand = "Diamond", Model = "X-Cut", Price = 5950.00m };

        private static readonly Discount QuantityDiscount = new QuantityDiscount
        {
            Name = "quantity discount",
            DiscountThresholds = new List<DiscountThreshold>
            {
                new DiscountThreshold {Discount = .1m, Price = 1000, Quantity = 20},
                new DiscountThreshold {Discount = .2m, Price = 2000, Quantity = 10},
                new DiscountThreshold {Discount = .2m, Price = 5000, Quantity = 5}
            }
        };

        private static readonly TotalCoupon BirthdayCoupon = new TotalCoupon
        {
            Name = "happy birthday",
            DiscountAmount = 5m
        };

        [TestMethod]
        public void ReceiptFiveXCut()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line {Bike = XCut, Quantity = 5});
            order.AddDiscount(QuantityDiscount);
            var orderReceipt = order.Receipt();
            Assert.AreEqual(ResultStatementFiveXCut, orderReceipt);
        }

        private const string ResultStatementFiveXCut = @"Order Receipt for Anywhere Bike Shop
	5 x Diamond X-Cut = $23,800.00
Sub-Total: $23,800.00
Tax: $1,725.50
Total: $25,525.50";

        [TestMethod]
        public void ReceiptOneDefy()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line {Bike = Defy, Quantity = 1});
            order.AddDiscount(QuantityDiscount);
            Assert.AreEqual(ResultStatementOneDefy, order.Receipt());
        }

        private const string ResultStatementOneDefy = @"Order Receipt for Anywhere Bike Shop
	1 x Giant Defy 1 = $1,000.00
Sub-Total: $1,000.00
Tax: $72.50
Total: $1,072.50";

        [TestMethod]
        public void ReceiptOneDefyHappyBirthday()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line { Bike = Defy, Quantity = 1 });
            order.AddDiscount(BirthdayCoupon);
            var orderReceipt = order.Receipt();
            Assert.AreEqual(ResultStatementOneDefyHappyBirthday, orderReceipt);
        }

        private const string ResultStatementOneDefyHappyBirthday = @"Order Receipt for Anywhere Bike Shop
	1 x Giant Defy 1 = $1,000.00
Sub-Total: $995.00
Tax: $72.14
Total: $1,067.14";

        [TestMethod]
        public void ReceiptTwentyDefyHappyBirthday()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line {Bike = Defy, Quantity = 20});
            order.AddDiscount(QuantityDiscount);
            order.AddDiscount(BirthdayCoupon);
            var orderReceipt = order.Receipt();
            Assert.AreEqual(ResultStatementTwentyDefyHappyBirthday, orderReceipt);
        }

        private const string ResultStatementTwentyDefyHappyBirthday = @"Order Receipt for Anywhere Bike Shop
	20 x Giant Defy 1 = $18,000.00
Sub-Total: $17,995.00
Tax: $1,304.64
Total: $19,299.64";

        [TestMethod]
        public void ReceiptOneElite()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line {Bike = Elite, Quantity = 1});
            Assert.AreEqual(ResultStatementOneElite, order.Receipt());
        }

        private const string ResultStatementOneElite = @"Order Receipt for Anywhere Bike Shop
	1 x Specialized Venge Elite = $2,000.00
Sub-Total: $2,000.00
Tax: $145.00
Total: $2,145.00";

        [TestMethod]
        public void ReceiptOneDuraAce()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line {Bike = DuraAce, Quantity = 1});
            Assert.AreEqual(ResultStatementOneDuraAce, order.Receipt());
        }

        private const string ResultStatementOneDuraAce = @"Order Receipt for Anywhere Bike Shop
	1 x Specialized S-Works Venge Dura-Ace = $5,000.00
Sub-Total: $5,000.00
Tax: $362.50
Total: $5,362.50";

        [TestMethod]
        public void HtmlReceiptOneDefy()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line {Bike = Defy, Quantity = 1});
            Assert.AreEqual(HtmlResultStatementOneDefy, order.HtmlReceipt());
        }

        private const string HtmlResultStatementOneDefy = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Giant Defy 1 = $1,000.00</li></ul><h3>Sub-Total: $1,000.00</h3><h3>Tax: $72.50</h3><h2>Total: $1,072.50</h2></body></html>";

        [TestMethod]
        public void HtmlReceiptOneElite()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line {Bike = Elite, Quantity = 1});
            Assert.AreEqual(HtmlResultStatementOneElite, order.HtmlReceipt());
        }

        private const string HtmlResultStatementOneElite = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Specialized Venge Elite = $2,000.00</li></ul><h3>Sub-Total: $2,000.00</h3><h3>Tax: $145.00</h3><h2>Total: $2,145.00</h2></body></html>";

        [TestMethod]
        public void HtmlReceiptOneDuraAce()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line {Bike = DuraAce, Quantity = 1});
            Assert.AreEqual(HtmlResultStatementOneDuraAce, order.HtmlReceipt());
        }

        private const string HtmlResultStatementOneDuraAce = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Specialized S-Works Venge Dura-Ace = $5,000.00</li></ul><h3>Sub-Total: $5,000.00</h3><h3>Tax: $362.50</h3><h2>Total: $5,362.50</h2></body></html>";
    }
}


