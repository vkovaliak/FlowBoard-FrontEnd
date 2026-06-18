using FlowBoard.Frontend.Domain.DTOs.Labels;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Cards;

public partial class CardLabels
{
    [Parameter] public List<LabelDto> Labels { get; set; } = [];
}