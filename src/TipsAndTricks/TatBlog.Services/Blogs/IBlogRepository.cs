﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default);
        Task<IList<Post>> GetPopularArticleAsync(int number, CancellationToken cancellationToken = default);
        Task<bool> IsPostSlugExistedAsync(int postid, string slug, CancellationToken cancellationToken = default);
        Task IncreaseViewCountAsync(int postid, CancellationToken cancellationToken = default);

    }
}
