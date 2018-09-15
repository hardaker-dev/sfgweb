using System;
using System.Collections.Generic;
using System.Text;

namespace SaasFeeGuides.ViewModels
{
    public class Activity
    {
        public string Name { get; set; }
        public Content[] TitleContent { get; set; }
        public Content[] DescriptionContent { get; set; }
        public Content MenuImageContentId { get; set; }
        public Content[] VideoContentId { get; set; }
        public Content[] ImageContentId { get; set; }
        public bool? IsActive { get; set; }

        public ActivitySku[] Skus { get; set; }
    }

}
