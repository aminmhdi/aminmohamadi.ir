using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyWeb.ViewModel.Category;
using MyWeb.ViewModel.Comment;

namespace MyWeb.ServiceLayer.Contracts
{
    public interface ICommentService
    {
        CommentInsertViewModel GetForCreate(long postId);
        Task Create(CommentInsertViewModel model);
        Task Edit(CommentEditViewModel model);
        Task<CommentDetailViewModel> GetDetails(long? id);
        Task<CommentEditViewModel> GetForEdit(long? id);
        Task DeleteAsync(long? id);
        CommentMainListViewModel GetCommentForMainPagedList(CommentSearchRequest request);
        Task<CommentMainListViewModel> GetCommentForMainPagedListAsync(CommentSearchRequest request);
        Task<CommentListViewModel> GetPagedListAsync(CommentSearchRequest request);
        Task<ICollection<CommentDetailViewModel>> GetPostComments(long? id);
        Task Active(long? id, long? userId);
        Task<int> CountOfUnconfirmedComments();
        Task<bool> IsInDb(long id);
        Task ClearCache();
    }
}
