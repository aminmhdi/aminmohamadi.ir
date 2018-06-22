
using System.Web.Mvc;
using AutoMapper;
using MyWeb.DomainClasses.Entities;
using MyWeb.ViewModel.Category;
using MyWeb.ViewModel.Post;

namespace MyWeb.AutoMapperProfiles
{
    public class CategoryProfile : Profile
    {

        protected override void Configure()
        {
            CreateMap<Category, CategoryEditViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<Category, CategoryDetailViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<Category, CategoryMainIdAndTitleDetailViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<CategoryInsertViewModel, Category>()
          .IgnoreAllNonExisting();

            CreateMap<CategoryEditViewModel, Category>()
                .IgnoreAllNonExisting();

            CreateMap<Category, CategoryMenuViewModel>()
              .IgnoreAllNonExisting();

            CreateMap<Category, SelectListItem>()
              .ForMember(d => d.Text, m => m.MapFrom(s => s.Title))
              .ForMember(d => d.Value, m => m.MapFrom(s => s.Id));
        }

        public override string ProfileName
        {
            get { return GetType().Name; }
        }
    }
}
