using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs.Authors
{
    /*
        a. Tạo interface IAuthorRepository và lớp AuthorRepository.
b. Tìm một tác giả theo mã số.
c. Tìm một tác giả theo tên định danh (slug).
d. Lấy và phân trang danh sách tác giả kèm theo số lượng bài viết của tác giả
đó. Kết quả trả về kiểu IPagedList<AuthorItem>.
e. Thêm hoặc cập nhật thông tin một tác giả.
f. Tìm danh sách N tác giả có nhiều bài viết nhất. N là tham số đầu vào.     
     */
    public interface IAuthorRepository
    {
        Task<Author> FindAuthorbyId(int id,CancellationToken cancellationToken = default);
        Task<Author> FindAuthorbyslugs(string urlslug,CancellationToken cancellationToken = default);
        Task<IList<Author>> GetFourPopulationAuthor(CancellationToken cancellationToken=default);

        Task<IList<Author>> Find_N_MostPostByAuthorAsync(int limit,CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateAsync(
        Author author, CancellationToken cancellationToken = default);
        Task<bool> DeleteAuthorAsync(
        int authorId, CancellationToken cancellationToken = default);
        Task<bool> IsAuthorSlugExistedAsync(
        int authorId,
        string slug,
        CancellationToken cancellationToken = default);
        Task<bool> SetImageUrlAsync(
        int authorId, string imageUrl,
        CancellationToken cancellationToken = default);

        Task<IPagedList<Author>> GetPagedAuthorAsync(PostQuey condition,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
        Task<IList<Author>> GetAllAuthorAsync(CancellationToken cancellationToken = default);



        Task<Author> GetAuthorBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

        Task<Author> GetCachedAuthorBySlugAsync(
            string slug, CancellationToken cancellationToken = default);

        Task<Author> GetAuthorByIdAsync(int authorId);

        Task<Author> GetCachedAuthorByIdAsync(int authorId);

        Task<IList<AuthorItem>> GetAuthorsAsync(
            CancellationToken cancellationToken = default);

        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
            Func<IQueryable<Author>, IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);

       

    }
}
