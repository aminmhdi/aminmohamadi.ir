using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace MyWeb.ServiceLayer
{
  public static class AssignableToRolePermissions
  {
    #region Fields

    private static readonly Lazy<IEnumerable<PermissionModel>> PermissionsLazy =
        new Lazy<IEnumerable<PermissionModel>>(GetPermision, LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<IEnumerable<string>> PermissionNamesLazy = new Lazy<IEnumerable<string>>(
        GetPermisionNames, LazyThreadSafetyMode.ExecutionAndPublication);
    #endregion

    #region permissionNames
    public static bool AllowSendPrivateMessage { get; set; }
    public static bool AllowSendNewsItem { get; set; }
    public static bool AllowSendBlogPostDraft { get; set; }
    public static bool AllowSendPollItem { get; set; }
    public static bool AllowSendFriendRequest { get; set; }
    public static bool CanUploadFile { get; set; }
    public static bool CanChangeProfilePicture { get; set; }
    public static bool CanModifyFirsAndLastName { get; set; }

    public const string CanEditRole = "CanEditRole";
    public const string CanDeleteRole = "CanDeleteRole";
    public const string CanViewRolesList = "CanViewRolesList";
    public const string CanCreateRole = "CanCreateRole";

    public const string CanEditUser = "CanEditUser";
    public const string CanCreateUser = "CanCreateUser";
    public const string CanDeleteUser = "CanDeleteUser";
    public const string CanSoftDeleteUser = "CanSoftDeleteUser";
    public const string CanViewUsersList = "CanViewUsersList";
    public const string CanEditUsersSetting = "CanEditUsersSetting";
    public const string CanAccessImages = "CanAccessImages";
    public const string CanViewAdminPanel = "CanViewAdminPanel";
    public const string CanAccessUsersFiles = "CanAccessUsersFiles";
    public const string CanAccessUsersAvatar = "CanAccessUsersAvatar";

    public const string CanAccessCategoryList = "CanAccessCategoryList";
    public const string CanCreateCategory = "CanCreateCategory";
    public const string CanEditCategory = "CanEditCategory";
    public const string CanDeleteCategory = "CanDeleteCategory";

    public const string CanAccessCommentList = "CanAccessCommentList";
    public const string CanCreateComment = "CanCreateComment";
    public const string CanEditComment = "CanEditComment";
    public const string CanDeleteComment = "CanDeleteComment";

    public const string CanAccessDashboard = "CanAccessDashboard";

    public const string CanAccessPostList = "CanAccessPostList";
    public const string CanCreatePost = "CanCreatePost";
    public const string CanEditPost = "CanEditPost";
    public const string CanDeletePost = "CanDeletePost";

    #endregion //permissions

    #region Permissions

    public static readonly PermissionModel CanEditRolePermission = new PermissionModel { Name = CanEditRole, Description = "ویرایش نقش" };
    public static readonly PermissionModel CanDeleteRolePermission = new PermissionModel { Name = CanDeleteRole, Description = "حذف نقش" };
    public static readonly PermissionModel CanViewRolesListPermission = new PermissionModel { Name = CanViewRolesList, Description = "مشاهده نقشها" };
    public static readonly PermissionModel CanCreateRolePermission = new PermissionModel { Name = CanCreateRole, Description = "ساخت نقش" };

    public static readonly PermissionModel CanEditUserPermission = new PermissionModel { Name = CanEditUser, Description = "ویرایش کاربر" };
    public static readonly PermissionModel CanCreateUserPermission = new PermissionModel { Name = CanCreateUser, Description = "ساخت کاربر" };
    public static readonly PermissionModel CanViewUsersListPermission = new PermissionModel { Name = CanViewUsersList, Description = "مشاهده کاربران" };
    public static readonly PermissionModel CanDeleteUserPermission = new PermissionModel { Name = CanDeleteUser, Description = "حذف کاربر" };
    public static readonly PermissionModel CanSoftDeleteUserPermission = new PermissionModel { Name = CanSoftDeleteUser, Description = "حذف منطقی کاربر" };
    public static readonly PermissionModel CanViewAdminPanelPermission = new PermissionModel { Name = CanViewAdminPanel, Description = "مشاهده پنل مدیریت" };
    public static readonly PermissionModel CanEditUsersSettingPermission = new PermissionModel { Name = CanEditUsersSetting, Description = "ویرایش تنظیمات کابران" };
    public static readonly PermissionModel CanAccessImagesPermission = new PermissionModel { Name = CanAccessImages, Description = "دسترسی به تصاویر" };
    public static readonly PermissionModel CanAccessUsersAvatarPermission = new PermissionModel { Name = CanAccessUsersAvatar, Description = "تصویر کاربران" };
    public static readonly PermissionModel CanAccessUsersFilesPermission = new PermissionModel { Name = CanAccessUsersFiles, Description = "دانلود فایهای ضمیمه شده" };

    public static readonly PermissionModel CanAccessCategoryListPermission = new PermissionModel { Name = CanAccessCategoryList, Description = "مشاهده دسته بندی ها" };
    public static readonly PermissionModel CanCreateCategoryPermission = new PermissionModel { Name = CanCreateCategory, Description = "ساخت دسته بندی" };
    public static readonly PermissionModel CanEditCategoryPermission = new PermissionModel { Name = CanEditCategory, Description = "ویرایش دسته بندی" };
    public static readonly PermissionModel CanDeleteCategoryPermission = new PermissionModel { Name = CanDeleteCategory, Description = "حذف دسته بندی" };

    public static readonly PermissionModel CanAccessCommentListPermission = new PermissionModel { Name = CanAccessCommentList, Description = "مشاهده نظرات" };
    public static readonly PermissionModel CanCreateCommentPermission = new PermissionModel { Name = CanCreateComment, Description = "ساخت نظر" };
    public static readonly PermissionModel CanEditCommentPermission = new PermissionModel { Name = CanEditComment, Description = "ویرایش نظر" };
    public static readonly PermissionModel CanDeleteCommentPermission = new PermissionModel { Name = CanDeleteComment, Description = "حذف نظر" };

    public static readonly PermissionModel CanAccessDashboardPermission = new PermissionModel { Name = CanAccessDashboard, Description = "مشاهده داشبورد" };

    public static readonly PermissionModel CanAccessPostListPermission = new PermissionModel { Name = CanAccessPostList, Description = "مشاهده نوشته ها" };
    public static readonly PermissionModel CanCreatePostPermission = new PermissionModel { Name = CanCreatePost, Description = "ساخت نوشته" };
    public static readonly PermissionModel CanEditPostPermission = new PermissionModel { Name = CanEditPost, Description = "ویرایش نوشته" };
    public static readonly PermissionModel CanDeletePostPermission = new PermissionModel { Name = CanDeletePost, Description = "حذف نوشته" };

    #endregion

    #region Properties
    public static IEnumerable<PermissionModel> Permissions
    {
      get
      {
        return PermissionsLazy.Value;
      }
    }

    public static IEnumerable<string> PermissionNames
    {
      get
      {
        return PermissionNamesLazy.Value;
      }
    }

    #endregion

    #region GetAllPermisions
    private static IEnumerable<PermissionModel> GetPermision()
    {
      return new List<PermissionModel>
            {
                CanAccessImagesPermission,
                CanAccessUsersAvatarPermission,
                CanAccessUsersFilesPermission,
                CanCreateRolePermission,
                CanCreateUserPermission,
                CanDeleteRolePermission,
                CanDeleteUserPermission,
                CanEditRolePermission,
                CanEditUserPermission,
                CanEditUsersSettingPermission,
                CanSoftDeleteUserPermission,
                CanViewAdminPanelPermission,
                CanViewRolesListPermission,
                CanViewUsersListPermission,

                CanAccessCategoryListPermission,
                CanCreateCategoryPermission,
                CanEditCategoryPermission,
                CanDeleteCategoryPermission,

                CanAccessCommentListPermission,
                CanCreateCommentPermission,
                CanEditCommentPermission,
                CanDeleteCommentPermission,

                CanAccessDashboardPermission,

                CanAccessPostListPermission,
                CanCreatePostPermission,
                CanEditPostPermission,
                CanDeletePostPermission
            };
    }

    private static IEnumerable<string> GetPermisionNames()
    {
      return new List<string>()
            {
               CanEditRole ,
               CanDeleteRole,
               CanViewRolesList ,
               CanCreateRole ,
               CanEditUser ,
               CanCreateUser ,
               CanDeleteUser,
               CanSoftDeleteUser ,
               CanViewUsersList ,
               CanEditUsersSetting ,
               CanAccessImages ,
               CanViewAdminPanel ,
               CanAccessUsersFiles ,
               CanAccessUsersAvatar,


                CanAccessCategoryList,
                CanCreateCategory,
                CanEditCategory,
                CanDeleteCategory,

                CanAccessCommentList,
                CanCreateComment,
                CanEditComment,
                CanDeleteComment,

                CanAccessDashboard,

                CanAccessPostList,
                CanCreatePost,
                CanEditPost,
                CanDeletePost
            };
    }
    #endregion

    #region GetAsSelectedList

    public static IEnumerable<SelectListItem> GetAsSelectListItems()
    {
      return Permissions.Select(a => new SelectListItem { Text = a.Description, Value = a.Name }).ToList();
    }
    #endregion
  }
}
