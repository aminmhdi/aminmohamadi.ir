using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EntityFramework.Extensions;
using MyWeb.DataLayer.Context;
using MyWeb.DomainClasses.Entities;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.ViewModel.Comment;
using ActiveStatus = MyWeb.ViewModel.Comment.ActiveStatus;

namespace MyWeb.ServiceLayer.EFServices.Comments
{
    public class CommentService : ICommentService
    {
        #region Fields

        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<Comment> _comment;
        #endregion

        #region Ctor

        public CommentService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _comment = _unitOfWork.Set<Comment>();
            _mappingEngine = mappingEngine;
        }
        #endregion

        #region Create
        public async Task Create(CommentInsertViewModel viewModel)
        {
            _comment.Add(_mappingEngine.Map<Comment>(viewModel));
            await _unitOfWork.SaveChangesAsync();
        }

        public CommentInsertViewModel GetForCreate(long postId)
        {
            return new CommentInsertViewModel { PostId = postId };
        }

        #endregion

        #region Edit
        public async Task Edit(CommentEditViewModel viewModel)
        {
            var comment = await _comment
              .Include(a => a.Creator)
              .FirstAsync(a => a.Id == viewModel.Id);

            viewModel.Creator = comment.Creator;
            viewModel.Post = comment.Post;

            _mappingEngine.Map(viewModel, comment);

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<CommentEditViewModel> GetForEdit(long? id)
        {
            var viewModel = await _comment
              .AsNoTracking()
              .ProjectTo<CommentEditViewModel>(_mappingEngine)
              .FirstOrDefaultAsync(a => a.Id == id);

            return viewModel ?? null;
        }
        #endregion

        #region Active,Deactive

        public async Task Active(long? id, long? userId)
        {
            var comment = await _comment
              .Include(q => q.Creator)
              .FirstAsync(a => a.Id == id);

            comment.ModifiedOn = DateTime.Now;
            comment.ModifierId = userId;
            comment.IsActive = !comment.IsActive;

            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region Details
        public async Task<CommentDetailViewModel> GetDetails(long? id)
        {
            var viewModel = await _comment
              .AsNoTracking()
              .ProjectTo<CommentDetailViewModel>(_mappingEngine)
              .FirstOrDefaultAsync(a => a.Id == id);

            return viewModel ?? null;
        }
        #endregion

        #region Delete
        public async Task DeleteAsync(long? id)
        {
            await _comment.Where(a => a.Id == id).DeleteAsync();
        }
        #endregion

        #region GetPagedList

        public CommentMainListViewModel GetCommentForMainPagedList(CommentSearchRequest request)
        {
            var result = _comment.Select(q => new CommentMainDetailViewModel
            {
                Id = q.Id,
                Body = q.Body,
                IsActive = q.IsActive,
                PostId = q.PostId,
                PostTitle = q.Post.Title,
                CreatorName = q.Creator.NameForShow,
                CreatorId = q.CreatorId,
                CreatedOn = q.CreatedOn
            }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Body))
                result = result.Where(a => a.Body.Contains(request.Body)).AsQueryable();

            if (request.Post != null)
                result = result.Where(a => a.PostId == request.Post).AsQueryable();

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
            var query = result
              .OrderByDescending(r => r.Id)
              .Skip(() => resultsToSkip)
              .Take(() => request.PageSize)
              .ToList();
            return new CommentMainListViewModel { Comments = query, SearchRequest = request };
        }

        public async Task<CommentMainListViewModel> GetCommentForMainPagedListAsync(CommentSearchRequest request)
        {
            var result = _comment.Select(q => new CommentMainDetailViewModel
            {
                Id = q.Id,
                Body = q.Body,
                IsActive = q.IsActive,
                PostId = q.PostId,
                PostTitle = q.Post.Title,
                CreatorName = q.Creator.NameForShow,
                CreatorId = q.CreatorId,
                CreatedOn = q.CreatedOn
            }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Body))
                result = result.Where(a => a.Body.Contains(request.Body)).AsQueryable();

            if (request.Post != null)
                result = result.Where(a => a.PostId == request.Post).AsQueryable();

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

            return new CommentMainListViewModel { Comments = query, SearchRequest = request };
        }

        public async Task<CommentListViewModel> GetPagedListAsync(CommentSearchRequest request)
        {
            var result = _comment
              .ProjectTo<CommentDetailViewModel>(_mappingEngine);

            if (!string.IsNullOrWhiteSpace(request.Body))
                result = result.Where(a => a.Body.Contains(request.Body)).AsQueryable();

            if (request.Post != null)
                result = result.Where(a => a.Post.Id == request.Post).AsQueryable();

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
            return new CommentListViewModel { Comments = query, SearchRequest = request };
        }

        #endregion

        #region GetPostComments

        public async Task<ICollection<CommentDetailViewModel>> GetPostComments(long? id)
        {
            var viewModel = await _comment
              .Where(q => q.PostId == id && q.IsActive)
              .AsNoTracking()
              .ProjectTo<CommentDetailViewModel>(_mappingEngine)
              .ToListAsync();

            return viewModel ?? null;
        }

        #endregion

        #region CountOfUnconfirmedComments

        public async Task<int> CountOfUnconfirmedComments()
        {
            var result = _comment
              .ProjectTo<CommentDetailViewModel>(_mappingEngine);

            result = result.Where(a => !a.IsActive).AsQueryable();

            return await result.CountAsync();
        }

        #endregion
        #region IsInDb
        public Task<bool> IsInDb(long id)
        {
            return _comment.AnyAsync(a => a.Id == id);
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
