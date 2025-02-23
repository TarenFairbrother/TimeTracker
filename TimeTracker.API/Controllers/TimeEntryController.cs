using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TimeTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TimeEntryController : ControllerBase
{
    private readonly ITimeEntryService timeEntryService;

    public TimeEntryController(ITimeEntryService timeEntryService)
    {
        this.timeEntryService = timeEntryService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<TimeEntryResponse>>> GetAllTimeEntries()
    {
        return Ok(await this.timeEntryService.GetAllTimeEntries());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<TimeEntryResponse>> GetTimeEntryById(int id)
    {
        var result = await this.timeEntryService.GetTimeEntryById(id);

        if (result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<List<TimeEntryResponse>>> GetTimeEntriesByProject(int projectId)
    {
        return Ok(await this.timeEntryService.GetTimeEntriesByProject(projectId));
    }

    [HttpPost]
    public async  Task<ActionResult<List<TimeEntryResponse>>> CreateTimeEntry(TimeEntryCreateRequest request)
    {
        var timeEntries = await this.timeEntryService.CreateTimeEntry(request);
        
        return Ok(timeEntries);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<List<TimeEntryResponse>>> UpdateTimeEntry(int id, TimeEntryUpdateRequest request)
    {
        var timeEntries = await this.timeEntryService.UpdateTimeEntry(id, request);

        if (timeEntries is null)
        {
            return NotFound("Time entry with the given Id was not found.");
        }
        
        return Ok(timeEntries);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<List<TimeEntryResponse>>> DeleteTimeEntry(int id)
    {
        var timeEntries = await this.timeEntryService.DeleteTimeEntry(id);

        if (timeEntries is null)
        {
            return NotFound("Time entry with the given Id was not found.");
        }
        
        return Ok(timeEntries);
    }
}