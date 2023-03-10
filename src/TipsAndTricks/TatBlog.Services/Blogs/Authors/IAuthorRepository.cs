using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
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
        
    }
}
