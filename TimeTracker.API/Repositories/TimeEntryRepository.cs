namespace TimeTracker.API.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly DataContext context;

    public TimeEntryRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<TimeEntry?> GetTimeEntryById(int id)
    {
        var timeEntry = await context.TimeEntries.FindAsync(id);
        return timeEntry;
    }

    public async Task<List<TimeEntry>> GetAllTimeEntries()
    {
        return await this.context.TimeEntries.ToListAsync();
    }

    public async Task<List<TimeEntry>> CreateTimeEntry(TimeEntry timeEntry)
    {
        this.context.TimeEntries.Add(timeEntry);
        await this.context.SaveChangesAsync();
        
        return await this.context.TimeEntries.ToListAsync();
    }

    public async Task<List<TimeEntry>> UpdateTimeEntry(int id, TimeEntry timeEntry)
    {
        var dbTimeEntry = await this.context.TimeEntries.FindAsync(id);
        if (dbTimeEntry == null)
        {
            throw new EntityNotFoundException($"Entity with id {id} was not found");
        }
        
        dbTimeEntry.Project = timeEntry.Project;
        dbTimeEntry.Start = timeEntry.Start;
        dbTimeEntry.End = timeEntry.End;
        dbTimeEntry.DateUpdated = DateTime.Now;
        
        await this.context.SaveChangesAsync();

        return await GetAllTimeEntries();
    }

    public async Task<List<TimeEntry>?> DeleteTimeEntry(int id)
    {
        var dbTimeEntry = await this.context.TimeEntries.FindAsync(id);
        if (dbTimeEntry == null)
        {
            return null;
        }
        
        this.context.TimeEntries.Remove(dbTimeEntry);
        
        await this.context.SaveChangesAsync();
        
        return await GetAllTimeEntries();
    }
}