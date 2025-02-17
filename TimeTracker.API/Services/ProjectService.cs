using Mapster;
using TimeTracker.Shared.Models.Project;

namespace TimeTracker.API.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        this.projectRepository = projectRepository;
    }

    public async Task<ProjectResponse?> GetProjectById(int id)
    {
        var result = await this.projectRepository.GetProjectById(id);
        if (result == null)
        {
            return null;
        }
        return result.Adapt<ProjectResponse>();
    }

    public async Task<List<ProjectResponse>> GetAllProjects()
    {
        var result = await this.projectRepository.GetAllProjects();
        return result.Select(t => t.Adapt<ProjectResponse>()).ToList();
    }

    public async Task<List<ProjectResponse>> CreateProject(ProjectCreateRequest request)
    {
        var newEntry = request.Adapt<Project>();
        newEntry.ProjectDetails = request.Adapt<ProjectDetails>();
        
        var result = await this.projectRepository.CreateProject(newEntry);
        return result.Select(t => t.Adapt<ProjectResponse>()).ToList();
    }

    public async Task<List<ProjectResponse>?> UpdateProject(int id, ProjectUpdateRequest request)
    {

        try
        {
            var updatedEntry = request.Adapt<Project>();
            updatedEntry.ProjectDetails = request.Adapt<ProjectDetails>();
        
            var result = await this.projectRepository.UpdateProject(id, updatedEntry);
        
            return result.Select(t => t.Adapt<ProjectResponse>()).ToList();
        }
        catch (EntityNotFoundException)
        {
            return null;
        }
    }

    public async Task<List<ProjectResponse>?> DeleteProject(int id)
    {
        var result = await this.projectRepository.DeleteProject(id);

        if (result is null)
        {
            return null;
        }
        
        return result.Select(t => t.Adapt<ProjectResponse>()).ToList();
    }
}