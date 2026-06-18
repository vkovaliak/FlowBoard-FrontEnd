using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Common;

public partial class UserAvatar
{
    [Parameter] public string UserName { get; set; } = string.Empty;
    [Parameter] public string? AvatarUrl { get; set; }
    [Parameter] public Size Size { get; set; } = Size.Medium;
    [Parameter] public string SizeStyle { get; set; } = string.Empty;

    private static readonly string[] Colors =
    [
        "#0079BF", "#61BD4F", "#FF9F1A", "#EB5A46",
        "#C377E0", "#00C2E0", "#FF78CB", "#344563",
        "#172B4D", "#00875A"
    ];

    private string GetInitial()
        => string.IsNullOrWhiteSpace(UserName)
            ? "?"
            : UserName.Trim()[0].ToString().ToUpper();

    private string GetColorStyle()
    {
        var hash = string.IsNullOrEmpty(UserName)
            ? 0
            : UserName.Sum(c => c);

        var color = Colors[Math.Abs(hash) % Colors.Length];
        return $"background-color: {color}; color: white;";
    }
}