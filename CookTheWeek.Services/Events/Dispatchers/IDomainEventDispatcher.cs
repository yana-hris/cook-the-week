namespace CookTheWeek.Services.Data.Events.Dispatchers
{
    
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent domainEvent);
    }
}
