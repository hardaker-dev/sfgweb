using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Models
{
    public class ActivityDate
    {
        public int ActivitySkuDateId { get; set; }
        public int ActivityId { get; set; }
        public int ActivitySkuId { get; set; }
        public string ActivityName { get; set; }
        public string ActivitySkuName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int NumPersons { get; set; }
        public double AmountPaid { get; set; }
        public double TotalPrice { get; set; }
    }
}
