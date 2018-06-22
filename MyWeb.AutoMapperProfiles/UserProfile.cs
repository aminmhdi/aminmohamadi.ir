using System;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using AutoMapper;
using MyWeb.DomainClasses.Entities;
using MyWeb.ViewModel.Account;
using MyWeb.ViewModel.User;

namespace MyWeb.AutoMapperProfiles
{
  public class UserProfile : Profile
  {
    protected override void Configure()
    {
      CreateMap<DateTime, string>().ConvertUsing(new ToPersianDateTimeConverter());

      CreateMap<User, UserViewModel>()
          .ForMember(d => d.Roles, m => m.Ignore()).IgnoreAllNonExisting();

      CreateMap<AddUserViewModel, User>()
          .ForMember(d => d.RegisterDate, m => m.MapFrom(s => DateTime.Now))
          .ForMember(d => d.LastActivityDate, m => m.MapFrom(s => DateTime.Now))
          .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserName.ToLower()))
          .IgnoreAllNonExisting();

      CreateMap<EditUserViewModel, User>()
          .ForMember(d => d.Roles, m => m.MapFrom(s => new Collection<UserRole>()))
          .ForMember(d => d.RegisterDate, m => m.Ignore())
          .ForMember(d => d.LastActivityDate, m => m.Ignore())
          .ForMember(d => d.BirthDay, m => m.Ignore())
          .ForMember(d => d.EmailConfirmed, m => m.Ignore())
          .ForMember(d => d.Email, m => m.Ignore())
          .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserName.ToLower()))
          .IgnoreAllNonExisting();

      CreateMap<User, EditUserViewModel>()
        .IgnoreAllNonExisting();

      CreateMap<RegisterViewModel, User>()
          .ForMember(d => d.RegisterDate, a => a.MapFrom(s => DateTime.Now))
          .ForMember(d => d.LastActivityDate, m => m.MapFrom(s => DateTime.Now))
          .ForMember(d => d.AvatarFileName, a => a.MapFrom(s => "user.svg"))
          .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserName.ToLower()))
          .IgnoreAllNonExisting();

      CreateMap<User, SelectListItem>()
        .ForMember(d => d.Text, m => m.MapFrom(s => s.NameForShow))
        .ForMember(d => d.Value, m => m.MapFrom(s => s.Id));

      CreateMap<User, EditUserMainPageViewModel>()
        .IgnoreAllNonExisting();

      CreateMap<EditUserMainPageViewModel, User>()
        .IgnoreAllNonExisting();
    }

    public override string ProfileName
    {
      get { return GetType().Name; }
    }
  }
}
