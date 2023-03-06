using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs.Authors;

namespace TatBlog.Services.Blogs.Authors
{
    public class AuthorRepository : IAuthorRepository
    {
        public Task<Author> FindAuthorbyId(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Author> FindAuthorbyslugs(string urlslug, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
