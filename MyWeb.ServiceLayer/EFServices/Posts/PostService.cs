using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EntityFramework.Extensions;
using MyWeb.DataLayer.Context;
using MyWeb.DomainClasses.Entities;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.Comment;
using MyWeb.ViewModel.Post;
using ActiveStatus = MyWeb.ViewModel.Post.ActiveStatus;

namespace MyWeb.ServiceLayer.EFServices.Posts
{
    public class PostService : IPostService
    {
        #region Fields

        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<Post> _post;
        private readonly ICommentService _commentService;
        private readonly IPostsReactService _postReactService;
        private readonly IEnumerable<SelectListItem> _allCategories;
        private readonly IEnumerable<SelectListItem> _allCategoriesForSearch;
        private readonly IEnumerable<SelectListItem> _allCreatorsForSearch;
        #endregion

        #region Ctor

        public PostService(IUnitOfWork unitOfWork,
          IMappingEngine mappingEngine,
          ICategoryService categoryService,
          IApplicationUserManager applicationUserManagerService,
          ICommentService commentService,
          IPostsReactService postReactService)
        {
            _unitOfWork = unitOfWork;
            _post = _unitOfWork.Set<Post>();
            _mappingEngine = mappingEngine;
            _allCategories = categoryService.GetAllCategoriesSelectListItem();
            _allCategoriesForSearch = categoryService.GetAllCategorySelectListItemForSearch();
            _allCreatorsForSearch = applicationUserManagerService.GetAllUsersSelectListItemForSearch();
            _commentService = commentService;
            _postReactService = postReactService;
        }
        #endregion

        #region Create
        public async Task Create(PostInsertViewModel viewModel)
        {
            _post.Add(_mappingEngine.Map<Post>(viewModel));
            await _unitOfWork.SaveChangesAsync();
        }

        public PostInsertViewModel GetForCreate()
        {
            return new PostInsertViewModel
            {
                Categories = _allCategories
            };
        }

        #endregion

        #region Edit
        public async Task Edit(PostEditViewModel viewModel)
        {
            var post = await _post
              .Include(q => q.Creator)
              .FirstAsync(a => a.Id == viewModel.Id);

            _mappingEngine.Map(viewModel, post);

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<PostEditViewModel> GetForEdit(long? id)
        {
            var viewModel = await _post
              .AsNoTracking()
              .ProjectTo<PostEditViewModel>(_mappingEngine)
              .FirstOrDefaultAsync(a => a.Id == id);

            if (viewModel == null)
                return null;

            viewModel.Categories = _allCategories;
            return viewModel;
        }
        #endregion

        #region Details
        public async Task<PostDetailViewModel> GetDetails(long? id)
        {
            var viewModel = await _post
              .AsNoTracking()
              .ProjectTo<PostDetailViewModel>(_mappingEngine)
              .FirstOrDefaultAsync(a => a.Id == id);

            return viewModel ?? null;
        }

        public async Task<PostDetailViewModel> GetForMainPageDetails(long? id)
        {
            var viewModel = await _post.Include(q=>q.Comments).Include(q=>q.Reacts)
              .AsNoTracking()
              .ProjectTo<PostDetailViewModel>(_mappingEngine)
              .FirstOrDefaultAsync(a => a.Id == id);

            if (viewModel == null)
                return null;

            viewModel.Comments = await _commentService.GetPostComments(id);
            viewModel.PostReacts = await _postReactService.GetPostReacts(id);

            return viewModel ?? null;
        }

        public async Task<PostMainTitleAndIdViewModel> GetForMainIdAndTitle(long? id)
        {
            var viewModel = await _post
                .AsNoTracking()
                .Select(q=> new PostMainTitleAndIdViewModel
                {
                    Title = q.Title,
                    Id = q.Id
                })
                .FirstOrDefaultAsync(a => a.Id == id);

            if (viewModel == null)
                return null;

            return viewModel ?? null;
        }
        #endregion

        #region Delete
        public async Task DeleteAsync(long? id)
        {
            await _post.Where(a => a.Id == id).DeleteAsync();
        }
        #endregion

        #region GetPagedList
        public async Task<PostListViewModel> GetPagedListAsync(PostSearchRequest request)
        {
            request.Categories = _allCategoriesForSearch;
            request.Creators = _allCreatorsForSearch;

            var result = _post
              .ProjectTo<PostDetailViewModel>(_mappingEngine);

            if (!string.IsNullOrWhiteSpace(request.Title))
                result = result.Where(a => a.Title.Contains(request.Title) || a.Body.Contains(request.Title) || a.Summary.Contains(request.Title)).AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Keyword))
                result = result.Where(a => a.Keyword.Contains(request.Keyword)).AsQueryable();

