using Bogus;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Fakers;

public class UserFaker : Faker<User>
{
    private string _passwordHash = "fake_hash";
    public UserFaker()
    {
        CustomInstantiator(f => new User(
            new PersonName(f.Person.FullName),
            new Email(f.Person.Email),
            new PhoneNumber(PhoneNumberFaker.Generate()),
            _passwordHash));
    }
    public static string GetAvatarImageUrl()
    {
        var faker = new Faker();
        return faker.Internet.Avatar();
    }
    public static User Create() => new UserFaker().Generate();
}