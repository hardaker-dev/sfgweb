using System.Threading.Tasks;

namespace SaasFeeGuides.ViewModels
{
    public class ActivitySkuLoc
    {
        public int Id { get;  set; }
        public string ActivityName { get;  set; }
        public string Name { get;  set; }
        public string Title { get;  set; }
        public string Description { get;  set; }
        public double PricePerPerson { get;  set; }
        public int MinPersons { get;  set; }
        public int MaxPersons { get;  set; }
        public string AdditionalRequirements { get;  set; }
        public double DurationDays { get;  set; }
        public double DurationHours { get;  set; }
        public string WebContent { get;  set; }
    }
}