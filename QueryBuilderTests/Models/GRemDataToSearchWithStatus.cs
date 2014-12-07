using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryBuilderTests
{
    class GRemDataToSearchWithStatus
    {
        public int? AuthID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string GRemNumber { get; set; }
        public int? Status { get; set; }
        public int StartRows { get; set; }
        public int EndRows { get; set; }
    }
}
