namespace SaasFeeGuides.Models
{
    public class ActivityEquiptment
    {
        public int ActivityId { get; set; }
        public int? ActivitySkuId { get; set; }
        public int EquiptmentId { get; set; }
        public int Count { get; set; } = 1;
        public bool GuideOnly { get; set; }

    }
}