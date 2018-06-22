using System.Collections.Generic;

namespace MyWeb.ViewModel.User
{
    public class UserListViewModel
    {
        public IList<UserViewModel> Users { get; set; }
        public UserSearchRequest SearchRequest { get; set; }
    }
}
