using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class CategoriesFilterModel
    {
        [DisplayName("Từ Khóa")]
        public string Keyword { get; set; }

        [DisplayName("Chủ Đề")]
        public int? CategoryId { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }

    }
}
