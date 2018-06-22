using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MyWeb.DataLayer.Context;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.ViewModel.PostReact;

namespace MyWeb.ServiceLayer.EFServices.PostsReact
{
  public class PostsReactService : IPostsReactService
  {
    #region Fields

    private readonly IMappingEngine _mappingEngine;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbSet<DomainClasses.Entities.PostReact> _postReact;
    #endregion

    #region Ctor

    public PostsReactService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
    {
      _unitOfWork = unitOfWork;
      _postReact = _unitOfWork.Set<DomainClasses.Entities.PostReact>();
      _mappingEngine = mappingEngine;
    }
    #endregion

    #region Create
    public async Task Create(PostReactInsertViewModel viewModel)
    {
      if (!await IsInDb(viewModel))
      {
        _postReact.Add(_mappingEngine.Map<DomainClasses.Entities.PostReact>(viewModel));
        await _unitOfWork.SaveChangesAsync();
      }

      else
      {
        var webView = await _postReact.FirstAsync(a => a.UserId == viewModel.UserId && a.PostId == viewModel.PostId);

        if (webView.Like != viewModel.Like)
        {
          _postReact.Remove(webView);
          _postReact.Add(_mappingEngine.Map<DomainClasses.Entities.PostReact>(viewModel));
        }

        else
          _postReact.Remove(webView);

        await _unitOfWork.SaveChangesAsync();
      }
    }

    #endregion

    #region GetPostReacts

    public async Task<ICollection<PostReactDetailViewModel>> GetPostReacts(long? id)
    {
      var viewModel = await _postReact
        .Where(q => q.PostId == id)
        .AsNoTracking()
        .ProjectTo<PostReactDetailViewModel>(_mappingEngine)
        .ToListAsync();

      return viewModel ?? null;
    }

    #endregion

    #region IsInDb
    public async Task<bool> IsInDb(PostReactInsertViewModel viewModel)
    {
      return await _postReact.AnyAsync(a => a.UserId == viewModel.UserId && a.PostId == viewModel.PostId);
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
