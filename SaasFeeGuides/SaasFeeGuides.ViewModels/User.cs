using System;
using System.Collections.Generic;
using System.Text;

namespace SaasFeeGuides.ViewModels
{
    public class User
    {
        public string AuthToken { get; set; }
        public int ExpiresIn { get; set; }
        public Customer Customer { get; set; }
    }
}
