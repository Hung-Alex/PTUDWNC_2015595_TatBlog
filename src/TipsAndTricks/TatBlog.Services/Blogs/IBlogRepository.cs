using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default);
        Task<IList<Post>> GetPopularArticleAsync(int number, CancellationToken cancellationToken = default);
        Task<bool> IsPostSlugExistedAsync(int postid, string slug, CancellationToken cancellationToken = default);
        Task IncreaseViewCountAsync(int postid, CancellationToken cancellationToken = default);
        Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default);
        Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);
        Task<Tag> FindTagItemByUrlSlugAsync(string slug,CancellationToken cancellationToken = default);
        Task<IList<TagItem>> GetAllTagssAsync(CancellationToken cancellationToken = default);
        Task<bool> RemoveTagById(int id, CancellationToken cancellationToken = default);
        Task<Category> FindCategoryByUrlSlug(string slug, CancellationToken cancellationToken = default);
        //Tìm một chuyên mục theo mã số cho trước
        //parram number => as id
        Task<Category> FindCategoryByNumber(int number, CancellationToken cancellationToken = default);
        //Thêm hoặc cập nhật một chuyên mục/chủ đề
        Task<bool> AddOrUpdateCategory(Category category,CancellationToken cancellationToken = default);
        Task<bool> RemoveCategorybyId(int id, CancellationToken cancellationToken = default);
        // Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa
        Task<bool> IsExistsSlug(string slug, CancellationToken cancellationToken = default);
        //Đếm số lượng bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery.
        Task<int> CountObject_Valid_Condition_InPostQuery(PostQuey query,CancellationToken cancellationToken);
        //s. Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery(kết quả trả về kiểu IPagedList<Post>)
        Task<IPagedList<Post>> FindAndPagination_Valid_Condition_InPostQuery(PostQuey query, IPagingParams pagingParams, CancellationToken cancellationToken);
        /*Tương tự câu trên nhưng yêu cầu trả về kiểu IPagedList<T>. 
         * Trong đó T là kiểu dữ liệu của đối tượng mới được tạo từ đối tượng Post.Hàm này có
        thêm một đầu vào là Func<IQueryable<Post>, IQueryable<T>> mapper
        để ánh xạ các đối tượng Post thành các đối tượng T theo yêu cầu
            */
        Task<IList<Post>> GetPostRandomsAsync(int numPosts, CancellationToken cancellationToken = default);
        Task<IList<Post>> FindAllPostValidCondition(PostQuey query, CancellationToken cancellationToken=default);
        Task<IPagedList<CategoryItem>> Paginationcategory( IPagingParams pagingParams,CancellationToken cancellationToken = default);
        Task<bool> ConvertStatusPublishedAsync(bool published, CancellationToken cancellationToken = default);
        Task<IPagedList<Post>> GetPagedPostsAsync(
        PostQuey condition,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    }
}
