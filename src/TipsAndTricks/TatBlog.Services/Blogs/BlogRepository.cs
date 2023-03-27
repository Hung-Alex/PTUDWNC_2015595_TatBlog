using Microsoft.EntityFrameworkCore;
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
using Microsoft.EntityFrameworkCore.Query;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Identity.Client;
using System.Net.WebSockets;
using Microsoft.VisualBasic;

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
            IQueryable<Tag> tagQuery = _context.Set<Tag>().Where(x => x.UrlSlug == slug);
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
        public async Task<bool> IsTagSlugExistedAsync(int tagid, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>().AnyAsync(x => x.Id != tagid && x.UrlSlug == slug, cancellationToken);
        }

        public async Task<bool> IsCategorySlugExistedAsync(int categoryid, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().AnyAsync(x => x.Id != categoryid && x.UrlSlug == slug, cancellationToken);
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

            return tagitem > 0;
        }
        //Tìm một chuyên mục (Category) theo tên định danh (slug)
        public async Task<Category> FindCategoryByUrlSlug(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().Where(cate => cate.UrlSlug == slug).SingleOrDefaultAsync(cancellationToken);
        }
        //Tìm một chuyên mục theo mã số cho trước
        public async Task<Category> FindCategoryByNumber(int number, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().Where(x => x.Id == number).SingleOrDefaultAsync();
        }
        //Thêm hoặc cập nhật một chuyên mục/chủ đề
        // In this case i don't check vaild data of model, i just made function add,update basic . same as function  on above, i also don't check valid data of model
        public async Task<bool> AddOrUpdateCategory(Category category, CancellationToken cancellationToken = default)
        {
            var categoryQuery = await _context.Set<Category>().SingleOrDefaultAsync(c => c.Id.Equals(category.Id), cancellationToken);
            if (categoryQuery != null)
            {
                categoryQuery.Name = category.Name;
                categoryQuery.Description = category.Description;
                categoryQuery.UrlSlug = category.UrlSlug;
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            else
            {
                await _context.Set<Category>().AddAsync(category, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<bool> AddOrUpdateTag(Tag tag, CancellationToken cancellationToken = default)
        {
            var tagQuery = await _context.Set<Tag>().SingleOrDefaultAsync(c => c.Id.Equals(tag.Id), cancellationToken);
            if (tagQuery != null)
            {
                tagQuery.Name = tag.Name;
                tagQuery.Description = tag.Description;
                tagQuery.UrlSlug = tag.UrlSlug;
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            else
            {
                await _context.Set<Tag>().AddAsync(tag, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }


        public async Task<bool> RemoveCategorybyId(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().Where(cate => cate.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<bool> IsExistsSlug(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().Where(x => x.UrlSlug == slug).CountAsync(cancellationToken) > 0;   // this method is find with primary key of specific table  ,it's pass argument is ID (int) =>do not use with include() method because returm ienumerable ,you should combie with SingleOrDefaultAsync(),FirstOrDefaultAsync() method 

        }

        public async Task<int> CountObject_Valid_Condition_InPostQuery(PostQuey query, CancellationToken cancellationToken)
        {
            IQueryable<Post> postQuery = _context.Set<Post>().Include(x => x.Tags);
            if (query.Year > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Year == query.Year);
            }
            if (query.Month > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Month == query.Month);
            }
            if (query.AuthorId > 0)
            {
                postQuery = postQuery.Where(x => x.AuthorId == query.AuthorId);
            }
            if (query.CategoryId > 0)
            {
                postQuery = postQuery.Where(x => x.CategoryId == query.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(query.UrlSlug))
            {
                postQuery = postQuery.Where(x => x.UrlSlug == query.UrlSlug);
            }
          
            return await postQuery.CountAsync(cancellationToken);

        }
        public async Task<IList<Post>> FindAllPostValidCondition(PostQuey condition, CancellationToken cancellationToken)
        {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);
            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                         x.ShortDescription.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword) ||
                                         x.Category.Name.Contains(condition.Keyword) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
            }
            if (condition.Day > 0)
            {
                posts = posts.Where(x => x.PostedDate.Day == condition.Day);
            }
            if (condition.Year > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }


            return await posts.ToListAsync(cancellationToken);


        }



        public async Task<IPagedList<Post>> FindAndPagination_Valid_Condition_InPostQuery(PostQuey condition, IPagingParams pagingParams, CancellationToken cancellationToken)
        {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);
            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                         x.ShortDescription.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword) ||
                                         x.Category.Name.Contains(condition.Keyword) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
            }
            if (condition.Day > 0)
            {
                posts = posts.Where(x => x.PostedDate.Day == condition.Day);
            }
            if (condition.Year > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }


            return await posts.ToPagedListAsync(pagingParams, cancellationToken);
        
            
        }

        public async Task<bool> TogglePublishedFlagAsync(
    int postId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Set<Post>().FindAsync(postId);

            if (post is null) return false;

            post.Published = !post.Published;
            await _context.SaveChangesAsync(cancellationToken);

            return post.Published;
        }
        public async Task<IList<Post>> GetPostRandomsAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            
                var random = new Random();
            IQueryable<Post> posts = _context.Set<Post>()
             .Include(x => x.Category)
             .Include(x => x.Author)
             .Include(x => x.Tags);
            return await posts.OrderBy(x=>Guid.NewGuid()).Take(numPosts).ToListAsync(cancellationToken);
            
        }

     

        public async Task<IPagedList<CategoryItem>> Paginationcategory(IPagingParams pagingParams, CancellationToken cancellationToken)
        {
            var tagQuery = _context.Set<Category>()
                                     .Select(x => new CategoryItem()
                                     {
                                         Id = x.Id,
                                         Name = x.Name,
                                         UrlSlug = x.UrlSlug,
                                         Description = x.Description,
                                         ShowOnMenu = x.ShowOnMenu,
                                         PostCount = x.Posts.Count(p => p.Published)
                                     });
            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<bool> ConvertStatusPublishedAsync(bool published, CancellationToken cancellationToken = default)
        {
            // Tìm thẻ theo ID
            var post = await _context.Set<Post>()
                .FirstOrDefaultAsync(p => p.Published == published, cancellationToken);
            if (post.Published == true)
            {
                post.Published = false;
            }
            else
            {
                post.Published = true;
            }
            return true;
        }
        private IQueryable<Post> FilterPosts(PostQuey condition)
        {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);

            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                         x.ShortDescription.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword) ||
                                         x.Category.Name.Contains(condition.Keyword) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
            }
            if (condition.Day>0)
            {
                posts = posts.Where(x => x.PostedDate.Day == condition.Day);
            }
            if (condition.Year > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;
        }
        private IQueryable<Category> FilterCategories(PostQuey condition)
        {
            
            IQueryable<Category> categories = _context.Set<Category>();
            if (condition.CategoryId > 0)
            {
                categories = categories.Where(x => x.Id == condition.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(condition.UrlSlug))
            {
                categories = categories.Where(x => x.UrlSlug==condition.UrlSlug) ;
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                categories = categories.Where(x => x.Name.Contains( condition.Keyword));
            }
            
            return categories;
           
        }
        private IQueryable<Tag> FilterTags(PostQuey condition)
        {

            IQueryable<Tag> tags = _context.Set<Tag>();
            if (condition.TagId > 0)
            {
                tags = tags.Where(x => x.Id == condition.TagId);
            }
            if (!string.IsNullOrWhiteSpace(condition.UrlSlug))
            {
                tags = tags.Where(x => x.UrlSlug == condition.UrlSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                tags = tags.Where(x => x.Name.Contains(condition.Keyword));
            }

            return tags;

        }
        public async Task<IPagedList<Post>> GetPagedPostsAsync(PostQuey condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
            pageNumber, pageSize,
            nameof(Post.PostedDate), "DESC",
            cancellationToken);
        }
        public async Task<IPagedList<Category>> GetPagedCategoriesAsync(PostQuey condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterCategories(condition).ToPagedListAsync(
            pageNumber, pageSize,
            nameof(Category.Id), "DESC",
            cancellationToken);
        }

        public async Task<Post> GetPostAsync(PostQuey query, CancellationToken cancellationToken)
        {
            IQueryable<Post> postQuery = _context.Set<Post>().Include(x => x.Category).Include(x => x.Author);
            if (query.Year > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Year == query.Year);
            }
            if (query.Month > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Month == query.Month);
            }
            if (query.Day > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Day==query.Day);
            }
            if (!string.IsNullOrWhiteSpace(query.UrlSlug))
            {
                postQuery = postQuery.Where(x => x.UrlSlug == query.UrlSlug);
            }
            return await postQuery.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>().Include(a => a.Posts).ToListAsync(cancellationToken);
        }

        public async Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(postId);
            }

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        }
        public async Task<Tag> GetTagAsync(
        string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }
        public async Task<Post> CreateOrUpdatePostAsync(
		Post post, IEnumerable<string> tags,
		CancellationToken cancellationToken = default)
	{
		if (post.Id > 0)
		{
			await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
		}
		else
		{
			post.Tags = new List<Tag>();
		}

		var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(x => new
			{
				Name = x,
				Slug = x.GenerateSlug(),
			})
			.GroupBy(x => x.Slug)
			.ToDictionary(g => g.Key, g => g.First().Name);


		foreach (var kv in validTags)
		{
			if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

			var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
			{
				Name = kv.Value,
				Description = kv.Value,
				UrlSlug = kv.Key
			};

			post.Tags.Add(tag);
		}

		post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

		if (post.Id > 0)
			_context.Update(post);
		else
			_context.Add(post);

		await _context.SaveChangesAsync(cancellationToken);

		return post;
	}

        public async  Task<bool> DeletePost(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>().Where(x=>x.Id==id).ExecuteDeleteAsync(cancellationToken)>0;
            
        }

        public async Task<IList<Post>> GetFeaturedPostToTakeNumber(int number, CancellationToken cancellationToken)
        {
            IQueryable<Post> postQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);
            postQuery=postQuery.OrderByDescending(x => x.ViewCount).Take(number);
            return await postQuery.ToListAsync(cancellationToken);
         
        }

        public async Task<IList<Tag>> GetAllTagAsync(CancellationToken CancellationToken = default)
        {
            return await _context.Set<Tag>().ToListAsync();  
        }

        public async Task<IDictionary<int, CountYear>> GetAllMonthOfPosts(CancellationToken cancellationToken = default)
        {
            IDictionary<int, CountYear> month = new Dictionary<int, CountYear>() { 
                {1,new CountYear{ Count=0,Year=0} },
                {2,new CountYear{ Count=0,Year=0} },
                {3,new CountYear { Count = 0, Year = 0 } },
                {4,new CountYear { Count = 0, Year = 0 } },
                {5,new CountYear { Count = 0, Year = 0 } },
                {6,new CountYear { Count = 0, Year = 0 } },
                {7,new CountYear { Count = 0, Year = 0 } },
                {8,new CountYear { Count = 0, Year = 0 } },
                {9,new CountYear { Count = 0, Year = 0 } },
                {10,new CountYear { Count = 0, Year = 0 } },
                {11,new CountYear { Count = 0, Year = 0 } },
                {12,new CountYear { Count = 0, Year = 0 } },
            }; 
            var monthListCount = _context.Set<Post>()
                .Where(x=>x.PostedDate.Month>=DateTime.Now.Month&&x.PostedDate.Year>=DateTime.Now.Year-1)
                .GroupBy(x => new { x.PostedDate.Month,x.PostedDate.Year}).Select(x => new { 
                month=x.Key.Month,
                year=x.Key.Year,
                count=x.Count()
                
            });
            if (monthListCount.Count()>0)
            {
                foreach (var item in monthListCount)
                {
                    if (month.ContainsKey(item.month))
                    {
                        month[item.month]= new CountYear{ Count=item.count,Year=item.year};
                    }
                }
            }

            return month;
        }

        public async Task<Category> getCategoryById(int id, CancellationToken cancellationToken = default)
        {
            var categoryQuery = await _context.Set<Category>().FindAsync(id,cancellationToken);
            return categoryQuery;
        }
        public async Task<Tag> getTagById(int id, CancellationToken cancellationToken = default)
        {
            var tagQuery = await _context.Set<Tag>().FindAsync(id, cancellationToken);
            return tagQuery;
        }
        public async Task<IPagedList<Tag>> GetPagedTagsAsync(PostQuey condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterTags(condition).ToPagedListAsync(
            pageNumber, pageSize,
            nameof(Tag.Id), "DESC",
            cancellationToken);
        }
    }
}
//select MONTH(posts.PostedDate),YEAR(posts.PostedDate) ,COUNT(posts.PostedDate) from posts
//where MONTH(posts.PostedDate) >= MONTH(GETDATE()) and YEAR(Posts.PostedDate)>= YEAR(GETDATE()) - 1
//group by MONTH(posts.PostedDate),YEAR(posts.PostedDate)
