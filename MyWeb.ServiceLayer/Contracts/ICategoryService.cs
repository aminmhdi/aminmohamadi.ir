using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyWeb.ViewModel.Category;

namespace MyWeb.ServiceLayer.Contracts
{
    public interface ICategoryService
    {
        CategoryInsertViewModel GetForCreate();
        Task Create(CategoryInsertViewModel model);
        Task Edit(CategoryEditViewModel model);
        Task<CategoryDetailViewModel> GetDetails(long? id);
        Task<CategoryMainIdAndTitleDetailViewModel> GetForMainIdAndTitle(long? id);
        Task<CategoryEditViewModel> GetForEdit(long? id);
        Task DeleteAsync(long? id);
        Task<CategoryListViewModel> GetPagedListAsync(CategorySearchRequest request);
        IEnumerable<SelectListItem> GetAllCategoriesSelectListItem();
        IEnumerable<SelectListItem> GetAllCategorySelectListItemForSearch();
        IEnumerable<CategoryMenuViewModel> GetAllCategoriesForMenu();
        Task<int> CountOfAll();
        Task<bool> IsInDb(long id);
        Task ClearCache();
    }
}
