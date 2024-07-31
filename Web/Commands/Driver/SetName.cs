using Web.Interfaces;

namespace Web.Commands.Driver;

public class SetName : ICommand<string>
{
    public void Execute(string stub) => stub += "ghg";
    
    public required string Name { get; init; }
    
    public const string CommandName = "SetNameToDriver";
}