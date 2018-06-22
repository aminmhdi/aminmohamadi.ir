using AutoMapper;
using MyWeb.DomainClasses.Entities;
using MyWeb.ViewModel.Comment;

namespace MyWeb.AutoMapperProfiles
{
  public class CommentProfile : Profile
  {

    protected override void Configure()
    {
      CreateMap<Comment, CommentEditViewModel>()
          .IgnoreAllNonExisting();

      CreateMap<Comment, CommentDetailViewModel>()
          .IgnoreAllNonExisting();

      CreateMap<CommentInsertViewModel, Comment>()
          .IgnoreAllNonExisting();

      CreateMap<CommentEditViewModel, Comment>()
          .IgnoreAllNonExisting();
    }

    public override string ProfileName
    {
      get { return GetType().Name; }
    }
  }
}
