using NPOI.SS.Formula.Functions;
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
    public interface  ICategoryResponsitory
    {
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default);
        Task<CategoryItem> GetCategorysByIdAsync(int categoryId, CancellationToken cancellationToken = default);
        Task<IPagedList<T>> GetCategoryByQueyAsync(CategoryQuery query,IPagingParams model,Func<IQueryable<Category>, IQueryable<T>> mapper,CancellationToken cancellationToken=default);

      
        Task<bool> AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);

       
        Task<bool> DeleteCategoryByIdAsync(int? id, CancellationToken cancellationToken = default);

        Task<bool> CheckCategorySlugExisted(int id, string slug, CancellationToken cancellationToken = default);
    }
}
