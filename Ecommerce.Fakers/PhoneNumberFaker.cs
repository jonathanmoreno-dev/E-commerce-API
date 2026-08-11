using Bogus;
using PhoneNumbers;

namespace Ecommerce.Fakers;

public static class PhoneNumberFaker
{
    private static readonly PhoneNumberUtil PhoneUtils =
        PhoneNumberUtil.GetInstance();

    private static readonly string[] Regions =
        PhoneUtils
            .GetSupportedRegions()
            .Where(x => !x.Equals("001", StringComparison.OrdinalIgnoreCase))
            .ToArray();

    public static string Generate()
    {
        var faker = new Faker();

        for (var attempt = 0; attempt < 100; attempt++)
        {
            var region = faker.PickRandom(Regions);

            var example = PhoneUtils.GetExampleNumber(region);

            if (example is null)
                continue;

            var formatted = PhoneUtils.Format(
                example,
                PhoneNumberFormat.E164);

            if (PhoneUtils.IsValidNumber(example))
                return formatted;
        }

        throw new InvalidOperationException(
            "Could not generate a valid international phone number.");
    }
}