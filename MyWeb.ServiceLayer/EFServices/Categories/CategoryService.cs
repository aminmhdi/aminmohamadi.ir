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
using MyWeb.ViewModel.Category;

namespace MyWeb.ServiceLayer.EFServices.Categories
{
  public class CategoryService : ICategoryService
  {
    #region Fields

    private readonly IMappingEngine _mappingEngine;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbSet<Category> _category;
    #endregion

    #region Ctor

    public CategoryService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
    {
      _unitOfWork = unitOfWork;
      _category = _unitOfWork.Set<Category>();
      _mappingEngine = mappingEngine;
    }
    #endregion

    #region Create
    public async Task Create(CategoryInsertViewModel viewModel)
    {
      _category.Add(_mappingEngine.Map<Category>(viewModel));
      await _unitOfWork.SaveChangesAsync();
    }

    public CategoryInsertViewModel GetForCreate()
    {
      return new CategoryInsertViewModel();
    }

    #endregion

    #region Edit
    public async Task Edit(CategoryEditViewModel viewModel)
    {
      var category = await _category
        .Include(q => q.Creator)
        .FirstAsync(a => a.Id == viewModel.Id);

      _mappingEngine.Map(viewModel, category);

      await _unitOfWork.SaveChangesAsync();
    }
    public async Task<CategoryEditViewModel> GetForEdit(long? id)
    {
      var viewModel = await _category
        .AsNoTracking()
        .ProjectTo<CategoryEditViewModel>(_mappingEngine)
        .FirstOrDefaultAsync(a => a.Id == id);

      return viewModel ?? null;
    }
    #endregion

    #region Details
    public async Task<CategoryDetailViewModel> GetDetails(long? id)
    {
      var viewModel = await _category
        .AsNoTracking()
        .ProjectTo<CategoryDetailViewModel>(_mappingEngine)
        .FirstOrDefaultAsync(a => a.Id == id);

      return viewModel ?? null;
    }

      public async Task<CategoryMainIdAndTitleDetailViewModel> GetForMainIdAndTitle(long? id)
      {
          var viewModel = await _category
              .AsNoTracking()
              .ProjectTo<CategoryMainIdAndTitleDetailViewModel>(_mappingEngine)
              .FirstOrDefaultAsync(a => a.Id == id);

          return viewModel ?? null;
      }
        #endregion

        #region Delete
        public async Task DeleteAsync(long? id)
    {
      await _category.Where(a => a.Id == id).DeleteAsync();
    }
    #endregion

    #region GetPagedList
    public async Task<CategoryListViewModel> GetPagedListAsync(CategorySearchRequest request)
    {
      var result = _category
        .ProjectTo<CategoryDetailViewModel>(_mappingEngine);

      if (!string.IsNullOrWhiteSpace(request.Title))
        result = result.Where(a => a.Title.Contains(request.Title) || a.Description.Contains(request.Title)).AsQueryable();

      request.Total = result.Count();
      var resultsToSkip = (request.PageIndex - 1) * request.PageSize;
      var query = await result
        .OrderByDescending(r => r.Id)
        .Skip(() => resultsToSkip)
        .Take(() => request.PageSize)
        .ToListAsync();
      return new CategoryListViewModel { Categories = query, SearchRequest = request };
    }
    #endregion

    #region CategorySelectList
    public IEnumerable<SelectListItem> GetAllCategoriesSelectListItem()
    {
      return _category
        .Where(q => q.IsActive)
        .AsNoTracking()
        .Project(_mappingEngine)
        .To<SelectListItem>()
        .ToList();
    }

    public IEnumerable<SelectListItem> GetAllCategorySelectListItemForSearch()
    {
      var list = new List<SelectListItem>
      {
        new SelectListItem
        {
          Text = "همه موارد",
          Value = "0"
        }
      };

      list.AddRange(
        _category.Where(q => q.IsActive)
        .AsNoTracking()
        .Project(_mappingEngine)
        .To<SelectListItem>()
        .ToList()
        );

      return list;
    }
    #endregion

    #region CategoryMenu
    public IEnumerable<CategoryMenuViewModel> GetAllCategoriesForMenu()
    {
      return _category
        .Where(q => q.IsActive)
        .AsNoTracking()
        .Select(q=> new CategoryMenuViewModel
        {
          Id = q.Id,
          Title = q.Title
        })
        .OrderBy(q=>q.Title)
        .ToList();
    }
    #endregion

    #region CountOfAll

    public async Task<int> CountOfAll()
    {
      var result = _category
        .ProjectTo<CategoryDetailViewModel>(_mappingEngine);

      return await result.CountAsync();
    }

    #endregion

    #region IsInDb
    public Task<bool> IsInDb(long id)
    {
      return _category.AnyAsync(a => a.Id == id);
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
