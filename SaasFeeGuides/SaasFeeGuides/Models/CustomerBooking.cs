using System;

namespace SaasFeeGuides.Models
{
    public class CustomerBooking
    {
        public int ActivityDateSkuId { get; set; }
        public string ActivitySkuName { get; set; }
        public DateTime DateTime { get; set; }
        public string CustomerEmail { get; set; }
     
        public int NumPersons { get; set; }
        public double PriceAgreed { get; set; }

        public bool HasPaid { get; set; }
        public bool HasConfirmed { get; set; }
        public string Name { get; set; }
    }
}
