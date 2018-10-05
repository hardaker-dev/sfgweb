using System;

namespace SaasFeeGuides.ViewModels
{
    public class HistoricCustomerBooking
    {
        public string ActivitySkuName { get; set; }
        public DateTime DateTime { get; set; }
        public string CustomerEmail { get; set; }
     
        public int NumPersons { get; set; }
        public double AmountPaid { get; set; }
        
    }
}
