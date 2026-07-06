using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Home;

public partial class HomeHeader
{
    [Parameter] public string UserName { get; set; } = "";

    private string Greeting => DateTime.Now.Hour switch
    {
        < 12 => "Good morning",
        < 18 => "Good afternoon",
        _ => "Good evening"
    };
}