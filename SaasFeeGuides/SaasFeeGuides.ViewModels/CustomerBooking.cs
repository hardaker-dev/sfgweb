using System;

namespace SaasFeeGuides.ViewModels
{
    public class CustomerBooking
    {
        public string ActivitySkuName { get; set; }
        public DateTime Date { get; set; }
        public string CustomerEmail { get; set; }
     
        public int NumPersons { get; set; }        
    }
}
