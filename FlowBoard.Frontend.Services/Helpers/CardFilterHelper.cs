using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Domain.Models.Cards;

namespace FlowBoard.Frontend.Services.Helpers;

public static class CardFilterHelper
{
    public static bool Matches(
        CardDto card, CardFilterModel filter, Guid currentUserId)
    {
        switch (filter.Assignee)
        {
            case AssigneeFilter.AssignedToMe
                when card.Assignees.All(
                    a => a.UserId != currentUserId):
                return false;

            case AssigneeFilter.Unassigned
                when card.Assignees.Count > 0:
                return false;
        }

        switch (filter.Status)
        {
            case StatusFilter.Completed when !card.IsCompleted:
                return false;

            case StatusFilter.Active when card.IsCompleted:
                return false;
        }

        if (!MatchesDueDate(card, filter.DueDate))
        {
            return false;
        }

        if (filter.LabelIds.Count > 0
            && !card.Labels.Any(
                l => filter.LabelIds.Contains(l.Id)))
        {
            return false;
        }

        return true;
    }

    private static bool MatchesDueDate(
        CardDto card, DueDateFilter filter)
    {
        var today = DateTime.Today;

        switch (filter)
        {
            case DueDateFilter.NoDueDate:
                return !card.DueDate.HasValue;

            case DueDateFilter.Overdue:
                return card.DueDate.HasValue
                    && card.DueDate.Value.Date < today
                    && !card.IsCompleted;

            case DueDateFilter.DueTomorrow:
                return card.DueDate.HasValue
                    && card.DueDate.Value.Date == today.AddDays(1);

            case DueDateFilter.DueNextWeek:
                var weekEnd = today.AddDays(7);
                return card.DueDate.HasValue
                    && card.DueDate.Value.Date >= today
                    && card.DueDate.Value.Date <= weekEnd;

            case DueDateFilter.Any:
            default:
                return true;
        }
    }
}