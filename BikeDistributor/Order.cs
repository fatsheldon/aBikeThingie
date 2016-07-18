using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeDistributor
{
    public class Order
    {
        public int Id { get; set; }
        private const decimal TaxRate = .0725m;
        public ICollection<Line> Lines { get; set; }
        public ICollection<Discount> Discounts { get; set; }
        public string Company { get; set; }

        public Order()
        {
            Lines = new List<Line>();
            Discounts = new List<Discount>();
        }
        public Order(string company)
            : this()
        {
            Company = company;
        }


        public void AddLine(Line line)
        {
            Lines.Add(line);
        }

        public void AddDiscount(Discount discount)
        {
            Discounts.Add(discount);
        }

        public string Receipt()
        {
            var totalAmount = 0m;
            var result = new StringBuilder(string.Format("Order Receipt for {0}{1}", Company, Environment.NewLine));
            foreach (var line in Lines)
            {
                var thisAmount = line.Quantity * line.Bike.Price;
                thisAmount = Discounts.Aggregate(thisAmount, (current, discount) => current - discount.GetLineDiscount(line));
                result.AppendLine(string.Format("\t{0} x {1} {2} = {3}", line.Quantity, line.Bike.Brand, line.Bike.Model, thisAmount.ToString("C")));
                totalAmount += thisAmount;
            }
            totalAmount = Discounts.Aggregate(totalAmount, (current, discount) => current - discount.GetTotalDiscount());
            result.AppendLine(string.Format("Sub-Total: {0}", totalAmount.ToString("C")));
            var tax = totalAmount * (decimal)TaxRate;
            result.AppendLine(string.Format("Tax: {0}", tax.ToString("C")));
            result.Append(string.Format("Total: {0}", (totalAmount + tax).ToString("C")));
            return result.ToString();
        }

        public string HtmlReceipt()
        {
            var totalAmount = 0m;
            var result = new StringBuilder(string.Format("<html><body><h1>Order Receipt for {0}</h1>", Company));
            if (Lines.Any())
            {
                result.Append("<ul>");
                foreach (var line in Lines)
                {
                    var thisAmount = line.Quantity * line.Bike.Price;
                    thisAmount = Discounts.Aggregate(thisAmount, (current, discount) => current - discount.GetLineDiscount(line));
                    result.Append(string.Format("<li>{0} x {1} {2} = {3}</li>", line.Quantity, line.Bike.Brand, line.Bike.Model, thisAmount.ToString("C")));
                    totalAmount += thisAmount;
                }
                result.Append("</ul>");
            }
            totalAmount = Discounts.Aggregate(totalAmount, (current, discount) => current - discount.GetTotalDiscount());
            result.Append(string.Format("<h3>Sub-Total: {0}</h3>", totalAmount.ToString("C")));
            var tax = totalAmount * TaxRate;
            result.Append(string.Format("<h3>Tax: {0}</h3>", tax.ToString("C")));
            result.Append(string.Format("<h2>Total: {0}</h2>", (totalAmount + tax).ToString("C")));
            result.Append("</body></html>");
            return result.ToString();
        }

    }
}
