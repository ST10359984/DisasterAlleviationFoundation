namespace DisasterAlleviationFoundation.Models
{
    public class Donations
    {
    }
}
namespace DisasterAlleviationFoundation.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public string DonorName { get; set; }
        public string ResourceType { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTime DateDonated { get; set; }
    }
}