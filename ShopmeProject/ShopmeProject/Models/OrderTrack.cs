using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopmeProject.Models
{
    [Table("order_track")]
    public class OrderTrack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        [StringLength(256)]
        public string Notes { get; set; }

        public DateTime UpdatedTime { get; set; }

        [EnumDataType(typeof(OrderStatus))]
        [StringLength(45)]
        [Required]
        public string Status { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [NotMapped]
        public string UpdatedTimeOnForm
        {
            get
            {
                return UpdatedTime.ToString("yyyy-MM-dd'T'HH:mm:ss");
            }
            set
            {
                UpdatedTime = DateTime.ParseExact(value, "yyyy-MM-dd'T'HH:mm:ss", null);
            }
        }
    }
}