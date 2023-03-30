using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class CategoryResponsitory : ICategoryResponsitory
    {
        private readonly BlogDbContext _context;
        public CategoryResponsitory(BlogDbContext context)
        {
            _context=context;
        }
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().AsNoTracking()
                .Where(x => string.IsNullOrWhiteSpace(name) ? true : x.Name.Contains(name)).
                Select(a => new CategoryItem()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description=a.Description,
                    UrlSlug = a.UrlSlug,
                    ShowOnMenu=a.ShowOnMenu,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
    }
}
