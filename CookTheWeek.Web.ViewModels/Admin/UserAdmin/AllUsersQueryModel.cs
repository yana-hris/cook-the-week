namespace CookTheWeek.Web.ViewModels.Admin.UserAdmin
{
   
    public class AllUsersQueryModel
    {
        public AllUsersQueryModel()
        {
            this.UserSortings = new HashSet<SelectViewModel>();
            this.Users = new List<UserAllViewModel>();
        }

        // Filters
        public string? SearchString { get; set; }



        // Pagination
        public int CurrentPage { get; set; }
        public int UsersPerPage { get; set; }
        public int TotalUsers { get; set; }


        // Sorting
        public int? UserSorting { get; set; }
        public ICollection<SelectViewModel> UserSortings { get; set; } = null!;



        // Result Collection
        public ICollection<UserAllViewModel> Users { get; set; } = null!;


        
    }
}
