using System;
using System.Collections.Generic;

namespace EmtityDBFirst.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? ProductCount { get; set; }
    }
}
