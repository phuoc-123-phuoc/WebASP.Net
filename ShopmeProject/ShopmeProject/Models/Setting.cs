using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopmeProject.Models
{
    public class Setting
    {
        [Key]
        [StringLength(128)]
        public string Key { get; set; }

        [Required]
        [StringLength(1024)]
        [AllowHtml]
        public string Value { get; set; }

        [Required]
        [StringLength(45)]
        [EnumDataType(typeof(SettingCategory))]
        public string Category { get; set; }
        public Setting()
        {

        }
        public Setting(string key, string value, SettingCategory category)
        {
            Key = key;
            Value = value;
            Category = category.ToString();
        }

    }
}