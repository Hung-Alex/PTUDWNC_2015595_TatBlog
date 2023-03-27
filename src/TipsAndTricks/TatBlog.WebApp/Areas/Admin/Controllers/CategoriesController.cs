using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IMediaManager _mediaManager;
        private readonly ILogger<PostsController> _logger;
        public CategoriesController(ILogger<PostsController> logger, IBlogRepository blogRepository, IMapper mapper, IMediaManager mediaManager)
        {
            _logger = logger;
            _mapper = mapper;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
        }
        public async Task<IActionResult> Index(

              CategoriesFilterModel model,
              [FromQuery(Name = "p")] int pageNumber = 1,
              [FromQuery(Name = "ps")] int pageSize = 10
              )
        {
            var categoriesQuery = _mapper.Map<PostQuey>(model);
            ViewBag.CategoriesList = await _blogRepository
                .GetPagedCategoriesAsync(categoriesQuery, pageNumber, 10);
            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            await PopulateCategoriesFitlterModelAsync(model);



            return View("Index", model);
        }
        #region EditPosts
        [HttpPost]
        public async Task<IActionResult> Edit([FromServices]
             IValidator<CategoriesEditModel> categoryValidator, CategoriesEditModel model)
        {

            //slug null
            var validationResult = await categoryValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }
            if (!ModelState.IsValid)
            {
                //await PopulatePostsEditModelAsync(model);
                return View(model);
            }

            var post = model.Id > 0
              ? await _blogRepository.getCategoryById(model.Id)
              : null;

            if (post == null)
            {
                post = _mapper.Map<Category>(model);
                post.Id = 0;
               
            }
            else
            {
                _mapper.Map(model, post);
                
            }

            
          

            await _blogRepository.AddOrUpdateCategory(post);


            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region EditCategory
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = id > 0
           ? await _blogRepository.getCategoryById(id)
           : null;

            var model = category == null
            ? new CategoriesEditModel()
            : _mapper.Map<CategoriesEditModel>(category);
           
            return View(model);
            
        }
        #endregion
        #region DeleteCategory

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryQuery = _blogRepository.RemoveCategorybyId(id);
            return RedirectToAction("Index");
        }
        #endregion
        #region validateurlSlug
        public async Task<IActionResult> VerifyCategorySlug(int id, string urlSlug)
        {
            var slugExisted = await _blogRepository.IsCategorySlugExistedAsync(id, urlSlug);
            return slugExisted ? Json($"slug '{urlSlug}'  đã được sử dụng") : Json(true);
        }
        #endregion
        #region LocSanPham
        public async Task PopulateCategoriesFitlterModelAsync(CategoriesFilterModel model)
        {
            var categories = await _blogRepository.GetCategoriesAsync();
            model.CategoryList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
        }
     
        #endregion
    }
}
