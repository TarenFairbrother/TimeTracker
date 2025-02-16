using TimeTracker.Shared.Entities;

namespace TimeTracker.API.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private static List<TimeEntry> timeEntries = new List<TimeEntry>
    {
        new TimeEntry { Id = 1, Project = "Time Tracker App", Start = DateTime.Now.AddHours(1) }
    };

    public TimeEntry? GetTimeEntryById(int id)
    {
        return timeEntries.FirstOrDefault(x => x.Id == id);
    }

    public List<TimeEntry> GetAllTimeEntries()
    {
        return timeEntries;
    }

    public List<TimeEntry> CreateTimeEntry(TimeEntry timeEntry)
    {
        timeEntries.Add(timeEntry);
        return timeEntries;
    }

    public List<TimeEntry>? UpdateTimeEntry(int id, TimeEntry timeEntry)
    {
        var timeEntryToUpdateIndex = timeEntries.FindIndex(x => x.Id == id);

        if (timeEntryToUpdateIndex == -1)
        {
            return null;
        }
        
        timeEntries[timeEntryToUpdateIndex] = timeEntry;
        
        return timeEntries;
    }

    public List<TimeEntry>? DeleteTimeEntry(int id)
    {
        var timeEntryToDelete = timeEntries.FirstOrDefault(t => t.Id == id);

        if (timeEntryToDelete == null)
        {
            return null;
        }

        timeEntries.Remove(timeEntryToDelete);
        
        return timeEntries;
    }
}