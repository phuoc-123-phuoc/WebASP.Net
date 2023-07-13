using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int? ProductId { get; set; }

        [Required]
        public int quantity { get; set; }

        [Required]
        public float productCost { get; set; }

        [Required]
        public float shippingCost { get; set; }

        [Required]
        public float unitPrice { get; set; }

        [Required]
        public float subtotal { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public int? OrderId { get; set; }
    }
}