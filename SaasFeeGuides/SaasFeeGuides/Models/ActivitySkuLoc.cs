using System.Threading.Tasks;

namespace SaasFeeGuides.Models
{
    public class ActivitySkuLoc
    {
        public int? Id { get; internal set; }
        public string ActivityName { get; internal set; }
        public string Name { get; internal set; }
        public string Title { get; internal set; }
        public string Description { get; internal set; }
        public double PricePerPerson { get; internal set; }
        public int MinPersons { get; internal set; }
        public int MaxPersons { get; internal set; }
        public string AdditionalRequirements { get; internal set; }
        public double DurationDays { get; internal set; }
        public double DurationHours { get; internal set; }
        public string WebContent { get; internal set; }
    }
}