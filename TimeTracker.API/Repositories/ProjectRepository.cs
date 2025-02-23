namespace TimeTracker.API.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly DataContext context;
    private readonly IUserContextService userContextService;

    public ProjectRepository(DataContext context, IUserContextService userContextService)
    {
        this.context = context;
        this.userContextService = userContextService;
    }

    public async Task<List<Project>> GetAllProjects()
    {
        var userId = this.userContextService.GetUserId();
        if (userId == null)
        {
            return new List<Project>();
        }
        
        return await this.context.Projects
            .Include(p => p.ProjectDetails)
            .Where(p => !p.IsDeleted && p.Users.Any(u => u.Id == userId))
            .ToListAsync();
    }
    
    public async Task<Project?> GetProjectById(int id)
    {
        var userId = this.userContextService.GetUserId();
        if (userId == null)
        {
            return null;
        }
        
        var project = await context.Projects
            .Include(p => p.ProjectDetails)
            .FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id && p.Users.Any(u => u.Id == userId));
        return project;
    }

    public async Task<List<Project>> CreateProject(Project project)
    {
        var user = await this.userContextService.GetUserAsync();
        if (user == null)
        {
            throw new EntityNotFoundException("User not found");
        }
        
        project.Users.Add(user);
        
        this.context.Projects.Add(project);
        await this.context.SaveChangesAsync();
        
        return await GetAllProjects();
    }

    public async Task<List<Project>> UpdateProject(int id, Project project)
    {
        var userId = this.userContextService.GetUserId();
        if (userId == null)
        {
            throw new EntityNotFoundException("User not found");
        }
        
        var dbProject = await this.context.Projects
            .Include(p => p.ProjectDetails)
            .FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id && p.Users.Any(u => u.Id == userId));
        
        if (dbProject == null)
        {
            throw new EntityNotFoundException($"Entity with id {id} was not found");
        }

        if (project.ProjectDetails != null && dbProject.ProjectDetails != null)
        {
            dbProject.ProjectDetails.Description = project.ProjectDetails.Description;
            dbProject.ProjectDetails.StartDate = project.ProjectDetails.StartDate;
            dbProject.ProjectDetails.EndDate = project.ProjectDetails.EndDate;
        }
        else if(project.ProjectDetails != null && dbProject.ProjectDetails == null)
        {
            dbProject.ProjectDetails = new ProjectDetails
            {
                Description = project.ProjectDetails.Description,
                StartDate = project.ProjectDetails.StartDate,
                EndDate = project.ProjectDetails.EndDate,
                Project = dbProject
            };
        }
        
        dbProject.Name = project.Name;
        dbProject.DateUpdated = DateTime.Now;
        
        await this.context.SaveChangesAsync();

        return await GetAllProjects();
    }

    public async Task<List<Project>?> DeleteProject(int id)
    {
        var userId = this.userContextService.GetUserId();
        if (userId == null)
        {
            return null;
        }
        
        var dbProject = await this.context.Projects
            .FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id && p.Users.Any(u => u.Id == userId));
        if (dbProject == null)
        {
            return null;
        }
        
        dbProject.IsDeleted = true;
        dbProject.DateDeleted = DateTime.Now;
        
        await this.context.SaveChangesAsync();
        
        return await GetAllProjects();
    }
}