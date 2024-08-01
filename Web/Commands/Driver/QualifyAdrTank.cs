using Newtonsoft.Json;
using Web.Interfaces;

namespace Web.Commands.Driver;

public class QualifyAdrTank : ICommand<string>
{
    public void Execute(string stub) => stub += "ghg";
    
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

    public const string CommandNameConstant = "QualifyAdrTankToDriver";

    private readonly string _commandName = null!;
}