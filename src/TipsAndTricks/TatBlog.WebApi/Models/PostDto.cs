namespace TatBlog.WebApi.Models
{
    public class PostDto
    {
        // ma bai viet
        public int Id { get; set; }
        //tieu de bai viet
        public string Title { get; set; }
        // mo ta hay gioi thieu ngan ve noi dung
        public string ShortDesciption { get; set; }
        //ten dinh danh de tao slug
        public string UrlSlug { get; set; }
        //duong dan tap tin hinh anh
        public string ImageUrl { get; set; }
        //so luot xem,doc bai viet
        public int ViewCount { get; set; }
        //ngay gio dang bai
        public DateTime PostedDate { get; set; }
        //ngay gio cap nhat lan cuoi
        public DateTime? ModifiedDate { get; set; }
        //chuyen muc cua bai viet
        public CategoryDto Category { get; set; }
        //tac gia cua bai viet
        public AuthorDto Author { get; set; }
        //danh sach tu khoa cua bia viet
        public IList<TagDto> Tags { get; set; }

    }
}
