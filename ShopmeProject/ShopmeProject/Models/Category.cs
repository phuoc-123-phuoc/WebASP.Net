using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{

    [Table("categories")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [StringLength(64)]
        [Index(IsUnique = true)]
        public string Alias { get; set; }

        [Required]
        [StringLength(128)]
        public string Image { get; set; }

        [Column("all_parent_ids")]
        [StringLength(256)]
        public string allParentIDs { get; set; }

        public bool Enabled { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Children { get; set; }

        public Category()
        {
            Children = new HashSet<Category>();
        }

        public virtual ICollection<Brand> Brands { get; set; } = new HashSet<Brand>();

        [NotMapped]
        public string getImagePath
        {
            get
            {
                if (Id == 0 || Image == null) return "/Images/image-thumbnail.png";
               

                return "/category-images/" + Id + "/" + Image;
            }
        }
    }
}