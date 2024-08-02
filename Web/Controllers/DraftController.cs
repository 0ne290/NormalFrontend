using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web.Commands;
using Web.Commands.Driver;
using Web.Interfaces;

namespace Web.Controllers;

[Route("draft")]
public class DraftController : Controller
{
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Update()
    {
        try
        {
            string macroCommandJson;
            using (var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
            {
                macroCommandJson = await reader.ReadToEndAsync();
            }

            var macroCommandJToken = JsonConvert.DeserializeObject<JToken>(macroCommandJson);
            if (macroCommandJToken == null)
                return BadRequest();

            var entityGuid = (string?)macroCommandJToken["EntityGuid"];
            if (entityGuid == null)
                return BadRequest();

            if (macroCommandJToken["Commands"] is not JArray commandsJArray)
                return BadRequest();

            var commands = new List<ICommand<string>>(commandsJArray.Count);
            foreach (var commandJToken in commandsJArray)
            {
                var commandName = (string?)commandJToken["CommandName"];
                ICommand<string>? command;
                switch (commandName)
                {
                    case AddHoursWorked.CommandNameConstant:
                        command = commandJToken.ToObject<AddHoursWorked>();
                        break;
                    case DequalifyAdr.CommandNameConstant:
                        command = commandJToken.ToObject<DequalifyAdr>();
                        break;
                    case DequalifyAdrTank.CommandNameConstant:
                        command = commandJToken.ToObject<DequalifyAdrTank>();
                        break;
                    case Dismiss.CommandNameConstant:
                        command = commandJToken.ToObject<Dismiss>();
                        break;
                    case QualifyAdr.CommandNameConstant:
                        command = commandJToken.ToObject<QualifyAdr>();
                        break;
                    case QualifyAdrTank.CommandNameConstant:
                        command = commandJToken.ToObject<QualifyAdrTank>();
                        break;
                    case Reinstate.CommandNameConstant:
                        command = commandJToken.ToObject<Reinstate>();
                        break;
                    case ResetHoursWorkedPerWeek.CommandNameConstant:
                        command = commandJToken.ToObject<ResetHoursWorkedPerWeek>();
                        break;
                    case Rest.CommandNameConstant:
                        command = commandJToken.ToObject<Rest>();
                        break;
                    case SetBranch.CommandNameConstant:
                        command = commandJToken.ToObject<SetBranch>();
                        break;
                    case SetName.CommandNameConstant:
                        command = commandJToken.ToObject<SetName>();
                        break;
                    case Work.CommandNameConstant:
                        command = commandJToken.ToObject<Work>();
                        break;
                    default:
                        return BadRequest();
                }

                if (command == null)
                    return BadRequest();

                commands.Add(command);
            }

            var macroCommand = new MacroCommand<string> { EntityGuid = entityGuid, Commands = commands };
            Console.WriteLine($"EntityGuid = {macroCommand.EntityGuid}");
            foreach (var command in macroCommand.Commands)
            {
                if (command is QualifyAdr cv)
                    Console.WriteLine(
                        $"\tCommandType = {command.GetType()}; AdrQualificationFlag = {cv.AdrQualificationFlag}");
                else
                    Console.WriteLine($"\tCommandType = {command.GetType()}");
            }

            return Ok();
        }
        catch (JsonSerializationException)
        {
            return BadRequest();
        }
    }
    
    [HttpGet]
    [Route("get-by-filters")]
    public async Task<IActionResult> GetByFilters(string filterString)
    {
        try
        {
            Console.WriteLine(filterString);

            var drivers = new Driver[] { new(13), new(12), new(22) };
            var filterExpression = DynamicExpressionParser.ParseLambda(typeof(Driver), typeof(bool), filterString);
            var resultDrivers = drivers.AsQueryable().Where(filterExpression).ToList();

            foreach (var driver in resultDrivers)
                Console.WriteLine(driver.Salary);
            Console.WriteLine();
            foreach (var driver in drivers)
                Console.WriteLine(driver.Salary);

            return Ok();
        }
        catch (ParseException)
        {
            return BadRequest();
        }
    }
    
    [HttpGet]
    [Route("get-by-guid/{guid}")]
    public async Task<IActionResult> GetByGuid(string guid)
    {
        return Ok();
    }
    
    [HttpGet]
    [Route("get-all")]
    public async Task<IActionResult> GetAll(string arg)
    {
        var c = DynamicExpressionParser.ParseLambda<string, bool>(new ParsingConfig(), true, "s => (new[] { \"One\", \"Two\", \"Three\" }).Contains(s)");
        
        return Ok(ExecuteExpression(c, arg));
    }

    private bool ExecuteExpression(Expression<Func<string, bool>> expression, string arg) =>
        expression.Compile().Invoke(arg);
}

public class Driver
{
    public Driver(int salary) => SetSalary(salary);
    
    public void SetSalary(int salary)
    {
        if (salary < 10)
            throw new ArgumentException("Too little.", nameof(salary));

        Salary = salary;
    }
    
    public int Salary { get; private set; }
}