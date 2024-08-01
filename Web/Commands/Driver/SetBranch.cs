using Newtonsoft.Json;
using Web.Interfaces;

namespace Web.Commands.Driver;

public class SetBranch : ICommand<string>
{
    public void Execute(string stub) => stub += "ghg";
    
    [JsonProperty(Required = Required.Always)]
    public required string BranchGuid { get; init; }
    
    [JsonProperty(Required = Required.Always)]
    public required string CommandName
    {
        get => _commandName;
        init
        {
            if (value != CommandNameConstant)
                throw new ArgumentOutOfRangeException(nameof(value), value, $"The command name must be {CommandNameConstant}.");

            _commandName = value;
        }
    }

    public const string CommandNameConstant = "SetBranchToDriver";

    private readonly string _commandName = null!;
}