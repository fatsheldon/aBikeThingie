using System.Collections.Generic;
using System.Linq;

namespace BikeDistributor
{
    public abstract class Discount : IDiscount
    {
        public string Name { get; private set; }

        protected Discount(string name)
        {
            Name = name;
        }

        public virtual double GetLineDiscount(Line line)
        {
            return 0d;
        }

        public virtual double GetTotalDiscount()
        {
            return 0d;
        }
    }

    public interface IDiscount
    {
        string Name { get; }
        double GetLineDiscount(Line line);
        double GetTotalDiscount();
    }

    public class DiscountThreshold
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
    }

    public class QuantityDiscount : Discount
    {
        private readonly IEnumerable<DiscountThreshold> _discountThresholds; 
        public QuantityDiscount(string name, IEnumerable<DiscountThreshold> discountThresholds) : base(name)
        {
            _discountThresholds = discountThresholds;
        }

        public override double GetLineDiscount(Line line)
        {
            var d = _discountThresholds
                .Where(dt => line.Bike.Price >= dt.Price && line.Quantity >= dt.Quantity)
                .OrderByDescending(dt => dt.Price)
                .ToList();
            return d.Any() ? d.First().Discount * line.Quantity * line.Bike.Price : 0d;
        }
    }

    public class TotalCoupon : Discount
    {
        private readonly double _discountAmount;
        public TotalCoupon(string name, double discountAmount) : base(name)
        {
            _discountAmount = discountAmount;
        }

        public override double GetTotalDiscount()
        {
            return _discountAmount;
        }
    }
}