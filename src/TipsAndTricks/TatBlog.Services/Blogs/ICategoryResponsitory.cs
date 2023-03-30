using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;

namespace TatBlog.Services.Blogs
{
    public interface  ICategoryResponsitory
    {
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default);
    }
}
