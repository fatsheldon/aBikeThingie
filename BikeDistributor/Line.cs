namespace BikeDistributor
{
    public class Line
    {
        public int Id { get; set; }
        public virtual Bike Bike { get; set; }
        public int Quantity { get; set; }
    }
}
