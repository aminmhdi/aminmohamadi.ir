using System;
using System.Collections.Generic;

namespace MyWeb.ServiceLayer
{
  public static class StandardRoles
  {
    #region Fields

    private static Lazy<IEnumerable<PermissionRecord>> _rolesWithPermissionsLazy =
        new Lazy<IEnumerable<PermissionRecord>>();
    private static Lazy<IEnumerable<string>> _rolesLazy = new Lazy<IEnumerable<string>>();
    #endregion

    #region Properties
    public static IEnumerable<string> SystemRoles
    {
      get
      {
        if (_rolesLazy.IsValueCreated)
          return _rolesLazy.Value;
        _rolesLazy = new Lazy<IEnumerable<string>>(GetSysmteRoles);
        return _rolesLazy.Value;
      }
    }

    public static IEnumerable<PermissionRecord> SystemRolesWithPermissions
    {
      get
      {
        if (_rolesWithPermissionsLazy.IsValueCreated)
          return _rolesWithPermissionsLazy.Value;
        _rolesWithPermissionsLazy = new Lazy<IEnumerable<PermissionRecord>>(GetDefaultRolesWithPermissions);
        return _rolesWithPermissionsLazy.Value;
      }
    }

    public static IEnumerable<PermissionRecord> SystemRemoteRolesWithPermissions
    {
      get
      {
        if (_rolesWithPermissionsLazy.IsValueCreated)
          return _rolesWithPermissionsLazy.Value;

        _rolesWithPermissionsLazy = new Lazy<IEnumerable<PermissionRecord>>(GetDefaultRolesWithPermissions);
        return _rolesWithPermissionsLazy.Value;
      }
    }



    #endregion

    #region DefaultRoles
    public const string Administrators = "مدیران";
    public const string Writers = "نویسندگان سایت";
    public const string ForumAdmins = "مدیران انجمن";
    public const string RegisterdUsers = "کاربران عضو شده";
    #endregion

    #region GetSystemRoles
    private static IEnumerable<string> GetSysmteRoles()
    {
      return new List<string>
            {
                Administrators,
                Writers,
                ForumAdmins,
                RegisterdUsers,

            };
    }
    #endregion

    #region GetDefaultRolesWithPermissions
    private static IEnumerable<PermissionRecord> GetDefaultRolesWithPermissions()
    {
      return new List<PermissionRecord>
            {
                new PermissionRecord
                {
                    RoleName = Administrators,
                    IsDefaultForRegister = false,
                    Permissions = AssignableToRolePermissions.Permissions
                },
                new PermissionRecord
                {
                    RoleName = Writers,
                    IsDefaultForRegister = false,
                    Permissions = AssignableToRolePermissions.Permissions
                },

                new PermissionRecord
                {
                    RoleName = ForumAdmins,
                    IsDefaultForRegister = false,
                    Permissions = AssignableToRolePermissions.Permissions
                },

                new PermissionRecord
                {
                    RoleName = RegisterdUsers,
                    IsDefaultForRegister = true,
                    Permissions = new List<PermissionModel>
                    {
                        AssignableToRolePermissions.CanAccessImagesPermission,
                        AssignableToRolePermissions.CanAccessUsersFilesPermission,
                        AssignableToRolePermissions.CanAccessUsersAvatarPermission
                    }
                }
            };
    }
    #endregion





  }
}
