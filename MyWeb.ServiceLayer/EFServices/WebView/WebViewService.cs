using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using MyWeb.DataLayer.Context;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.ViewModel.WebView;

namespace MyWeb.ServiceLayer.EFServices.WebView
{
  public class WebViewService : IWebViewService
  {
    #region Fields

    private readonly IMappingEngine _mappingEngine;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbSet<DomainClasses.Entities.WebView> _webView;
    #endregion

    #region Ctor

    public WebViewService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
    {
      _unitOfWork = unitOfWork;
      _webView = _unitOfWork.Set<DomainClasses.Entities.WebView>();
      _mappingEngine = mappingEngine;
    }
    #endregion

    #region Create
    public async Task Create(WebViewInsertViewModel viewModel)
    {
      try
      {
        if (!await IsInDb(viewModel))
        {
          _webView.Add(_mappingEngine.Map<DomainClasses.Entities.WebView>(viewModel));
          await _unitOfWork.SaveChangesAsync();
        }

        else
        {
          var webView = await _webView.FirstAsync(a => a.Session == viewModel.Session);
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

    #region IsInDb
    public async Task<bool> IsInDb(WebViewInsertViewModel viewModel)
    {
      return await _webView.AnyAsync(a => a.Session == viewModel.Session && a.Ip == viewModel.Ip);
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
