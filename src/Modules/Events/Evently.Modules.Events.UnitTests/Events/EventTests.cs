using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evently.Common.Domain;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.Events.DomainEvents;
using Evently.Modules.Events.UnitTests.Abstractions;
using FluentAssertions;

namespace Evently.Modules.Events.UnitTests.Events;
public class EventTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnFailrure_WhenEndDatePrecedesStartDate()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;
        DateTime endAtUtc = startAtUtc.AddMinutes(-1);
        //Act

        Result<Event> result = Event.Create(
            category, 
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startAtUtc, 
            endAtUtc);
        //Assert    
        result.Error.Should().Be(EventErrors.EndDatePrecedesStartDate);
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenEventCreated()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;
        //Act

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startAtUtc,
            null);
        //Assert    
        result.IsFailure.Should().BeFalse();
        EventCreatedDomainEvent domainEvent = AssertDomainEventWasPublished<EventCreatedDomainEvent>(result.Value);
        domainEvent.EventId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Publish_ShouldReturnFailure_WhenEventNotDraft()
    {
        //Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        @event.Publish();

        //Act
        Result publishResult = @event.Publish();

        //Assert
        publishResult.Error.Should().Be(EventErrors.NotDraft);
    }


    [Fact]
    public void Publish_ShouldRaiseDomainEvent_WhenEventPublished()
    {
        //Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        //Act
        @event.Publish();

        //Assert
        EventPublishedDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventPublishedDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventRescheduled()
    {
        //Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        //Act
        @event.Reschedule(startsAtUtc.AddDays(1), startsAtUtc.AddDays(2));

        //Assert
        EventRescheduledDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventRescheduledDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventCanceled()
    {
        //Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        //Act
        @event.Cancel(startsAtUtc.AddMinutes(-1));

        //Assert
        EventCanceledDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventCanceledDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyCanceled()
    {
        //Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        @event.Cancel(startsAtUtc.AddMinutes(-1));

        //Act
        Result cancelResult = @event.Cancel(startsAtUtc.AddMinutes(-1));

        //Assert
        cancelResult.Error.Should().Be(EventErrors.AlreadyCanceled);
    }

    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyStarted()
    {
        //Arrange
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        //Act
        Result cancelResult = @event.Cancel(startsAtUtc.AddMinutes(1));

        //Assert
        cancelResult.Error.Should().Be(EventErrors.AlreadyStarted);
    }
    [Fact]
    public void TestResultError()
    {
        var error1 = EventErrors.EndDatePrecedesStartDate;
        var error2 = EventErrors.EndDatePrecedesStartDate;

        error1.Should().Be(error2);
    }
}
