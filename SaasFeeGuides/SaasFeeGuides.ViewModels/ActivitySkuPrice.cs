using System;
using System.Collections.Generic;
using System.Text;

namespace SaasFeeGuides.ViewModels
{
    public class ActivitySkuPrice
    {
        public int ActivitySkuId { get; set; }
        public string Name { get; set; }
        public IList<Content> DescriptionContent { get; set; }
        public string DiscountCode { get; set; }
        public double Price { get; set; }
        public int MaxPersons { get; set; }
        public int MinPersons { get; set; }
        public double? DiscountPercentage { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
