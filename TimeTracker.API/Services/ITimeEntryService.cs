using TimeTracker.Shared.Models.TimeEntry;

namespace TimeTracker.API.Services;

public interface ITimeEntryService
{
    Task<TimeEntryResponse?> GetTimeEntryById(int id);
    Task<List<TimeEntryResponse>> GetAllTimeEntries();
    Task<TimeEntryResponseWrapper> GetTimeEntries(int skip, int limit);
    Task<List<TimeEntryResponse>> CreateTimeEntry(TimeEntryCreateRequest request);
    Task<List<TimeEntryResponse>?> UpdateTimeEntry(int id, TimeEntryUpdateRequest request);
    Task<List<TimeEntryResponse>?> DeleteTimeEntry(int id);
    Task<List<TimeEntryResponse>> GetTimeEntriesByProject(int projectId);
    
}