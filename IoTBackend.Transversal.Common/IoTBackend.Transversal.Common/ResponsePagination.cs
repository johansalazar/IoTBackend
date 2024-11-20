using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Transversal.Common
{
    public class ResponsePagination<T> : ResponseGeneric<T>
    {
        public int? PageNumber { get; set; }
        public int? TotalPages { get; set; }
        public int? TotalCount { get; set; }

        public bool? HasPreviousPage => PageNumber > 1;
        public bool? HasNextPage => PageNumber < TotalPages;
    }
}
