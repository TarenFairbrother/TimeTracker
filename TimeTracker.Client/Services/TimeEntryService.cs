using System.Net.Http.Json;
using Mapster;
using TimeTracker.Shared.Models.TimeEntry;

namespace TimeTracker.Client.Services;

public class TimeEntryService : ITimeEntryService
{
    private readonly HttpClient http;
    
    public event Action? OnChange;
    public List<TimeEntryResponse> TimeEntries { get; set; } = new();

    public TimeEntryService(HttpClient http)
    {
        this.http = http;
    }
    
    public async Task GetTimeEntriesByProject(int projectId)
    {
        List<TimeEntryResponse>? result = null;
        if (projectId <= 0)
        {
            result = await this.http.GetFromJsonAsync<List<TimeEntryResponse>>("api/timeentry");
        }
        else
        {
            result = await this.http.GetFromJsonAsync<List<TimeEntryResponse>>($"api/timeentry/project/{projectId}");
        }

        if (result != null)
        {
            TimeEntries = result;
            OnChange?.Invoke();
        }
    }

    public async Task<TimeEntryResponse> GetTimeEntryById(int id)
    {
        return await this.http.GetFromJsonAsync<TimeEntryResponse>($"api/timeentry/{id}");
    }

    public async Task CreateTimeEntry(TimeEntryRequest request)
    {
        await this.http.PostAsJsonAsync("api/timeentry", request.Adapt<TimeEntryCreateRequest>());
    }

    public async Task UpdateTimeEntry(int id, TimeEntryRequest request)
    {
        await this.http.PutAsJsonAsync($"api/timeentry/{id}", request.Adapt<TimeEntryUpdateRequest>());
    }

    public async Task DeleteTimeEntry(int id)
    {
       await this.http.DeleteAsync($"api/timeentry/{id}");
    }
}