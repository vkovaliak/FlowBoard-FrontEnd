using System.ComponentModel.DataAnnotations;

namespace FlowBoard.Frontend.Domain.Models.Boards;

public class CreateBoardModel
{
    [Required(ErrorMessage = "Board name is required.")]
    [StringLength(100, ErrorMessage = "Board name must not be more than 100 characters.")]
    public string Name { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
    public string? Background { get; set; }
}