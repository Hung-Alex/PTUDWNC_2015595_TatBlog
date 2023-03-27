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
    public class TagsController : Controller
    {


        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IMediaManager _mediaManager;
        private readonly ILogger<TagsController> _logger;
        public TagsController(ILogger<TagsController> logger, IBlogRepository blogRepository, IMapper mapper, IMediaManager mediaManager)
        {
            _logger = logger;
            _mapper = mapper;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
        }
        #region Index
        [HttpGet]
        public async Task<IActionResult> Index(

            TagsFilterModel model,
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10
            )
        {
            var categoryQuery = _mapper.Map<PostQuey>(model);


            ViewBag.TagsList = await _blogRepository
                .GetPagedTagsAsync(categoryQuery, pageNumber, 10);
            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            await PopulateTagsFitlterModelAsync(model);



            return View("Index", model);
        }
        #endregion

        #region EditPost
        [HttpPost]
        public async Task<IActionResult> Edit([FromServices]
             IValidator<TagsEditModel> categoryValidator, TagsEditModel model)
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

            var tag = model.Id > 0
              ? await _blogRepository.getTagById(model.Id)
              : null;

            if (tag == null)
            {
                tag = _mapper.Map<Tag>(model);
                tag.Id = 0;

            }
            else
            {
                _mapper.Map(model, tag);

            }


            await _blogRepository.AddOrUpdateTag(tag);


            return RedirectToAction(nameof(Index));
        }
            #endregion

            #region DeleteTag
            [HttpPost]
            public async Task<IActionResult> DeleteTag(int id)
            {
                var changestatusPublished = await _blogRepository.RemoveTagById(id);

                return RedirectToAction("Index");
            }
            #endregion
            #region EditGet
            [HttpGet]
            public async Task<IActionResult> Edit(int id)
            {
                var tag = id > 0
               ? await _blogRepository.getTagById(id)
               : null;

                var model = tag == null
                ? new TagsEditModel()
                : _mapper.Map<TagsEditModel>(tag);

                return View(model);

            }
            #endregion
            #region validate
            [HttpPost]
            public async Task<IActionResult> VerifyTagSlug(int id, string urlSlug)
            {
                var slugExisted = await _blogRepository.IsTagSlugExistedAsync(id, urlSlug);
                return slugExisted ? Json($"slug '{urlSlug}'  đã được sử dụng") : Json(true);
            }
            #endregion

            #region filter
            public async Task PopulateTagsFitlterModelAsync(TagsFilterModel model)
            {
                var tags = await _blogRepository.GetAllTagAsync();


                model.TagList = tags.Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                });
            }

            #endregion
        }
    }

