using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public class Currency
    {
        [Key]
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(3)]
        public string Symbol { get; set; }

        [Required]
        [StringLength(4)]
        public string Code { get; set; }
        public Currency()
        {

        }
        public Currency(string name, string symbol, string code)
        {
            Name = name;
            Symbol = symbol;
            Code = code;
        }
    }
}