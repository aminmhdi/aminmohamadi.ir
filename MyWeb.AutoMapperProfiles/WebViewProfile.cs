using AutoMapper;
using MyWeb.DomainClasses.Entities;
using MyWeb.ViewModel.WebView;

namespace MyWeb.AutoMapperProfiles
{
  public class WebViewProfile : Profile
  {

    protected override void Configure()
    {
      CreateMap<WebViewInsertViewModel, WebView>()
          .IgnoreAllNonExisting();
    }

    public override string ProfileName
    {
      get { return GetType().Name; }
    }
  }
}
