using System;
using System.Collections.Generic;

namespace attd_v2_spec_flow.Entities
{
    public partial class User
    {
        public long Id { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
    }
}
