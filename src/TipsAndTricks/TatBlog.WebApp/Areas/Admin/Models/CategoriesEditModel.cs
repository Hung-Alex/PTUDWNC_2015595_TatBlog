using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class CategoriesEditModel
    {
        public int Id { get; set; }

        [DisplayName("Tên chủ đề")]
        public string Name { get; set; }

        [DisplayName("Slug")]
        [Remote("VerifyCategorySlug", "Categories", "Admin", HttpMethod = "POST", AdditionalFields = "Id")]
        public string UrlSlug { get; set; }

        [DisplayName("Nội dung")]
        public string Description { get; set; }

        [DisplayName("Hiển trên menu")]
        public bool ShowOnMenu { get; set; }


        public IEnumerable<SelectListItem> ShowOnMenuList { get; set; }
        
    }
}
