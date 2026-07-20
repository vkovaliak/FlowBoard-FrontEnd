using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.Models.Cards;

public class CardFilterModel
{
    public AssigneeFilter Assignee { get; set; } = AssigneeFilter.Any;
    public StatusFilter Status { get; set; } = StatusFilter.Any;
    public DueDateFilter DueDate { get; set; } = DueDateFilter.Any;
    public HashSet<Guid> LabelIds { get; set; } = [];

    public bool IsActive =>
        Assignee != AssigneeFilter.Any
        || Status != StatusFilter.Any
        || DueDate != DueDateFilter.Any
        || LabelIds.Count > 0;

    public int ActiveCount
    {
        get
        {
            var count = 0;
            if (Assignee != AssigneeFilter.Any) count++;
            if (Status != StatusFilter.Any) count++;
            if (DueDate != DueDateFilter.Any) count++;
            if (LabelIds.Count > 0) count++;
            return count;
        }
    }

    public CardFilterModel Clone() => new()
    {
        Assignee = Assignee,
        Status = Status,
        DueDate = DueDate,
        LabelIds = [.. LabelIds]
    };

    public void Reset()
    {
        Assignee = AssigneeFilter.Any;
        Status = StatusFilter.Any;
        DueDate = DueDateFilter.Any;
        LabelIds.Clear();
    }
}