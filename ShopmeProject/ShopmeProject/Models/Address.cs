using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public int? CountryId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int? CustomerId { get; set; }

        [Column("default_address")]
        public bool defaultForShipping { get; set; }

        [NotMapped]
        public string getAddress
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