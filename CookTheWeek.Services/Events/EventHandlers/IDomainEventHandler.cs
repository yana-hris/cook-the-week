namespace CookTheWeek.Services.Data.Events.EventHandlers
{
    using System.Threading.Tasks;

    public interface IDomainEventHandler<TEvent>
    {
        Task HandleAsync(TEvent domainEvent);
    }
}
