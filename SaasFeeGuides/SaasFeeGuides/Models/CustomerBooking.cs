using System;

namespace SaasFeeGuides.Models
{
    public class CustomerBooking
    {
        public string ActivitySkuName { get; set; }
        public DateTime Date { get; set; }
        public string CustomerEmail { get; set; }
     
        public int NumPersons { get; set; }
        public double PriceAgreed { get; set; }

        public bool HasPaid { get; set; }
        public bool HasConfirmed { get; set; }
        
    }
}
