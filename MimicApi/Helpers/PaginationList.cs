using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MimicApi.Helpers
{
    public class PaginationList<T> : List<T>
    {
        public Pagination Pagination { get; set; }
    }
}
