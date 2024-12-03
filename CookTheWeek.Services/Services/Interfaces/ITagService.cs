namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Web.ViewModels;

    public interface ITagService
    {
        Task<ICollection<SelectViewModel>> GetAllTagsAsync();
    }
}
