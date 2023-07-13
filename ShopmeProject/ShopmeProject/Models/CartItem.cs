using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int Quantity { get; set; }

        [NotMapped]
        public float shippingCost { get; set; }

        [NotMapped]
        public float getsubtotal
        {
            get
            {
                if (Quantity > 0)
                {
                    return Product.getDiscountPrice * Quantity;
                }
                return 0;
            }
        }
    }
}