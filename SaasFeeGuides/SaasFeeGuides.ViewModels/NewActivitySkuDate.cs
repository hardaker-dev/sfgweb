using System;
using System.Collections.Generic;

namespace SaasFeeGuides.ViewModels
{
    public class NewActivitySkuDate
    {
        public string ActivityName { get; set; }
        public string ActivitySkuName { get; set; }
        public DateTime DateTime { get; set; }
        public IList<CustomerBooking> CustomerBookings { get; set; }
    }
}