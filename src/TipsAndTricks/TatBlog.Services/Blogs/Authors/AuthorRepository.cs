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

namespace TatBlog.Services.Blogs.Authors
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogDbContext _context;
        public AuthorRepository(BlogDbContext context)
        {
            _context = context;
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

    }
}
