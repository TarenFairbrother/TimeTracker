using Microsoft.AspNetCore.Mvc;
using TimeTracker.Shared.Models.Project;

namespace TimeTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService projectService;

    public ProjectController(IProjectService projectService)
    {
        this.projectService = projectService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectResponse>> GetProjectById(int id)
    {
        var result = await this.projectService.GetProjectById(id);

        if (result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectResponse>>> GetAllProjects()
    {
        return Ok(await this.projectService.GetAllProjects());
    }

    [HttpPost]
    public async  Task<ActionResult<List<ProjectResponse>>> CreateProject(ProjectCreateRequest request)
    {
        var Projects = await this.projectService.CreateProject(request);
        
        return Ok(Projects);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<List<ProjectResponse>>> UpdateProject(int id, ProjectUpdateRequest request)
    {
        var Projects = await this.projectService.UpdateProject(id, request);

        if (Projects is null)
        {
            return NotFound("Time entry with the given Id was not found.");
        }
        
        return Ok(Projects);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<List<ProjectResponse>>> DeleteProject(int id)
    {
        var Projects = await this.projectService.DeleteProject(id);

        if (Projects is null)
        {
            return NotFound("Time entry with the given Id was not found.");
        }
        
        return Ok(Projects);
    }
}