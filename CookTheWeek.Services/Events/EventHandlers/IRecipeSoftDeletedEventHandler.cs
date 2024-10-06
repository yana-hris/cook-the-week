namespace CookTheWeek.Services.Data.Events.EventHandlers
{
    using System.Threading.Tasks;

    public interface IRecipeSoftDeletedEventHandler
    {
        Task HandleAsync(RecipeSoftDeletedEvent domainEvent);
    }
}
