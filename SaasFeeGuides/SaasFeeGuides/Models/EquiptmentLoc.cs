using System.Threading.Tasks;

namespace SaasFeeGuides.Models
{
    public class EquiptmentLoc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public double RentalPrice { get; set; }
        public bool CanRent { get; set; }
    }
}