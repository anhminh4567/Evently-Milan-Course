using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Evently.ArchitectureTests.Abstractions;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure;
using NetArchTest.Rules;

namespace Evently.ArchitectureTests.Layers;
public class ModuleTests : BaseTest
{
    [Fact]
    public void UserModules_ShouldNotHaveDependenciesOn_AnyOtherModules()
    {
        string[] otherModules = [EventsNamespace, TicketingNamespace, AttendanceNamespace];
        string[] integrationEventModules = [EventsIntegrationEventsNamespace, UsersIntegrationEventsNamespace, AttendanceIntegrationEventsNamespace];

        var userAssemblies = new List<Assembly>()
        {
            typeof(User).Assembly ,// domain
            Modules.Users.Application.AssemblyReference.Assembly,
            Modules.Users.Presentation.AssemblyReference.Assembly,
            typeof(UsersModule).Assembly
        };
        // test here
        Types.InAssemblies(userAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessFul();
    }
    [Fact]
    public void TicketingModules_ShouldNotHaveDependenciesOn_AnyOtherModules()
    {
        string[] otherModules = [UsersNamespace, EventsNamespace, AttendanceNamespace];
        string[] integrationEventModules = [EventsIntegrationEventsNamespace, UsersIntegrationEventsNamespace, AttendanceIntegrationEventsNamespace];

        var ticketingAssemblies = new List<Assembly>()
        {
            typeof(Order).Assembly ,// domain
            Modules.Ticketing.Application.AssemblyReference.Assembly,
            Modules.Ticketing.Presentation.AssemblyReference.Assembly,
            typeof(TicketingModule).Assembly
        };
        // test here
        Types.InAssemblies(ticketingAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessFul();
    }
    [Fact]
    public void EventsModules_ShouldNotHaveDependenciesOn_AnyOtherModules()
    {
        string[] otherModules = [UsersNamespace, TicketingNamespace, AttendanceNamespace];
        string[] integrationEventModules = [TicketingIntegrationEventsNamespace, UsersIntegrationEventsNamespace, AttendanceIntegrationEventsNamespace];

        var EventsAssemblies = new List<Assembly>()
        {
            typeof(Event).Assembly ,// domain
            Modules.Events.Application.MetaClass.EventApplicationAssembly,
            Modules.Events.Presentation.MetaClass.Assembly,
            typeof(EventsModule).Assembly
        };
        // test here
        Types.InAssemblies(EventsAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessFul();
    }
}
