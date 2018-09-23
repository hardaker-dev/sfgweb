using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Models
{
    public class Activity
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string TitleContentId { get; set; }
        public string DescriptionContentId { get; set; }
        public string MenuImageContentId { get; set; }
        public string VideoContentId { get; set; }
        public string ImageContentId { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; }


        public IList<ActivitySku> Skus { get; set; }
        public IList<ActivityEquiptment> Equiptment { get; set; }
    }
}
