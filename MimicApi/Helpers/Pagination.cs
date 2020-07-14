using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicApi.Helpers
{
    public class Pagination
    {
        public int Page { get; set; }
        public int Quantity { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
