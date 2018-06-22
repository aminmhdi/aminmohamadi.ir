using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using MyWeb.DataLayer.Context;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.ViewModel.WebPostView;
using System.Linq;
using MyWeb.DomainClasses.Entities;
using MyWeb.ViewModel.Comment;
using MyWeb.ViewModel.Dashboard;

namespace MyWeb.ServiceLayer.EFServices.WebPostView
{
  public class WebPostViewService : IWebPostViewService
  {
    #region Fields

    private readonly IMappingEngine _mappingEngine;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbSet<DomainClasses.Entities.WebPostView> _webPostView;
    #endregion

    #region Ctor

    public WebPostViewService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
    {
      _unitOfWork = unitOfWork;
      _webPostView = _unitOfWork.Set<DomainClasses.Entities.WebPostView>();
      _mappingEngine = mappingEngine;
    }
    #endregion

    #region Create
    public async Task Create(WebPostViewInsertViewModel viewModel)
    {
      try
      {
        if (!await IsInDb(viewModel))
        {
          if (!await IsInDbSameUserId(viewModel))
          {
            _webPostView.Add(_mappingEngine.Map<DomainClasses.Entities.WebPostView>(viewModel));
            await _unitOfWork.SaveChangesAsync();
          }
        }

        else
        {
          var webView = await _webPostView.FirstAsync(a => a.Session == viewModel.Session && a.PostId == viewModel.PostId);
          _mappingEngine.Map(viewModel, webView);
          await _unitOfWork.SaveChangesAsync();
        }
      }
      catch (Exception e)
      {
        Console.Write(e);
      }
    }

    #endregion

    #region MostViewPosts

    public async Task<List<MostViewedPostDashboardViewModel>> GetMostViewPosts()
    {
      return await _webPostView
        .Include(q => q.Post)
        .Where(q => q.Post.IsActive)
        .GroupBy(q => q.Post.Id)
        .OrderByDescending(id => id.Count())
        .Select(s => new
        {
          s.FirstOrDefault().Post,
          Count = s.Count()
        })
        .Select(x => new MostViewedPostDashboardViewModel
        {
          CategoryName = x.Post.Category.Title,
          CreatedOn = x.Post.CreatedOn,
          Comments = x.Post.Comments,
          IsActive = x.Post.IsActive,
          Title = x.Post.Title,
          CreatorName = x.Post.Creator.NameForShow,
          Id = x.Post.Id,
          PostReacts = x.Post.Reacts,
          ViewCount = x.Count
        })
        .Skip(() => 0)
        .Take(() => 6)
        .ToListAsync();
    }

    #endregion

    #region IsInDb
    public async Task<bool> IsInDb(WebPostViewInsertViewModel viewModel)
    {
      return await _webPostView.AnyAsync(a => a.Session == viewModel.Session && a.Ip == viewModel.Ip && a.PostId == viewModel.PostId && a.UserId == viewModel.UserId);
    }
    #endregion

    #region IsInDbSameUserId
    public async Task<bool> IsInDbSameUserId(WebPostViewInsertViewModel viewModel)
    {
      var r = await _webPostView.AnyAsync(a => a.PostId == viewModel.PostId & a.Session == viewModel.Session & a.UserId == viewModel.UserId & a.Ip == viewModel.Ip);
      return r;
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
