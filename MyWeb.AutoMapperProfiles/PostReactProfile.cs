using AutoMapper;
using MyWeb.DomainClasses.Entities;
using MyWeb.ViewModel.PostReact;

namespace MyWeb.AutoMapperProfiles
{
  public class PostReactProfile : Profile
  {
    protected override void Configure()
    {
      CreateMap<PostReact, PostReactDetailViewModel>()
        .IgnoreAllNonExisting();

      CreateMap<PostReactInsertViewModel, PostReact>()
            .IgnoreAllNonExisting();
    }

    public override string ProfileName
    {
      get { return GetType().Name; }
    }
  }
}
