using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Domain.Models.Boards;

namespace FlowBoard.Frontend.Domain.Constants;

public static class BoardTemplateOptions
{
    public static readonly BoardTemplateOption[] All =
    [
        new(BoardTemplate.Kanban,
            "Kanban",
            "Classic workflow board",
            "ViewKanban",
            ["To Do", "In Progress", "Done"]),

        new(BoardTemplate.Empty,
            "Empty",
            "Start from scratch",
            "Add",
            []),

        new(BoardTemplate.Sprint,
            "Sprint",
            "Agile sprint board",
            "DirectionsRun",
            ["Backlog", "To Do", "In Progress", "Review", "Done"]),

        new(BoardTemplate.Roadmap,
            "Roadmap",
            "Plan your milestones",
            "Timeline",
            ["Planned", "In Progress", "Completed", "On Hold"])
    ];
}