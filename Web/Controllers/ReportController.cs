using System.Text;
using Microsoft.AspNetCore.Mvc;
using Web.Daos;

namespace Web.Controllers;

[Route("report")]
public class ReportController : Controller
{
    [HttpGet]
    [Route("telephone-subscribers")]
    public IActionResult TelephoneSubscribers()
    {
        var telephoneSubscribers =
            TelephoneSubscriberDao.ReadRandomTelephoneSubscribersFromFile("RandomTelephoneSubscribers.txt",
                Encoding.UTF8);
        
        return View(telephoneSubscribers);
    }
}