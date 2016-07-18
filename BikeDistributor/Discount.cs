using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BikeDistributor
{
    public abstract class Discount : IDiscount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; } 

        public virtual decimal GetLineDiscount(Line line)
        {
            return 0m;
        }

        public virtual decimal GetTotalDiscount()
        {
            return 0m;
        }
    }

    public interface IDiscount
    {
        string Name { get; }
        decimal GetLineDiscount(Line line);
        decimal GetTotalDiscount();
    }

    public class DiscountThreshold
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }

    public class QuantityDiscount : Discount
    {
        public ICollection<DiscountThreshold> DiscountThresholds { get; set; }

        public QuantityDiscount()
        {
            DiscountThresholds = new List<DiscountThreshold>();
        }

        public override decimal GetLineDiscount(Line line)
        {
            var d = DiscountThresholds
                .Where(dt => line.Bike.Price >= dt.Price && line.Quantity >= dt.Quantity)
                .OrderByDescending(dt => dt.Price)
                .ToList();
            return d.Any() ? d.First().Discount * line.Quantity * line.Bike.Price : 0m;
        }

        public void AddThreshold(DiscountThreshold discountThreshold)
        {
            DiscountThresholds.Add(discountThreshold);
        }
    }

    public class TotalCoupon : Discount
    {
        public decimal DiscountAmount { get; set; }

        public override decimal GetTotalDiscount()
        {
            return DiscountAmount;
        }
    }
}