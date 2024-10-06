namespace CookTheWeek.Data.Events.EventHandlers
{
    using System.Threading.Tasks;

    using CookTheWeek.Data.Events.Events;

    public interface IRecipeSoftDeletedEventHandler
    {
        Task HandleAsync(RecipeSoftDeletedEvent domainEvent);
    }
}
