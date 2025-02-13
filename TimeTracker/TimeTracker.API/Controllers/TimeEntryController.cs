using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Services;
using TimeTracker.Shared.Models.TimeEntry;

namespace TimeTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeEntryController : ControllerBase
{
    private readonly ITimeEntryService timeEntryService;

    public TimeEntryController(ITimeEntryService timeEntryService)
    {
        this.timeEntryService = timeEntryService;
    }

    [HttpGet]
    public ActionResult<List<TimeEntryResponse>> GetAllTimeEntries()
    {
        return Ok(this.timeEntryService.GetAllTimeEntries());
    }
    
    [HttpGet("{id}")]
    public ActionResult<TimeEntryResponse> GetTimeEntryById(int id)
    {
        var result = this.timeEntryService.GetTimeEntryById(id);

        if (result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }

    [HttpPost]
    public ActionResult<List<TimeEntryResponse>> CreateTimeEntry(TimeEntryCreateRequest request)
    {
        var timeEntries = this.timeEntryService.CreateTimeEntry(request);
        
        return Ok(timeEntries);
    }
    
    [HttpPut("{id}")]
    public ActionResult<List<TimeEntryResponse>> UpdateTimeEntry(int id, TimeEntryUpdateRequest request)
    {
        var timeEntries = this.timeEntryService.UpdateTimeEntry(id, request);

        if (timeEntries is null)
        {
            return NotFound("Time entry with the given Id was not found.");
        }
        
        return Ok(timeEntries);
    }
    
    [HttpDelete("{id}")]
    public ActionResult<List<TimeEntryResponse>> DeleteTimeEntry(int id)
    {
        var timeEntries = this.timeEntryService.DeleteTimeEntry(id);

        if (timeEntries is null)
        {
            return NotFound("Time entry with the given Id was not found.");
        }
        
        return Ok(timeEntries);
    }
}