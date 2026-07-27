using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.Models.Boards;

public record BoardTemplateOption(
    BoardTemplate Template,
    string Title,
    string Description,
    string Icon,
    string[] Columns
);