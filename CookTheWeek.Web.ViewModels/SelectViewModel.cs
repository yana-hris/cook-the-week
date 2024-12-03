namespace CookTheWeek.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class SelectViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
