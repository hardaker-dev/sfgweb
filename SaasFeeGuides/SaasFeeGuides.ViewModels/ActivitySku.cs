namespace SaasFeeGuides.ViewModels
{
    public class ActivitySku
    {
        public string ActivityName { get; set; }
        public string Name { get; set; }
        public Content[] TitleContent { get; set; }
        public Content[] DescriptionContent { get; set; }
        public double PricePerPerson { get;  set; }
        public int MinPersons { get; set; }
        public int MaxPersons { get; set; }
        public Content[] AdditionalRequirementsContent { get; set; }
        public double DurationDays { get;  set; }
        public double DurationHours { get; set; }
        public Content[] WebContent { get; set; }
    }
}