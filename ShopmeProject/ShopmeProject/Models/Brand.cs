using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(45)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Logo is required")]
        [StringLength(128)]
        public string Logo { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();


        public string getLogoPath
        {
            get
            {
                if (Id == 0 || Logo == null) return "/Images/image-thumbnail.png";


                return "/brand-logos/" + Id + "/" + Logo;
            }
        }
    }
}