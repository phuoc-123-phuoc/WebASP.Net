using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Alias field is required.")]
        [StringLength(255)]
        public string Alias { get; set; }

        [Required(ErrorMessage = "The ShortDescription field is required.")]
        [StringLength(512)]
        [AllowHtml]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "The FullDescription field is required.")]
        [StringLength(4000)]
        [AllowHtml]
        public string FullDescription { get; set; }

       
        [Display(Name = "Created Time")]
        public DateTime CreatedTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "Updated Time")]
        public DateTime? UpdatedTime { get; set; }

        public bool Enabled { get; set; }

        [Display(Name = "In Stock")]
        public bool InStock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The Cost field must be a positive number.")]
        public float Cost { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The Price field must be a positive number.")]
        public float Price { get; set; }

        [Range(0, 100, ErrorMessage = "The DiscountPercent field must be a percentage between 0 and 100.")]
        [Display(Name = "Discount Percent")]
        public float DiscountPercent { get; set; }

        [Display(Name = "Length (inch):")]
        public float Length { get; set; }

        [Display(Name = "Width (inch):")]
        public float Width { get; set; }
        [Display(Name = "Height (inch):")]
        public float Height { get; set; }
        [Display(Name = "Weight (pounds):")]
        public float Weight { get; set; }

        [Display(Name = "Review Count")]
        public int ReviewCount { get; set; }

        [Display(Name = "Average Rating")]
        public float AverageRating { get; set; }

        //[Required(ErrorMessage = "The MainImage field is required.")]
        public string MainImage { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        public int? BrandId { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; } = new HashSet<ProductImage>();

        public virtual ICollection<ProductDetail> Details { get; set; } = new HashSet<ProductDetail>();

        [NotMapped]
        public bool CustomerCanReview { get; set; }

        [NotMapped]
        public bool ReviewedByCustomer { get; set; }


        [NotMapped]
        public string getImagePath
        {
            get
            {
                if (Id == 0 || MainImage == null) return "/Images/image-thumbnail.png";


                return "/product-images/" + Id + "/" + MainImage;
            }
        }

        [NotMapped]
        public float getDiscountPrice
        {
            get
            {
                if (DiscountPercent > 0)
                {
                    return Price * ((100 - DiscountPercent) / 100);
                }
                return this.Price;
            }
        }

        [NotMapped]
        public string ShortName
        {
            get
            {
                if (Name.Length > 70)
                {
                    return Name.Substring(0, 70) + "...";
                }
                return Name;
            }
        }

        //public Product()
        //{
        //    Images = new HashSet<ProductImage>();
        //    Details = new List<ProductDetail>();
        //}

        //public void AddExtraImage(string imageName)
        //{
        //    this.Images.Add(new ProductImage(imageName, this));
        //}
    }
}