using System.Collections.Generic;

namespace SaasFeeGuides.ViewModels
{
    public class ActivitySku
    {
        public string ActivityName { get; set; }
        public string Name { get; set; }
        public IList<Content> TitleContent { get; set; }
        public IList<Content> DescriptionContent { get; set; }
        public double PricePerPerson { get;  set; }
        public int MinPersons { get; set; }
        public int MaxPersons { get; set; }
        public IList<Content> AdditionalRequirementsContent { get; set; }
        public double DurationDays { get;  set; }
        public double DurationHours { get; set; }
        public IList<Content> WebContent { get; set; }
        public int ActivityId { get; set; }
        public int Id { get; set; }
    }
}