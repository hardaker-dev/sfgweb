using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Helpers
{
    public enum DataError
    {
        UniqueIndexViolation = 2601,
        CannotFindRecord = 50001,
        ResourceConflict = 50002
    }
}
