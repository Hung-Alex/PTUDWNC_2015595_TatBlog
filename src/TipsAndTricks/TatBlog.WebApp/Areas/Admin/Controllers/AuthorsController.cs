using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Blogs.Authors;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IAuthorRepository _authorRepository;

        private readonly IMapper _mapper;
        private readonly IMediaManager _mediaManager;
        private readonly ILogger<PostsController> _logger;
        public AuthorsController(ILogger<PostsController> logger, IBlogRepository blogRepository, IMapper mapper, IMediaManager mediaManager,IAuthorRepository authorRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
            _authorRepository = authorRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(

            AuthorFilterModel model,
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10
            )
        {
            var postQuery = _mapper.Map<PostQuey>(model);
            

            ViewBag.AuthorsList = await _authorRepository
                .GetPagedAuthorAsync(postQuery, pageNumber, 10);
            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            await PopulateAuthorsFitlterModelAsync(model);



            return View("Index", model);
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var changestatusPublished = await _authorRepository.DeleteAuthorAsync(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromServices]
             IValidator<AuthorEditModel> authorValidator, AuthorEditModel model)
        {

            //slug null
            var validationResult = await authorValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }
            if (!ModelState.IsValid)
            {
                
                return View(model);
            }

            var author = model.Id > 0
              ? await _authorRepository.FindAuthorbyId(model.Id)
              : null;

            if (author == null)
            {
                author = _mapper.Map<Author>(model);
                author.Id = 0;
                author.JoinedDate = DateTime.Now;
            }
            else
            {
                _mapper.Map(model, author);
                
                author.JoinedDate = DateTime.Now;
            }

            // Nếu người dùng có upload hình ảnh minh họa cho bài viết
            if (model.ImageFile?.Length > 0)
            {
                // Thực hiện việc lưu tập tin vào thư mực uploads
                var newImagePath = await _mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(), model.ImageFile.FileName, model.ImageFile.ContentType);

                // Nếu lưu thành công, xóa tập tin hình ảnh cũ (nếu có)
                if (!string.IsNullOrWhiteSpace(newImagePath))
                {
                    await _mediaManager.DeleteFileAsync(author.ImageURL);
                    author.ImageURL = newImagePath;
                }
            }

            await _authorRepository.AddOrUpdateAsync(author);


            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {

            var author = id > 0
            ? await _authorRepository.FindAuthorbyId(id)
            : null;

            var model = author == null
            ? new AuthorEditModel()
            : _mapper.Map<AuthorEditModel>(author);
          
            return View(model);
        }
        #region
        public async Task PopulateAuthorsFitlterModelAsync(AuthorFilterModel model)
        {  
            var authors = await _blogRepository.GetAuthorsAsync();
           
            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString(),
            });
          
        }
        

        #endregion
        [HttpPost]
        public async Task<IActionResult> VerifyAuthorSlug(int id, string urlSlug)
        {
            var slugExisted = await _authorRepository.IsAuthorSlugExistedAsync(id, urlSlug);
            return slugExisted ? Json($"slug '{urlSlug}'  đã được sử dụng") : Json(true);
        }
    }
}
