using Web.Interfaces;

namespace Web.Commands.Driver;

public class QualifyAdr : ICommand<string>
{
    public void Execute(string stub) => stub += "ghg";
    
    public required int AdrQualificationFlag { get; init; }
    
    public const string CommandName = "QualifyAdrToDriver";
}