using ShopmeProject.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(128)]
    [Required]
    [Index(IsUnique = true)]
    public string Email { get; set; }

    [StringLength(100)]
    [Required]
    [DataType(DataType.Password)]
   // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
     //   ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string Password { get; set; }

    [StringLength(45)]
    [Required]
    [Column("first_name")]
    public string FirstName { get; set; }

    [StringLength(45)]
    [Required]
    [Column("last_name")]
    public string LastName { get; set; }

    [StringLength(64)]
    public string Photos { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();

    [NotMapped]
    public string PhotosImagePath
    {
        get
        {
            if (Id == 0 || Photos == null) return "/Images/default-user.jpg";

            return "/user-photos/" + Id + "/" + Photos;
        }
    }

}
