using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;

namespace ShopmeProject.Models
{
    public class Order
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

        [StringLength(64)]
        [Required]
        public string addressLine1 { get; set; }

        [StringLength(64)]
        public string addressLine2 { get; set; }

        [StringLength(15)]
        [Required]
        [Column("phone_number")]
        public string phoneNumber { get; set; }

        [StringLength(45)]
        [Required]
        public string city { get; set; }

        [StringLength(45)]
        [Required]
        public string state { get; set; }

        [StringLength(45)]
        [Required]
        public string country { get; set; }

        [StringLength(10)]
        [Required]
        [Column("postal_code")]
        public string postalCode { get; set; }

        [Required]
        public float shippingCost { get; set; }

        [Required]
        public float productCost { get; set; }

        [Required]
        public float subtotal { get; set; }

        [Required]
        public float tax { get; set; }

        [Required]
        public float total { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int? CustomerId { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new HashSet<OrderDetail>();
        public virtual ICollection<OrderTrack> OrderTrack { get; set; } = new HashSet<OrderTrack>();

        [Required]
        [StringLength(45)]
        [EnumDataType(typeof(OrderStatus))]
        public string status { get; set; }

        [Required]
        [StringLength(45)]
        [EnumDataType(typeof(PaymentMethod))]
        public string paymentMethod { get; set; }

        [Required]
        public int deliverDays { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "orderTime")]
        public DateTime orderTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
       // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "deliverDate")]
        public DateTime? deliverDate { get; set; }

        [NotMapped]
        public string getDestination
        {
            get
            {
                string destination = city + ", ";

                if (!string.IsNullOrEmpty(state))
                    destination += state + ", ";


                destination += country;

                return destination;
            }
        }

        [NotMapped]
        public string getShippingAddress
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

                if (!string.IsNullOrEmpty(country))
                    address += ", " + country;

                if (!string.IsNullOrEmpty(postalCode))
                    address += ". Postal Code: " + postalCode;
                if (!string.IsNullOrEmpty(phoneNumber))
                    address += ". Phone Number: " + phoneNumber;

                return address;
            }
        }

        [NotMapped]
        public string RecipientName
        {
            get
            {
                string name = FirstName;
                if (!string.IsNullOrEmpty(LastName))
                    name += " " + LastName;
                return name;
            }
        }

        [NotMapped]
        public string RecipientAddress
        {
            get
            {
                string address = addressLine1;

                if (!string.IsNullOrEmpty(addressLine2))
                    address += ", " + addressLine2;

                if (!string.IsNullOrEmpty(city))
                    address += ", " + city;

                if (!string.IsNullOrEmpty(state))
                    address += ", " + state;

                address += ", " + country;

                if (!string.IsNullOrEmpty(postalCode))
                    address += ". " + postalCode;

                return address;
            }
        }

        [NotMapped]
        public string ProductNames
        {
            get
            {
                StringBuilder productNames = new StringBuilder();
                productNames.Append("<ul>");

                foreach (OrderDetail detail in OrderDetail)
                {
                    productNames.Append("<li>").Append(detail.Product.ShortName).Append("</li>");
                }

                productNames.Append("</ul>");

                return productNames.ToString();
            }
        }

    }
}