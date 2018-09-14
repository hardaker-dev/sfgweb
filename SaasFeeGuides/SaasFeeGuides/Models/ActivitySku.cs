using System.Threading.Tasks;

namespace SaasFeeGuides.Models
{
    public class ActivitySku
    {
        public int? Id { get; internal set; }
        public string ActivityName { get; internal set; }
        public string Name { get; internal set; }
        public Task<string> TitleContentId { get; internal set; }
        public Task<string> DescriptionContentId { get; internal set; }
        public double PricePerPerson { get; internal set; }
        public int MinPersons { get; internal set; }
        public int MaxPersons { get; internal set; }
        public Task<string> AdditionalRequirementsContentId { get; internal set; }
        public double DurationDays { get; internal set; }
        public double DurationHours { get; internal set; }
        public Task<string> WebContentId { get; internal set; }
    }
}