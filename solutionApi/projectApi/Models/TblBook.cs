using System;
using System.Collections.Generic;

#nullable disable

namespace projectApi.Models
{
    public partial class TblBook
    {
        public int BookId { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
