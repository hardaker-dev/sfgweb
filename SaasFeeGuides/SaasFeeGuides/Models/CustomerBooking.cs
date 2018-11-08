using System;

namespace SaasFeeGuides.Models
{
    public class CustomerBooking
    {
        public int? Id { get; set; }
        public int ActivityDateSkuId { get; set; }
        public string ActivitySkuName { get; set; }
        public DateTime DateTime { get; set; }
        public string CustomerEmail { get; set; }
     
        public int NumPersons { get; set; }
        public double PriceAgreed { get; set; }

        public bool HasPaid { get; set; }
        public bool HasConfirmed { get; set; }
        public bool HasCancelled { get; set; }
        public string CustomerDisplayName { get; set; }
        public string CustomerNotes { get; set; }
        public string PriceOptionName { get; internal set; }
    }
}
