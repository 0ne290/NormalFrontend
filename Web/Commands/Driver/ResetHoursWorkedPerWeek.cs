using Web.Interfaces;

namespace Web.Commands.Driver;

public class ResetHoursWorkedPerWeek : ICommand<string>
{
    public void Execute(string stub) => stub += "ghg";
    
    public const string CommandName = "ResetHoursWorkedPerWeekToDriver";
}