using Microsoft.AspNetCore.Mvc;
using TimeTracker.Shared.Entities;

namespace TimeTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeEntryController : ControllerBase
{
    private static List<TimeEntry> timeEntries = new List<TimeEntry>
    {
        new TimeEntry { Id = 1, Project = "Time Tracker App", Start = DateTime.Now.AddHours(1) }
    };

    [HttpGet]
    [Route("all")]
    public ActionResult<List<TimeEntry>> GetAllTimeEntries()
    {
        return Ok(timeEntries);
    }
}