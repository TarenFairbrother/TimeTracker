namespace TimeTracker.API.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly DataContext context;

    public ProjectRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<Project?> GetProjectById(int id)
    {
        var project = await context.Projects
            .Include(p => p.ProjectDetails)
            .Where(p => !p.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == id);
        return project;
    }

    public async Task<List<Project>> GetAllProjects()
    {
        return await this.context.Projects
                .Include(p => p.ProjectDetails)
                .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<Project>> CreateProject(Project project)
    {
        this.context.Projects.Add(project);
        await this.context.SaveChangesAsync();
        
        return await GetAllProjects();
    }

    public async Task<List<Project>> UpdateProject(int id, Project project)
    {
        var dbProject = await this.context.Projects
            .Include(p => p.ProjectDetails)
            .FirstOrDefaultAsync(x => x.Id == id);
        
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
        var dbProject = await this.context.Projects.FindAsync(id);
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