using System.Text;
using Bogus;
using Bogus.DataSets;
using Web.Models;

namespace Web.Daos;

public static class TelephoneSubscriberDao
{
    public static void WriteRandomTelephoneSubscribersToFile(Encoding encoding, string path, int numberOfRandomTelephoneSubscribers)
    {
        var faker = new Faker("ru");

        var writer = new StreamWriter(path, false, encoding);

        for (var i = 0; i < numberOfRandomTelephoneSubscribers - 1; i++)
        {
            writer.Write($"{faker.Name.FirstName(Name.Gender.Male)}; ");
            writer.Write($"{faker.Name.LastName(Name.Gender.Male)}; ");
            writer.Write($"{faker.Phone.PhoneNumber("+7 (###) ###-##-##")}; ");
            writer.WriteLine(
                $"{faker.Address.ZipCode()}, г. {faker.Address.City()}, {faker.Address.StreetAddress()}, кв. {faker.Random.Int(1, 100)}");
        }
        
        writer.Write($"{faker.Name.FirstName(Name.Gender.Male)}; ");
        writer.Write($"{faker.Name.LastName(Name.Gender.Male)}; ");
        writer.Write($"{faker.Phone.PhoneNumber("+7 (###) ###-##-##")}; ");
        writer.Write(
            $"{faker.Address.ZipCode()}, г. {faker.Address.City()}, {faker.Address.StreetAddress()}, кв. {faker.Random.Int(1, 100)}");

        writer.Dispose();
    }
    
    public static IReadOnlyList<TelephoneSubscriber> ReadRandomTelephoneSubscribersFromFile(string path, Encoding encoding)
    {
        var serializedSubscribers = File.ReadAllLines(path, encoding);
        
        var deserializedSubscribers = new TelephoneSubscriber[serializedSubscribers.Length];

        for (var i = 0; i < deserializedSubscribers.Length; i++)
            deserializedSubscribers[i] = new TelephoneSubscriber(serializedSubscribers[i].Split("; "));

        return deserializedSubscribers;
    }
}