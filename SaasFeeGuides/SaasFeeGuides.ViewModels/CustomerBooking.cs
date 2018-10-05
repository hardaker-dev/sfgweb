using System;

namespace SaasFeeGuides.ViewModels
{
    public class CustomerBooking
    {
        public string ActivitySkuName { get; set; }
        public DateTime DateTime { get; set; }
        public string CustomerEmail { get; set; }
     
        public int NumPersons { get; set; }       
        
        public bool HasPaid { get; set; }
        public bool HasConfirmed { get; set; }
    }
}
