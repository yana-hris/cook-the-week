namespace CookTheWeek.Common.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum DifficultyLevel
    {
        [Display(Name = "Easy")]
        Easy = 1,
        [Display(Name = "Medium")]
        Medium = 2,
        [Display(Name = "Hard")]
        Hard = 3
    }
}
