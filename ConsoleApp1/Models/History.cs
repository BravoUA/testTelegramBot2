using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
     class History
    {
        [Key]
        public int id { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public string? DateParches { get; set; }
        public decimal TotalAmoung { get; set; }
        public double ProductPrice { get; set; }
        public int Totalweight { get; set; }

    }
}
