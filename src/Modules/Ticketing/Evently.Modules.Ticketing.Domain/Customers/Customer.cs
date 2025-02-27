using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Customers;

public sealed class Customer : Entity
{

    // this is the same as the User.cs

    // but this is specific and scoped to only this module,

    // this willl duplicate data from User.cs and store in its own Db
    private Customer()
    {
    }

    public string Id { get; private set; }

    public string Email { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public static Customer Create(string id, string email, string firstName, string lastName)
    {
        return new Customer
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };
    }

    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
