using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.Users;

public class Customer : User
{
    public override UserTypes Type { get; } = UserTypes.Customer;

    public Customer(string name, string password) : base(name, password)
    {
    }
}