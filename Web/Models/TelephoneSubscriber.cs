namespace Web.Models;

public class TelephoneSubscriber
{
    public TelephoneSubscriber(IReadOnlyList<string> info) => _info = info;
    
    public string FirstName => _info[0];
    
    public string LastName => _info[1];
    
    public string PhoneNumber => _info[2];
    
    public string Address => _info[3];

    private readonly IReadOnlyList<string> _info;
}