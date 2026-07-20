namespace FlowBoard.Frontend.Domain.Enums;

public enum AssigneeFilter
{
    Any,
    AssignedToMe,
    Unassigned
}

public enum StatusFilter
{
    Any,
    Completed,
    Active
}

public enum DueDateFilter
{
    Any,
    NoDueDate,
    Overdue,
    DueTomorrow,
    DueNextWeek
}