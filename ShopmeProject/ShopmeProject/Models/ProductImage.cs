using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [NotMapped]
        public string getImagePath
        {
            get
            {
                if (Id == 0 || Name == null) return "/Images/image-thumbnail.png";


                return "/product-images/" + ProductId + "/extras/" + Name;
            }
        }


    }
}