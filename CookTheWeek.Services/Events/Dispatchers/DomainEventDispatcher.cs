namespace CookTheWeek.Services.Data.Events.Dispatchers
{
    using System.Threading.Tasks;

    using CookTheWeek.Services.Data.Events.EventHandlers;

    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IEnumerable<IRecipeSoftDeletedEventHandler> eventHandlers;

        public DomainEventDispatcher(IEnumerable<IRecipeSoftDeletedEventHandler> eventHandlers)
        {
            this.eventHandlers = eventHandlers;
        }
        public async Task DispatchAsync<TEvent>(TEvent domainEvent)
        {
            foreach (var handler in eventHandlers)
            {
                if(handler is IRecipeSoftDeletedEventHandler softDeleteHandler)
                {
                    await softDeleteHandler.HandleAsync((RecipeSoftDeletedEvent)(object)domainEvent);
                }
            }
        }
    }
}
