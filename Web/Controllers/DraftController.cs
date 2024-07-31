using System.Dynamic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web.Commands.Driver;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers;

[Route("draft")]
public class DraftController : Controller
{
    public static bool HasProperty(dynamic value, string name)
    {
        if (value is ExpandoObject eo)
            return (eo as IDictionary<string, object>).ContainsKey(name);
        
        return value.GetType().GetProperty(name) != null;
    }
    
    [HttpPost]
    [Route("index")]
    public async Task<IActionResult> Index()
    {
        string macroCommandJson;
        using (var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
        {
            macroCommandJson = await reader.ReadToEndAsync();
        }
        
        var macroCommand = JsonConvert.DeserializeObject<JToken>(macroCommandJson);
        if (macroCommand == null)
            return BadRequest();
        
        var commands = macroCommand["Commands"] as JArray;
        
        if (commands != null)
        {
            var typedCommands = new List<ICommand<string>>(commands.Count);
            foreach (var command in commands)
            {
                var commandName = (string?)command["CommandName"];
                switch (commandName)
                {
                    case AddHoursWorked.CommandNameConstant:
                        var typedCommand = command.ToObject<AddHoursWorked>();
                        if (typedCommand == null)
                            return BadRequest();
                        
                        typedCommands.Add(typedCommand);
                        
                        break;
                    case "MyType2":
                        break;
                    default:
                        return BadRequest();
                }
            }
            
            // Получить из JToken-макрокоманды поле EntityGuid и свалидировать его и вручную создать экземляр макрокоманды из этого поля и полученных выше комманд
        }
    }
}