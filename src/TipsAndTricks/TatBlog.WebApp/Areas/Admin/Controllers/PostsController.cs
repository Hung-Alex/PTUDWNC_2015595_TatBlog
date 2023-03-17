using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    
    public class PostsController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IMediaManager _mediaManager;
        private readonly ILogger<PostsController> _logger;
        public PostsController(ILogger<PostsController> logger,IBlogRepository blogRepository,IMapper mapper,IMediaManager mediaManager)
        {
            _logger = logger;
            _mapper = mapper;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(
            
            PostFilterModel model,
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10
            )
        {
            var postQuery = _mapper.Map<PostQuey>(model);

            ViewBag.PostsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, 10);
            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            await PopulatePostsFitlterModelAsync(model);
            

            
            return View("Index",model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromServices]
             IValidator<PostEditModel> postValidator,PostEditModel model)
        {

            //slug null
            var validationResult=await postValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }
            if (!ModelState.IsValid)
            {
                await PopulatePostsEditModelAsync(model);
                return View(model);
            }

            var post = model.Id > 0
              ? await _blogRepository.GetPostByIdAsync(model.Id)
              : null;

            if (post == null)
            {
                post = _mapper.Map<Post>(model);
                post.Id = 0;
                post.PostedDate = DateTime.Now;
            }
            else
            {
                _mapper.Map(model, post);
                post.Category = null;
                post.PostedDate = DateTime.Now;
            }

            // Nếu người dùng có upload hình ảnh minh họa cho bài viết
            if (model.ImageFile?.Length > 0)
            {
                // Thực hiện việc lưu tập tin vào thư mực uploads
                var newImagePath = await _mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(), model.ImageFile.FileName, model.ImageFile.ContentType);

                // Nếu lưu thành công, xóa tập tin hình ảnh cũ (nếu có)
                if (!string.IsNullOrWhiteSpace(newImagePath))
                {
                    await _mediaManager.DeleteFileAsync(post.ImageUrl);
                    post.ImageUrl = newImagePath;
                }
            }

            await _blogRepository.CreateOrUpdatePostAsync(post,model.GetSelectedTags());


            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
           
            var post = id > 0
            ? await _blogRepository.GetPostByIdAsync(id, true)
            : null;
         
            var model = post == null
            ? new PostEditModel()
            : _mapper.Map<PostEditModel>(post);
            await PopulatePostsEditModelAsync(model);
            return View(model);
        }
        public async Task PopulatePostsFitlterModelAsync(PostFilterModel model)
        {
            var categories=await _blogRepository.GetCategoriesAsync();
            var authors=await _blogRepository.GetAuthorsAsync();
            model.AuthorList=authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value=a.Id.ToString(),
            });
            model.CategoryList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
        }
        public async Task PopulatePostsEditModelAsync(PostEditModel model)
        {
            var categories = await _blogRepository.GetCategoriesAsync();
            var authors = await _blogRepository.GetAuthorsAsync();
            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString(),
            });
            model.CategoryList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
        }
        [HttpPost]
        public async Task<IActionResult> VerifyPostSlug(int id,string urlSlug)
        {
            var slugExisted = await _blogRepository.IsPostSlugExistedAsync(id, urlSlug);
            return slugExisted?Json($"slug '{urlSlug}'  đã được sử dụng"):Json(true);
        }
    }
}
