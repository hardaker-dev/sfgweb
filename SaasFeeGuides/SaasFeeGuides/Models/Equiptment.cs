using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Models
{
    public class Equiptment
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public Task<string> TitleContentId { get; set; }
        public double RentalPrice { get; set; }
        public double ReplacementPrice { get; set; }
        public bool CanRent { get; set; }
    }
}
