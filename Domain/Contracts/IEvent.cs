using MediatR;

namespace Domain.Contracts;

public interface IEvent : INotification
{
    
}

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{

}