using System.ComponentModel.DataAnnotations;

namespace MyWeb.DomainClasses.Entities
{
    /// <summary>
    /// Represents the customer name fortatting enumeration
    /// </summary>
    public enum UserNameShowFormat : int
    {
        /// <summary>
        /// Show emails
        /// </summary>
        [Display(Name = "نمایش ایمیل")]
        ShowEmail = 1,
        /// <summary>
        /// Show usernames
        /// </summary>
        [Display(Name = "نایش نام کاربری")]
        ShowUsername = 2,
        /// <summary>
        /// Show first name
        /// </summary>
        [Display(Name = "نمایش نام")]
        ShowName = 3
    }

}
