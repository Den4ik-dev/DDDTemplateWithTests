using Domain.Product.Events;

namespace Domain.Common.Models;

public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
    void AddDomainEvent(IDomainEvent domainEvent);
}
