﻿using System;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.ServiceLayer.Contracts
{
    public class CustomClaimsIdentityFactory : IClaimsIdentityFactory<User, long>
    {
        internal const string IdentityProviderClaimType =
            "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

        internal const string DefaultIdentityProviderClaimValue = "ASP.NET Identity";

        /// <summary>
        ///     Constructor
        /// </summary>
        public CustomClaimsIdentityFactory()
        {
            RoleClaimType = ClaimsIdentity.DefaultRoleClaimType;
            UserIdClaimType = ClaimTypes.NameIdentifier;
            UserNameClaimType = ClaimsIdentity.DefaultNameClaimType;
            SecurityStampClaimType = Constants.DefaultSecurityStampClaimType;
        }

        /// <summary>
        ///     Claim type used for role claims
        /// </summary>
        public string RoleClaimType { get; set; }

        /// <summary>
        ///     Claim type used for the user name
        /// </summary>
        public string UserNameClaimType { get; set; }

        /// <summary>
        ///     Claim type used for the user id
        /// </summary>
        public string UserIdClaimType { get; set; }

        /// <summary>
        ///     Claim type used for the user security stamp
        /// </summary>
        public string SecurityStampClaimType { get; set; }

        /// <summary>
        ///     Create a ClaimsIdentity from a user
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        public virtual async Task<ClaimsIdentity> CreateAsync(UserManager<User, long> manager, User user, string authenticationType)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var id = new ClaimsIdentity(authenticationType, UserNameClaimType, RoleClaimType);
            id.AddClaim(new Claim(UserIdClaimType, user.Id.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.String));
            id.AddClaim(new Claim(UserNameClaimType, user.NameForShow, ClaimValueTypes.String));
            id.AddClaim(new Claim(IdentityProviderClaimType, DefaultIdentityProviderClaimValue, ClaimValueTypes.String));
            if (manager.SupportsUserSecurityStamp)
            {
                id.AddClaim(new Claim(SecurityStampClaimType,
                    await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture()));
            }
            if (manager.SupportsUserRole)
            {
                var roles = await manager.GetRolesAsync(user.Id).WithCurrentCulture();
                foreach (var roleName in roles)
                {
                    id.AddClaim(new Claim(RoleClaimType, roleName, ClaimValueTypes.String));
                }
            }
            if (manager.SupportsUserClaim)
            {
                id.AddClaims(await manager.GetClaimsAsync(user.Id).WithCurrentCulture());
            }
            return id;
        }


    }
}
