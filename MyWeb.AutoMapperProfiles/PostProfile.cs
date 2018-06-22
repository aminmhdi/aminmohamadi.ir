using AutoMapper;
using MyWeb.DomainClasses.Entities;
using MyWeb.ViewModel.Post;

namespace MyWeb.AutoMapperProfiles
{
    public class PostProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Post, PostEditViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<Post, PostDetailViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<Post, PostMainDetailViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<PostInsertViewModel, Post>()
                .IgnoreAllNonExisting();

            CreateMap<PostEditViewModel, Post>()
                .IgnoreAllNonExisting();
        }

        public override string ProfileName
        {
            get { return GetType().Name; }
        }
    }
}
