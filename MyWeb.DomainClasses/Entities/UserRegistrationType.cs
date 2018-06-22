using System.ComponentModel.DataAnnotations;

namespace MyWeb.DomainClasses.Entities
{
    public enum UserRegistrationType : int
    {
        /// <summary>
        /// Standard account creation
        /// </summary>
        [Display(Name = "استاندارد")]
        Standard = 1,
        /// <summary>
        /// Email validation is required after registration
        /// </summary>
        [Display(Name = "فعال سازی با ایمیل")]
        EmailValidation = 2,
        /// <summary>
        /// A customer should be approved by administrator
        /// </summary>
        [Display(Name = "با تآیید مدیر")]
        AdminApproval = 3,
        /// <summary>
        /// Registration is disabled
        /// </summary>
        [Display(Name = "غیر فعال")]
        Disabled = 4,
    }
}
