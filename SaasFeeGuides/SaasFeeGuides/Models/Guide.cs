using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Models
{
    public class Guide
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }

    }

    public class GuideComparer : IEqualityComparer<Guide>
    {
        public bool Equals(Guide x, Guide y)
        {
            return x.Email.Equals(y.Email, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Guide obj)
        {
            return obj.Email.GetHashCode();
        }
    }
}
