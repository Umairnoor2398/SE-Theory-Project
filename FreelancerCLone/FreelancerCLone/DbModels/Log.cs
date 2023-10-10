using System;
using System.Collections.Generic;

namespace FreelancerCLone.DbModels
{
    public partial class Log
    {
        public int Id { get; set; }
        public string Level { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Class { get; set; } = null!;
        public string Function { get; set; } = null!;
        public DateTime AddedOn { get; set; }
    }
}
