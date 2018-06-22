using System.ComponentModel;

namespace MyWeb.ViewModel.User
{
    public class UserViewModel
    {
        public long Id { get; set; }
        [DisplayName("ایمیل")]
        public string Email { get; set; }
        [DisplayName("نام کاربری")]
        public string UserName { get; set; }
        [DisplayName("قفل")]
        public bool IsBanned { get; set; }
        [DisplayName("حذف")]
        public bool IsDeleted { get; set; }
        [DisplayName("تاییده ایمیل")]
        public bool EmailConfirmed { get; set; }
        [DisplayName("کاربر سیستمی")]
        public bool IsSystemAccount { get; set; }
        [DisplayName("نام نمایشی")]
        public string NameForShow { get; set; }
        [DisplayName("تاریخ عضویت")]
        public string RegisterDate { get; set; }
        [DisplayName(" آخرین فعالیت")]
        public string LastActivityDate { get; set; }
        [DisplayName("نقش")]
        public string Roles { get; set; }
    }
}
