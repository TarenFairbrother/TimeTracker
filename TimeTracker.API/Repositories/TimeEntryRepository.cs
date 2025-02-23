namespace TimeTracker.API.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly DataContext context;
    private readonly IUserContextService userContextService;

    public TimeEntryRepository(DataContext context, IUserContextService userContextService)
    {
        this.context = context;
        this.userContextService = userContextService;
    }

    public async Task<List<TimeEntry>> GetAllTimeEntries()
    {
        var userId = this.userContextService.GetUserId();

        if (userId == null)
        {
            return new List<TimeEntry>();
        }
        
        return await this.context.TimeEntries
            .Include(t => t.Project)
            .ThenInclude(p => p.ProjectDetails)
            .Where(t => t.User.Id == userId)
            .ToListAsync();
    }
    
    public async Task<TimeEntry?> GetTimeEntryById(int id)
    {
        var userId = this.userContextService.GetUserId();
        if (userId == null)
        {
            return null;
        }
        
        var timeEntry = await context.TimeEntries
            .Include(t => t.Project)
            .ThenInclude(p => p.ProjectDetails)
            .FirstOrDefaultAsync(x => x.Id == id && x.User.Id == userId);
        return timeEntry;
    }

    public async Task<List<TimeEntry>> CreateTimeEntry(TimeEntry timeEntry)
    {
        var user = await this.userContextService.GetUserAsync();
        if (user == null)
        {
            throw new EntityNotFoundException("User was not found");
        }
        
        timeEntry.User = user;
        
        this.context.TimeEntries.Add(timeEntry);
        await this.context.SaveChangesAsync();

        return await GetAllTimeEntries();
    }

    public async Task<List<TimeEntry>> UpdateTimeEntry(int id, TimeEntry timeEntry)
    {
        var userId = this.userContextService.GetUserId();
        if (userId == null)
        {
            throw new EntityNotFoundException("User was not found");
        }
        
        var dbTimeEntry = await this.context.TimeEntries
            .FirstOrDefaultAsync(x => x.Id == id && x.User.Id == userId);
        if (dbTimeEntry == null)
        {
            throw new EntityNotFoundException($"Entity with id {id} was not found");
        }

        dbTimeEntry.ProjectId = timeEntry.ProjectId;
        dbTimeEntry.Start = timeEntry.Start;
        dbTimeEntry.End = timeEntry.End;
        dbTimeEntry.DateUpdated = DateTime.Now;

        await this.context.SaveChangesAsync();

        return await GetAllTimeEntries();
    }

    public async Task<List<TimeEntry>?> DeleteTimeEntry(int id)
    {
        var userId = this.userContextService.GetUserId();
        if (userId == null)
        {
            return null;
        }
        
        var dbTimeEntry = await this.context.TimeEntries
            .FirstOrDefaultAsync(x => x.Id == id && x.User.Id == userId);
        if (dbTimeEntry == null)
        {
            return null;
        }

        this.context.TimeEntries.Remove(dbTimeEntry);

        await this.context.SaveChangesAsync();

        return await GetAllTimeEntries();
    }

    public async Task<List<TimeEntry>> GetTimeEntriesByProject(int projectId)
    {
        var userId = this.userContextService.GetUserId();
        if (userId == null)
        {
            throw new EntityNotFoundException("User was not found");
        }
        
        return await this.context.TimeEntries
            .Include(t => t.Project)
            .ThenInclude(p => p.ProjectDetails)
            .Where(t => t.ProjectId == projectId && t.User.Id == userId)
            .ToListAsync();
    }
}