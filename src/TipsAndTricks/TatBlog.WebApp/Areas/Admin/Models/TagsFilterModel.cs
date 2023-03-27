using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class TagsFilterModel
    {
        [DisplayName("Từ Khóa")]
        public string Keyword { get; set; }

        [DisplayName("Thẻ")]
        public int? TagId { get; set; }

        public IEnumerable<SelectListItem> TagList { get; set; }
    }
}
