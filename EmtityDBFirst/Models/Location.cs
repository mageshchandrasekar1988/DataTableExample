using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmtityDBFirst.Models
{
    public partial class Location
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
    }
}
