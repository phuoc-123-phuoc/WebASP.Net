using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        [Required]
        [StringLength(150)]
        public string Discription { get; set; }

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();

        [NotMapped]
        public bool isChecked { get; set; }
    }
}