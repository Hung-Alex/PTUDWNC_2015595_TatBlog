namespace TatBlog.WebApi.Models
{
    public class PostDetail
    {
        //ma bai viet
        public int Id { get; set; }
        // tieu de bai viet 
        public string Title { get; set; }
        //mo ta hay gioi thieu ngan ve noi dung
        public string ShortDesciption { get; set; }
        //noi dung chi tiet cua bai viet
        public string Description { get; set; }
        //metadata
        public string Meta { get; set; }
        //ten dinh danh de tao URL
        public string UrlSlug { get; set; }
        //duong dan den tap tin hinh anh
        public string ImageUrl { get; set; }
        //so luot xem ,doc bai viet
        public int ViewCount { get; set; }
        //ngay gio dang bai
        public DateTime PostedDate { get; set; }
        //ngay gio cap nhap lan cuoi
        public DateTime ? ModifiedDate { get; set; }
        //chuyen muc cua bai viet
        public CategoryDto Category { get; set; }
        //tac gia cua bai viet
        public AuthorDto Author { get; set; }
        //danh sach cac tu khoa cua bai viet
        public IList<TagDto> Tags { get; set; }

    }
}
