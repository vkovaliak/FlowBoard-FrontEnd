using System.ComponentModel.DataAnnotations;

namespace FlowBoard.Frontend.Domain.Models.Cards;

public class CreateCardModel
{
    [Required(ErrorMessage = "Card title is required.")]
    [StringLength(100, ErrorMessage = "Title must not be more than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }
}