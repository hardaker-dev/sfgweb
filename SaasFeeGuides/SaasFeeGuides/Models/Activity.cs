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
        public Task<string> TitleContentId { get; set; }
        public Task<string> DescriptionContentId { get; set; }
        public Task<string> MenuImageContentId { get; set; }
        public Task<string> VideoContentIds { get; set; }
        public Task<string> ImageContentIds { get; set; }
        public bool? IsActive { get; set; }
    }
}
