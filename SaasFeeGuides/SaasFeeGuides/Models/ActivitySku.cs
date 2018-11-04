using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaasFeeGuides.Models
{
    public class ActivitySku : IEquatable<ActivitySku>
    {
        public int? Id { get; internal set; }
        public string ActivityName { get; internal set; }
        public string Name { get; internal set; }
        public string TitleContentId { get; internal set; }
        public string DescriptionContentId { get; internal set; }
        public double PricePerPerson { get; internal set; }
        public int MinPersons { get; internal set; }
        public int MaxPersons { get; internal set; }
        public string AdditionalRequirementsContentId { get; internal set; }
        public double DurationDays { get; internal set; }
        public double DurationHours { get; internal set; }
        public string WebContentId { get; internal set; }
        public int ActivityId { get; internal set; }
        public IList<ActivitySkuPrice> PriceOptions { get; internal set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Activity);
        }

        public bool Equals(ActivitySku other)
        {
            return other != null &&
                   EqualityComparer<int?>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}