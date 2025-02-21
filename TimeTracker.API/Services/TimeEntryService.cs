using Mapster;

namespace TimeTracker.API.Services;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository timeEntryRepository;

    public TimeEntryService(ITimeEntryRepository timeEntryRepository)
    {
        this.timeEntryRepository = timeEntryRepository;
    }

    public async Task<TimeEntryResponse?> GetTimeEntryById(int id)
    {
        var result = await this.timeEntryRepository.GetTimeEntryById(id);
        if (result == null)
        {
            return null;
        }
        return result.Adapt<TimeEntryResponse>();
    }

    public async Task<List<TimeEntryResponse>> GetAllTimeEntries()
    {
        var result = await this.timeEntryRepository.GetAllTimeEntries();
        return result.Select(t => t.Adapt<TimeEntryResponse>()).ToList();
    }

    public async Task<List<TimeEntryResponse>> CreateTimeEntry(TimeEntryCreateRequest request)
    {
        var newEntry = request.Adapt<TimeEntry>();
        
        var result = await this.timeEntryRepository.CreateTimeEntry(newEntry);
        return result.Select(t => t.Adapt<TimeEntryResponse>()).ToList();
    }

    public async Task<List<TimeEntryResponse>?> UpdateTimeEntry(int id, TimeEntryUpdateRequest request)
    {

        try
        {
            var updatedEntry = request.Adapt<TimeEntry>();
        
            var result = await this.timeEntryRepository.UpdateTimeEntry(id, updatedEntry);
        
            return result.Select(t => t.Adapt<TimeEntryResponse>()).ToList();
        }
        catch (EntityNotFoundException)
        {
            return null;
        }
    }

    public async Task<List<TimeEntryResponse>?> DeleteTimeEntry(int id)
    {
        var result = await this.timeEntryRepository.DeleteTimeEntry(id);

        if (result is null)
        {
            return null;
        }
        
        return result.Select(t => t.Adapt<TimeEntryResponse>()).ToList();
    }

    public async Task<List<TimeEntryResponse>> GetTimeEntriesByProject(int projectId)
    {
        var result = await this.timeEntryRepository.GetTimeEntriesByProject(projectId);
        return result.Select(t => t.Adapt<TimeEntryResponse>()).ToList();
    }
}