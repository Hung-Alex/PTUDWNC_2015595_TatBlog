namespace TatBlog.WebApi.Models
{
    public class PostFilterModel
    {
        public string Keyword { get; set; }
        public bool ?PublishedOnly { get; set; } = true;
        public bool?NotPublished { get; set; } = false;
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
    }
}