            if (request.CategoryId != 0)
                result = result.Where(a => a.CategoryId == request.CategoryId).AsQueryable();

            if (request.CreatorId != 0)
                result = result.Where(a => a.CreatorId == request.CreatorId).AsQueryable();

            switch (request.IsActive)
            {
                case ActiveStatus.Enable:
                    result = result.Where(a => a.IsActive).AsQueryable();
                    break;
                case ActiveStatus.Disable:
                    result = result.Where(a => !a.IsActive).AsQueryable();
                    break;
                case ActiveStatus.All:
                    break;
            }

            request.Total = result.Count();
            var resultsToSkip = (request.PageIndex - 1) * request.PageSize;
            var query = await result
              .OrderByDescending(r => r.Id)
              .Skip(() => resultsToSkip)
              .Take(() => request.PageSize)
              .ToListAsync();

            return new PostListViewModel { Posts = query, SearchRequest = request };
        }

        public async Task<PostMainListViewModel> GetForMainPagePagedListForAsync(PostSearchRequest request)
        {
            var result = _post.Select(q => new PostMainDetailViewModel
            {
                Summary = q.Summary,
                CategoryTitle = q.Category.Title,
                Title = q.Title,
                Keywords = q.Keyword,
                CreatedOn = q.CreatedOn,
                CategoryId = q.CategoryId,
                CreatorId = q.CreatorId,
                IsActive = q.IsActive,
                CreatorName = q.Creator.NameForShow,
                Id = q.Id,
                CommentsCount = q.Comments.Count(a => a.IsActive)
            }).AsQueryable();
              //.ProjectTo<PostMainDetailViewModel>(_mappingEngine);

            request.PageSize = 5;

            if (request.CategoryId != 0)
                result = result.Where(q => q.CategoryId == request.CategoryId);

            if (request.Term != null)
                result = result.Where(q => q.Summary.Contains(request.Term) | q.Title.Contains(request.Term) | q.Keywords.Contains(request.Term));

            request.Total = result.Count();
            var resultsToSkip = (request.PageIndex - 1) * request.PageSize;
            var query = await result
              .Where(q => q.IsActive)
              .OrderByDescending(r => r.Id)
              .Skip(() => resultsToSkip)
              .Take(() => request.PageSize)
              .ToListAsync();

            if (request.CategoryId != 0)
            {
                request.CategoryTitle = _allCategories.FirstOrDefault(q => q.Value == request.CategoryId.ToString())?.Text;
            }


            return new PostMainListViewModel { Posts = query, SearchRequest = request };
        }

        public PostRelatedOrRecentListViewModel GetForMainPageListForRelated(PostSearchRequest request)
        {
            var result = _post.Where(q=>q.IsActive).Select(q => new PostRelatedOrRecentDetailViewModel
            {
                Title = q.Title,
                Id = q.Id,
                CategoryId = q.CategoryId
            }).AsQueryable();

            if (request.CategoryId != 0)
                result = result.Where(q => q.CategoryId == request.CategoryId & q.Id != request.PostId);

            request.Total = result.Count();
            var resultsToSkip = (request.PageIndex - 1) * request.PageSize;
            var query = result
              .OrderByDescending(r => r.Id)
              .Skip(() => resultsToSkip)
              .Take(() => request.PageSize)
              .ToList();

            return new PostRelatedOrRecentListViewModel { Posts = query, SearchRequest = request };
        }
        #endregion

        #region GetKeywords

        public IEnumerable<string> GetKeywords()
        {
            var result = _post.Where(q => q.IsActive).Select(q => new
            {
                q.Keyword,
                q.Id,
            }).AsQueryable();
              //.ProjectTo<PostDetailViewModel>(_mappingEngine);

            const int postCount = 10;

            var query = result
              .OrderByDescending(r => r.Id)
              .Take(() => postCount)
              .ToList();

            var allWordsString = query.Aggregate("", (current, item) => current + item.Keyword + ",");

            var dividedWords = allWordsString.Split(',').Select(s => s.Trim()).Where(q => q != string.Empty);

            dividedWords = dividedWords.Distinct().ToArray().Take(30).ToArray();

            return dividedWords;
        }

        #endregion

        #region CountOfAll

        public async Task<int> CountOfAll()
        {
            var result = _post
              .ProjectTo<PostDetailViewModel>(_mappingEngine);

            return await result.CountAsync();
        }

        #endregion

        #region IsInDb
        public Task<bool> IsInDb(long id)
        {
            return _post.AnyAsync(a => a.Id == id);
        }
        #endregion

        #region ClearCache()
        public Task ClearCache()
        {
            return Task.Delay(10);
        }
        #endregion

    }
}
