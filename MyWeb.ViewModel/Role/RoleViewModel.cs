using System.ComponentModel;

namespace MyWeb.ViewModel.Role
{
    public class RoleViewModel
    {
        public long Id { get; set; }
        [DisplayName("نام گروه")]
        public string Name { get; set; }
        [DisplayName("توضیحات")]
        public string Description { get; set; }
        [DisplayName("فعال باشد؟")]
        public virtual bool IsActive { get; set; }
        [DisplayName("گروه سیستمی است؟")]
        public bool IsSystemRole { get; set; }
        [DisplayName("گروه پیشفرض ثبت نام است؟")]
        public bool IsDefaultForRegister { get; set; }
    }
}
