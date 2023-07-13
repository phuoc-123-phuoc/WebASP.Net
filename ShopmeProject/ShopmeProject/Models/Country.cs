using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(45)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [StringLength(45)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        public virtual ICollection<State> States { get; set; } = new HashSet<State>();
    }
}