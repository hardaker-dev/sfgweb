﻿using System;
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
        public Content[] VideoContentIds { get; set; }
        public Content[] ImageContentIds { get; set; }
        public bool? IsActive { get; set; }
    }

}
