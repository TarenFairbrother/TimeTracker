using Mapster;
using TimeTracker.API.Repositories;
using TimeTracker.Shared.Entities;
using TimeTracker.Shared.Models.TimeEntry;

namespace TimeTracker.API.Services;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository timeEntryRepository;

    public TimeEntryService(ITimeEntryRepository timeEntryRepository)
    {
        this.timeEntryRepository = timeEntryRepository;
    }

    public TimeEntryResponse? GetTimeEntryById(int id)
    {
        var result = this.timeEntryRepository.GetTimeEntryById(id);
        if (result == null)
        {
            return null;
        }
        return result.Adapt<TimeEntryResponse>();
    }

    public List<TimeEntryResponse> GetAllTimeEntries()
    {
        var result = this.timeEntryRepository.GetAllTimeEntries();
        return result.Select(t => t.Adapt<TimeEntryResponse>()).ToList();
    }

    public List<TimeEntryResponse> CreateTimeEntry(TimeEntryCreateRequest request)
    {
        var newEntry = request.Adapt<TimeEntry>();
        
        var result = this.timeEntryRepository.CreateTimeEntry(newEntry);
        return result.Select(t => t.Adapt<TimeEntryResponse>()).ToList();
    }

    public List<TimeEntryResponse>? UpdateTimeEntry(int id, TimeEntryUpdateRequest request)
    {
        var updatedEntry = request.Adapt<TimeEntry>();
        
        var result = this.timeEntryRepository.UpdateTimeEntry(id, updatedEntry);
        if (result is null)
        {
            return null;
        }
        
        return result.Select(t => t.Adapt<TimeEntryResponse>()).ToList();
    }

    public List<TimeEntryResponse>? DeleteTimeEntry(int id)
    {
        var result = this.timeEntryRepository.DeleteTimeEntry(id);

        if (result is null)
        {
            return null;
        }
        
        return result.Select(t => t.Adapt<TimeEntryResponse>()).ToList();
    }
}