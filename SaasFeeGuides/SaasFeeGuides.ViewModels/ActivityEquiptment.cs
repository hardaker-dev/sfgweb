namespace SaasFeeGuides.ViewModels
{
    public class ActivityEquiptment
    {
        public int EquiptmentId { get; set; }
        public string Name { get; set; }
        public Content[] TitleContent { get; set; }
        public double RentalPrice { get; set; }
        public double ReplacementPrice { get; set; }
        public bool CanRent { get; set; }
        public bool GuideOnly { get; set; }
    }
}