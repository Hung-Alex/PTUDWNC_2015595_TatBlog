using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs.Authors;


namespace TatBlog.Services.Blogs.Authors
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogDbContext _context;
        public AuthorRepository(BlogDbContext context)
        {
            _context = context;
        }
        public Task<Author> FindAuthorbyId(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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

        
    }
}
