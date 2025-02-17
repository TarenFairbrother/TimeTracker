using TimeTracker.Shared.Models.Project;

namespace TimeTracker.Shared.Models.TimeEntry;

public record struct TimeEntryByProductResponse(
    int Id,
    DateTime Start,
    DateTime? End
);