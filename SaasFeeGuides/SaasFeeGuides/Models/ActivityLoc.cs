using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Models
{
    public class ActivityLoc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MenuImage { get; set; }
        public string Videos { get; set; }
        public string Images { get; set; }

        public IList<ActivitySkuLoc> Skus { get; set; }
        public IList<EquiptmentLoc> Equiptment { get; internal set; }
    }
}
