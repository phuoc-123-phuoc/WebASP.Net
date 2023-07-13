using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [StringLength(45)]
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

        [StringLength(15)]
        [Required]
        [Column("phone_number")]
        public string phoneNumber { get; set; }

        [StringLength(64)]
        [Required]
        public string addressLine1 { get; set; }

        [StringLength(64)]
        public string addressLine2 { get; set; }

        [StringLength(45)]
        [Required]
        public string city { get; set; }

        [StringLength(45)]
        [Required]
        public string state { get; set; }

        [StringLength(10)]
        [Required]
        [Column("postal_code")]
        public string postalCode { get; set; }

        [StringLength(64)]
        [Column("verification_code")]
        public string verificationCode { get; set; }

        public bool Enabled { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "Created Time")]
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public int? CountryId { get; set; }

        [Required]
        [StringLength(15)]
        [EnumDataType(typeof(AuthenticationType))]
        [Column("authentication_type")]
        public string authenticationType { get; set; }

        [StringLength(30)]
        [Column("reset_password_token")]
        public string resetPasswordToken { get; set; }

        [NotMapped]
        public string FullName { get {

                return FirstName + " " + LastName;
            
            } }

        [NotMapped]
        public string Address
        {
            get
            {
                string address = FirstName;

                if (!string.IsNullOrEmpty(LastName))
                    address += " " + LastName;

                if (!string.IsNullOrEmpty(addressLine1))
                    address += ", " + addressLine1;

                if (!string.IsNullOrEmpty(addressLine2))
                    address += ", " + addressLine2;

                if (!string.IsNullOrEmpty(city))
                    address += ", " + city;

                if (!string.IsNullOrEmpty(state))
                    address += ", " + state;

                address += ", " + Country.Name;

                if (!string.IsNullOrEmpty(postalCode))
                    address += ". Postal Code: " + postalCode;
                if (!string.IsNullOrEmpty(phoneNumber))
                    address += ". Phone Number: " + phoneNumber;

                return address;
            }
        }

    }


}