namespace SaasFeeGuides.ViewModels
{
    public class Equiptment
    {
        public string Name { get; set; }
        public Content[] TitleContent { get; set; }
        public double RentalPrice { get; set; }
        public double ReplacementPrice { get; set; }
        public bool CanRent { get; set; }
    }
}