using System;
using System.Collections.Generic;

namespace EmtityDBFirst.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
