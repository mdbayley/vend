namespace VendOMatic
{
    public class Denomination
    {
        public decimal Value { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return $"{Count} x {Value:#,##0.00}";
        }
    }
}