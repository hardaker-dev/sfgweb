using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.ViewModels
{
    public class ActivityDateAdmin : ActivityDate
    {
        public double AmountPaid { get; set; }
        public double TotalPrice { get; set; }
    }
}
