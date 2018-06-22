using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyWeb.DomainClasses.Entities;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.ServiceLayer.EFServices.Users;

namespace MyWeb.ServiceLayer.Common
{
    public class ApplicationSignInManager : SignInManager<User, long>, IApplicationSignInManager
    {

        #region Fields
        private readonly ApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;
        #endregion

        #region Constructor

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }
        #endregion


    }
}
