using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Board;

public partial class BoardHeader
{
    [Parameter] 
    public string BoardName { get; set; } = string.Empty;
    
    [Parameter] 
    public bool IsPublic { get; set; }
    
    [Parameter] 
    public EventCallback OnInviteClick { get; set; }
}