using System;
using System.Collections.Generic;

namespace attd_v2_spec_flow.Entities
{
    public partial class OrderLine
    {
        public long Id { get; set; }
        public string? ItemName { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public long? OrderId { get; set; }
    }
}
