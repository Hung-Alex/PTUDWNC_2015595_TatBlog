using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
        {
            if (category.Id > 0)
                _context.Update(category);
            else
                _context.Add(category);

            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> DeleteCategoryByIdAsync(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null || _context.Categories == null)
            {
                Console.WriteLine("Không có chuyên mục nào");
                return await Task.FromResult(false);
            }

            var category = await _context.Set<Category>().FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);

                Console.WriteLine($"Đã xóa chuyên mục với id {id}");
            }

            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> CheckCategorySlugExisted(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }

        public Task<IPagedList<T>> GetCategoryByQueyAsync(CategoryQuery query, IPagingParams model, Func<IQueryable<Category>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

      
        public  async Task<CategoryItem> GetCategorysByIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var categroyQuery= await _context
                .Set<Category>()
                .AsNoTracking()
                .Where(x=>x.Id==categoryId)
                .Select(x=>new CategoryItem()
                { 
                    Id=x.Id,
                    Name=x.Name,
                    UrlSlug=x.UrlSlug,
                    Description=x.Description,
                    ShowOnMenu=x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                }
                ).SingleOrDefaultAsync();
            return categroyQuery;
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
