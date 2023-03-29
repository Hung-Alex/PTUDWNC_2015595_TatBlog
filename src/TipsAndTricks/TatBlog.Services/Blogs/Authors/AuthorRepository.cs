using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs.Authors;
using TatBlog.Services.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Dynamic.Core;


namespace TatBlog.Services.Blogs.Authors
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public AuthorRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public async Task<Author> FindAuthorbyId(int id, CancellationToken cancellationToken = default)
        {
            var authorQuery = _context.Set<Author>().FindAsync(id, cancellationToken);
            return authorQuery.Result;
        }

        public Task<Author> FindAuthorbyslugs(string urlslug, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Author>> GetFourPopulationAuthor(CancellationToken cancellationToken = default)
        {
            var authorList=new List<Author>();
            var groupQuery = from s in _context.Authors
                             join d in _context.Posts on s.Id equals d.AuthorId
                             select s;
            var dataGroup = groupQuery.GroupBy(x => x.Id).Select(x => new {
                authorId = x.Key,
                count = x.Count(),
                value = x.Select(x => new Author
                {
                    Id = x.Id
                ,
                    FullName = x.FullName
                ,
                    UrlSlug = x.UrlSlug
                ,
                    ImageURL = x.ImageURL
                ,
                    JoinedDate = x.JoinedDate
                ,
                    Email = x.Email
                ,
                    Notes = x.Notes
                }).FirstOrDefault()

            }).OrderBy(x => x.authorId);
            foreach (var item in dataGroup)
            {
                authorList.Add(item.value);
            }
            return authorList.Take(4).ToList();

        }

        public async Task<bool> AddOrUpdateAsync(
        Author author, CancellationToken cancellationToken = default)
        {
            if (author.Id > 0)
            {
                _context.Authors.Update(author);
                
            }
            else
            {
                _context.Authors.Add(author);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        public async Task<bool> DeleteAuthorAsync(
        int authorId, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Where(x => x.Id == authorId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<bool> IsAuthorSlugExistedAsync(
        int authorId,
        string slug,
        CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .AnyAsync(x => x.Id != authorId && x.UrlSlug == slug, cancellationToken);
        }
        public async Task<bool> SetImageUrlAsync(
        int authorId, string imageUrl,
        CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Where(x => x.Id == authorId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(a => a.ImageURL, a => imageUrl),
                    cancellationToken) > 0;
        }

        public async Task<IPagedList<Author>> GetPagedAuthorAsync(PostQuey condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterAuthor(condition).ToPagedListAsync(
            pageNumber, pageSize,
            nameof(Author.Id), "DESC",
            cancellationToken);
        }

        public async Task<IList<Author>> GetAllAuthorAsync(CancellationToken cancellationToken = default)
        {
            var queryAuthor
                =await _context.Set<Author>().ToListAsync(cancellationToken);
            return queryAuthor;
        }

        private IQueryable<Author> FilterAuthor(PostQuey condition)
        {

            IQueryable<Author> author = _context.Set<Author>();
            if (condition.AuthorId > 0)
            {
                author = author.Where(x => x.Id == condition.AuthorId);
            }
            if (!string.IsNullOrWhiteSpace(condition.UrlSlug))
            {
                author = author.Where(x => x.UrlSlug == condition.UrlSlug);
            }
            if (condition.Year>0)
            {
                author = author.Where(x => x.JoinedDate.Year == condition.Year);
            }
            if (condition.Month > 0)
            {
                author = author.Where(x => x.JoinedDate.Month == condition.Month);
            }
            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                author = author.Where(x => x.FullName.Contains(condition.Keyword));
            }

            return author;

        }




        public async Task<Author> GetAuthorBySlugAsync(
       string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }

        public async Task<Author> GetCachedAuthorBySlugAsync(
      string slug, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"author.by-slug.{slug}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetAuthorBySlugAsync(slug, cancellationToken);
                });
        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            return await _context.Set<Author>().FindAsync(authorId);
        }

        public async Task<Author> GetCachedAuthorByIdAsync(int authorId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"author.by-id.{authorId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetAuthorByIdAsync(authorId);
                });
        }

        public async Task<IList<AuthorItem>> GetAuthorsAsync(
    CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .OrderBy(a => a.FullName)
                .Select(a => new AuthorItem()
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    JoinedDate = a.JoinedDate,
                    ImageURL = a.ImageURL,
                    UrlSlug = a.UrlSlug,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
           IPagingParams pagingParams,
           string name = null,
           CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
            .AsNoTracking()
                .Where(x=>string.IsNullOrWhiteSpace(name)?true: x.FullName.Contains(name))
                .Select(a => new AuthorItem()
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    JoinedDate = a.JoinedDate,
                    ImageURL = a.ImageURL,
                    UrlSlug = a.UrlSlug,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
         Func<IQueryable<Author>, IQueryable<T>> mapper,
         IPagingParams pagingParams,
         string name = null,
         CancellationToken cancellationToken = default)
        {
            var authorQuery = _context.Set<Author>().AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
            }

            return await mapper(authorQuery)
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IList<Author>> Find_N_MostPostByAuthorAsync(int limit, CancellationToken cancellationToken = default)
        {
            var authorList = new List<Author>();
            var groupQuery = from s in _context.Authors
                             join d in _context.Posts on s.Id equals d.AuthorId
                             select s;
            var dataGroup = groupQuery.GroupBy(x => x.Id).Select(x => new {
                authorId = x.Key,
                count = x.Count(),
                value = x.Select(x => new Author
                {
                    Id = x.Id
                ,
                    FullName = x.FullName
                ,
                    UrlSlug = x.UrlSlug
                ,
                    ImageURL = x.ImageURL
                ,
                    JoinedDate = x.JoinedDate
                ,
                    Email = x.Email
                ,
                    Notes = x.Notes
                }).FirstOrDefault()

            }).OrderBy(x => x.authorId);
            foreach (var item in dataGroup)
            {
                authorList.Add(item.value);
            }
            return authorList.Take(limit).ToList();
        }
    }
}
