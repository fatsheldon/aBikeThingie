using System.Collections.Generic;
using System.Linq;
using BikeDbContext;
using BikeDistributor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbContextSqlite
{
    [TestClass]
    public class SqliteDbcontextTests
    {
        [TestMethod]
        public void AddBike()
        {
            var ctx = new BikeDistributorContext("BikeDb");

            Assert.IsFalse(ctx.Bikes.Any());

            var bike = new Bike { Brand = "Giant", Model = "Defy 1", Price = 1000m };
            ctx.Bikes.Add(bike);
            ctx.SaveChanges();
            Assert.IsTrue(ctx.Bikes.Any());
            Assert.IsTrue(bike.Id > -1);
            ctx.Bikes.Remove(bike);
            ctx.SaveChanges();
            Assert.IsFalse(ctx.Bikes.Any());
            ctx.Dispose();
        }

        [TestMethod]
        public void AddDiscounts()
        {
            var ctx = new BikeDistributorContext("BikeDb");

            var discount = new TotalCoupon{Name = "test coupon", DiscountAmount = 11m};
            ctx.TotalCouponDiscounts.Add(discount);
            ctx.SaveChanges();

            var discount2 = new QuantityDiscount
            {
                DiscountThresholds = new List<DiscountThreshold> {new DiscountThreshold {Discount = .1m, Price = 1000m, Quantity = 10}},
                Name = "wholesale quantity discount"
            };
            ctx.Discounts.Add(discount2);
            ctx.SaveChanges();

            Assert.IsTrue(ctx.Discounts.Count() == 2);

            ctx.Discounts.Remove(discount);
            ctx.Discounts.Remove(discount2);
            ctx.SaveChanges();
            Assert.IsFalse(ctx.Discounts.Any());

            ctx.Dispose();            
        }

        [TestMethod]
        public void AddOrder()
        {
            var ctx = new BikeDistributorContext("BikeDb");

            var bike = new Bike { Brand = "Giant", Model = "Defy 1", Price = 1000m };
            ctx.Bikes.Add(bike);
            var discount = new QuantityDiscount
            {
                DiscountThresholds = new List<DiscountThreshold> { new DiscountThreshold { Discount = .1m, Price = 1000m, Quantity = 10 } },
                Name = "wholesale quantity discount"
            };
            ctx.Discounts.Add(discount);
            ctx.SaveChanges();

            var order = new Order("a company name")
            {
                Discounts = new List<Discount> { discount},
                Lines = new List<Line> { new Line { Bike = bike, Quantity=2} }
            };
            ctx.Orders.Add(order);
            ctx.SaveChanges();

            Assert.IsTrue(ctx.Orders.Count() == 1);

            ctx.Orders.Remove(order);
            ctx.Bikes.Remove(bike);
            ctx.Discounts.Remove(discount);
            ctx.SaveChanges();

            Assert.IsFalse(ctx.Discounts.Any());
            Assert.IsFalse(ctx.Bikes.Any());
            Assert.IsFalse(ctx.Orders.Any());

            ctx.Dispose();
        }
    }
}
