using System.Data.Entity;
using BikeDistributor;

namespace BikeDbContext
{
    public interface IBikeDistributorContext
    {
        DbSet<Bike> Bikes { get; set; }
        DbSet<Discount> Discounts { get; set; }
        DbSet<Order> Orders { get; set; }
    }
    public class BikeDistributorContext : DbContext, IBikeDistributorContext
    {
        public BikeDistributorContext(string nameOrConnectionString):base(nameOrConnectionString)
        {
            Database.SetInitializer<BikeDistributorContext>(null);            
        }
        
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<TotalCoupon> TotalCouponDiscounts { get; set; }
        public DbSet<QuantityDiscount> QuantityDiscounts { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Lines)
                .WithRequired()
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<QuantityDiscount>()
                .HasMany(qd => qd.DiscountThresholds)
                .WithRequired()
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
