using AutoMapper;
using MyWeb.DomainClasses.Entities;
using MyWeb.ViewModel.WebPostView;
using MyWeb.ViewModel.WebView;

namespace MyWeb.AutoMapperProfiles
{
  public class WebPostViewProfile : Profile
  {

    protected override void Configure()
    {
      CreateMap<WebPostViewInsertViewModel, WebPostView>()
          .IgnoreAllNonExisting();
    }

    public override string ProfileName
    {
      get { return GetType().Name; }
    }
  }
}
