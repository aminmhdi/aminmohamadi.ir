using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StructureMap.Configuration.DSL;
using StructureMap.Web;
using MyWeb.DataLayer.Context;
using MyWeb.DomainClasses.Entities;
using MyWeb.ServiceLayer.Common;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.ServiceLayer.EFServices.Roles;
using MyWeb.ServiceLayer.EFServices.Users;

namespace MyWeb.IocConfig
{
    public class AspNetIdentityRegistery : Registry
    {
        public AspNetIdentityRegistery()
        {
            For<MyWebContext>().HybridHttpOrThreadLocalScoped()
                               .Use(context => (MyWebContext)context.GetInstance<IUnitOfWork>());
            For<DbContext>().HybridHttpOrThreadLocalScoped()
                 .Use(context => (MyWebContext)context.GetInstance<IUnitOfWork>());

            For<IUserStore<User, long>>()
                 .HybridHttpOrThreadLocalScoped()
                 .Use<CustomUserStore>();

            For<IRoleStore<Role, long>>()
                 .HybridHttpOrThreadLocalScoped()
                 .Use<RoleStore<Role, long, UserRole>>();

            For<IAuthenticationManager>()
                 .Use(() => HttpContext.Current.GetOwinContext().Authentication);

            For<IApplicationSignInManager>()
                 .HybridHttpOrThreadLocalScoped()
                 .Use<ApplicationSignInManager>();

            For<IApplicationRoleManager>()
                 .HybridHttpOrThreadLocalScoped()
                 .Use<RoleManager>();

            //For<IIdentityMessageService>().Use<SmsService>();
            For<IIdentityMessageService>().Use<EmailService>();

            For<IApplicationUserManager>().HybridHttpOrThreadLocalScoped()
               .Use<ApplicationUserManager>()
               //.Ctor<IIdentityMessageService>("smsService").Is<SmsService>()
               .Ctor<IIdentityMessageService>("emailService").Is<EmailService>()
               //.Setter<IIdentityMessageService>(userManager => userManager.SmsService).Is<SmsService>()
               .Setter<IIdentityMessageService>(userManager => userManager.EmailService).Is<EmailService>();

            For<ApplicationUserManager>().HybridHttpOrThreadLocalScoped()
                 .Use(context => (ApplicationUserManager)context.GetInstance<IApplicationUserManager>());

            For<ICustomRoleStore>()
                      .HybridHttpOrThreadLocalScoped()
                      .Use<CustomRoleStore>();

            For<ICustomUserStore>()
                  .HybridHttpOrThreadLocalScoped()
                  .Use<CustomUserStore>();

            Policies.SetAllProperties(y =>
            {
                y.OfType<IApplicationUserManager>();
                y.OfType<IAuthenticationManager>();
            });
        }
    }

}
