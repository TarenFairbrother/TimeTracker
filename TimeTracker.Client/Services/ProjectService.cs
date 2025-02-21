using System.Net.Http.Json;
using Mapster;
using TimeTracker.Shared.Models.Project;

namespace TimeTracker.Client.Services;

public class ProjectService : IProjectService
{
    private readonly HttpClient http;
    
    
    public event Action? OnChange;

    public ProjectService(HttpClient http)
    {
        this.http = http;
    }
    public List<ProjectResponse> Projects { get; set; } = new();
    
    public async Task LoadAllProjects()
    {
        var result = await this.http.GetFromJsonAsync<List<ProjectResponse>>("api/project");
        if (result is not null)
        {
            this.Projects = result;
            OnChange?.Invoke();
        }
    }

    public async Task<ProjectResponse> GetProjectById(int id)
    {
        return await this.http.GetFromJsonAsync<ProjectResponse>($"api/project/{id}");
    }

    public async Task CreateProject(ProjectRequest request)
    {
        await this.http.PostAsJsonAsync("api/project", request.Adapt<ProjectCreateRequest>());
    }

    public async Task UpdateProject(int id, ProjectRequest request)
    {
        await this.http.PutAsJsonAsync($"api/project/{id}", request.Adapt<ProjectUpdateRequest>());
    }

    public async Task DeleteProject(int id)
    {
        await this.http.DeleteAsync($"api/project/{id}");
    }
}