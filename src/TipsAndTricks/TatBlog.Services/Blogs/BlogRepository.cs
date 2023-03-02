﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using System.Linq.Dynamic.Core;
using TatBlog.Core.DTO;
using TatBlog.Core.Contracts;
using TatBlog.Services.Extensions;
using System.Security.Cryptography.X509Certificates;

namespace TatBlog.Services.Blogs
{

    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;

        }

       

        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            return await categories.OrderByDescending(x => x.Name)
                                  .Select(x => new CategoryItem()
                                  {
                                      Id = x.Id,
                                      Name = x.Name,
                                      UrlSlug = x.UrlSlug,
                                      Description = x.Description,
                                      ShowOnMenu = x.ShowOnMenu,
                                      PostCount = x.Posts.Count(p => p.Published)
                                  }).ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
           
                var tagQuery = _context.Set<Tag>()
                                          .Select(x => new TagItem()
                                          {
                                              Id = x.Id,
                                              Name = x.Name,
                                              UrlSlug = x.UrlSlug,
                                              Description = x.Description,
                                              PostCount = x.Posts.Count(p => p.Published)
                                          });
                return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
            
        }

        public async Task<IList<Post>> GetPopularArticleAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                             .Include(x => x.Author)
                             .Include(x => x.Category)
                             .OrderByDescending(p => p.ViewCount)
                             .Take(numPosts)
                             .ToListAsync(cancellationToken);
        }

        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postQuery = _context.Set<Post>().Include(x => x.Category).Include(x => x.Author);
            if (year > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Year == year);
            }
            if (month > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Month == month);
            }
            if (!string.IsNullOrWhiteSpace(slug))
            {
                postQuery = postQuery.Where(x => x.UrlSlug == slug);
            }
            return await postQuery.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<Tag> FindTagItemByUrlSlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tagQuery = _context.Set<Tag>().Where(x => x.UrlSlug==slug);
            return await tagQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task IncreaseViewCountAsync(int postid, CancellationToken cancellationToken = default)
        {
           
            await _context.Set<Post>().Where(x => x.Id == postid).ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        public async Task<bool> IsPostSlugExistedAsync(int postid, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>().AnyAsync(x => x.Id != postid && x.UrlSlug == slug, cancellationToken);
        }

       

        public async Task<IList<TagItem>> GetAllTagssAsync(CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                                        .Select(x => new TagItem()
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            UrlSlug = x.UrlSlug,
                                            Description = x.Description,
                                            PostCount = x.Posts.Count(p => p.Published)
                                        });
            return await tagQuery.ToListAsync(cancellationToken);

        }
        //Xóa một thẻ theo mã cho trước
        public async Task<bool> RemoveTagById(int id, CancellationToken cancellationToken = default)
        {
            var tagitem = _context.Set<Tag>().Where(x => x.Id == id).ExecuteDelete();
           
            return tagitem>0;
        }
        //Tìm một chuyên mục (Category) theo tên định danh (slug)
        public async Task<Category> FindCategoryByUrlSlug(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().Where(cate => cate.UrlSlug == slug).SingleOrDefaultAsync(cancellationToken);
        }
    }
}
