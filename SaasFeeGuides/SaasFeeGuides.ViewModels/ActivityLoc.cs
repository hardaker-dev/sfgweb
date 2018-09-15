using System;
using System.Collections.Generic;
using System.Text;

namespace SaasFeeGuides.ViewModels
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

        public ActivitySkuLoc[] Skus { get; set; }
    }

}
