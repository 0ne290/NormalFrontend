using Web.Interfaces;

namespace Web.Commands.Driver;

public class SetBranch : ICommand<string>
{
    public void Execute(string stub) => stub += "ghg";
    
    public required string BranchGuid { get; init; }
    
    public const string CommandName = "SetBranchToDriver";
}