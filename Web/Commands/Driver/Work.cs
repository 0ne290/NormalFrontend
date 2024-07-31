using Web.Interfaces;

namespace Web.Commands.Driver;

public class Work : ICommand<string>
{
    public void Execute(string stub) => stub += "ghg";
    
    public const string CommandName = "WorkToDriver";
}